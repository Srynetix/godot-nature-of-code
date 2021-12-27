using Godot;
using System.Collections.Generic;

namespace Examples
{
    namespace Chapter9
    {
        public class C9Example2 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Example 9.2:\nSimple Smart Rockets";
            }

            private Font defaultFont;
            private int lifeCounter;
            private readonly int lifetime = 100;
            private readonly float mutationRate = 0.01f;
            private readonly int populationSize = 50;
            private Vector2 target;
            private RocketPopulation population;
            private float maxValue;
            private Vector2 initialPosition;

            public override void _Ready()
            {
                defaultFont = Assets.SimpleDefaultFont.Regular;

                var size = GetViewportRect().Size;
                target = new Vector2(size.x / 2, 50);
                initialPosition = new Vector2(size.x / 2, size.y);
                population = new RocketPopulation(initialPosition, mutationRate, populationSize, lifetime);
                maxValue = initialPosition.DistanceSquaredTo(target);
            }

            public override void _Process(float delta)
            {
                if (lifeCounter < lifetime)
                {
                    population.Live();
                    lifeCounter++;
                }
                else
                {
                    lifeCounter = 0;
                    population.Fitness(maxValue, target);
                    population.Selection();
                    population.Reproduction();
                }

                Update();
            }

            public override void _Draw()
            {
                var size = GetViewportRect().Size;

                population.Draw(this);
                DrawCircle(target, 20, Colors.AliceBlue);

                DrawString(defaultFont, new Vector2(20, size.y - 250), "Target position: " + target);
                DrawString(defaultFont, new Vector2(20, size.y - 225), "Generation #" + (population.Generations + 1));
                DrawString(defaultFont, new Vector2(20, size.y - 200), "Last best fitness: " + (population.LastBestFitness) + "%");

            }
        }

        public class RocketPopulation
        {
            public int Lifetime;
            public float MutationRate;
            public Rocket[] Population;
            public Vector2 InitialPosition;
            public List<RocketDNA> Pool;
            public int Generations;
            public int LastBestFitness;

            public RocketPopulation(Vector2 initialPosition, float mutationRate, int populationSize, int lifetime)
            {
                InitialPosition = initialPosition;
                Lifetime = lifetime;
                MutationRate = mutationRate;
                Population = new Rocket[populationSize];
                for (int i = 0; i < populationSize; ++i)
                {
                    Population[i] = new Rocket(initialPosition, lifetime);
                }

                Pool = new List<RocketDNA>();
            }

            public void Fitness(float maxValue, Vector2 target)
            {
                foreach (var rocket in Population)
                {
                    float d = rocket.Location.DistanceSquaredTo(target);
                    rocket.Fitness = 1 - MathUtils.Map(d, 0, maxValue, 0, 1);
                }
            }

            private RocketDNA PickRandomDNAFromPool()
            {
                return Pool[MathUtils.RandRangei(0, Pool.Count - 2)];
            }

            private RocketDNA[] PickPartnersFromPool()
            {
                var parents = new RocketDNA[2];

                parents[0] = PickRandomDNAFromPool();
                parents[1] = parents[0];

                while (parents[1] == parents[0])
                {
                    parents[1] = PickRandomDNAFromPool();
                }

                return parents;
            }

            public void Selection()
            {
                Pool.Clear();

                var bestFitness = 0;

                foreach (var dna in Population)
                {
                    var n = (int)(dna.Fitness * 100.0f);
                    if (n > bestFitness)
                    {
                        bestFitness = n;
                    }

                    for (var i = 0; i <= n; ++i)
                    {
                        Pool.Add(dna.Dna);
                    }
                }

                LastBestFitness = bestFitness;
            }

            public void Reproduction()
            {
                for (var i = 0; i < Population.Length; ++i)
                {
                    var parents = PickPartnersFromPool();
                    var child = parents[0].Crossover(parents[1]);
                    child.Mutate(MutationRate);

                    Population[i] = new Rocket(InitialPosition, child);
                }

                Generations++;
            }

            public void Live()
            {
                for (var i = 0; i < Population.Length; ++i)
                {
                    Population[i].Run();
                }
            }

            public void Draw(CanvasItem canvas)
            {
                for (var i = 0; i < Population.Length; ++i)
                {
                    Population[i].Draw(canvas);
                }
            }
        }

        public class RocketDNA
        {
            public float MaxForce = 0.2f;
            public Vector2[] Genes;

            public RocketDNA(int size)
            {
                Genes = new Vector2[size];
                for (var i = 0; i < size; ++i)
                {
                    Genes[i] = MathUtils.RandVector2Unit();
                    Genes[i] *= MathUtils.RandRangef(0, MaxForce);
                }
            }

            public RocketDNA Crossover(RocketDNA partner)
            {
                var child = new RocketDNA(Genes.Length);
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
                        Genes[i] = MathUtils.RandVector2Unit() * MathUtils.RandRangef(0, MaxForce);
                    }
                }
            }
        }

        public class Rocket
        {
            public Vector2 Location;
            public Vector2 Velocity;
            public Vector2 Acceleration;
            public float Fitness;
            public RocketDNA Dna;

            private int _geneCounter;

            public Rocket(Vector2 initialPosition, int lifetime) : this(initialPosition, new RocketDNA(lifetime)) { }
            public Rocket(Vector2 initialPosition, RocketDNA dna)
            {
                Dna = dna;
                Location = initialPosition;
            }

            public void ApplyForce(Vector2 force)
            {
                Acceleration += force;
            }

            public void Draw(CanvasItem canvas)
            {
                canvas.DrawCircle(Location, 6, Colors.Green);
                canvas.DrawCircle(Location + (Velocity.Normalized() * 5), 2, Colors.Red);
            }

            public void Run()
            {
                if (_geneCounter < Dna.Genes.Length)
                {
                    ApplyForce(Dna.Genes[_geneCounter]);
                    _geneCounter++;
                    Update();
                }
            }

            public void Update()
            {
                Velocity += Acceleration;
                Location += Velocity;
                Acceleration *= 0;
            }
        }
    }
}
