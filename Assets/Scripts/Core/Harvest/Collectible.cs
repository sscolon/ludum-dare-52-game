using System.Collections;
using UnityEngine;

namespace Mechanizer
{
    public class Collectible : MonoBehaviour, IWeighted
    {
        private bool _isCollected;
        [Header("Components")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Material _spriteDefaultMaterial;
        [SerializeField] private Material _spriteWhiteMaterial;

        [Header("Collectible Stats")]
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _collectibleTag;
        [SerializeField] private float _spawnSpeed = 3f;
        [SerializeField] private float _collectSpeed = 3f;
        [SerializeField] private int _weight;
        public Sprite Icon { get => _icon; set => _icon = value; }
        public string CollectibleTag { get => _collectibleTag; set => _collectibleTag = value; }
        public bool IsCollected { get => _isCollected; set => _isCollected = value; }

        private void Start()
        {
            StartCoroutine(SpawnRoutine());
        }

        public void Collect()
        {
            StartCoroutine(CollectRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            _spriteRenderer.transform.localScale = Vector3.zero;
            _spriteRenderer.material = _spriteDefaultMaterial;

            Vector3 startPos = Vector3.zero;
            Vector3 midPos = new Vector3(0f, 2f, 0f);
            Vector3 endPos = SpriteUtility.GetLocalCenter(_spriteRenderer);

            Vector3 startScale = Vector3.zero;
            Vector3 midScale = new Vector3(2f, 2f, 1f);
            Vector3 endScale = Vector3.one;
            float time = 0f;
            while (time < 1f)
            {
                time += Time.deltaTime * +_spawnSpeed;

                Vector3 v1 = VectorUtility.SmoothStep(startPos, midPos, time);
                Vector3 v2 = VectorUtility.SmoothStep(midPos, endPos, time);
                Vector3 arc = VectorUtility.SmoothStep(v1, v2, time);

                Vector3 s1 = VectorUtility.SmoothStep(startScale, midScale, time);
                Vector3 s2 = VectorUtility.SmoothStep(midScale, endScale, time);
                Vector3 scale = VectorUtility.SmoothStep(s1, s2, time);

                _spriteRenderer.transform.localScale = scale;
                _spriteRenderer.transform.localPosition = arc;
                yield return null;
            }
        }

        private IEnumerator CollectRoutine()
        {
            _spriteRenderer.material = _spriteWhiteMaterial;
            Vector3 targetSize = new Vector3(3f, 0f, 0f);
            float time = 0f;
            while (time < 1f)
            {
                time += Time.deltaTime * _collectSpeed;
                Vector3 scale = VectorUtility.SmoothStep(Vector3.one, targetSize, time);
                _spriteRenderer.transform.localScale = scale;
                yield return null;
            }

            Destroy(gameObject);
        }

        public int GetWeight()
        {
            return _weight;
        }
    }
}