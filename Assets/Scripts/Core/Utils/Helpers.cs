using UnityEngine;

namespace Mechanizer
{
    public static class Helpers
    {
        private static Camera _camera;
        public static Camera MainCamera
        {
            get
            {
                if (_camera == null)
                    _camera = Camera.main;
                return _camera;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            _camera = null;
        }
    }
}
