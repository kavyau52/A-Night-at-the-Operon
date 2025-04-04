using GraphControl;
using GraphData;
using MotionVisualizer3D;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;
using Thermodynamics;
using static GraphData.GraphDataManager;
using static WPFUtility.UtilityFunctions;
using System.Threading; // Add this import
using System.ComponentModel; // Add this import
using System.Diagnostics;
using System;
using System.Threading.Tasks;


namespace Visualizer.ChemicalReactions
{
    class ChemicalReactionsDriver
    {
        static internal void Run()
        {
            
            // Add this line
              //  Step1();
            //Step2();
              //Step3();
              //Step4();
              //Step5();
                Step6();
        }

        static internal void Step1() {
            
            const double containerSize = 25;

            const double deltaTime = .01;
            const double temperature = 293.17;
            const double reactionRadius = 2;

            var container = new ReactingParticleContainer(containerSize, reactionRadius);

            const double mass = 1e-24;


          //  container.RegisterParticleType("Molecule", mass, ConvertColor(Colors.NavajoWhite));
            //new code for registering particletypes
            container.RegisterParticleType("LacOperon", mass, ConvertColor(Colors.Green));
            container.RegisterParticleType("RNAPolymerase", mass, ConvertColor(Colors.Blue));
            container.RegisterParticleType("mRNA", mass, ConvertColor(Colors.Fuchsia));
            container.RegisterParticleType("BetaGal", mass, ConvertColor(Colors.Red));
            


            var generator = new BoltzmannGenerator(container, temperature, container.Dictionary.Map["LacOperon"]);

            const int nParticles = 1000;
          
            container.AddRandomParticles(generator, "LacOperon", nParticles / 10);
            container.AddRandomParticles(generator, "RNAPolymerase", nParticles / 10);
            

            // Adding new code here to define Equations
           

            var visualization = new ChemicalVisualization(container)
            {
                BoxColor = Colors.IndianRed
            };

            var viz = new MotionVisualizer3DControl(visualization)
            {
                TimeIncrement = deltaTime,
                TimeScale = 1,
                SlowDraw = false
            };

            Timeline.MaximumPoints = 3000;

            AddChemicalGraphs(viz, container, visualization);
            viz.Manager.AddText("Time elapsed (s)", ConvertColor(Colors.Crimson), () => TimeElapsed().ToString());
            visualization.StopTime = 1;

            viz.Show();
        }

        static internal void Step2()
        {

            const double containerSize = 25;

            const double deltaTime = .01;
            const double temperature = 293.17;
            const double reactionRadius = 2;

            var container = new ReactingParticleContainer(containerSize, reactionRadius);

            const double mass = 1e-24;


            //  container.RegisterParticleType("Molecule", mass, ConvertColor(Colors.NavajoWhite));
            //new code for registering particletypes
            container.RegisterParticleType("LacOperon", mass, ConvertColor(Colors.Green));
            container.RegisterParticleType("RNAPolymerase", mass, ConvertColor(Colors.Blue));
            container.RegisterParticleType("mRNA", mass, ConvertColor(Colors.Fuchsia));
            container.RegisterParticleType("BetaGal", mass, ConvertColor(Colors.Red));
            container.RegisterParticleType("Inhibitor", mass, ConvertColor(Colors.DarkRed)); //Suggested by Astra AI
            container.RegisterParticleType("BoundInhibitor", mass, ConvertColor(Colors.Purple));


            var generator = new BoltzmannGenerator(container, temperature, container.Dictionary.Map["LacOperon"]);

            const int nParticles = 1000;

            container.AddRandomParticles(generator, "LacOperon", nParticles / 10);
            container.AddRandomParticles(generator, "RNAPolymerase", nParticles / 10);
            container.AddRandomParticles(generator, "Inhibitor", nParticles / 5); // Add the inhibitor//Suggested by Astra AI

            // Adding new code here to define Equations
           
            var visualization = new ChemicalVisualization(container)
            {
                BoxColor = Colors.IndianRed
            };

            var viz = new MotionVisualizer3DControl(visualization)
            {
                TimeIncrement = deltaTime,
                TimeScale = 1,
                SlowDraw = false
            };

            Timeline.MaximumPoints = 3000;

            AddChemicalGraphs(viz, container, visualization);
            viz.Manager.AddText("Time elapsed (s)", ConvertColor(Colors.Crimson), () => TimeElapsed().ToString());
            visualization.StopTime = 1;

            viz.Show();
            // Track BetaGal production
            List<int> betaGalCounts = new List<int>();
            double simulationTime = 5; // Run for 5 seconds
            double currentTime = 0;
           
            // Print BetaGal counts //Suggested by Astra AI
            Debug.WriteLine("BetaGal Counts (with inhibitor):");
            foreach (int count in betaGalCounts)
            {
                Debug.WriteLine(count);
            }



        }

