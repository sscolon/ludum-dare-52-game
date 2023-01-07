using System;
using UnityEngine;

namespace Mechanizer
{
    public abstract class ProjectileBase : Attack
    {
        public Vector2 Origin { get; set; }
        public Vector2 Target { get; set; }

        public event Action OnDeath;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (DoDamage(collision))
            {
                OnDeath?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}