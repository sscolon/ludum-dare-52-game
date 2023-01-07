using UnityEngine;

namespace Mechanizer
{
    public class CameraFollow : MonoBehaviour
    {
        private float _shakeStrength;
        private float _shakeTime;
        private float _shakeDuration;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Vector3 _targetPosition;
        [SerializeField] private Transform _target;
        public Transform Target { get => _target; set => _target = value; }
        public void Shake(float strength, float duration)
        {
            _shakeStrength = strength;
            _shakeTime = duration;
            _shakeDuration = duration;
        }

        private void UpdateShake()
        {
            _shakeTime -= Time.deltaTime;
            float max = _shakeStrength * (_shakeTime / _shakeDuration);
            float x = Random.Range(-max, max);
            float y = Random.Range(-max, max);
            Vector3 offset = new Vector3(x, y, 0);
            transform.position += offset;
        }

        private void LateUpdate()
        {
            if (Target)
                _targetPosition = Target.position;
            transform.position = _targetPosition + _offset;
            UpdateShake();
        }
    }
}
