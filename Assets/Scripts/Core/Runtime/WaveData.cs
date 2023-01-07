using UnityEngine;

namespace Mechanizer
{
    [CreateAssetMenu(menuName = "Mechanizer/Wave Data")]
    public class WaveData : ScriptableObject
    {
        private Wave[] _wavesBacking;
        private Wave[] _waves 
        { 
            get        
            {
                if(_wavesBacking == null)
                {
                    _wavesBacking = new Wave[_waveContainers.Length];
                    for(int i = 0; i < _waveContainers.Length; i++)
                    {
                        var container = _waveContainers[i];
                        _wavesBacking[i] = container.Wave;
                    }
                }
                return _wavesBacking;
            } 
        }

        [SerializeField] private WaveContainer[] _waveContainers;
        public Wave NextWave(Rand rand)
        {
            Wave wave = rand.Next(_waves);
            return wave;
        }
    }
}
