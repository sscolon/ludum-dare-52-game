using UnityEngine;

namespace Mechanizer
{
    [CreateAssetMenu(menuName = "Mechanizer/Wave Data")]
    public class WaveData : ScriptableObject
    {
        [SerializeField] private Wave[] _waves;
        public Wave NextWave(Rand rand)
        {
            Wave wave = rand.Next(_waves);
            return wave;
        }
    }
}
