using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mechanizer
{
    [RequireComponent(typeof(Collectible))]
    public class CollectibleFeedback : MonoBehaviour
    {
        private Collectible _collectible;
        [SerializeField] private Feedback _spawnFeedback;
        [SerializeField] private Feedback _collectFeedback;
        private void OnEnable()
        {
            if (_collectible == null)
                _collectible = GetComponent<Collectible>();
            _collectible.OnSpawn += PlaySpawnFeedback;
            _collectible.OnCollect += PlayCollectFeedback;
        }

        private void OnDisable()
        {
            _collectible.OnSpawn -= PlaySpawnFeedback;
            _collectible.OnCollect -= PlayCollectFeedback;
        }

        private void PlaySpawnFeedback()
        {
            _spawnFeedback.CreateFeedback(gameObject);
        }

        private void PlayCollectFeedback()
        {
            _collectFeedback.CreateFeedback(gameObject);
        }
    }
}
