using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using QTD;
using QTD.Towers;

namespace QTD.UI
{
    [RequireComponent(typeof(Button))]
    public class TowerSelect : MonoBehaviour
    {
        [SerializeField]
        private GameObject _towerToSelect;

        [SerializeField]
        private Sprite _towerSprite;

        [SerializeField]
        private Image _image;

        [SerializeField]
        private TextMeshProUGUI _costText;

        private Button _button;

        private int _towerCost;

        void Awake()
        {
            _button = GetComponent<Button>();
            _towerCost = _towerToSelect.GetComponent<Tower>().InitialCost;
        }

        void Start()
        {
            _image.sprite = _towerSprite;
            _costText.text = _towerCost.ToString();
        }

        void OnEnable()
        {
            GameManager.instance.goldChangeEvent.AddListener(CheckGoldSufficient);
        }

        void OnDisable()
        {
            GameManager.instance.goldChangeEvent.RemoveListener(CheckGoldSufficient);
        }

        /// <summary>
        /// This will be called by button component "OnClick"
        /// </summary>
        public void HandleButtonClick()
        {
            if (GameManager.instance.IsGoldSufficient(_towerCost))
                TowerManager.instance.SelectTower(_towerToSelect);
        }

        private void CheckGoldSufficient(int gold)
        {
            _button.interactable = _towerCost <= gold;
        }
    }
}
