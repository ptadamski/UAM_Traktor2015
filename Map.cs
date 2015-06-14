using GeneticAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorProj
{      
    public class Map<_Pos, _Ent>
    {
        IDictionary<_Pos, _Ent> entries;
        _Ent sentry;

        public Map(IList<_Pos> positions, IFactory<_Ent, _Pos> factory)
        {
            foreach (var pos in positions)
            {
                _Ent entry = factory.Create();
                if (!entries.TryGetValue(pos, out entry))
                    entries.Add(pos, entry);
            }
        }

        public _Ent this[_Pos pos]
        {
            get
            {
                _Ent entry;
                if (entries.TryGetValue(pos, out entry))
                    return entry;
                else
                    return sentry;
            }
            set 
            {
                _Ent entry;
                if (entries.TryGetValue(pos, out entry))
                    entries[pos] = value;
            }
        }
    }
}
