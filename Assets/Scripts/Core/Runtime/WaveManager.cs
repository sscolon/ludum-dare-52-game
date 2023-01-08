using System;
using UnityEngine;

namespace Mechanizer
{
    public class WaveManager : MonoBehaviour
    {
        private int _waveIndex;
        private int _requiredKills;
        private Wave _currentWave;
        [SerializeField] private WaveData _waveData;

        public int WaveIndex { get => _waveIndex; }
        public int RequiredKills { get => _requiredKills; }

        public int Difficulty { get; private set; } = 0;

        public event Action OnWaveComplete;
        public void NextWave(Rand rand)
        {
            _waveIndex++;
            _currentWave = _waveData.NextWave(rand, Difficulty);
            Debug.Log(_currentWave);
            _requiredKills = _currentWave.GetEnemyCount();

            for (int i = 0; i < _currentWave.spawns.Count; i++)
            {
                var spawn = _currentWave.spawns[i];
                Vector2 rootPosition = transform.root.position;
                Vector2 position = rootPosition + spawn.position;
                GameObject instance = GameObject.Instantiate(spawn.prefab, position, spawn.prefab.transform.rotation);
                if (instance.TryGetComponent(out Enemy enemy))
                {
                    enemy.Health.OnDeath += DecrementEnemyKill;
                    void DecrementEnemyKill()
                    {
                        _requiredKills--;
                        if(_requiredKills <= 0)
                        {
                            OnWaveComplete?.Invoke();
                        }
                        enemy.Health.OnDeath -= DecrementEnemyKill;
                    }
                }
            }
            Difficulty += 1;
        }
    }
}