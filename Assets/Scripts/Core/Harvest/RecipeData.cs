using UnityEngine;

namespace Mechanizer
{
    [CreateAssetMenu(menuName = "Mechanizer/Recipe Data")]
    public class RecipeData : ScriptableObject
    {
        [SerializeField] private Recipe _defaultRecipe;
        [SerializeField] private Recipe[] _specialRecipes;
        public Recipe FindBestRecipe(int[] tags)
        {
            for(int i = 0; i < _specialRecipes.Length; i++)
            {
                Recipe specialRecipe = _specialRecipes[i];
                if (specialRecipe.IsMatch(tags))
                    return specialRecipe;
            }

            return _defaultRecipe;
        }
    }
}
