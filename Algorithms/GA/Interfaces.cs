using System;
using System.Collections.Generic;
using System.IO;
//using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public interface IIndividual<_Chromosome> : ICloneable
    {
        _Chromosome Chromosome { get; set; }
    }

    public interface IFactory<_T,_A>
    {
        _T Create();
       // _T Create(_T copy);
        _T Create(_A args);

        _A Args { get; set; }
        _T Last { get; }
    }

    public interface IChromosome<_Locus, _Gen> : ICloneable
    {
        void Concatenate(IList<_Locus> loci, IList<_Gen> gens);
        void Concatenate(IChromosome<_Locus, _Gen> chromosome);
        void Split(_Locus locus, 
            out IChromosome<_Locus, _Gen> chromosome);
        void Mutate(_Locus locus);
        void Mutate(IList<_Locus> loci);
        void Populate(IList<_Locus> loci, _Gen sentry);
        void Mix(ICollection<_Locus> loci, IList<IChromosome<_Locus, _Gen>> genePool, IList<int> indices);

        ICollection<_Locus> Loci { get; }
        _Gen this[_Locus index] { get; set; }
    }

    public interface IReproducer<_Individual>
    {
        void Reproduce(IList<_Individual> parents,
            out IList<_Individual> children);
        void CreatePacks(IList<_Individual> population, int populationLimit,
            out IList<_Individual>[] packs);
    }

    public interface ISelector<_Individual>
    {
        void Select(IList<_Individual> population, IList<float> fitnessFactors, int populationLimit,
            out IList<_Individual> selectedPopulation);
    }

    public interface IFitnessFunc<_Chromosome>
    {
        float Fit(_Chromosome chromosome);
    }

    public interface IGeneticAlgorithm<_Individual>
    {
        bool Iterate(bool force = false);
        void Select(IList<_Individual> population, IList<float> fitnessFactors, 
            out IList<_Individual> selectedPopulation);
        void Populate(IList<_Individual> oldPopulation, int populationLimit, 
            out IList<_Individual> newPopulation);
        bool Evaluate(IList<_Individual> population,
            out IList<float> fitnessFactors, 
            out float fitnessOverall);
        bool Stop { get; }
        IList<_Individual> Population { get; }
        float Fitness { get; }
    }
}
