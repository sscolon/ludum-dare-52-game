using System;
using UnityEngine;

namespace Mechanizer
{
    public class PlayerHarvester : MonoBehaviour
    {
        [SerializeField] private RecipeData _recipeData;
        [SerializeField] private Transform _craftTransform;
        private string[] _components = new string[3];
        public Transform CraftTransform { get => _craftTransform; }
        public event Action<Collectible> OnCollect;
        private void Start()
        {
            ClearComponents();
        }

        private void Craft()
        {
            //Only allow crafting at full component count.
            if (CanAddComponent())
                return;

            Recipe recipe = _recipeData.FindBestRecipe(_components);
            GameObject prefab = recipe.Prefab;
            GameObject instance = Instantiate(prefab, _craftTransform.position, prefab.transform.rotation);
            if (instance.TryGetComponent(out IHarvested harvested))
            {
                harvested.OnHarvestInit(this);
            }

            ClearComponents();
        }

        private void AddComponent(string tag)
        {
            int i = GetFreeComponent();
            if (i == -1)
            {
                Debug.LogWarning($"Inventory is full, cannot pick up component");
                return;
            }

            _components[i] = tag;
            if (!CanAddComponent())
                Craft();
        }

        private bool CanAddComponent()
        {
            return GetFreeComponent() != -1;
        }

        private void ClearComponents()
        {
            for (int i = 0; i < _components.Length; i++)
            {
                _components[i] = string.Empty;
            }
        }

        private int GetFreeComponent()
        {
            for (int i = 0; i < _components.Length; i++)
            {
                if (string.IsNullOrEmpty(_components[i]))
                    return i;
            }

            return -1;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //No need to attempt to add components when.. you know lol.
            if (!CanAddComponent())
                return;

            if (collision.TryGetComponent(out Collectible collectible))
            {
                if (collectible.IsCollected)
                    return;

                collectible.IsCollected = true;
                AddComponent(collectible.CollectibleTag);
                OnCollect?.Invoke(collectible);
                collectible.Collect();
            }
        }
    }
}