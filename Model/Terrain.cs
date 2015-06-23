using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraktorProj.Commons;

namespace TraktorProj.Model
{
    public class Terrain<_Item, _Location>
    {
        public Terrain(IDictionary<_Location, _Item> items)
        {
            this.items = items;
        }

        protected IDictionary<_Location, _Item> items;

        public _Item this[_Location index]
        {
            get 
            {
                _Item item;
                if (items.TryGetValue(index, out item))
                    return item;
                return default(_Item);
                //throw new IndexOutOfRangeException();
            }
            set
            {
                _Item item;
                if (items.TryGetValue(index, out item))
                    items[index] = value;
                else
                    throw new IndexOutOfRangeException();
            }
        }

        protected _Location size;

        public _Location Size
        {
            get { return size; }
            set { size = value; }
        }
    }


    public class Terrain2 : Terrain<int, Pos2>
    {          
        public Terrain2(IDictionary<Pos2, int> items):
            base(items)
        {
        }

        public Terrain2(int height, int width, int value) :
            base(new Dictionary<Pos2, int>(new Pos2()))
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    items.Add(new Pos2(i, j), value);
            this.size = new Pos2(width, height);
        }
        
        public Terrain2(string filepath) :
            base(new Dictionary<Pos2, int>(new Pos2()))
        {
            ReadFromFile(filepath);
        }

        void ReadFromFile(string filepath)
        {
            CSV csv = new CSV(filepath, ',', false);
                                                     
            size = new Pos2(0, 0);

            items.Clear();

            for (int i = 0; i < csv.Table.Rows.Count; i++)
            {
                for (int j = 0; j < csv.Table.Rows[i].ItemArray.Length; j++)
                {
                    items.Add(new Pos2(j, i), int.Parse(csv.Table.Rows[i].ItemArray[j] as string));
                    size.X = Math.Max(size.X, i);
                    size.Y = Math.Max(size.Y, j);
                }
            }
            size.X += 1;
            size.Y += 1;
        }

        void SaveToFile(string aFilepath)
        {
        }
    }
}
