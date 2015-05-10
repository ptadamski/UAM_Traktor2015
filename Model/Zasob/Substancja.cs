using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorProj.Model
{
    public class Substancja : IEqualityComparer<Substancja>
    {
        //swiatlo, woda, azot, potas, siarka, wegiel org., tlen
        //swiatlo jest ideowo niewyczerpywalne, reszta tak, ale to jest jakas baza danych w pliku txt

        public Substancja(string nazwa, bool wyczerpywalnosc)
        {
            this.nazwa = nazwa;
            this.wyczerpywalnosc = wyczerpywalnosc;
        }

        private bool wyczerpywalnosc;

        public bool Wyczerpywalnosc
        {
            get { return wyczerpywalnosc; }
            set { wyczerpywalnosc = value; }
        }

        private string nazwa;

        public string Nazwa
        {
            get { return nazwa; }
            set { nazwa = value; }
        }

        #region impl IEqualityComparer<Zasob>

        public bool Equals(Substancja x, Substancja y)
        {
            return x.nazwa == y.nazwa;
        }

        public int GetHashCode(Substancja obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }
}
