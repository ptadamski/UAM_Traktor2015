using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TraktorProj.Interface;

namespace TraktorProj.Model
{
    public class Pole
    {
        public Pole(int wysokosc, int szerokosc)
        {
            this.podpola = new Podpole[wysokosc, szerokosc];
            this.liczbaKolumn = szerokosc;
            this.liczbaWierszy = wysokosc;
        }

        private int liczbaWierszy;

        private int liczbaKolumn;

        public bool przesun(Kierunek kierunek, ref Point pozycja)
        {
            switch (kierunek)
            {
                case Kierunek.Prawo:
                    if (pozycja.X < liczbaKolumn)
                    {
                        pozycja.X += 1.0;
                        return true;
                    }
                    break;
                case Kierunek.Lewo:
                    if (pozycja.X >= 0)
                    {
                        pozycja.X -= 1;
                        return true;
                    }
                    break;
                case Kierunek.Gora:
                    if (pozycja.Y < liczbaWierszy)
                    {
                        pozycja.Y += 1;
                        return true;
                    }
                    break;
                case Kierunek.Dol:
                    if (pozycja.Y >= 0)
                    {
                        pozycja.Y -= 1;
                        return true;
                    }
                    break;
            }
            return false;
        }

        private Podpole[,] podpola;

        public Podpole[,] Podpola
        {
            get { return podpola; }
            set { podpola = value; }
        }
    }
}