        static internal void Step3()
        {
            const double containerSize = 25;
            const double deltaTime = 0.001; // Reduced deltaTime
            const double temperature = 293.17;
            const double reactionRadius = 2;

            var container = new ReactingParticleContainer(containerSize, reactionRadius);

            const double mass = 1e-24;

            // Register particle types
            container.RegisterParticleType("LacOperon", mass, ConvertColor(Colors.Green));
            container.RegisterParticleType("RNAPolymerase", mass, ConvertColor(Colors.Blue));
            container.RegisterParticleType("mRNA", mass, ConvertColor(Colors.Fuchsia));
            container.RegisterParticleType("BetaGal", mass, ConvertColor(Colors.Red));
            container.RegisterParticleType("Inhibitor", mass, ConvertColor(Colors.DarkRed));
            container.RegisterParticleType("BoundInhibitor", mass, ConvertColor(Colors.Purple));
            container.RegisterParticleType("Lactose", mass, ConvertColor(Colors.Yellow)); 
            container.RegisterParticleType("Glucose", mass, ConvertColor(Colors.Orange)); 
            container.RegisterParticleType("Galactose", mass, ConvertColor(Colors.Pink)); 
            container.RegisterParticleType("LactoseBoundInhibitor", mass, ConvertColor(Colors.Teal));

            var generator = new BoltzmannGenerator(container, temperature, container.Dictionary.Map["LacOperon"]);

            const int nParticles = 1000;

            // Add particles
            container.AddRandomParticles(generator, "LacOperon", nParticles / 10);
            container.AddRandomParticles(generator, "RNAPolymerase", nParticles / 10);
            container.AddRandomParticles(generator, "Inhibitor", nParticles / 5); // Add the inhibitor
            container.AddRandomParticles(generator, "Lactose", nParticles / 5); // Add Lactose //Suggested by Astra AI


            var visualization = new ChemicalVisualization(container)
            {
                BoxColor = Colors.IndianRed
            };

            var viz = new MotionVisualizer3DControl(visualization)
            {
                TimeIncrement = deltaTime,
                TimeScale = 1,
                SlowDraw = false
            };

            Timeline.MaximumPoints = 3000;

            AddChemicalGraphs(viz, container, visualization);
            viz.Manager.AddText("Time elapsed (s)", ConvertColor(Colors.Crimson), () => TimeElapsed().ToString());
            visualization.StopTime = 1;

            viz.Show();

            // Track BetaGal production
            List<int> betaGalCounts = new List<int>();
            double simulationTime = 5; // Run for 5 seconds
            double currentTime = 0; 
        }

