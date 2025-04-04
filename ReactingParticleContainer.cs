using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using DongUtility;

namespace Thermodynamics
{
    /// <summary>
    /// A container for particles that can react chemically with each other.
    /// </summary>

    public class ReactingParticleContainer : ParticleContainer
    {

        public Equations Equations
        {
            get;
            private set;
        }
        private static readonly Random rand = new Random(); // Instantiate Random object once
        // double CollisionRadius { get; private set; } // Changed to public
        private readonly object locker = new object(); //initailize locker

        protected double CollisionRadius { get; private set; }

        public ReactingParticleContainer(double xSize, double ySize, double zSize, double collisionRadius) :
            base(xSize, ySize, zSize)
        {
            CollisionRadius = collisionRadius;
        }

        public ReactingParticleContainer(double side, double collisionRadius) :
            base(side)
        {
            CollisionRadius = collisionRadius;
        }

        /// <summary>
        /// Adds a specific particle of a given type.
        /// </summary>
        public void AddParticle(string name, Vector position, Vector velocity)
        {
            ParticlesToAdd.Add(Dictionary.MakeParticle(position, velocity, name));
        }

        /// <summary>
        /// Adds a number of random particles of a specific type.
        /// </summary>
        public void AddRandomParticles(RandomGenerator generator, int number, string name)
        {
            for (int i = 0; i < number; ++i)
            {
                AddParticleDirectly(generator.GetRandomParticle(name));
                //ParticlesToAdd.Add(Dictionary.MakeParticle(generator.GetRandomVector(), generator.GetRandomVelocity(), name));
            }
        }

        /// <summary>
        /// Checks whether a particle is close enough to another particle to react.
        /// </summary>
        private void CheckCollisions(Molecule particle)
        {
            if (ParticlesToRemove.Contains(particle))
            {
                return;
            }

            var particles = GetNearbyParticles(particle, CollisionRadius);
            var particleList = new List<Molecule>();

            foreach (var part in particles)
            {
                if (!ParticlesToRemove.Contains(part) && Vector.Distance(particle.Position, part.Position) < CollisionRadius) //checks if the dist between the particles is close enough to colllide and if so, it'll add it to the ParticlesToRemove list
                {
                    particleList.Add(part);
                }
            }

            if (particleList.Count > 0)
            {
                React(particle, particleList);
            
            }
        }

        /// <summary>
        /// Defines how particles react when they collide.
        /// </summary>
       
