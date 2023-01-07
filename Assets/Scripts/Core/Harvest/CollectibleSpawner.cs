using UnityEngine;

namespace Mechanizer
{
    public class CollectibleSpawner : MonoBehaviour
    {
        private float _time;
        private Rand _rand;
        [SerializeField] private int _minX;
        [SerializeField] private int _maxX;
        [SerializeField] private int _minY;
        [SerializeField] private int _maxY;
        [SerializeField] private float _timeBetweenSpawns;
        [SerializeField] private CollectibleData _collectibleData;
        public float TimeBetweenSpawns { get => _timeBetweenSpawns; set => _timeBetweenSpawns = value; }
        private void Start()
        {
            _rand = new Rand(GameManager.Main.Seed);
        }

        private void Update()
        {
            _time += Time.deltaTime;
            if (_time >= TimeBetweenSpawns)
            {
                _time = 0f;
                Spawn();
            }
        }

        private void Spawn()
        {
            Collectible collectible = _collectibleData.NextCollectible(_rand);
            int x = _rand.Next(_minX, _maxX);
            int y = _rand.Next(_minY, _maxY);
            Vector2 spawnPoint = new Vector2(x + 0.5f, y + 0.5f);
            Instantiate(collectible, spawnPoint, collectible.transform.rotation);
        }
    }
}
