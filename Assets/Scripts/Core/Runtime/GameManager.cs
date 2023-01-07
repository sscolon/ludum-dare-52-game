using UnityEngine;

namespace Mechanizer
{
    public class GameManager : MonoBehaviour
    {
        private int _score;
        private Rand _rand;
        [SerializeField] private int _seed;
        [SerializeField] private WaveManager _waveManager;
        public int Score { get => _score; set => _score = value; }
        public static GameManager Main { get; private set; }
        private void Awake()
        {
            if (Main == null)
            {
                Main = this;
                DontDestroyOnLoad(this);
            }
            else if (Main != this)
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

        private void Start()
        {
            //THIS IS TEMPORARY, JUST FOR TESTING PURPOSES>
            NewGame();
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