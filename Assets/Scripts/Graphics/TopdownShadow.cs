using UnityEngine;

namespace Mechanizer
{
    public class TopdownShadow : MonoBehaviour
    {
        private Vector3 _size;
        private float _startY;
        [SerializeField] private Transform _referenceTransform;
        private void OnEnable()
        {
            _startY = _referenceTransform.transform.localPosition.y;
            _size = Vector3.one;
        }

        private void LateUpdate()
        {
            if (_referenceTransform == null)
            {
                return;
            }
            float distance = Mathf.Abs(_referenceTransform.localPosition.y) - Mathf.Abs(_startY) + 1;
            transform.localScale = _size / distance;
        }

        public void SetDefaultY(float y)
        {
            _startY = y;
        }
    }
}
