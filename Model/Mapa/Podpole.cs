using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorProj.Model
{
    public class Podpole //magazyn zasobow i roslin
    {
        //albo Fractional zamiast int ?
        //private CzarnaSkrzynka<Substancja, Substancja, int>[] przemiany;

        private Dictionary<Resource, int> zasoby;

        public Dictionary<Resource, int> Zasoby
        {
            get { return zasoby; }
        }

        private List<Roslina> rosliny;

        public List<Roslina> Rosliny
        {
            get { return rosliny; }
            set { rosliny = value; }
        }

        void pobierz(Resource typZasobu, int wymagane, out int dostepne)
        {
            dostepne = 0;
            if (typZasobu.IsInfinite)
            {
                if (zasoby.TryGetValue(typZasobu, out dostepne))
                {
                    if (dostepne > wymagane)
                        dostepne = wymagane;
                    zasoby[typZasobu] -= dostepne;
                }
            }
            else
                dostepne = wymagane;
        }

        void dostarcz(Resource typZasobu, int dostarczone, out int przyswojone)
        {
            przyswojone = dostarczone;
            zasoby[typZasobu] += dostarczone;
        }
    }
}
