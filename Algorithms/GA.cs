using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraktorProj.Model;
using GeneticAlgorithm;

namespace TraktorProj.Algorithms
{
    public class GeneticAlgorithm<Individual> : IGeneticAlgorithm<Individual>
    {
        public GeneticAlgorithm(IList<Individual> population, IReproducer<Individual> reproducer,
            ISelector<Individual> selector, IFitnessFunc<Individual> fitness)
        {
            this.population = population;
            this.reproducer = reproducer;
            this.selector = selector;
            this.fitness = fitness;
        }

        private ISelector<Individual> selector;
        private IReproducer<Individual> reproducer;
        private IFitnessFunc<Individual> fitness;

        private bool stop;

        public bool Stop { get{ return stop; } }

        private IList<Individual> population;

        public IList<Individual> Population { get { return population; } }

        public void Iterate(bool force = false)
        {
            IList<float> fitnessFactors = new List<float>();
            IDictionary<Individual, float> part = new Dictionary<Individual, float>();

            stop = Evaluate(population, fitnessFactors);
            if (!stop || force)
            {
                Select(population, fitnessFactors, out part);
                Populate(part, population.Count, out population);
            }
        }

        public bool Evaluate(IList<Individual> population, IList<float> fitnessFactors)
        {
            if (population == null)
                throw new Exception();

            if (fitnessFactors == null)
                throw new Exception();

            fitnessFactors.Clear();
            foreach (var individual in population)
                fitnessFactors.Add(fitness.Fit(individual));

            return false;
        }

        public void Select(IList<Individual> population, IList<float> fitnessFactors,
            out IDictionary<Individual, float> selectedPopulation)
        {
            if (population == null)
                throw new Exception();

            //if (selectedPopulation == null)
            //    throw new Exception();

            selectedPopulation = new Dictionary<Individual, float>();
            selector.Select(population, fitnessFactors, out selectedPopulation);
        }

        public void Populate(IDictionary<Individual, float> oldPopulation, int populationLimit,
            out IList<Individual> newPopulation)
        {
            if (oldPopulation == null)
                throw new Exception();

            //if (newPopulation == null)
            //    throw new Exception();

            newPopulation = new List<Individual>();

            IList<IList<Individual>> pairs;
            IList<Individual> children;

            reproducer.Pairing(oldPopulation, out pairs);

            for (int i = 0; i < pairs.Count && newPopulation.Count < populationLimit; i++)
            {
                reproducer.Reproduce(pairs[i], out children);
                var length = children.Count + newPopulation.Count < populationLimit ?
                    children.Count : populationLimit - newPopulation.Count;
                for (int j = 0; j < length; j++)
                    newPopulation.Add(children[j]);
            }
        }
    }
}
