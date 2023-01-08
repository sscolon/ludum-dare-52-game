using UnityEngine;

namespace Mechanizer
{
    [RequireComponent(typeof(IDamageable))]
    public class CollectibleDeathSpawner : MonoBehaviour
    {
        private IDamageable _damageable;
        [SerializeField] private float _radius;
        [SerializeField] private int _min;
        [SerializeField] private int _max;
        [SerializeField] private CollectibleData _collectibleData;
        private void OnEnable()
        {
            if (_damageable == null)
                _damageable = GetComponent<IDamageable>();
            _damageable.Health.OnDeath += SpawnCollectibles;
        }

        private void OnDisable()
        {
            _damageable.Health.OnDeath -= SpawnCollectibles;
        }

        private void SpawnCollectibles()
        {
            int count = Random.Range(_min, _max);
            LayerMask mask = LayerMask.GetMask("Wall");
            for (int i = 0; i < count; i++)
            {
                Collectible collectible = _collectibleData.NextCollectible(GameManager.Main.Rand);
                float x = Random.Range(-_radius, _radius);
                float y = Random.Range(-_radius, _radius);
                Vector3 offset = new Vector3(x, y, 0);
                Vector3 position = transform.position + offset;
                var hit = Physics2D.Raycast(transform.position, (position - transform.position).normalized, Vector3.Distance(transform.position, position), mask);
                if (hit)
                {
                    position = hit.point;
                }
                
                Instantiate(collectible, position, collectible.transform.rotation);
            }
        }
    }
}
