using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorProj.Model
{
    public class Resource : IEqualityComparer<Resource>
    {
        //swiatlo, woda, azot, potas, siarka, wegiel org., tlen
        //swiatlo jest ideowo niewyczerpywalne, reszta tak, ale to jest jakas baza danych w pliku txt

        public Resource(string name, bool isInfinite = false)
        {
            this.name = name;
            this.isInfinite = isInfinite;
        }

        private bool isInfinite;

        public bool IsInfinite
        {
            get { return isInfinite; }
            set { isInfinite = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #region impl IEqualityComparer<Zasob>

        public bool Equals(Resource x, Resource y)
        {
            return x.name == y.name;
        }

        public int GetHashCode(Resource obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }
}
