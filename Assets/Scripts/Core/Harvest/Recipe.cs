using System.Collections.Generic;
using UnityEngine;

namespace Mechanizer
{
    [CreateAssetMenu(menuName = "Mechanizer/Recipe")]
    public class Recipe : ScriptableObject
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private List<int> _ingredients;
        [SerializeField] private bool _requireOrder;

        public GameObject Prefab => _prefab;
        public bool IsMatch(int[] ingredients)
        {
            int anyCount = 0;
            for (int i = 0; i < _ingredients.Count; i++)
            {

                if (_ingredients[i] == -1)
                    anyCount++;
            }

            if (_requireOrder)
            {
                for (int i = 0; i < _ingredients.Count && i < ingredients.Length; i++)
                {
                    if (ingredients[i] == -1)
                        continue;
                    var ingredient = _ingredients[i];
                    if (ingredients[i] != ingredient)
                        return false;
                }
            }
            else
            {
                for (int i = 0; i < ingredients.Length; i++)
                {
                    if (!_ingredients.Contains(ingredients[i]))
                    {
                        if (anyCount <= 0)
                            return false;
                        else
                            anyCount--;
                    }
                }
            }

            return true;
        }
    }
}
