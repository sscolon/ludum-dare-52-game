using System;
using System.Collections;
using UnityEngine;

namespace Mechanizer
{
    public class GameManager : MonoBehaviour
    {
        private int _score;
        private Rand _rand;
        private Coroutine _scoreRoutine;
        private PlayerEntity _player;
        [SerializeField] private int _seed;
        [SerializeField] private float _scoreTimeRate;
        [SerializeField] private int _scoreTimeBonus;
        [SerializeField] private WaveManager _waveManager;
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private DeathManager _deathManager;

        public Rand Rand { get => _rand; }
        public int Seed { get => _seed; }
        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
                OnScoreChanged?.Invoke(_score);
            }
        }

        public event Action<int> OnScoreChanged;
        public static GameManager Main { get; private set; }
        private void Awake()
        {
            if (Main == null)
            {
                Main = this;
            }
            else if (Main != this)
            {
                Destroy(this.gameObject);
            }
            NewGame();
        }

        private void OnEnable()
        {
            _waveManager.OnWaveComplete += NextWaveRoutine;
        }

        private void OnDisable()
        {
            _waveManager.OnWaveComplete -= NextWaveRoutine;
        }


        public void NewGame()
        {
            _rand = new Rand(_seed);
            
            var player = _playerManager.NewPlayer();
            player.transform.position = Vector3.zero;
            _player = player.GetComponentInChildren<PlayerEntity>();
            _player.Health.OnDeath += HandleDeath;
            _scoreRoutine = StartCoroutine(IncreaseScoreRoutine());
            //Incase we want to override seed.
            Debug.Log($"Seed: {_rand.Seed}");
            NextWaveRoutine();
        }

        private void NextWaveRoutine()
        {
            //We're gonna have a cooldown to the next wave later.
            //I'm thinking like 2 or 3 seconds. That'd be good.
            _waveManager.NextWave(_rand);
        }

        private void HandleDeath()
        {
            StopCoroutine(_scoreRoutine);
            _deathManager.Activate(this);
        }

        private IEnumerator IncreaseScoreRoutine()
        {
            float time = 0f;
            while (true)
            {
                time += Time.deltaTime;
                int difficultyMultiplier = Mathf.Clamp(_waveManager.Difficulty, 0, 10);
                if(time >= _scoreTimeRate)
                {
                    time = 0f;
                    Score += _scoreTimeBonus * difficultyMultiplier;
                }
                yield return null;
            }
        }
    }
}