        public void React(Molecule particle, List<Molecule> nearby)  //Suggested by Astra AI
        {
            lock (locker)
            {
                string particleName = particle.Info.Name;

                // Add this line to output the particle name
                Debug.WriteLine($"React method called for particle: {particleName}");

                // Output the contents of Dictionary.Map
                Debug.WriteLine("Contents of Dictionary.Map:");
                foreach (var keyValuePair in Dictionary.Map)
                {
                    Debug.WriteLine($"  Key: {keyValuePair.Key}, Value: {keyValuePair.Value.Name}");
                }

                // Define potential reactions based on nearby particles
                if (particleName == "LacOperon")
                {
                    Molecule rnaPolymeraseNearby = nearby.FirstOrDefault(n => n.Info.Name == "RNAPolymerase");
                    Molecule promoterNearby = nearby.FirstOrDefault(n => n.Info.Name == "Promoter");
                    Molecule promoterBoundLacOperonNearby = nearby.FirstOrDefault(n => n.Info.Name == "PromoterBoundLacOperon");

                    if (rnaPolymeraseNearby != null)
                    {
                        // Add this line to output the reaction being considered
                        Debug.WriteLine($"  Considering reaction: LacOperon + RNAPolymerase -> mRNA");

                        // Create a temporary Equations object for this specific reaction
                        var reaction = new Equations(new Dictionary<string, double>
                {
                    { "LacOperon + RNAPolymerase -> mRNA", 0.1} // Reduced probability
                });

                        // Apply the reaction
                        ApplyReactions(particle, nearby, reaction);
                    }
                    else if (promoterNearby != null)
                    {
                        // Add this line to output the reaction being considered
                        Debug.WriteLine($"  Considering reaction: LacOperon + Promoter -> PromoterBoundLacOperon");

                        // Create a temporary Equations object for the binding reaction
                        var bindingReaction = new Equations(new Dictionary<string, double>
                {
                    { "LacOperon + Promoter -> PromoterBoundLacOperon", 0.9 } // High probability for binding
                });

                        // Apply the binding reaction
                        ApplyReactions(particle, nearby, bindingReaction);
                    }
                }
                else if (particleName == "PromoterBoundLacOperon")
                {
                    Molecule rnaPolymeraseNearby = nearby.FirstOrDefault(n => n.Info.Name == "RNAPolymerase");

                    if (rnaPolymeraseNearby != null)
                    {
                        // Add this line to output the reaction being considered
                        Debug.WriteLine($"  Considering reaction: PromoterBoundLacOperon + RNAPolymerase -> mRNA");

                        // Create a temporary Equations object for this specific reaction
                        // Increase the probability of mRNA production
                        var reaction = new Equations(new Dictionary<string, double>
                {
                    { "PromoterBoundLacOperon + RNAPolymerase -> mRNA", 0.5} // Increased probability
                });

                        // Apply the reaction
                        ApplyReactions(particle, nearby, reaction);
                    }
                }
                else if (particleName == "CyclicAMP")
                {
                    Molecule capNearby = nearby.FirstOrDefault(n => n.Info.Name == "CAP");

                    if (capNearby != null)
                    {
                        // Add this line to output the reaction being considered
                        Debug.WriteLine($"  Considering reaction: CyclicAMP + CAP -> Promoter");

                        // Create a temporary Equations object for the binding reaction
                        var bindingReaction = new Equations(new Dictionary<string, double>
                {
                    { "CyclicAMP + CAP -> Promoter", 0.9 } // High probability for binding
                });

                        // Apply the binding reaction
                        ApplyReactions(particle, nearby, bindingReaction);
                    }
                }
                else if (particleName == "mRNA")
                {
                    // Add this line to output the reaction being considered
                    Debug.WriteLine($"  Considering reaction: mRNA -> BetaGal");

                    // Create a temporary Equations object for this specific reaction
                    var reaction = new Equations(new Dictionary<string, double>
            {
                { "mRNA -> BetaGal", 0.05} // Reduced probability
            });

                    // Apply the reaction
                    ApplyReactions(particle, nearby, reaction);
                }
            }
        }


        /// <summary>
        /// Updates a single particle in the container.
        /// </summary>
        public override void ParticleUpdate(Molecule part)
        {
            CheckCollisions(part);
            CheckDecay(part);
            
        }

