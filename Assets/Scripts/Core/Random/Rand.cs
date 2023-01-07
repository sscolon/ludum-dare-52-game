using Random = System.Random;

namespace Mechanizer
{
    public class Rand
    {
        private Random _random;
        public int Seed { get; private set; }
        public Rand(int seed = 0)
        {
            if (seed == 0)
            {
                seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            }
            Seed = seed;
            _random = new Random(seed);
        }

        public int Next(int min, int max)
        {
            return _random.Next(min, max);
        }

        public T Next<T>(T[] weighteds) where T : IWeighted
        {
            int total = 0;
            for (int i = 0; i < weighteds.Length; i++)
            {
                var w = weighteds[i];
                total += w.GetWeight();
            }

            int rand = _random.Next(0, total);
            int weight = 0;
            for (int i = 0; i < weighteds.Length; i++)
            {
                var w = weighteds[i];
                weight += w.GetWeight();
                if (rand <= weight)
                {
                    return (T)w;
                }
            }

            return default;
        }
    }
}
