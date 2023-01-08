using UnityEngine;

namespace Mechanizer
{
    public class GameCursor : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _trailRenderer;
        [SerializeField] private SpriteRenderer _cursorRenderer;
        private void Start()
        {
            Cursor.visible = false;
        }

        private void LateUpdate()
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldMousePosition = Helpers.MainCamera.ScreenToWorldPoint(mousePosition);
            worldMousePosition.z = 0.0f;

            _cursorRenderer.transform.position = worldMousePosition;
            float distance = Vector3.Distance(_trailRenderer.transform.position, _cursorRenderer.transform.position);
            _trailRenderer.size = new Vector2(distance, _trailRenderer.size.y);
            _trailRenderer.transform.rotation = Helpers.LookAt(_cursorRenderer.transform, _trailRenderer.transform);
        }
    }
}