        /// <summary>
        /// Checks if a particle undergoes decay.
        /// </summary>
        protected virtual void CheckDecay(Molecule part)
        {
            if (part.Info.Name == "BoundInhibitor")
            {
                // Define the probability of dissociation
                double dissociationProbability = 0.01; // Low probability for slow dissociation

                // Check if the bound inhibitor should dissociate
                if (rand.NextDouble() < dissociationProbability)
                {
                    // Calculate the position and velocity of the products
                    Vector lacOperonPosition = part.Position;
                    Vector inhibitorPosition = part.Position;
                    Vector lacOperonVelocity = part.Velocity;
                    Vector inhibitorVelocity = part.Velocity;

                    // Remove the bound inhibitor
                    ParticlesToRemove.Add(part);

                    // Add the lac operon and inhibitor
                    AddParticle("LacOperon", lacOperonPosition, lacOperonVelocity);
                    AddParticle("Inhibitor", inhibitorPosition, inhibitorVelocity);
                }
            }
        }


        
        public void ApplyReactions(Molecule particle, List<Molecule> nearby, Equations equations) //Suggested by Astra AI
        {
            //Random rand = new Random();
            foreach (var reaction in equations.TimeLabeledEquations)
            {
                string equation = reaction.Key;
                double probability = reaction.Value;

                // Extract reactants and products from the equation string
                var parts = equation.Split(" -> ");
                var reactants = parts[0].Split(" + ").Select(s => s.Trim()).ToList();
                var products = parts[1].Split(" + ").Select(s => s.Trim()).ToList();

                // Check if all reactants exist in the container
                bool canReact = reactants.All(reactant => GetNParticles(reactant) > 0);

                // Ensure that the current particle is a reactant
                if (!reactants.Contains(particle.Info.Name))
                {
                    canReact = false;
                    break;
                }

                // Check if all other reactants are nearby
                foreach (string reactant in reactants)
                {
                    if (reactant != particle.Info.Name)
                    {
                        if (!nearby.Any(n => n.Info.Name == reactant))
                        {
                            continue; // If any reactant is not nearby, exit
                        }
                    }
                }

                if (canReact && rand.NextDouble() < probability)
                {
                    if (equation == "CyclicAMP + CAP -> Promoter")
                    {
                        // Find the CAP molecule
                        Molecule cap = nearby.FirstOrDefault(n => n.Info.Name == "CAP");

                        if (cap != null)
                        {
                            // Calculate average position and velocity
                            Vector avgPosition = (particle.Position + cap.Position) / 2;
                            Vector avgVelocity = (particle.Velocity + cap.Velocity) / 2;

                            // Remove the reactants
                            ParticlesToRemove.Add(particle);
                            ParticlesToRemove.Add(cap);

                            // Add the product
                            AddParticle("Promoter", avgPosition, avgVelocity);
                        }
                    }
                    else if (equation == "LacOperon + Promoter -> PromoterBoundLacOperon")
                    {
                        // Find the Promoter molecule
                        Molecule promoter = nearby.FirstOrDefault(n => n.Info.Name == "Promoter");

                        if (promoter != null)
                        {
                            // Calculate average position and velocity
                            Vector avgPosition = (particle.Position + promoter.Position) / 2;
                            Vector avgVelocity = (particle.Velocity + promoter.Velocity) / 2;

                            // Remove the reactants
                            ParticlesToRemove.Add(particle);
                            ParticlesToRemove.Add(promoter);

                            // Add the product
                            AddParticle("PromoterBoundLacOperon", avgPosition, avgVelocity);
                        }
                    }
                    else
                    {
                        // Calculate average position and velocity of reactants
                        Vector totalVelocity = new Vector(0, 0, 0);
                        Vector totalPosition = new Vector(0, 0, 0);
                        Vector totalMomentum = new Vector(0, 0, 0);

                        List<Molecule> reactantsToRemove = new List<Molecule> { particle };

                        foreach (string reactant in reactants)
                        {
                            Molecule molecule;
                            if (reactant == particle.Info.Name)
                            {
                                molecule = particle;
                            }
                            else
                            {
                                molecule = nearby.FirstOrDefault(n => n.Info.Name == reactant);
                            }

                            if (molecule != null)
                            {
                                totalMomentum += molecule.Momentum;
                                totalVelocity += molecule.Velocity;
                                totalPosition += molecule.Position;
                                reactantsToRemove.Add(molecule);
                            }
                            else
                            {
                                return; // If any reactant is missing, exit
                            }
                        }

                        Vector avgVelocity = totalVelocity / reactants.Count;
                        Vector avgPosition = totalPosition / reactants.Count;

                        // Remove reactants
                        foreach (Molecule reactant in reactantsToRemove)
                        {
                            ParticlesToRemove.Add(reactant);
                        }

                        // Add products
                        foreach (string product in products)
                        {
                            if (Dictionary.Map.ContainsKey(product))
                            {
                                const double mass = 1e-24;
                                AddParticle(product, avgPosition, avgVelocity);
                            }
                        }
                    }
                }
            }
        }



        public void ApplyReactionsToAllParticles(double deltaTime)
        {
            foreach (var particle in Particles)
            {
                // Move the particle
                particle.Position += particle.Velocity * deltaTime;

                // Apply reactions
                CheckCollisions(particle);

                // Keep the particle within the bounds of the container
                CheckParticle(particle);
            }

            // Remove particles that are marked for removal
            foreach (var particleToRemove in ParticlesToRemove)
            {
                Particles.Remove(particleToRemove);
            }
            ParticlesToRemove.Clear();

            // Add any new particles that have been created
            foreach (var particleToAdd in ParticlesToAdd)
            {
                Particles.Add(particleToAdd);
            }
            ParticlesToAdd.Clear();
        }
    }
}
