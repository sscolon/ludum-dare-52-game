using UnityEngine;

namespace Mechanizer
{
    public class StateAnimator
    {
        private Animator _animator;
        private string _animation;
        public StateAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void Play(string animation)
        {
            if (_animation == animation)
                return;

            _animator.Play(animation);
            _animation = animation;
        }
    }
}
