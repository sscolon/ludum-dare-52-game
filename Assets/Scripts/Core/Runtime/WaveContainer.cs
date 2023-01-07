using UnityEngine;

namespace Mechanizer
{
    public class WaveContainer : MonoBehaviour
    {
        [SerializeField] private Wave _wave;
        public Wave Wave { get => _wave; }
    }
}
