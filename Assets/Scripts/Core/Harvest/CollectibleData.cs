using UnityEngine;

namespace Mechanizer
{
    [CreateAssetMenu(menuName = "Mechanizer/Collectible Data")]
    public class CollectibleData : ScriptableObject
    {
        [SerializeField] private Collectible[] _collectibles;
        public Collectible NextCollectible(Rand rand)
        {
            Collectible c = rand.Next(_collectibles);
            return c;
        }
    }
}
