using UnityEngine;

namespace Mechanizer
{
    public class PlayerRootUI : MonoBehaviour
    {
        private PlayerChildUI[] _children;
        [SerializeField] private PlayerEntity _context;
        private void OnEnable()
        {
            _children = GetComponentsInChildren<PlayerChildUI>();
            for (int i = 0; i < _children.Length; i++)
            {
                var child = _children[i];
                child.Context = _context;
            }
        }
    }
}
