using UnityEngine;

namespace Mechanizer
{
    public class AfterImage : MonoBehaviour
    {
        private float _time;
        private float _timeBetweenTrails;

        [Header("References")]
        [SerializeField] private GameObject _trailPrefab;
        [SerializeField] private SpriteRenderer _spriteToMimick;
        [SerializeField] private Rigidbody2D _referenceBody;
        [SerializeField] private float _velocityThreshold = 20f;

        [Header("After Images")]
        [Tooltip("The amount of after images to create per second.")]
        [Range(0, 60)]
        [SerializeField] private float _afterImagesPerSecond = 10f;

        [Tooltip("The time in seconds it should take for an individual trail to finish fading.")]
        [SerializeField] private float _afterImageFadeTime = 0.25f;

        [Tooltip("The starting alpha of each individual trail piece.")]
        [SerializeField] private float _afterImageStartingAlpha = 0.5f;
        private void Start()
        {
            _timeBetweenTrails = 1f / _afterImagesPerSecond;
        }

        private void Update()
        {
            if (_referenceBody.velocity.magnitude < _velocityThreshold)
            {
                return;
            }

            _time += Time.deltaTime;
            if (_time >= _timeBetweenTrails)
            {
                _time = 0f;
                CreateTrail();
            }
        }

        private void CreateTrail()
        {
            GameObject trailObj = Instantiate(_trailPrefab);
            trailObj.gameObject.hideFlags = HideFlags.HideInHierarchy;

            AfterImageSprite trail = trailObj.GetComponent<AfterImageSprite>();
            trail.spriteRenderer.color = new Color(_spriteToMimick.color.r, _spriteToMimick.color.g, _spriteToMimick.color.b, _afterImageStartingAlpha);
            trail.spriteRenderer.sprite = _spriteToMimick.sprite;

            trail.spriteRenderer.flipX = _spriteToMimick.flipX;
            trail.spriteRenderer.flipY = _spriteToMimick.flipY;
            trail.spriteRenderer.sortingLayerID = _spriteToMimick.sortingLayerID;

            trail.transform.position = _spriteToMimick.transform.position;
            trail.transform.rotation = _spriteToMimick.transform.rotation;
            trail.transform.localScale = _spriteToMimick.transform.lossyScale;
            trail.StartFade(_afterImageStartingAlpha, _afterImageFadeTime);
        }
    }
}
