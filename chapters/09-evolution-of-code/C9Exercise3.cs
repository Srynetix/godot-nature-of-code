using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Examples
{
    namespace Chapter9
    {
        /// <summary>
        /// Exercise 9.3: Mating pool algorithm with rank.
        /// </summary>
        public class C9Exercise3 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Exercise 9.3:\nMating pool algorithm, Rank";
            }

            private Font defaultFont;
            private readonly string targetString = "to be or not to be";
            private int iterations;
            private readonly int populationCount = 100;
            private DNA[] population;

            private DNA parentA;
            private DNA parentB;

            private void ExtractParentsFromMatingPoolRank()
            {
                // Rank population
                var populationOrderedByRank = (DNA[])population.Clone();
                Array.Sort(populationOrderedByRank, new DNAFitnessComparer());
                var orderedFitness = new float[population.Length];

                // Once sorted, apply another fitness based on rank
                int totalCount = populationOrderedByRank.Length * 2;
                for (var i = 0; i < populationOrderedByRank.Length; ++i)
                {
                    orderedFitness[i] = (populationOrderedByRank.Length - i) / (float)totalCount;
                }

                var parentAIdx = MathUtils.RandRangei(0, population.Length - 1);
                parentA = populationOrderedByRank[parentAIdx];
                while (MathUtils.Randf() > orderedFitness[parentAIdx])
                {
                    // Take another
                    parentAIdx = MathUtils.RandRangei(0, population.Length - 1);
                    parentA = populationOrderedByRank[parentAIdx];
                }

                // Same principle for the second parent
                var parentBIdx = MathUtils.RandRangei(0, population.Length - 1);
                parentB = populationOrderedByRank[parentBIdx];
                while (parentB != parentA && MathUtils.Randf() > orderedFitness[parentBIdx])
                {
                    // Take another
                    parentBIdx = MathUtils.RandRangei(0, population.Length - 1);
                    parentB = populationOrderedByRank[parentBIdx];
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

                ExtractParentsFromMatingPoolRank();
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

            private class DNAFitnessComparer : IComparer
            {
                public int Compare(object x, object y)
                {
                    return Comparer<float>.Default.Compare(((DNA)x).Fitness, ((DNA)y).Fitness);
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
