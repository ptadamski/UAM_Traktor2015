using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public class GA<_Individual,_Locus,_Gen> : IGeneticAlgorithm<_Individual>
        where _Individual : IIndividual<IChromosome<_Locus,_Gen>>
        where _Locus : Algebra.IArithmetic<_Locus>
    {
        public GA(IList<_Individual> population, IReproducer<_Individual> reproducer,
            ISelector<_Individual> selector, IFitnessFunc<_Individual> fitness, int selectionLimit)
        {
            this.population = population;
            this.reproducer = reproducer;
            this.selector = selector;
            this.fitness = fitness;
            this.selectionLimit = selectionLimit;
        }

        private ISelector<_Individual> selector;
        private IReproducer<_Individual> reproducer;
        private IFitnessFunc<_Individual> fitness;
        private int selectionLimit;

        private bool stop;

        public bool Stop { get { return stop; } }

        private IList<_Individual> population;

        public IList<_Individual> Population { get { return population; } }

        public bool Iterate(bool force = false)
        {
            IList<float> fitnessFactors = new List<float>();
            IList<_Individual> part = new List<_Individual>();

            //float fitnessOverall = 0.0f;

            stop = Evaluate(population, out fitnessFactors, out fitnessOverall);
            if (!stop || force)
            {
                Select(population, fitnessFactors, out part);
                Populate(part, population.Count, out population);
            }

            return !stop || force; 
        }

        public bool Evaluate(IList<_Individual> population, out IList<float> fitnessFactors, out float fitnessOverall)
        {
            fitnessFactors = new List<float>();
            foreach (var individual in population)
                fitnessFactors.Add(fitness.Fit(individual));

            fitnessOverall=0.0f;
            foreach (var fit in fitnessFactors)
                fitnessOverall += fit;
            fitnessOverall /= fitnessFactors.Count;
            return fitnessOverall >= 1.0;
        }

        public void Select(IList<_Individual> population, IList<float> fitnessFactors,
            out IList<_Individual> selectedPopulation)
        {
            selector.Select(population, fitnessFactors, selectionLimit, out selectedPopulation);
        }

        public void Populate(IList<_Individual> oldPopulation, int populationLimit,
            out IList<_Individual> newPopulation)
        {
            newPopulation = new List<_Individual>();

            IList<_Individual>[] packs;
            IList<_Individual> children;

            reproducer.CreatePacks(oldPopulation, populationLimit, out packs);

            for (int i = 0; i < packs.Length && newPopulation.Count < populationLimit; i++)
            {
                reproducer.Reproduce(packs[i], out children);
                var length = children.Count + newPopulation.Count < populationLimit ?
                    children.Count : populationLimit - newPopulation.Count;
                for (int j = 0; j < length; j++)
                    newPopulation.Add(children[j]);
            }
        }

        private float fitnessOverall = 0.0f;

        public float Fitness
        {
            get { return fitnessOverall; }
        }
    }
}
