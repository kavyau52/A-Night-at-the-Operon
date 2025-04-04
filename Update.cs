using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thermodynamics
{         
    internal class MoveParticles(double DeltaTime, ParticleContainer container, List<Molecule> particles) 
    {
        private ParticleContainer Container = container;
        private double DeltaTime = DeltaTime;
        private List<Molecule> Particles = particles;

        public void Update()
        {
            foreach (var part in Particles)
            {
                part.Update(DeltaTime);
                Container.CheckParticle(part);
                Container.ParticleUpdate(part);
            }
        }
    }
}
