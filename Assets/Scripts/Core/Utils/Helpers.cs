using UnityEngine;

namespace Mechanizer
{
    public static class Helpers
    {
        private static CameraFollow _mainCameraFollow;
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

        public static CameraFollow MainCameraFollow
        {
            get
            {
                if (_mainCameraFollow == null)
                    _mainCameraFollow = MainCamera.GetComponent<CameraFollow>();
                return _mainCameraFollow;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            _camera = null;
        }
    }
}
