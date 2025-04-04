using DongUtility;
using System.Diagnostics;
using System.Drawing;

namespace Thermodynamics
{
    /// <summary>
    /// A container class for holding gas particles in a cubical container
    /// </summary>
    public class ParticleContainer(double xSize, double ySize, double zSize)
    {
        /// <summary>
        /// The size of the container
        /// Each component of the vector is the size in that dimension
        /// </summary>
        public Vector Size { get; set; } = new Vector(xSize, ySize, zSize);
        /// <summary>
        /// All the particles in the container
        /// </summary>
        public List<Molecule> Particles { get; } = [];

        /// <summary>
        /// A dictionary to keep track of all the particle types
        /// </summary>
        public ParticleDictionary Dictionary { get; } = new ParticleDictionary();

        /// <summary>
        /// Access to the random generator
        /// </summary>
        static protected Random Random { get { return RandomGenerator.RandomGen; } }

        public ParticleContainer(double size) :
            this(size, size, size)
        { }

        public void RegisterParticleType(string name, double mass, Color color, Shapes.Shapes3D shape = Shapes.Shapes3D.Sphere)
        {
            Dictionary.AddParticle(new ParticleInfo(name, mass, color, shape));
        }

        public List<Molecule> ParticlesToAdd { get; } = [];
        public List<Molecule> ParticlesToRemove { get; } = [];

        public void AddParticle(Molecule part)
        {
            ParticlesToAdd.Add(part);
        }

        public void RemoveParticle(Molecule part)
        {
            ParticlesToRemove.Add(part);
        }

        /// <summary>
        /// Adds a particle directly
        /// Not for general use; hence, it is protected
        /// </summary>
        protected virtual void AddParticleDirectly(Molecule part)
        {
            Particles.Add(part);
        }

        /// <summary>
        /// Removes a particle directly
        /// Not for general use; hence, it is protected
        /// </summary>
        /// <param name="part"></param>
        protected virtual void RemoveParticleDirectly(Molecule part)
        {
            Particles.Remove(part);
        }

        /// <summary>
        /// Gets the total number of particles of a given name
        /// </summary>
        public int GetNParticles(string name)
        {
            int total = 0;

            foreach (var part in Particles)
            {
                if (part.Info.Name == name)
                {
                    ++total;
                }
            }
            // Add this line to output the particle count
            Debug.WriteLine($"Particle count for {name}: {total}");

            return total;
        }

        /// <summary>
        /// Adds a number of particles at random
        /// </summary>
        /// <param name="generator">The random generator to use</param>
        /// <param name="name">The name of the particle type</param>
        /// <param name="number">The number of particles to add</param>
        public void AddRandomParticles(RandomGenerator generator, string name, int number)
        {
            for (int i = 0; i < number; ++i)
            {
                AddParticleDirectly(generator.GetRandomParticle(name));
            }
        }

        /// <summary>
        /// Updates all particles for a given time increment
        /// </summary>
        
        public virtual void Update(double DeltaTime)
        {
            ParticlesToAdd.Clear();
            ParticlesToRemove.Clear();
            const double reactionRadius = 2;
            const double CollisionRadius = reactionRadius;

            Setup();

            // Glucose suppression of cAMP
            double glucoseSuppressionFactor = 0.01; // Adjust this value to control the rate of suppression

            // Iterate through a copy of the Particles list to avoid modification issues
            List<Molecule> particlesCopy = new List<Molecule>(Particles);
            foreach (Molecule particle in particlesCopy)
            {
                if (particle.Info.Name == "CyclicAMP")
                {
                    // Check if there is glucose nearby
                    bool glucoseNearby = false;
                    foreach (Molecule nearbyParticle in GetNearbyParticles(particle, CollisionRadius))
                    {
                        if (nearbyParticle.Info.Name == "Glucose")
                        {
                            glucoseNearby = true;
                            break;
                        }
                    }

                    if (glucoseNearby)
                    {
                        // Remove the CyclicAMP particle with a certain probability
                        if (Random.NextDouble() < glucoseSuppressionFactor)
                        {
                            ParticlesToRemove.Add(particle);
                        }
                    }
                }
            }

            // Remove and add particles
            foreach (var particleToRemove in ParticlesToRemove)
            {
                Particles.Remove(particleToRemove);
            }
            ParticlesToRemove.Clear();

            foreach (var particleToAdd in ParticlesToAdd)
            {
                Particles.Add(particleToAdd);
            }
            ParticlesToAdd.Clear();

            int numberofThreads = 1;

            if (Particles != null && Particles.Count > 0)
            {
                for (int i = 0; i < Particles.Count; i++)
                {
                    Molecule particle = Particles[i];
                    new MoveParticles(DeltaTime, this, new List<Molecule> { particle }).Update();
                }
            }
        }

