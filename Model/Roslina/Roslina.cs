﻿using System;
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

        public Roslina()
        {
            this.wymaganiaZyciowe = new Dictionary<Substancja,float>();
            this.zapasy = new Dictionary<Substancja,float>();
            this.stadiumZycia = Stadium.Nasiono; //level   
            this.etapZycia = 0;//dowiadczenie
        }

        public Roslina(Dictionary<Substancja, float> zapasy, 
            Dictionary<Substancja, float> wymaganiaZyciowe, Stadium stadiumZycia)
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

        public Dictionary<Substancja, float> WymaganiaZyciowe
        {
            get { return wymaganiaZyciowe; }
        }

        public void zyj(TimeSpan czas, Podpole miejsce)
        {
            throw new NotImplementedException();
        }

        public void gin()
        {
            throw new NotImplementedException();
        }

        public void wprowadz(Dictionary<Substancja, int> substancje)
        {
            throw new NotImplementedException();
        }

        public void rosnij(Podpole miejsce)
        {
            throw new NotImplementedException();
        }

        public void rozmnazaj(Podpole miejsce, IRoslina potomek)
        {
            throw new NotImplementedException();
        }

        public void wydzielaj(Podpole miejsce, out Dictionary<Substancja, int> metabolity)
        {
            throw new NotImplementedException();
        }
    }
}
