using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace QTD
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        [Header("Main Menu")]
        [SerializeField]
        private GameObject _menuCanvas;

        [Header("Game")]
        [SerializeField]
        private GameObject _gameCanvas;

        [SerializeField]
        private GameObject _gamePausePanel;

        [SerializeField]
        private GameObject _gameOverPanel;

        [SerializeField]
        private TextMeshProUGUI _gameOverText;

        [Header("Gameplay")]
        [SerializeField]
        private TextMeshProUGUI _waveText;

        private float _showWaveDuration = 3f;

        [SerializeField]
        private TextMeshProUGUI _healthText;

        [SerializeField]
        private TextMeshProUGUI _goldText;

        [SerializeField]
        private GameObject _towerSelects;

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        public void ShowMainMenu()
        {
            _menuCanvas.SetActive(true);
            _gameCanvas.SetActive(false);
        }

        public void ShowGamePlay()
        {
            _menuCanvas.SetActive(false);
            _gameCanvas.SetActive(true);
            _gamePausePanel.SetActive(false);
            _gameOverPanel.SetActive(false);
        }

        public void ShowGamePause()
        {
            _menuCanvas.SetActive(false);
            _gameCanvas.SetActive(true);
            _gamePausePanel.SetActive(true);
            _gameOverPanel.SetActive(false);
        }

        public void ShowGameEnd(string gameOverText)
        {
            _menuCanvas.SetActive(false);
            _gameCanvas.SetActive(true);
            _gamePausePanel.SetActive(false);
            _gameOverPanel.SetActive(true);
            _gameOverText.text = gameOverText;
        }

        public void ShowTowerSelects(bool isPlacing)
        {
            _towerSelects.SetActive(!isPlacing);
        }

        public void SetHealthText(string text)
        {
            _healthText.text = text;
        }

        public void SetGoldText(string text)
        {
            _goldText.text = text;
        }

        public void ShowWave(string text)
        {
            _waveText.text = text;
            StopAllCoroutines();
            StartCoroutine(ShowWaveAnimation());
        }

        private IEnumerator ShowWaveAnimation()
        {
            _waveText.gameObject.SetActive(true);

            yield return new WaitForSeconds(_showWaveDuration);

            _waveText.gameObject.SetActive(false);
        }
    }
}
