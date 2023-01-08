using UnityEngine;

namespace Mechanizer
{
    [RequireComponent(typeof(Throwable))]
    public class FireBurster : MonoBehaviour
    {
        private Throwable _throwable;
        [SerializeField] private FireActor _firePrefab;
        [SerializeField] private float _blastRadius;
        [SerializeField] private int _minFireCount;
        [SerializeField] private int _maxFireCount;
        private void OnEnable()
        {
            if (_throwable == null)
                _throwable = GetComponent<Throwable>();
            _throwable.OnDeath += CreateFire;
        }
        private void OnDisable()
        {
            _throwable.OnDeath -= CreateFire;
        }

        private void CreateFire()
        {
            int fireCount = Random.Range(_minFireCount, _maxFireCount);
            for (int i = 0; i < fireCount; i++)
            {
                float x = Random.Range(-_blastRadius, _blastRadius);
                float y = Random.Range(-_blastRadius, _blastRadius);
                Vector3 offset = new Vector3(x, y, 0);
                Vector3 fireSpawnPoint = transform.position + offset;
                Instantiate(_firePrefab, fireSpawnPoint, _firePrefab.transform.rotation);
            }
        }
    }
}