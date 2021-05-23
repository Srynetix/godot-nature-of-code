using System.Collections.Generic;

/// <summary>
/// Generic algorithms.
/// </summary>
namespace GA
{
    /// <summary>
    /// DNA.
    /// </summary>
    public class DNA
    {
        public delegate float FitnessFunctionDef(DNA dna, string target);

        private float _fitness;

        public float Fitness => _fitness;
        public char[] Genes { get; }
        public FitnessFunctionDef FitnessFunction;

        public DNA(int length)
        {
            Genes = new char[length];
            for (var i = 0; i < Genes.Length; ++i)
            {
                Genes[i] = (char)MathUtils.RandRangei(32, 127);
            }
        }

        public void CalculateFitness(string target)
        {
            _fitness = FitnessFunction(this, target);
        }

        public DNA Crossover(DNA partner)
        {
            var child = new DNA(Genes.Length);
            child.FitnessFunction = FitnessFunction;
            int midpoint = MathUtils.RandRangei(0, Genes.Length - 1);
            for (var i = 0; i < Genes.Length; ++i)
            {
                if (i > midpoint)
                {
                    child.Genes[i] = Genes[i];
                }
                else
                {
                    child.Genes[i] = partner.Genes[i];
                }
            }
            return child;
        }

        public void Mutate(float mutationRate)
        {
            for (var i = 0; i < Genes.Length; ++i)
            {
                if (MathUtils.Randf() < mutationRate)
                {
                    Genes[i] = (char)MathUtils.RandRangei(32, 127);
                }
            }
        }

        public string GetPhrase()
        {
            return new string(Genes);
        }
    }

    public static class MatingPool
    {
        public static DNA PickRandomDNA(DNA[] population)
        {
            return population[MathUtils.RandRangei(0, population.Length - 1)];
        }

        public static DNA PickRandomDNAFromPool(List<DNA> pool)
        {
            return pool[MathUtils.RandRangei(0, pool.Count - 1)];
        }

        public static List<DNA> CreateMatingPool(DNA[] population)
        {
            var matingPool = new List<DNA>();

            foreach (var dna in population)
            {
                var n = (int)(dna.Fitness * 100);
                for (var i = 0; i < n; ++i)
                {
                    matingPool.Add(dna);
                }
            }

            return matingPool;
        }

        public static DNA[] PickPartnersFromPool(List<DNA> pool)
        {
            var parents = new DNA[2];

            parents[0] = PickRandomDNAFromPool(pool);
            parents[1] = parents[0];

            while (parents[1] == parents[0])
            {
                parents[1] = PickRandomDNAFromPool(pool);
            }

            return parents;
        }

        public static DNA[] PickPartnersUsingMonteCarlo(DNA[] population)
        {
            var parents = new DNA[2];

            parents[0] = PickRandomDNA(population);
            while (MathUtils.Randf() > parents[0].Fitness)
            {
                // Take another
                parents[0] = PickRandomDNA(population);
            }

            // Same principle for the second parent
            parents[1] = PickRandomDNA(population);
            while (parents[1] != parents[0] && MathUtils.Randf() > parents[1].Fitness)
            {
                // Take another
                parents[1] = PickRandomDNA(population);
            }

            return parents;
        }
    }
}
