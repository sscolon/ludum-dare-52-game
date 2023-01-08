using System;
using UnityEngine;

namespace Mechanizer
{
    public abstract class ProjectileBase : Attack
    {
        private Vector3 _lastPosition;
        private float _traveledDistance;
        [SerializeField] private float _range=25f;
        public Vector2 Origin { get; set; }
        public Vector2 Target { get; set; }

        public event Action OnDeath;
        private void Start()
        {
            _lastPosition = transform.position;
        }
        private void Update()
        {
            float distance = Vector3.Distance(_lastPosition, transform.position);
            _traveledDistance += distance;
            if(_traveledDistance >= _range)
            {
                Kill();
            }
            _lastPosition = transform.position;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (DoDamage(collision))
            {
                Kill();
            }
        }

        private void Kill()
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}