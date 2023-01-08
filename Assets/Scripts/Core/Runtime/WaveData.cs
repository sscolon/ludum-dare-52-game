using System.Collections.Generic;
using UnityEngine;

namespace Mechanizer
{
    [CreateAssetMenu(menuName = "Mechanizer/Wave Data")]
    public class WaveData : ScriptableObject
    {
        [SerializeField] private List<WaveContainer> _waveContainers;
        public void AddWaveContainer(WaveContainer container)
        {
            _waveContainers.Add(container);
        }

        public Wave NextWave(Rand rand, int difficulty)
        {
            List<Wave> waves = new List<Wave>();
            for(int i = 0; i < _waveContainers.Count; i++)
            {
                Wave w = _waveContainers[i].Wave;
                if (w.difficulty <= difficulty)
                    waves.Add(w);
            }
            Wave wave = rand.Next(waves.ToArray());
            return wave;
        }
    }
}
