using System.Collections.Generic;
using UnityEngine;

namespace Mechanizer
{
    public class Wave : MonoBehaviour, IWeighted
    {
        public int weight;
        public int difficulty;
        public List<Spawn> spawns;
        public int GetWeight()
        {
            return weight;
        }

        public int GetEnemyCount()
        {
            int count = 0;
            for(int i = 0; i < spawns.Count; i++)
            {
                var spawn = spawns[i];
                if(spawn.prefab.TryGetComponent(out Enemy enemy))
                {
                    count++;
                }
            }
            return count;
        }

        public int GetSpawnAtPosition(Vector2 position)
        {
            for(int i = 0; i < spawns.Count; i++)
            {
                var spawn = spawns[i];
                if (spawn.position == position)
                    return i;
            }

            return -1;
        }
    }
}