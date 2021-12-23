using Godot;
using System.Collections.Generic;

namespace Examples {
    namespace Chapter9 {
        public class C9Example2 : Node2D
        {
            private int lifeCounter = 0;
            private int lifetime = 200;
            private float mutationRate = 0.01f;
            private int populationSize = 100;
            private Vector2 target;
            private RocketPopulation population;
            private Vector2 initialPosition;

            public override void _Ready()
            {
                var size = GetViewportRect().Size;
                target = new Vector2(size.x / 2, 0);
                initialPosition = new Vector2(size.x / 2, size.y);
                population = new RocketPopulation(initialPosition, mutationRate, populationSize, lifetime);
            }

            public override void _Process(float delta)
            {
                for (int i = 0; i < 10; ++i) {
                    if (lifeCounter < lifetime) {
                        population.Live(null);
                        lifeCounter++;
                    } else {
                        lifeCounter = 0;
                        population.Fitness(target);
                        population.Selection();
                        population.Reproduction();
                    }
                }

                // Update();
            }

            public override void _Draw()
            {
                if (lifeCounter < lifetime) {
                    population.Live(this);
                    lifeCounter++;
                } else {
                    lifeCounter = 0;
                    population.Fitness(target);
                    population.Selection();
                    population.Reproduction();
                }
            }
        }

        public class RocketPopulation {
            public int Lifetime;
            public float MutationRate;
            public Rocket[] Population;
            public Vector2 InitialPosition;
            public List<RocketDNA> Pool;
            public int Generations;

            public RocketPopulation(Vector2 initialPosition, float mutationRate, int populationSize, int lifetime) {
                InitialPosition = initialPosition;
                Lifetime = lifetime;
                MutationRate = mutationRate;
                Population = new Rocket[populationSize];
                for (int i = 0; i < populationSize; ++i) {
                    Population[i] = new Rocket(initialPosition, lifetime);
                }

                Pool = new List<RocketDNA>();
            }

            public void Fitness(Vector2 target) {
                foreach (var rocket in Population) {
                    float d = rocket.Location.DistanceTo(target);
                    rocket.Fitness = 1 - MathUtils.Map(d, 0, InitialPosition.DistanceTo(target), 0, 1);
                }
            }

            private RocketDNA PickRandomDNAFromPool() {
                return Pool[MathUtils.RandRangei(0, Pool.Count - 2)];
            }

            private RocketDNA[] PickPartnersFromPool() {
                var parents = new RocketDNA[2];

                parents[0] = PickRandomDNAFromPool();
                parents[1] = parents[0];

                while (parents[1] == parents[0])
                {
                    parents[1] = PickRandomDNAFromPool();
                }

                return parents;
            }

            public void Selection() {
                Pool.Clear();

                var bestFitness = 0;

                foreach (var dna in Population)
                {
                    var n = (int)(dna.Fitness * 100.0f);
                    if (n > bestFitness) {
                        bestFitness = n;
                    }

                    for (var i = 0; i <= n; ++i)
                    {
                        Pool.Add(dna.Dna);
                    }
                }

                GD.Print(bestFitness);
            }

            public void Reproduction() {
                for (var i = 0; i < Population.Length; ++i)
                {
                    var parents = PickPartnersFromPool();
                    var child = parents[0].Crossover(parents[1]);
                    child.Mutate(MutationRate);

                    Population[i] = new Rocket(InitialPosition, child);
                }
            }

            public void Live(CanvasItem canvas) {
                for (var i = 0; i < Population.Length; ++i) {
                    Population[i].Run(canvas);
                }

                Generations++;
            }
        }

        public class RocketDNA {
            public float MaxForce = 0.1f;
            public Vector2[] Genes;

            public RocketDNA(int size) {
                Genes = new Vector2[size];
                for (var i = 0; i < size; ++i) {
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

        public class Rocket {
            public Vector2 Location;
            public Vector2 Velocity;
            public Vector2 Acceleration;
            public float Fitness;
            public RocketDNA Dna;

            private int _geneCounter = 0;

            public Rocket(Vector2 initialPosition, int lifetime) {
                Dna = new RocketDNA(lifetime);
                Location = initialPosition;
            }

            public Rocket(Vector2 initialPosition, RocketDNA dna) {
                Dna = dna;
                Location = initialPosition;
            }

            public void ApplyForce(Vector2 force) {
                Acceleration += force;
            }

            public void Draw(CanvasItem canvas) {
                canvas.DrawCircle(Location, 10, Colors.Green);
            }

            public void Run(CanvasItem canvas) {
                if (_geneCounter < Dna.Genes.Length) {
                    ApplyForce(Dna.Genes[_geneCounter]);
                    _geneCounter++;
                    Update();
                }

                if (canvas != null) {
                    Draw(canvas);
                }
            }

            public void Update() {
                Velocity += Acceleration;
                Location += Velocity;
                Acceleration *= 0;
            }
        }
    }
}
