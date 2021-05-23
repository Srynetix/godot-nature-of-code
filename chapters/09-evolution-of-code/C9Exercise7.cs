using Godot;
using GA;

namespace Examples
{
    namespace Chapter9
    {
        /// <summary>
        /// Exercise 9.7: Exponential fitness.
        /// </summary>
        public class C9Exercise7 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Exercise 9.7:\nExponential fitness";
            }

            private Font defaultFont;
            private readonly string targetString = "to be or not to be";
            private readonly float mutationRate = 0.01f;
            private float maxFitness;
            private float averageFitness;
            private string bestPhrase;
            private readonly int totalPopulation = 200;
            private int foundAtIteration = -1;
            private int iterations;
            private DNA[] population;

            public override void _Ready()
            {
                defaultFont = Assets.SimpleDefaultFont.Regular;

                population = new DNA[totalPopulation];
                for (var i = 0; i < population.Length; ++i)
                {
                    population[i] = new DNA(targetString.Length);
                    population[i].FitnessFunction = (dna, target) =>
                    {
                        int score = 0;
                        for (var i = 0; i < dna.Genes.Length; ++i)
                        {
                            if (dna.Genes[i] == target[i])
                            {
                                score++;
                            }
                        }

                        float maxScore = Mathf.Pow(2, target.Length);
                        return (score * score) / (float)(target.Length * target.Length);
                    };
                }
            }

            public override void _Process(float delta)
            {
                maxFitness = 0;
                averageFitness = 0;

                // Calculate fitness
                foreach (var dna in population)
                {
                    dna.CalculateFitness(targetString);
                    if (dna.Fitness > maxFitness)
                    {
                        maxFitness = dna.Fitness;
                        bestPhrase = dna.GetPhrase();

                        if (dna.Fitness == 1 && foundAtIteration == -1)
                        {
                            foundAtIteration = iterations;
                        }
                    }

                    averageFitness += dna.Fitness;
                }

                averageFitness /= population.Length;

                var pool = MatingPool.CreateMatingPool(population);

                for (var i = 0; i < population.Length; ++i)
                {
                    var parents = MatingPool.PickPartnersFromPool(pool);
                    var child = parents[0].Crossover(parents[1]);
                    child.Mutate(mutationRate);

                    population[i] = child;
                }

                iterations++;
                Update();
            }

            public override void _Draw()
            {
                var size = GetViewportRect().Size;
                DrawString(defaultFont, new Vector2(20, size.y - 250), "Target string: " + targetString);
                DrawString(defaultFont, new Vector2(20, size.y - 225), "Population size: " + totalPopulation);
                DrawString(defaultFont, new Vector2(20, size.y - 200), "Max fitness: " + (maxFitness * 100) + "%");
                DrawString(defaultFont, new Vector2(20, size.y - 175), "Avg fitness: " + (averageFitness * 100) + "%");
                DrawString(defaultFont, new Vector2(20, size.y - 150), "Best phrase: " + bestPhrase);
                DrawString(defaultFont, new Vector2(20, size.y - 125), "Iteration: " + iterations);
                DrawString(defaultFont, new Vector2(20, size.y - 100), "Mutation rate: " + (mutationRate * 100) + "%");
                DrawString(defaultFont, new Vector2(20, size.y - 75), "Found at iteration: " + ((foundAtIteration == -1) ? "N/A" : foundAtIteration.ToString()));

                DrawString(defaultFont, new Vector2(300, size.y - 250), "[0]: " + population[0].GetPhrase());
                DrawString(defaultFont, new Vector2(300, size.y - 225), "[1]: " + population[1].GetPhrase());
                DrawString(defaultFont, new Vector2(300, size.y - 200), "[2]: " + population[2].GetPhrase());
                DrawString(defaultFont, new Vector2(300, size.y - 175), "[3]: " + population[3].GetPhrase());

                DrawString(defaultFont, new Vector2(500, size.y - 250), "[4]: " + population[4].GetPhrase());
                DrawString(defaultFont, new Vector2(500, size.y - 225), "[5]: " + population[5].GetPhrase());
                DrawString(defaultFont, new Vector2(500, size.y - 200), "[6]: " + population[6].GetPhrase());
                DrawString(defaultFont, new Vector2(500, size.y - 175), "[7]: " + population[7].GetPhrase());
            }
        }
    }
}
