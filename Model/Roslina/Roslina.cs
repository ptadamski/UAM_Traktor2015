using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraktorProj.Interface;

namespace TraktorProj.Model
{
    public class Roslina : IRoslina
    {
        public enum Stadium { Brak = 5, Nasiono = 0, Mloda, Dojrzala, Przekwitla, Martwa }

        public Roslina(Dictionary<Substancja, float> zapasy, Dictionary<Substancja, float> wymaganiaZyciowe, Stadium stadiumZycia)
        {
            this.wymaganiaZyciowe = wymaganiaZyciowe;
            this.zapasy = zapasy;
            this.stadiumZycia = stadiumZycia; //level   
            this.etapZycia = 0;//dowiadczenie
        }

        Dictionary<Substancja, float> zapasy;

        private int etapZycia;

        private Stadium stadiumZycia;

        public Stadium StadiumZycia
        {
            get { return stadiumZycia; }
            set { stadiumZycia = value; }
        }

        private Dictionary<Substancja, float> wymaganiaZyciowe;

        public readonly Dictionary<Substancja, float> WymaganiaZyciowe
        {
            get { return wymaganiaZyciowe; }
        }
    }
}
