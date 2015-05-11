using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorProj.Model
{
    public class Podpole //magazyn zasobow
    {
        //albo Fractional zamiast int ?
        //private CzarnaSkrzynka<Substancja, Substancja, int>[] przemiany;

        private Dictionary<Substancja, int> zasoby;

        public Dictionary<Substancja, int> Zasoby
        {
            get { return zasoby; }
        }

        void pobierz(Substancja typZasobu, int wymagane, out int dostepne)
        {
            dostepne = 0;
            if (typZasobu.Wyczerpywalnosc)
            {
                if (zasoby.TryGetValue(typZasobu, out dostepne))
                {
                    if (dostepne > wymagane)
                        dostepne = wymagane;
                    zasoby[typZasobu] -= dostepne;
                };

            }
            else
                dostepne = wymagane;
        }

        void dostarcz(Substancja typZasobu, int dostarczone, out int przyswojone)
        {
            przyswojone = dostarczone;
            zasoby[typZasobu] += dostarczone;
        }
    }
}
