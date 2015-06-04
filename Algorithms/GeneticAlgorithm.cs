using System;
using System.Collections.Generic;
using System.IO;
//using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public interface IChromosome<Gen>
    {
        void Concat(Gen[] sequence, bool atomic = true);
        void Concat(int index, Gen[] sequence, bool atomic = true);
        IList<Gen> Cutoff();
        IList<Gen[]> Cutoff(int index);
        public int Count { get; }
        public Gen[] this[int index] { get; }
    }

    public interface IReproducer<Individual>
    {
        void Reproduce(IList<Individual> parents, 
            out IList<Individual> children);
        void Pairing(IDictionary<Individual, float> population, 
            out IList<IList<Individual>> pairs);
    }

    public interface ISelector<Individual>
	{
        void Select(IList<Individual> population, IList<float> fitnessFactors, 
            out IDictionary<Individual, float> selectedPopulation);
	}

    public interface IFitnessFunc<Individual>
    {
        float Fit(Individual individual);
    }

    public interface IGeneticAlgorithm<Individual>
    {
        void Iterate();
        void Select(IList<Individual> population, IList<float> fitnessFactors, 
            out IDictionary<Individual, float> selectedPopulation);
        void Populate(IDictionary<Individual, float> oldPopulation, int populationLimit,
            out IList<Individual> newPopulation);      
        bool Evaluate(IList<Individual> population, out IList<float> fitnessFactors);
        public bool Stop { get; }
        public IList<Individual> Population { get; }
    }      
}
