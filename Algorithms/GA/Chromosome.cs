using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public class Chromosome<_Locus, _Gen> : IChromosome<_Locus, _Gen>
        where _Locus : Algebra.IArithmetic<_Locus>
    {
        static private IList<_Gen> genotype = new List<_Gen>();

        static public IList<_Gen> Genotype
        {
            get { return genotype; }
            set { genotype = value; }
        }
                              
        public Chromosome()
        {
        }

        public Chromosome(IList<_Gen> gens, IList<_Locus> loci)
        {
            this.Concatenate(loci, gens);
        }

        public Chromosome(IList<_Locus> loci, _Gen initializer)
        {
            this.Populate(loci, initializer);
        }                                                   

        public void Concatenate(IChromosome<_Locus, _Gen> chromosome) 
        {
            _Locus maxLocus = sequence.FirstOrDefault().Key;
            foreach (var pair in sequence)
                maxLocus = pair.Key.CompareTo(maxLocus) > 0 ? pair.Key : maxLocus;


            foreach (var locus in chromosome.Loci)
            {
                try
                {
                    sequence[locus.Add(maxLocus)] = chromosome[locus];
                }
                catch (KeyNotFoundException)
                {
                    sequence.Add(locus, chromosome[locus]);
                }
            }
        }

        public void Concatenate(IList<_Locus> loci, IList<_Gen> gens)
        {                        
            _Locus maxLocus = sequence.FirstOrDefault().Key;
            foreach (var pair in sequence)
                maxLocus = pair.Key.CompareTo(maxLocus) > 0 ? pair.Key : maxLocus;

            var length = Math.Min(loci.Count, gens.Count);
            for (int i = 0; i < length; i++)
            {
                try
                {
                    sequence[loci[i].Add(maxLocus)] = gens[i];
                }
                catch (KeyNotFoundException)
                {
                    sequence.Add(loci[i], gens[i]);
                }
            }
        }

        public void Split(_Locus locus, out IChromosome<_Locus, _Gen> chromosome)
        {

            IList<_Gen> gens = new List<_Gen>();
            IList<_Locus> loci = new List<_Locus>();

            foreach (var pair in sequence)
            {
                if (pair.Key.CompareTo(locus) > 0)
                {
                    loci.Add(pair.Key);
                    gens.Add(pair.Value);
                }
            }

            foreach (var loc in loci)
                sequence.Remove(loc);

            chromosome = new Chromosome<_Locus, _Gen>(gens, loci);
        }

        public void Cut(_Locus locus) 
        {     
            IList<_Gen> gens = new List<_Gen>();
            IList<_Locus> loci = new List<_Locus>();

            foreach (var pair in sequence)
                if (pair.Key.CompareTo(locus) > 0)
                    sequence.Remove(pair.Key);
        }
                        
        public void Populate(IList<_Locus> loci, _Gen sentry)
        {
            foreach (var locus in loci)
            {
                try
                {
                    sequence[locus] = sentry;
                }
                catch (KeyNotFoundException)
                {
                    sequence.Add(locus, sentry);
                }    
            }
        }
                         
        public void Mutate(_Locus locus)
        {
            _Gen gen;
            var r = genotypeRandom.Next(genotype.Count);
            if (sequence.TryGetValue(locus, out gen))
                sequence[locus] = genotype[r];
            //else
            //    sequence.Add(locus, genotype[r]);
        }
                              
        public void Mutate(IList<_Locus> loci)
        {       
            foreach (var locus in loci)
            {                
                var r = genotypeRandom.Next(genotype.Count);
                try
                {
                    sequence[locus] = genotype[r];
                }
                catch (KeyNotFoundException)
                {
                    sequence.Add(locus, genotype[r]);
                }
            }

        }

        public void Randomize() 
        {
            sequence.Clear();
            foreach (var pair in sequence)
                sequence[pair.Key] = genotype[genotypeRandom.Next(genotype.Count)];
        }
               
        public object Clone()
        {
            Chromosome<_Locus, _Gen> result = new Chromosome<_Locus, _Gen>();
            //moze dodac jeszcze jeden typ genetyczny, zeby genotyp byl statyczny
            foreach (var pair in sequence)
            {
                try
                {
                    result.sequence[pair.Key] = pair.Value;
                }
                catch (KeyNotFoundException)
                {
                    result.sequence.Add(pair.Key, pair.Value);
                }
            }
            return result;
        }

        //public IEnumerator<KeyValuePair<_Locus, _Gen>> GetEnumerator()
        //{
        //    return sequence.GetEnumerator();
        //}

        //System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        //{
        //    return sequence.GetEnumerator();
        //}

        public void Mix(ICollection<_Locus> loci, IList<IChromosome<_Locus, _Gen>> genePool, IList<int> indices) 
        {
            sequence.Clear();

            int i = 0;
            foreach (var locus in loci)
            {               
                var index = indices[i % indices.Count] % genePool.Count;
                sequence.Add(locus, genePool[index][locus]);
                i++;
            }
        }

        public ICollection<_Locus> Loci { get { return sequence.Keys; } }
   
        public _Gen this[_Locus index]
        {
            get { return sequence[index]; }
            set { sequence[index] = value; }
        }

        private static Random genotypeRandom = new Random();
        private IDictionary<_Locus,_Gen> sequence = new Dictionary<_Locus,_Gen>();
    }
}
