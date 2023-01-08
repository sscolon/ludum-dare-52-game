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

        public static Quaternion LookAt(Transform target, Transform transform)
        {
            Vector3 dir = target.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public static Quaternion LookAt(Vector3 target, Transform transform)
        {
            Vector3 dir = target - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
