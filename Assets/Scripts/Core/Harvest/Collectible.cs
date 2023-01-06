using UnityEngine;

namespace Mechanizer
{
    public class Collectible : MonoBehaviour
    {
        private bool _isCollected;
        private StateAnimator _animator;
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _collectibleTag;
        public Sprite Icon { get => _icon; set => _icon = value; }
        public string CollectibleTag { get => _collectibleTag; set => _collectibleTag = value; }
        public bool IsCollected { get => _isCollected; set => _isCollected = value; }
        private void OnEnable()
        {
            if(_animator == null)
                _animator = new StateAnimator(GetComponent<Animator>());
        }

        public void Collect()
        {
            _animator.Play("collect");
        }
    }
}