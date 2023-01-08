using System.Collections;
using UnityEngine;

namespace Mechanizer
{
    public class PlayerRootUI : MonoBehaviour
    {
        private PlayerChildUI[] _children;
        [SerializeField] private PlayerEntity _context;
        [SerializeField] private RectTransform _childrenParent;
        [SerializeField] private float _spawnSpeed=2f;
        private void OnEnable()
        {
            _children = GetComponentsInChildren<PlayerChildUI>();
            for (int i = 0; i < _children.Length; i++)
            {
                var child = _children[i];
                child.Context = _context;
            }
        }

        private void Start()
        {
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            float time = 0.0f;
            while(time < 1.0f)
            {
                time += Time.deltaTime * _spawnSpeed;
                _childrenParent.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time);
                yield return null;
            }
        }
    }
}
