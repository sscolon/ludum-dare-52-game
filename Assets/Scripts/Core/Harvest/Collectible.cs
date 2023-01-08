using System;
using System.Collections;
using UnityEngine;

namespace Mechanizer
{
    public class Collectible : MonoBehaviour, IWeighted
    {
        private bool _isCollected;
        [Header("Components")]
        [SerializeField] private Transform _heightTransform;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Material _spriteDefaultMaterial;
        [SerializeField] private Material _spriteWhiteMaterial;

        [Header("Collectible Stats")]
        [SerializeField] private AudioClip _spawnStartSound;
        [SerializeField] private AudioClip _spawnFinishSound;

        [SerializeField] private Sprite _icon;
        [SerializeField] private int _collectibleId;
        [SerializeField] private float _spawnHeight = 32f;
        [SerializeField] private float _spawnSpeed = 3f;
        [SerializeField] private float _collectSpeed = 3f;
        [SerializeField] private int _weight;

        public Sprite Icon { get => _icon; set => _icon = value; }
        public int CollectibleId { get => _collectibleId; set => _collectibleId = value; }
        public bool IsCollected { get => _isCollected; set => _isCollected = value; }
        public bool HasSpawned { get; private set; }

        public event Action OnSpawn;
        public event Action OnCollect;
        private void Start()
        {
            HasSpawned = false;
            StartCoroutine(SpawnRoutine());
        }

        public void Collect()
        {
            StartCoroutine(CollectRoutine());
        }
        private void PlaySound(AudioClip audioClip)
        {
            SoundSettings settings = SoundSettings.CreateDefaultSound(audioClip);
            settings.useVolumeLoss = true;
            settings.transform = transform;
            SoundManager.PlaySound(settings);
        }
        private IEnumerator SpawnRoutine()
        {
            OnSpawn?.Invoke();
            PlaySound(_spawnStartSound);
            _spriteRenderer.transform.localScale = Vector3.one;
            _spriteRenderer.material = _spriteDefaultMaterial;
            float time = 0f;
            while (time < 1.0f)
            {
                time += Time.deltaTime * _spawnSpeed;
                float height = Mathf.Lerp(_spawnHeight, 0, time);
                _heightTransform.localPosition = new Vector3(_heightTransform.localPosition.x, height);
                yield return null;
            }
            PlaySound(_spawnFinishSound);
            HasSpawned = true;
        }

        private IEnumerator CollectRoutine()
        {
            OnCollect?.Invoke();
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