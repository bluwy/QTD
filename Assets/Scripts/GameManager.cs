using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace QTD
{
    [RequireComponent(typeof(AudioSource))]
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
        private AudioClip _menuMusic;

        [SerializeField]
        private AudioClip _gameMusic;

        [SerializeField]
        private List<Wave> _waves = new List<Wave>();

        private bool paused = false;

        [HideInInspector]
        public UnityEvent<int> goldChangeEvent = new UnityEvent<int>();

        private bool _isGameStarted = false;
        private bool _isGameEnded = false;
        private bool _isResettingGame = false;
        private bool _isSpawnFinish = false;

        private AudioSource _audioSource;

        void Awake()
        {
            if (instance == null)
                instance = this;

            Time.timeScale = 1;
            Application.targetFrameRate = 60;

            _audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            // Init UI
            UIManager.instance.ShowMainMenu();
            UIManager.instance.SetHealthText(_health.ToString());
            UIManager.instance.SetGoldText(_gold.ToString());
            PlayMenuMusic();
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

        /// <summary>
        /// Restart the game by reloading the scene
        /// </summary>
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
            PlayGameMusic();
            StartCoroutine(Spawn());
        }

        public void EndGame()
        {
            _isGameEnded = true;
            StopAllCoroutines();
            Time.timeScale = 0;
            _audioSource.Stop();
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

        /// <summary>
        /// Spawn waves
        /// </summary>
        private IEnumerator Spawn()
        {
            // Wait before start spawning
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

        private void PlayMenuMusic()
        {
            _audioSource.Stop();
            _audioSource.clip = _menuMusic;
            _audioSource.Play();
        }

        private void PlayGameMusic()
        {
            _audioSource.Stop();
            _audioSource.clip = _gameMusic;
            _audioSource.Play();
        }
    }
}
