using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thermodynamics
{
    public class Equations
    {
        public Dictionary<string, double> TimeLabeledEquations { 
            get; 
            private set; 
        }
        public string Equation {
            get; 
        }
        public List<string> Reactants {
            get; 
        }
        public List<string> Products { 
            get;
        }
        public List<string> ApplyReactants { 
            get;
            set; 
        } 
        = new List<string>();
        public List<int> ReactantPositions {
            get; set; } = new List<int>();
        public bool ReactantsApplied => Reactants.All(reactant => ApplyReactants.Contains(reactant));

        public Equations(Dictionary<string, double> equations)
        {
            TimeLabeledEquations = new Dictionary<string, double>(equations);
        }

        public Equations(string equation)
        {
            Equation = equation;
            var parts = equation.Split(" -> ");
            Reactants = parts[0].Split(" + ").ToList();
            Products = parts[1].Split(" + ").ToList();
        }

        public Equations(Equations other) : this(other.Equation) { }

        public static List<List<List<string>>> PopulateLists(Dictionary<string, double> equations)
        {
            var allReactants = new List<List<string>>();
            var allProducts = new List<List<string>>();

            foreach (var equation in equations.Keys)
            {
                var parts = equation.Split(" -> ");
                allReactants.Add(parts[0].Split(" + ").ToList());
                allProducts.Add(parts[1].Split(" + ").ToList());
            }

            return new List<List<List<string>>> { allReactants, allProducts };
        }
    }
}


