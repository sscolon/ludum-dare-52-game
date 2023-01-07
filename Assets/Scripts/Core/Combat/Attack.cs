﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanizer
{
    public class Attack : MonoBehaviour
    {
        private List<Collider2D> _results;
        [SerializeField] private Damage _damage;
        [SerializeField] private Hitbox _hitbox;
        public Damage Damage { get => _damage; set => _damage = value; }
        public Hitbox Hitbox { get => _hitbox; set => _hitbox = value; }

        public event Action OnPerformStart;
        public event Action<Collider2D, IDamageable> OnDamageStart;
        public event Action<Collider2D, IDamageable> OnDamageFinish;
        public event Action OnPerformFinish;
        public void Perform()
        {
            OnPerformStart?.Invoke();
            if (_results == null)
                _results = new List<Collider2D>();
   
            Hitbox.Overlap(transform, _results);
            for (int i = 0; i < _results.Count; i++)
            {
                var result = _results[i];
                DoDamage(result);
            }
            OnPerformFinish?.Invoke();
        }

        public void DoDamage(Collider2D collider)
        {
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                OnDamageStart?.Invoke(collider, damageable);
                damageable.Health.Value -= _damage.Value;
                OnDamageFinish?.Invoke(collider, damageable);
            }
        }
    }
}