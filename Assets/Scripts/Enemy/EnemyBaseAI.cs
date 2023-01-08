using System.Collections;
using UnityEngine;

namespace Mechanizer
{
    public abstract class EnemyBaseAI : MonoBehaviour
    {
        private bool _hasSpawned;
        [Header("Spawning (Base)")]
        [SerializeField] private Transform _heightTransform;
        [SerializeField] private float _spawnHeight = 32;
        [SerializeField] private float _spawnSpeed;
        [SerializeField] private AudioClip _spawnStartSound;
        [SerializeField] private AudioClip _spawnFinishSound;
        public StateAnimator Animator { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }

        public GameObject Target { get; private set; }
        public float DistanceToTarget { get; private set; }
        public Vector3 DirectionToTarget { get; private set; }
        protected virtual void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = new StateAnimator(GetComponent<Animator>());
            Target = GameObject.FindGameObjectWithTag("Player");
        
            StartCoroutine(SpawnRoutine());
        }

        private void Update()
        {
            if (!_hasSpawned || Target == null)
            {
                Animator.Play("idle");
                return;
            }

            DirectionToTarget = (Target.transform.position - transform.position).normalized;
            DistanceToTarget = Vector3.Distance(transform.position, Target.transform.position);
            UpdateAI();
        }

        private void FixedUpdate()
        {
            if (!_hasSpawned || Target == null)
                return;
            FixedUpdateAI();
        }

        private void PlaySound(AudioClip audioClip)
        {
            SoundSettings settings = SoundSettings.CreateDefaultSound(audioClip);
            settings.useVolumeLoss = true;
            settings.transform = transform;
            SoundManager.PlaySound(settings);
        }

        protected void Move(Vector2 targetSpeed, float acceleration, float deceleration)
        {
            Vector2 diff = targetSpeed - Rigidbody.velocity;
            float accelX = (Mathf.Abs(targetSpeed.x) > 0.01f) ? acceleration : deceleration;
            float accelY = (Mathf.Abs(targetSpeed.y) > 0.01f) ? acceleration : deceleration;

            Vector2 movement = Vector2.zero;
            movement.x = Mathf.Abs(diff.x) * accelX * Mathf.Sign(diff.x) * acceleration;
            movement.y = Mathf.Abs(diff.y) * accelY * Mathf.Sign(diff.y) * acceleration;
            Rigidbody.AddForce(movement);
        }

        private IEnumerator SpawnRoutine()
        {
            PlaySound(_spawnStartSound);
            float time = 0f;
            while (time < 1.0f)
            {
                time += Time.deltaTime * _spawnSpeed;
                float height = Mathf.Lerp(_spawnHeight, 0, time);
                _heightTransform.localPosition = new Vector3(_heightTransform.localPosition.x, height);
                yield return null;
            }
            PlaySound(_spawnFinishSound);
            _hasSpawned = true;
        }

        protected abstract void UpdateAI();
        protected abstract void FixedUpdateAI();
    }
}
