using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace QTD.Towers
{
    [RequireComponent(typeof(CircleCollider2D))]
    public abstract class Tower : MonoBehaviour
    {
        [SerializeField]
        protected GameObject _towerSprite;

        [SerializeField]
        protected GameObject _projectile;

        [SerializeField]
        protected LayerMask _enemyLayer;

        [Header("UI")]
        [SerializeField]
        private GameObject _rangeCircle;

        [SerializeField]
        private GameObject _uiCanvas;

        [SerializeField]
        private Button _upgradeButton;

        [SerializeField]
        private TextMeshProUGUI _upgradeText;

        [SerializeField]
        private TextMeshProUGUI _sellText;

        protected int _level = 1;

        /// <summary>
        /// The tile the tower is placed on
        /// </summary>
        public GridTile PlacedTile { get; set; }

        public abstract bool IsMaxLevel { get; }

        public abstract int InitialCost { get; }

        public abstract int UpgradeCost { get; }

        public abstract int SellPrice { get; }

        protected abstract float AttackInterval { get; }

        protected abstract float AttackRange { get; }

        private float _intervalCounter = 0;

        protected List<Enemy> _nearbyEnemies = new List<Enemy>();

        void Start()
        {
            UpdateStats();
            HideAllUI();
        }

        void OnEnable()
        {
            GameManager.instance.goldChangeEvent.AddListener(CheckUpgradable);
        }

        void OnDisable()
        {
            GameManager.instance.goldChangeEvent.RemoveListener(CheckUpgradable);
        }

        void Update()
        {
            if (PlacedTile is null || _nearbyEnemies.Count <= 0) return;

            Enemy closestEnemy = GetClosestEnemy();

            // Aim at closest enemy
            _towerSprite.transform.up = closestEnemy.transform.position - transform.position;

            // Countdown attack interval
            if (_intervalCounter > 0)
            {
                _intervalCounter -= Time.deltaTime;
                return;
            }

            LaunchProjectile(closestEnemy);

            _intervalCounter = AttackInterval;
        }

        public void ShowAttackRange()
        {
            _rangeCircle.SetActive(true);
        }

        public void ShowOptions()
        {
            _uiCanvas.SetActive(true);
        }

        public void HideAllUI()
        {
            _rangeCircle.SetActive(false);
            _uiCanvas.SetActive(false);
        }

        /// <summary>
        /// Called bu upgrade button
        /// </summary>
        public void Upgrade()
        {
            if (!IsMaxLevel && GameManager.instance.IsGoldSufficient(UpgradeCost))
            {
                GameManager.instance.UseGold(UpgradeCost);
                _level++;
                UpdateStats();
            }
        }

        /// <summary>
        /// Called by sell button
        /// </summary>
        public void Sell()
        {
            GameManager.instance.AddGold(SellPrice);
            // Allows tile to be placed by other tower
            PlacedTile.Tower = null;
            Destroy(gameObject);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            // Check if gameobject layer intersect enemy layer mask
            if (((1 << col.gameObject.layer) & _enemyLayer.value) != 0)
                _nearbyEnemies.Add(col.gameObject.GetComponent<Enemy>());
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if (((1 << col.gameObject.layer) & _enemyLayer.value) != 0)
                _nearbyEnemies.Remove(col.gameObject.GetComponent<Enemy>());
        }

        protected abstract void LaunchProjectile(Enemy enemy);

        protected virtual Enemy GetClosestEnemy()
        {
            float smallestSqrMagnitude = float.PositiveInfinity;
            Enemy closestEnemy = null;

            foreach (Enemy enemy in _nearbyEnemies)
            {
                Vector2 delta = enemy.transform.position - transform.position;

                if (delta.sqrMagnitude < smallestSqrMagnitude)
                {
                    closestEnemy = enemy.GetComponent<Enemy>();
                    smallestSqrMagnitude = delta.sqrMagnitude;
                }
            }

            return closestEnemy;
        }

        protected virtual void UpdateStats()
        {
            GetComponent<CircleCollider2D>().radius = AttackRange;
            _rangeCircle.transform.localScale = new Vector3(AttackRange, AttackRange, AttackRange);
            _sellText.text = SellPrice.ToString();
            _upgradeButton.gameObject.SetActive(!IsMaxLevel);

            if (!IsMaxLevel)
                _upgradeText.text = UpgradeCost.ToString();
        }

        // Listen to gold change
        private void CheckUpgradable(int gold)
        {
            _upgradeButton.interactable = !IsMaxLevel && GameManager.instance.IsGoldSufficient(UpgradeCost);
        }
    }
}
