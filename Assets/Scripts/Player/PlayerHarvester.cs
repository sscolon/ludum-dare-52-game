using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanizer
{
    public class PlayerHarvester : MonoBehaviour
    {
        [SerializeField] private RecipeData _recipeData;
        [SerializeField] private Transform _craftTransform;
        private int[] _components = new int[3];
        public Transform CraftTransform { get => _craftTransform; }

        public int ComponentCount { get; private set; } = 3;
        public int[] Components => _components;
        public List<Sprite> ComponentSprites { get; private set; }
        public event Action OnClear;
        public event Action<Collectible> OnCollect;
        private void Start()
        {
            ComponentSprites = new List<Sprite>();
            ClearComponents();
        }

        private void Craft()
        {
            //Only allow crafting at full component count.
            if (CanAddComponent())
                return;

            Recipe recipe = _recipeData.FindBestRecipe(_components);
            GameObject prefab = recipe.Prefab;
            Debug.Log(prefab);
            GameObject instance = Instantiate(prefab, _craftTransform.position, prefab.transform.rotation);
            if (instance.TryGetComponent(out IHarvested harvested))
            {
                harvested.OnHarvestInit(this);
            }

          //  ClearComponents();
        }

        private void AddComponent(int tag)
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

        public void ClearComponents()
        {
            for (int i = 0; i < _components.Length; i++)
            {
                _components[i] = -1;
            }
            ComponentSprites.Clear();
            OnClear?.Invoke();
        }

        private int GetFreeComponent()
        {
            for (int i = 0; i < _components.Length; i++)
            {
                if (_components[i] == -1)
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
                if (!collectible.HasSpawned)
                    return;

                collectible.IsCollected = true;
                AddComponent(collectible.CollectibleId);
                GameManager.Main.Score += 50;
                ComponentSprites.Add(collectible.Icon);
                OnCollect?.Invoke(collectible);
                collectible.Collect();
            }
        }
    }
}