using UnityEngine;

namespace Mechanizer
{
    public class ContinuousAttack : Attack
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            DoDamage(collision);
        }
    }
}