using UnityEngine;

namespace Mechanizer
{
    public class ContinuousAttack : Attack
    {
        [SerializeField] private bool _destroyAfterDamage = false;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            DoDamage(collision); 
        }
    }
}