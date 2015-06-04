using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using TraktorProj;

namespace GeneticAlgorithm
{
    public class CrossOverReproducer<_Individual, _Locus, _Gen> : IReproducer<_Individual>
        where _Individual : IIndividual<IChromosome<_Locus,_Gen>>
    {
        public CrossOverReproducer(double mutationRate, IFactory<_Individual, _Locus> breeder, int locusCount)
        {
            this.mutationRate = mutationRate;
            this.breeder = breeder;
            //this.splicer = splicer;
            this.Init(2, 2, locusCount);
        }

        public void Init(int parentCount, int childrenCount, int locusCount)
        {
            Dictionary<Pair<int, int>, int> input = new Dictionary<Pair<int, int>, int>();

            for (int i = 0; i < parentCount; i++)
                for (int j = 0; j < childrenCount; j++)
                    input.Add(new Pair<int, int>() { First = j, Second = i }, locusCount); //czy na pewno 1 glos? czemu nie wiecej np liczebnosc loci

            reproductionRandomizer = new Generator(input);
        }

        private static Random mutationRandom = new Random();
        private static Random pairingRandom = new Random();
        private double mutationRate = 0.0;
        private IFactory<_Individual, _Locus> breeder;
        //private IFactory<IChromosome<Locus,Gen>> splicer;
        private Generator reproductionRandomizer;
        private Random reproductionRandom = new Random();

        /// <summary>
        /// tyle ile parentow, tyle chidrenow
        /// </summary>
        /// <param name="parents"></param>
        /// <param name="children"></param>
        public void Reproduce(IList<_Individual> parents, out IList<_Individual> children)
        {
            if (parents.Count==0)
	{
                throw new Exception("No Parent");
            }

            children = new List<_Individual>();

            var genePool = new List<IChromosome<_Locus,_Gen>>();
            foreach (var parent in parents)
                genePool.Add(parent.Chromosome);

            for (int i = 0; i < parents.Count; i++)
            {                                                    
                var child = breeder.Create();
                children.Add(child);

                var loci = parents[i].Chromosome.Loci;

                var indices = new List<int>();
                for (int j = 0; j < loci.Count; j++)
                    indices.Add(reproductionRandomizer.MoveNext(i));

                child.Chromosome.Mix(loci, genePool, indices);

                var childLoci = child.Chromosome.Loci.ToList();
                foreach (var locus in loci)
                {                               
                    if (mutationRandom.NextDouble() <= mutationRate)   
                        child.Chromosome.Mutate(locus);
                }
            }
        }

        public void CreatePacks(IList<_Individual> population, int populationLimit, out IList<_Individual>[] packs)
        {
            packs = new List<_Individual>[populationLimit/2];
            for (int i = 0; i < packs.Length; i++)
                packs[i] = new List<_Individual>();

            for (int i = 0; i < packs.Length; i++)
            {
                var r1 = pairingRandom.Next(population.Count);
                var r2 = pairingRandom.Next(population.Count);
                packs[i].Add(population[r1]);
                packs[i].Add(population[r2]);
            }

            //int r=0;
            //foreach (var individual in population)
            //{                              
            //    packs[r].Add(individual);
            //}
        }
    }

}
