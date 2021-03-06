﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace QTD
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The enemy max health")]
        private float _health = 100f;

        [SerializeField]
        [Tooltip("The enemy speed in units/second")]
        private float _speed = 5f;

        [SerializeField]
        [Tooltip("The gold the player earns when defeating this enemy")]
        private int _gold = 10;

        [SerializeField]
        private SpriteRenderer _sprite;

        public bool IsDead { get; private set; }

        private List<Vector2> _path = new List<Vector2>();

        private Tween _pathTween;

        private float _totalPathDistance
        {
            get
            {
                if (_totalPathDistanceCached is null)
                {
                    float distance = 0f;

                    for (int i = 0; i < _path.Count - 1; i++)
                        distance += (_path[i + 1] - _path[i]).magnitude;

                    _totalPathDistanceCached = distance;

                }

                return (float)_totalPathDistanceCached;
            }
        }
        private float? _totalPathDistanceCached;

        /// <summary>
        /// Total time required to walk throught the path
        /// </summary>
        private float _totalTimeRequired
        {
            get { return _totalPathDistance / _speed; }
        }

        private Rigidbody2D _rigidbody;

        private float _maxHealth;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _maxHealth = _health;
        }

        void Update()
        {
            if (_pathTween is object && _pathTween.IsActive() && _pathTween.IsPlaying())
            {
                Vector2 lookAt = GetPosition(0.01f);
                transform.up = new Vector3(lookAt.x, lookAt.y, transform.position.z) - transform.position;
            }
        }

        public void ReceiveDamage(float health)
        {
            _health -= health;

            if (_health <= 0)
                Die(false);

            if (!IsDead)
                UpdateAlpha();
        }

        /// <summary>
        /// Set walking path
        /// </summary>
        public void SetPath(List<Vector2> path)
        {
            _path = path;

            // Purge cache
            _totalPathDistanceCached = null;

            InitPathTween();
        }

        /// <summary>
        /// Start walking on the path
        /// </summary>
        public void StartWalking()
        {
            _pathTween.Play();
        }

        /// <summary>
        /// Stop walking on the path
        /// </summary>
        public void StopWalking()
        {
            _pathTween.Pause();
        }

        /// <summary>
        /// Get future position after a duration
        /// </summary>
        public Vector2 GetPosition(float afterDuration)
        {
            return _pathTween.PathGetPoint(_pathTween.ElapsedPercentage() + afterDuration / _totalTimeRequired);
        }

        /// <summary>
        /// Setup the tween when setting tha path vectors
        /// </summary>
        private void InitPathTween()
        {
            // If there's an exisiting tween, kill
            if (_pathTween is object)
                _pathTween.Kill();

            // NOTE: I can't do .SetLookAt to rotate the enemy because DOTween reported a casting error.
            // Seems like an internal bug. Use manual Update() instead.
            _pathTween = _rigidbody
                .DOPath(_path.ToArray(), _totalTimeRequired, PathType.Linear, PathMode.TopDown2D)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _pathTween.Kill();
                    _pathTween = null;
                    Die(true);
                })
                .Pause();
        }

        /// <summary>
        /// Handle enemy dead when health reaches <= 0
        /// </summary>
        /// <param name="successReachEnd">Whether the enemy reaches the end of path</param>
        private void Die(bool successReachEnd)
        {
            if (IsDead) return;
            IsDead = true;

            StopWalking();
            _pathTween.Kill();
            Destroy(gameObject);

            if (successReachEnd)
                GameManager.instance.DecrementHealth();
            else
                GameManager.instance.AddGold(_gold);
        }

        /// <summary>
        /// Adjust sprite alpha based on health
        /// </summary>
        private void UpdateAlpha()
        {
            Color spriteColor = _sprite.color;
            spriteColor.a = _health / _maxHealth;
            _sprite.color = spriteColor;
        }
    }
}
