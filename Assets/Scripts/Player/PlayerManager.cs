using UnityEngine;

namespace Mechanizer
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        public GameObject NewPlayer()
        {
            return GameObject.Instantiate(_playerPrefab);
        }
    }
}