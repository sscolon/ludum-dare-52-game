using UnityEngine;

namespace Mechanizer
{
    public class GameManager : MonoBehaviour
    {
        private int _score;
        private Rand _rand;
        [SerializeField] private int _seed;
        [SerializeField] private WaveManager _waveManager;
        private static GameManager _gameManager;
        public int Score { get => _score; set => _score = value; }
        private void Awake()
        {
            if (_gameManager == null)
            {
                _gameManager = this;
                DontDestroyOnLoad(this);
            }
            else if (_gameManager != this)
            {
                Destroy(this.gameObject);
            }
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
    }
}