using System;
using UnityEngine;

namespace Mechanizer
{
    public abstract class ProjectileBase : Attack
    {
        public event Action OnDeath;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (DoDamage(collision))
            {
                OnDeath?.Invoke();
            }
        }
    }
}