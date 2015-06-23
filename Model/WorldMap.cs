using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraktorProj.Interface;

namespace TraktorProj.Model
{
    public class WorldMap<_Location, _Item> 
    {
        private IDictionary<_Location, _Item> items;

        public _Item this[_Location position]
        {
            get
            {
                _Item obj;
                if (!items.TryGetValue(position, out obj))
                    throw new IndexOutOfRangeException();
                return obj; 
            }
            set
            {
                _Item obj;
                if (items.TryGetValue(position, out obj))
                    items[position] = value;
            }
        }
    


    }
}
