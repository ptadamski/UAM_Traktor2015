using GeneticAlgorithm;
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorProj
{
    public class FieldFatory : IFactory<Field, Point2i>
    {
        private Point2i args;
        private Field last;

        public FieldFatory(Point2i args)
        {
            this.args = args;
        }

        public Field Create(Point2i args)
        {
            last = new Field(args.X, args.Y, false);
            return last;
        }

        public Field Create()
        {
            last = new Field(args.X, args.Y, false);
            return last;
        }

        public Point2i Args
        {
            get
            {
                return args;
            }
            set
            {
                args = value;
            }
        }

        public Field Last
        {
            get { return last; }
        }
    }

    public class Field : Individual<Point2i, IPlant>
    {
        public Field(int rowCount, int colCount, bool randomize = true)
            : base(new Chromosome<Point2i, IPlant>())
        {
            this.rowCount = rowCount;
            this.colCount = colCount;
            if (randomize)  
                this.Init();
        }

        private void Init()
        {
            var positions = new Point2i[rowCount * colCount];
            for (int i = 0, k = 0; i < rowCount; i++)
                for (int j = 0; j < colCount; j++, k++)
                    positions[k] = new Point2i(i, j);

            //chromosome.Populate(positions, new Plant(""));
            chromosome.Mutate(positions);
        }

        private int rowCount;
        private int colCount;

        public override object Clone() 
        {
            Field result = new Field(rowCount, colCount);
            result.rowCount = rowCount;
            result.colCount = colCount;
            result.chromosome = (IChromosome<Point2i, IPlant>)chromosome.Clone();
            return result;
        }

        public override string ToString() 
        {
            StringBuilder sb = new StringBuilder();

            var fields = new string[rowCount, colCount];
            var counter = new Dictionary<string, int>();

            foreach (var locus in Chromosome.Loci)
            {                                                 
                fields[locus.X, locus.Y] = Chromosome[locus].Name;
                try
                {
                    counter[fields[locus.X, locus.Y]]++;
                }
                catch (KeyNotFoundException)
                {
                    counter.Add(fields[locus.X, locus.Y], 1);
                }
            }

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    sb.AppendFormat("{0},", fields[i, j]);
                }
                sb.AppendLine();
            }
            sb.AppendLine();

            foreach (var pair in counter)
            {
                sb.AppendFormat("{0} {1}, ", pair.Key, pair.Value);
            }
            sb.AppendLine();


            return sb.ToString();
        }
    }


    public class Plant : IPlant
    {
        public Plant(string name)
        {
            this.name = name;
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

    public interface IPlant 
    {
        string Name { get; set; }
    }
}