        static internal void Step4()
        {
            

            const double containerSize = 25;
            const double deltaTime = 0.001; // Reduced deltaTime
            const double temperature = 293.17;
            const double reactionRadius = 2;

            var container = new ReactingParticleContainer(containerSize, reactionRadius);

            const double mass = 1e-24;

            // Register particle types
            container.RegisterParticleType("LacOperon", mass, ConvertColor(Colors.Green));
            container.RegisterParticleType("RNAPolymerase", mass, ConvertColor(Colors.Blue));
            container.RegisterParticleType("mRNA", mass, ConvertColor(Colors.Fuchsia));
            container.RegisterParticleType("BetaGal", mass, ConvertColor(Colors.Red));
            container.RegisterParticleType("Inhibitor", mass, ConvertColor(Colors.DarkRed));
            container.RegisterParticleType("BoundInhibitor", mass, ConvertColor(Colors.Purple));
            container.RegisterParticleType("Lactose", mass, ConvertColor(Colors.Yellow)); // Register Lactose
            container.RegisterParticleType("Glucose", mass, ConvertColor(Colors.Orange)); // Register Glucose
            container.RegisterParticleType("Galactose", mass, ConvertColor(Colors.Pink)); // Register Galactose
            container.RegisterParticleType("LactoseBoundInhibitor", mass, ConvertColor(Colors.Teal));


            container.RegisterParticleType("CyclicAMP", mass, ConvertColor(Colors.Yellow)); // Register CyclicAMP
            container.RegisterParticleType("CAP", mass, ConvertColor(Colors.Orange)); // Register CAP
            container.RegisterParticleType("Promoter", mass, ConvertColor(Colors.Purple)); // Register Promoter (cAMP-CAP complex)
            container.RegisterParticleType("PromoterBoundLacOperon", mass, ConvertColor(Colors.LimeGreen));

            var generator = new BoltzmannGenerator(container, temperature, container.Dictionary.Map["LacOperon"]);

            const int nParticles = 1000;

            // Add particles
            container.AddRandomParticles(generator, "LacOperon", nParticles / 10);
            container.AddRandomParticles(generator, "RNAPolymerase", nParticles / 10);
            container.AddRandomParticles(generator, "CyclicAMP", nParticles / 5); // Add CyclicAMP
            container.AddRandomParticles(generator, "CAP", nParticles / 5); // Add CAP

            var visualization = new ChemicalVisualization(container)
            {
                BoxColor = Colors.IndianRed
            };

            var viz = new MotionVisualizer3DControl(visualization)
            {
                TimeIncrement = deltaTime,
                TimeScale = 1,
                SlowDraw = false
            };

            Timeline.MaximumPoints = 3000;

            AddChemicalGraphs(viz, container, visualization);

            viz.Manager.AddText("Time elapsed (s)", ConvertColor(Colors.Crimson), () => TimeElapsed().ToString());
            visualization.StopTime = 1;

            viz.Show();

            // Track BetaGal production
            List<int> betaGalCounts = new List<int>();
            double simulationTime = 5; // Run for 5 seconds
            double currentTime = 0;

            // Add the loop here

        }