        /// <summary>
        /// A function that can be overridden to update particles in a specific way
        /// </summary>
        public virtual void ParticleUpdate(Molecule part)
        {
           
        }

        public int GetNPromoterBoundLacOperon()
        {
            int total = 0;

            foreach (var part in Particles)
            {
                if (part.Info.Name == "PromoterBoundLacOperon")
                {
                    ++total;
                }
            }

            return total;
        }

        /// <summary>
        /// Find all particles near a given particle.
        /// By default, returns all particles.
        /// Can be overridden in derived classes
        /// </summary>
        /// <param name="center">The position of the current particle</param>
        /// <param name="rad">The radius to look within</param>        /// <param name="toBeRemoved">A list of particles that have already been removed from simulation</param>
        /// <returns>All particles within the radius rad from the given particle, plus maybe some extra</returns>
     
        protected virtual IEnumerable<Molecule> GetNearbyParticles(Molecule center, double rad)
        {
            List<Molecule> nearbyParticles = new List<Molecule>();

            foreach (Molecule p in Particles)
            {
                if (p != center) // Exclude the center particle itself
                {
                    double distance = Vector.Distance(p.Position, center.Position);
                    if (distance <= rad)
                    {
                        nearbyParticles.Add(p);
                    }
                }
            }

            return nearbyParticles;
        }

        /// <summary>
        /// Prepare for a single loop
        /// </summary>
        protected virtual void Setup()
        { }

        /// <summary>
        /// A function that extracts a specific property of a particle
        /// </summary>
        public delegate double ParticleFunction(Molecule part);

        /// <summary>
        /// Get a list of values for a given property of all particles
        /// Useful for histograms
        /// </summary>
        public List<double> GetParticlePropertyList(ParticleFunction func)
        {
            var response = new List<double>();

            foreach (var part in Particles)
            {
                response.Add(func(part));
            }

            return response;
        }

        /// <summary>
        /// Make sure the particle lies within the bounds of the box
        /// Reflect it back if it is not
        /// </summary>
        public virtual void CheckParticle(Molecule particle)
        {
            Vector newVec = particle.Position;
            if (particle.Position.X < 0 || particle.Position.X > Size.X)
            {
                particle.Velocity = new Vector(-particle.Velocity.X, particle.Velocity.Y, particle.Velocity.Z);
                if (particle.Position.X < 0)
                {
                    newVec.X = 0;
                }
                else if (particle.Position.X > Size.X)
                {
                    newVec.X = Size.X;
                }
            }
            if (particle.Position.Y < 0 || particle.Position.Y > Size.Y)
            {
                particle.Velocity = new Vector(particle.Velocity.X, -particle.Velocity.Y, particle.Velocity.Z);
                if (particle.Position.Y < 0)
                {
                    newVec.Y = 0;
                }
                else if (particle.Position.Y > Size.Y)
                {
                    newVec.Y = Size.Y;
                }
            }
            if (particle.Position.Z < 0 || particle.Position.Z > Size.Z)
            {
                particle.Velocity = new Vector(particle.Velocity.X, particle.Velocity.Y, -particle.Velocity.Z);
                if (particle.Position.Z < 0)
                {
                    newVec.Z = 0;
                }
                else if (particle.Position.Z > Size.Z)
                {
                    newVec.Z = Size.Z;
                }
            }
            particle.Position = newVec;
        }
    }
}
