using GeneticAlgorithm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TraktorProj
{
    public class MarketPrice
    {
        public decimal Buy { get; set; }
        public decimal Sell { get; set; }
    }

    public class Market : IFitnessFunc<Field>
    {
        public Market(IDictionary<string, MarketPrice> stock)
        {
            this.stock = stock;
        }

        public Market(string filePath)
        {
            ReadFromFile(filePath);
        }

        public void ReadFromFile(string filePath)
        {
            string str;
            var reader = new StreamReader(filePath);
            var regex = new Regex(@"^([a-zA-Z0-9]*) (-?\d*) (-?\d*)");
            Match match;
            string name;
            MarketPrice data;

            while ((str = reader.ReadLine()) != null)
            {
                match = regex.Match(str);
                data = new MarketPrice();

                name = match.Groups[1].Value;
                data.Buy = decimal.Parse(match.Groups[2].Value);
                data.Sell = decimal.Parse(match.Groups[3].Value);
                stock.Add(name, data);
            }

            reader.Close();
        }

        IDictionary<string, MarketPrice> stock = new Dictionary<string, MarketPrice>();

        void TryBuy(IList<string> items, IList<int> amounts, out decimal price)
        {
            price = 0;
            if (items.Count != amounts.Count)
                return;

            for (int i = 0; i < items.Count; i++)
                price += stock[items[i]].Buy * amounts[i];
        }

        void TrySell(IList<string> items, IList<int> amounts, out decimal price)
        {
            price = 0;
            if (items.Count != amounts.Count)
                return;

            for (int i = 0; i < items.Count; i++)
                price += stock[items[i]].Sell * amounts[i];
        }

        public IList<string> Products { get { return stock.Keys.ToList(); } }

        public float Fit(Field individual)
        {
            var plants = new List<string>();
            var amounts = new List<int>();

            foreach (var locus in individual.Chromosome.Loci)
            {                
                amounts.Add(1);
                plants.Add(individual.Chromosome[locus].Name );
            }

            //for (int i = 0; i < individual.Rows; i++)
            //    for (int j = 0; j < individual.Cols; j++)
            //        foreach (var plant in individual[i, j].Plants)  
            //{
            //    amountList.Add(1);
            //    buyList.Add(plant.Name);
            //    sellList.Add(plant.Name);
            //}

            decimal cost, profit;
            TryBuy(plants, amounts, out cost);
            TrySell(plants, amounts, out profit);

            return (float)((profit - 2 * cost) / cost);
        }
    }
}