        static internal void Step5()
        {
            
            const double containerSize = 25;
            const double deltaTime = 0.001; // Reduced deltaTime
            const double temperature = 293.17;
            const double reactionRadius = 2;

            var container = new ReactingParticleContainer(containerSize, reactionRadius);

            const double mass = 1e-24;

            // Register particle types
            container.RegisterParticleType("LacOperon", mass, ConvertColor(Colors.Green));
            container.RegisterParticleType("RNAPolymerase", mass, ConvertColor(Colors.Blue));
            container.RegisterParticleType("mRNA", mass, ConvertColor(Colors.Fuchsia));
            container.RegisterParticleType("BetaGal", mass, ConvertColor(Colors.Red));
            container.RegisterParticleType("Inhibitor", mass, ConvertColor(Colors.DarkRed));
            container.RegisterParticleType("BoundInhibitor", mass, ConvertColor(Colors.Purple));
            container.RegisterParticleType("Lactose", mass, ConvertColor(Colors.Yellow)); // Register Lactose
            container.RegisterParticleType("CyclicAMP", mass, ConvertColor(Colors.Yellow)); // Register CyclicAMP
            container.RegisterParticleType("CAP", mass, ConvertColor(Colors.Orange)); // Register CAP
            container.RegisterParticleType("Promoter", mass, ConvertColor(Colors.Purple)); // Register Promoter (cAMP-CAP complex)
            container.RegisterParticleType("PromoterBoundLacOperon", mass, ConvertColor(Colors.LimeGreen));
            container.RegisterParticleType("Glucose", mass, ConvertColor(Colors.Brown)); // Register Glucose

            var generator = new BoltzmannGenerator(container, temperature, container.Dictionary.Map["LacOperon"]);
            

            const int nParticles = 1000;

            // Add particles
            container.AddRandomParticles(generator, "LacOperon", nParticles / 10);
            container.AddRandomParticles(generator, "RNAPolymerase", nParticles / 10);
            container.AddRandomParticles(generator, "Inhibitor", nParticles / 5); // Add the inhibitor
            container.AddRandomParticles(generator, "Lactose", nParticles / 5); // Add Lactose
            container.AddRandomParticles(generator, "CyclicAMP", nParticles / 5); // Add CyclicAMP //Suggested by Astra AI
            container.AddRandomParticles(generator, "CAP", nParticles / 5); // Add CAP //Suggested by Astra AI
            container.AddRandomParticles(generator, "Glucose", nParticles / 5); // Add Glucose

            var visualization = new ChemicalVisualization(container)
            {
                BoxColor = Colors.IndianRed
            };

            var viz = new MotionVisualizer3DControl(visualization)
            {
                TimeIncrement = deltaTime,
                TimeScale = 1,
                SlowDraw = false
            };

            Timeline.MaximumPoints = 3000;

            AddChemicalGraphs(viz, container, visualization);
            viz.Manager.AddText("Time elapsed (s)", ConvertColor(Colors.Crimson), () => TimeElapsed().ToString());
            visualization.StopTime = 1;

            viz.Show();

            // Track BetaGal production
            List<int> betaGalCounts = new List<int>();
            double simulationTime = 5; // Run for 5 seconds
            double currentTime = 0;

            // Add the loop here //Suggested by Astra AI

            while (currentTime < simulationTime)
            {
                container.Update(deltaTime);
                Thread.Sleep(50); // Increased delay

                // Record BetaGal count
                betaGalCounts.Add(container.GetNParticles("BetaGal"));
                currentTime += deltaTime;
            }
            

            // Print BetaGal counts
            Console.WriteLine("BetaGal Counts (with cAMP, CAP, and Glucose):");
            foreach (int count in betaGalCounts)
            {
                Console.WriteLine(count);
            }
            
        }

        static internal void Step6()
        {
            Debug.WriteLine("Running Step 6: Testing with Glucose, Lactose, and Both");
            /*
            // (a) Glucose Only
            Debug.WriteLine("Scenario (a): Glucose Only");
            RunSimulation(glucoseOnly: true, lactoseOnly: false);
            
            // (b) Lactose Only
            Debug.WriteLine("Scenario (b): Lactose Only");
            RunSimulation(glucoseOnly: false, lactoseOnly: true);
            */
            // (c) Glucose and Lactose
            Debug.WriteLine("Scenario (c): Glucose and Lactose");
            RunSimulation(glucoseOnly: true, lactoseOnly: true);
        }

