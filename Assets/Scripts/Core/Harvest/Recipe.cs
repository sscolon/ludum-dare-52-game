using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mechanizer
{
    [CreateAssetMenu(menuName = "Mechanizer/Recipe")]
    public class Recipe : ScriptableObject
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private List<string> _tags;
        [SerializeField] private bool _requireOrder;

        public GameObject Prefab => _prefab;
        public List<string> Tags => _tags;
        public bool IsMatch(string[] tags)
        {
            if (_requireOrder)
            {
                for (int i = 0; i < _tags.Count && i < tags.Length; i++)
                {
                    string tag = _tags[i];
                    if (tags[i] != tag)
                        return false;
                }
            }
            else
            {
                for(int i = 0; i < tags.Length; i++)
                {
                    if (!_tags.Contains(tags[i]))
                        return false;
                }
            }

            return true;
        }
    }
}
