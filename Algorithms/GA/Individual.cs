using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public class Individual<_Locus, _Gen> : IIndividual<IChromosome<_Locus, _Gen>>
        where _Locus : Algebra.IArithmetic<_Locus>
    {
        public Individual(IChromosome<_Locus,_Gen> chromosome)
        {
            //this.mutateRate = mutateRate;
            this.chromosome = chromosome;
        }

        public virtual object Clone()
        {
            var result = new Individual<_Locus, _Gen>(null);
            result.chromosome = (IChromosome<_Locus, _Gen>) chromosome.Clone();
            return result;
        }

        public IChromosome<_Locus, _Gen> Chromosome
        {
            get { return chromosome; }
            set { chromosome = value; }
        }

        protected IChromosome<_Locus,_Gen> chromosome;
        //protected static Random mutateRandom = new Random();
        //protected double mutateRate;
    }
}
