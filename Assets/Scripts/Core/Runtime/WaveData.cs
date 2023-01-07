using UnityEngine;

namespace Mechanizer
{
    [CreateAssetMenu(menuName = "Mechanizer/Wave Data")]
    public class WaveData : ScriptableObject
    {
        private Wave[] _waves;

        [SerializeField] private WaveContainer[] _waveContainers;
        private void OnEnable()
        {
    _waves = new Wave[_waveContainers.Length];
            for(int i = 0; i < _waveContainers.Length; i++)
            {
                _waves[i] = _waveContainers[i].Wave;
            }
        }

        public Wave NextWave(Rand rand)
        {
            Wave wave = rand.Next(_waves);
            return wave;
        }
    }
}
