using System.Collections.Generic;
using UnityEngine;

namespace Mechanizer
{
    [CreateAssetMenu(menuName = "Mechanizer/Wave Data")]
    public class WaveData : ScriptableObject
    {
        private Wave[] _waves;

        [SerializeField] private List<WaveContainer> _waveContainers;
        public void Init()
        {
            _waves = new Wave[_waveContainers.Count];
            for (int i = 0; i < _waveContainers.Count; i++)
            {
                _waves[i] = _waveContainers[i].Wave;
            }
        }

        public void AddWaveContainer(WaveContainer container)
        {
            _waveContainers.Add(container);
        }

        public Wave NextWave(Rand rand, int difficulty)
        {
            List<Wave> waves = new List<Wave>();
            for(int i = 0; i < _waves.Length; i++)
            {
                Wave w = _waves[i];
                if (w.difficulty <= difficulty)
                    waves.Add(w);
            }
            Wave wave = rand.Next(waves.ToArray());
            return wave;
        }
    }
}
