using Godot;

namespace Examples
{
    namespace Chapter9
    {
        /// <summary>
        /// Exercise 9.2: Mating pool algorithm with Monte Carlo.
        /// </summary>
        public class C9Exercise2 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Exercise 9.2:\nMating pool algorithm, Monte Carlo";
            }

            private Font defaultFont;
            private readonly string targetString = "to be or not to be";
            private int iterations;
            private readonly int populationCount = 100;
            private DNA[] population;

            private DNA parentA;
            private DNA parentB;

            private void ExtractParentsFromMatingPoolMonteCarlo()
            {
                parentA = population[MathUtils.RandRangei(0, population.Length - 1)];
                while (MathUtils.Randf() > parentA.Fitness)
                {
                    // Take another
                    parentA = population[MathUtils.RandRangei(0, population.Length - 1)];
                }

                // Same principle for the second parent
                parentB = population[MathUtils.RandRangei(0, population.Length - 1)];
                while (parentB != parentA && MathUtils.Randf() > parentB.Fitness)
                {
                    // Take another
                    parentB = population[MathUtils.RandRangei(0, population.Length - 1)];
                }
            }

            public override void _Ready()
            {
                defaultFont = Assets.SimpleDefaultFont.Regular;
                population = new DNA[populationCount];
                for (var i = 0; i < populationCount; ++i)
                {
                    population[i] = new DNA(targetString.Length);
                }
            }

            public override void _Process(float delta)
            {
                foreach (var dna in population)
                {
                    dna.CalculateFitness(targetString);
                }

                ExtractParentsFromMatingPoolMonteCarlo();
                iterations++;

                Update();
            }

            public override void _Draw()
            {
                var size = GetViewportRect().Size;
                DrawString(defaultFont, new Vector2(20, size.y - 150), "Target string: " + targetString);
                DrawString(defaultFont, new Vector2(20, size.y - 125), "Parent A: " + DisplayDNA(parentA));
                DrawString(defaultFont, new Vector2(20, size.y - 100), "Parent B: " + DisplayDNA(parentB));
                DrawString(defaultFont, new Vector2(20, size.y - 75), "Iterations: " + iterations);
            }

            private string DisplayDNA(DNA dna)
            {
                if (dna != null)
                {
                    return new string(dna.Genes);
                }
                else
                {
                    return "N/A";
                }
            }

            private class DNA
            {
                private float _fitness;

                public char[] Genes { get; }
                public float Fitness => _fitness;

                public void CalculateFitness(string target)
                {
                    int score = 0;
                    for (var i = 0; i < Genes.Length; ++i)
                    {
                        if (Genes[i] == target[i])
                        {
                            score++;
                        }
                    }

                    _fitness = (float)score / target.Length();
                }

                public DNA(int genesLength)
                {
                    Genes = new char[genesLength];
                    for (var i = 0; i < genesLength; ++i)
                    {
                        Genes[i] = StringGenerator.Generate(1)[0];
                    }
                }
            }
        }
    }
}
