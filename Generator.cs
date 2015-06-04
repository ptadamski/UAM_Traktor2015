using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TraktorProj
{
    public class Pair<T, S> : IEqualityComparer<Pair<T, S>>
    {
        public T First { get; set; }
        public S Second { get; set; }

        public bool Equals(Pair<T, S> x, Pair<T, S> y)
        {
            return x.First.Equals(y.First) && y.Second.Equals(y.Second);
        }

        public int GetHashCode(Pair<T, S> obj)
        {
            return obj.First.GetHashCode() ^ obj.Second.GetHashCode();
        }
    }

    public class Generator
    {
        public Generator(string path)
        {
            ReadFromFile(path);
        }

        public Generator(IDictionary<Pair<int, int>, int> inputItems)
        {
            InternalListing li;
            foreach (var input in inputItems)
            {
                try
                {
                    li = this.items[input.Key.First]; 
                }
                catch (KeyNotFoundException)
                {
                    li = new InternalListing();
                    this.items[input.Key.First] = li;
                }
                li.Items.Add(new Data() { attempts = 0, votes = input.Value, dst = input.Key.Second });
            }

            foreach (var pair in items)
                pair.Value.Recount();
        }

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
                catch (KeyNotFoundException)
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

        public ICollection<int> States 
        { get { return items.Keys; } }

        public int this[int state] 
        { get { return MoveNext(state); } }

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

            public  IList<Data> Items
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

            public Data this[int index]
            { get { return Items[index]; } }
        }
    }
}
