using System.Collections;
using UnityEngine;

namespace Mechanizer
{
    public class FireActor : Attack
    {
        private Coroutine _routine;
        [Header("Fire Attributes")]
        [SerializeField] private float _spawnSpeed;
        [SerializeField] private float _despawnSpeed;
        [SerializeField] private float _tickRate;
        [SerializeField] private float _duration;
        private void Start()
        {
            DoRoutine(SpawnRoutine());
        }

        private void DoRoutine(IEnumerator newRoutine)
        {
            if (_routine != null)
                StopCoroutine(_routine);
            _routine = StartCoroutine(newRoutine);
        }

        private IEnumerator SpawnRoutine()
        {
            float spawnTime = 0f;
            while (spawnTime < 1.0f)
            {
                spawnTime += Time.deltaTime * _spawnSpeed;
                Vector3 scale = Vector3.Lerp(Vector3.zero, Vector3.one, spawnTime);
                transform.localScale = scale;
                yield return null;
            }

            DoRoutine(TickRoutine());
        }

        private IEnumerator TickRoutine()
        {
            float tickTime = 0f;
            float time = 0f;
            while(time < _duration)
            {
                tickTime += Time.deltaTime;
                time += Time.deltaTime;
                if (tickTime >= _tickRate)
                {
                    Perform();
                    tickTime = 0f;
                }
                yield return null;
            }

            DoRoutine(DeSpawnRoutine());
        }

        private IEnumerator DeSpawnRoutine()
        {
            float spawnTime = 0f;
            while (spawnTime < 1.0f)
            {
                spawnTime += Time.deltaTime * _despawnSpeed;
                Vector3 scale = Vector3.Lerp(Vector3.one, Vector3.zero, spawnTime);
                transform.localScale = scale;
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}