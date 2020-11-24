using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace QTD
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [SerializeField]
        private int _health = 3;

        [SerializeField]
        private int _gold = 100;

        [SerializeField]
        [Tooltip("Time in seconds before enemies start spawning")]
        private int _timeBeforeSpawn = 10;

        [SerializeField]
        private List<Wave> _waves = new List<Wave>();

        private bool paused = false;

        [HideInInspector]
        public UnityEvent<int> goldChangeEvent = new UnityEvent<int>();

        private bool _isGameStarted = false;
        private bool _isGameEnded = false;
        private bool _isResettingGame = false;
        private bool _isSpawnFinish = false;

        void Awake()
        {
            if (instance == null)
                instance = this;

            Time.timeScale = 1;
            Application.targetFrameRate = 60;
        }

        void Start()
        {
            // Init UI
            UIManager.instance.ShowMainMenu();
            UIManager.instance.SetHealthText(_health.ToString());
            UIManager.instance.SetGoldText(_gold.ToString());
        }

        void Update()
        {
            if (_isGameEnded)
            {
                if (Input.anyKeyDown)
                    ResetGame();
            }
            else if (_isSpawnFinish && FindObjectOfType<Enemy>() is null)
            {
                EndGame();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (paused)
                    UnpauseGame();
                else
                    PauseGame();
            }
        }

        public void ResetGame()
        {
            if (_isResettingGame) return;

            _isResettingGame = true;
            // Disable all tweens, else might throw null reference
            DOTween.KillAll();
            // Reload the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void StartGame()
        {
            _isGameStarted = true;
            UIManager.instance.ShowGamePlay();
            StartCoroutine(Spawn());
        }

        public void EndGame()
        {
            _isGameEnded = true;
            StopAllCoroutines();
            Time.timeScale = 0;
            UIManager.instance.ShowGameEnd(_health <= 0 ? "GAME OVER" : "WIN!");
        }

        public void PauseGame()
        {
            if (paused) return;

            paused = true;
            Time.timeScale = 0;
            UIManager.instance.ShowGamePause();
        }

        public void UnpauseGame()
        {
            if (!paused) return;

            paused = false;
            Time.timeScale = 1;
            UIManager.instance.ShowGamePlay();
        }

        public void ChangeGameSpeed(float to)
        {
            // TODO: UI
            Time.timeScale = to;
        }

        public void DecrementHealth()
        {
            _health--;
            UIManager.instance.SetHealthText(_health.ToString());
            CheckHealth();
        }

        public void AddGold(int by)
        {
            _gold += by;
            UIManager.instance.SetGoldText(_gold.ToString());
            goldChangeEvent.Invoke(_gold);
        }

        public void UseGold(int by)
        {
            _gold -= by;
            UIManager.instance.SetGoldText(_gold.ToString());
            goldChangeEvent.Invoke(_gold);
        }

        public bool IsGoldSufficient(int amount)
        {
            return amount <= _gold;
        }

        private IEnumerator Spawn()
        {
            yield return new WaitForSeconds(_timeBeforeSpawn);

            for (int i = 0; i < _waves.Count; i++)
            {
                string waveName = "Wave " + (i + 1).ToString() + ": " + _waves[i].Name;
                UIManager.instance.ShowWave(waveName);
                yield return StartCoroutine(_waves[i].Spawn(this));
            }

            _isSpawnFinish = true;
        }

        private void CheckHealth()
        {
            if (_health <= 0)
                EndGame();
        }
    }
}
