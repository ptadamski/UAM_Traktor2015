using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace generator
{

    public class Generator
    {                 
        private static int sentry = default(int);

        private Dictionary<int, InternalListing> items = new Dictionary<int, InternalListing>();

        public void ReadFromFile(string path)
        {
            string str;
            var reader = new StreamReader(path);
            var regex = new Regex(@"(\d*) (\d*) (\d*)");
            Data data;
            Match match;
            int src;
            InternalListing il;

            while ((str = reader.ReadLine()) != null)
            {
                match = regex.Match(str);
                data = new Data();
                                                             
                src = int.Parse(match.Groups[1].Value);
                data.dst = int.Parse(match.Groups[2].Value);
                data.votes = int.Parse(match.Groups[3].Value);

                try
                {
                    il = items[src];
                }
                catch (KeyNotFoundException ex)
                {
                    il = new InternalListing();
                    items.Add(src, il);
                }

                il.Items.Add(data);
            }

            foreach (var pair in items)
                pair.Value.Recount();

            reader.Close();
        }

        public void WriteToFile(string path)
        {
        }

        public int MoveNext(int state)
        {
            InternalListing obj;
            if (items.TryGetValue(state, out obj))
                return obj.MoveNext();
            return sentry;
        }

        public void Reset()
        {
            foreach (var pair in items)
                pair.Value.Reset();
        }

        public ICollection<int> States { get { return items.Keys; } }

        private class Data
        {
            public int dst;
            public int attempts;
            public int votes;
        }

        private class InternalListing
        {
            private static Random rand = new Random();

            public InternalListing()
            {
            }

            private int attempts = 0;
            private int votes = 0;

            private IList<Data> items = new List<Data>();

            public IList<Data> Items
            {
                get { return items; }
                set { items = value; }
            }

            public void Recount()
            {
                votes = 0;
                foreach (var item in items)
                    votes += item.votes;
            }

            public int MoveNext()
            {
                if (attempts == votes)
                    Reset();

                int k;
                do
                {
                    k = rand.Next(items.Count);
                } while (items[k].votes <= items[k].attempts);
                items[k].attempts++;
                attempts++;
                return items[k].dst;
            }

            public void Reset()
            {
                foreach (var item in items)
                    item.attempts = 0;
                attempts = 0;
            }
        }
    }
}