        private static void RunSimulation(bool glucoseOnly, bool lactoseOnly)
        {
            const double containerSize = 25;
            const double deltaTime = 0.001;
            const double temperature = 293.17;
            const double reactionRadius = 2;
            const double mass = 1e-24;
            const int nParticles = 1000;

            var container = new ReactingParticleContainer(containerSize, reactionRadius);

            // Register particle types
            container.RegisterParticleType("LacOperon", mass, ConvertColor(Colors.Green));
            container.RegisterParticleType("RNAPolymerase", mass, ConvertColor(Colors.Blue));
            container.RegisterParticleType("mRNA", mass, ConvertColor(Colors.Fuchsia));
            container.RegisterParticleType("BetaGal", mass, ConvertColor(Colors.Red));
            container.RegisterParticleType("Inhibitor", mass, ConvertColor(Colors.DarkRed));
            container.RegisterParticleType("BoundInhibitor", mass, ConvertColor(Colors.Purple));
            container.RegisterParticleType("Lactose", mass, ConvertColor(Colors.Yellow));
            container.RegisterParticleType("CyclicAMP", mass, ConvertColor(Colors.Yellow));
            container.RegisterParticleType("CAP", mass, ConvertColor(Colors.Orange));
            container.RegisterParticleType("Promoter", mass, ConvertColor(Colors.Purple));
            container.RegisterParticleType("PromoterBoundLacOperon", mass, ConvertColor(Colors.LimeGreen));
            container.RegisterParticleType("Glucose", mass, ConvertColor(Colors.Brown));

            var generator = new BoltzmannGenerator(container, temperature, container.Dictionary.Map["LacOperon"]);

            // Add particles
            container.AddRandomParticles(generator, "LacOperon", nParticles / 10);
            container.AddRandomParticles(generator, "RNAPolymerase", nParticles / 10);
            container.AddRandomParticles(generator, "Inhibitor", nParticles / 5);
            container.AddRandomParticles(generator, "CyclicAMP", nParticles / 5);
            container.AddRandomParticles(generator, "CAP", nParticles / 5);

            // Add Glucose and/or Lactose based on the scenario //Suggested by Astra AI
            if (glucoseOnly)
            {
                container.AddRandomParticles(generator, "Glucose", nParticles / 5);
            }
            if (lactoseOnly)
            {
                container.AddRandomParticles(generator, "Lactose", nParticles / 5);
            }
            if (glucoseOnly && lactoseOnly)
            {
                container.AddRandomParticles(generator, "Glucose", nParticles / 5);
                container.AddRandomParticles(generator, "Lactose", nParticles / 5);
            }

            var visualization = new ChemicalVisualization(container) { BoxColor = Colors.IndianRed };
            var viz = new MotionVisualizer3DControl(visualization) { TimeIncrement = deltaTime, TimeScale = 1, SlowDraw = false };
            Timeline.MaximumPoints = 3000;
            AddChemicalGraphs(viz, container, visualization);
            viz.Manager.AddText("Time elapsed (s)", ConvertColor(Colors.Crimson), () => TimeElapsed().ToString());
            visualization.StopTime = 1;
            viz.Show();
            
        }


        static private void AddChemicalGraphs(MotionVisualizer3DControl viz, ParticleContainer container,
            ChemicalVisualization visualization)
        {
            


            var timelineInfo = new List<TimelineInfo>();
          

            foreach (var info in container.Dictionary.Map.Values)
            {
                var prototype = new TimelinePrototype(info.Name, info.Color);
                //timelineInfo.Add(new TimelineInfo(prototype,
                //   new BasicFunctionPair(() => visualization.Time, () => container.GetNParticles(info.Name))));
                var timelineEntry = new TimelineInfo(prototype,//Suggested by Astra AI
                    new BasicFunctionPair(() => visualization.Time, () => container.GetNParticles(info.Name)));
                timelineInfo.Add(timelineEntry);
                
                Debug.WriteLine($"  TimelineInfo - Name: {info.Name}, Color: {info.Color}, Time: {visualization.Time}, Particle Count: {container.GetNParticles(info.Name)}");
            }

            viz.Manager.AddGraph(timelineInfo, "Time (s)", "Number of particles");
        }

        private static readonly Stopwatch watch = new();
        static private double TimeElapsed()
        {
            if (!watch.IsRunning)
            {
                watch.Start();
            }
            return watch.ElapsedMilliseconds / 1000.0;
        }
    }
}
