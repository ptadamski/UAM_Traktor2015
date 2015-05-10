using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TraktorProj.Interface;

namespace TraktorProj.Model
{
    //composite: compisite
    public class Traktor : ICiagnik
    {
        public Traktor(int maxLiczbaMaszyn, Pole pole)
        {
            this.maxLiczbaMaszyn = maxLiczbaMaszyn;
            this.pole = pole;
        }

        private Pole pole;

        private int idxMaszyna;

        private int maxLiczbaMaszyn;

        private int maxCzasPracy;

        private Point pozycja;

        private int paliwo;

        public int MaxLiczbaMaszyn
        {
            get { return maxLiczbaMaszyn; }
            set { maxLiczbaMaszyn = value; }
        }

        public int MaxCzasPracy
        {
            get { return maxCzasPracy; }
            set { maxCzasPracy = value; }
        }

        private List<IMaszynaRolnicza> maszynyRolnicze;

        public double X
        {
            get { return pozycja.X; }
            set { pozycja.X = value; }
        }

        public double Y
        {
            get { return pozycja.Y; }
            set { pozycja.Y = value; }
        }

        public int Paliwo
        {
            get { return paliwo; }
            set { paliwo = value; }
        }

        public void idz(Kierunek kierunek)
        {
            paliwo -= 1;
            pole.przesun(kierunek, ref pozycja); ;
        }

        public void pracuj()
        {
            foreach (var maszyna in maszynyRolnicze)
            {
                maszyna.pracuj();
            }
        }

        public void wez(IMaszynaRolnicza maszynaRolnicza)
        {
            if (maszynyRolnicze.Count < maxLiczbaMaszyn)
                maszynyRolnicze.Add(maszynaRolnicza);
            else
                maszynyRolnicze[idxMaszyna] = maszynaRolnicza;
            idxMaszyna = (idxMaszyna + 1) % maxLiczbaMaszyn;
        }

        public void odstaw(IMaszynaRolnicza maszynaRolnicza)
        {
            if (maszynyRolnicze.Contains(maszynaRolnicza))
            {
                maszynyRolnicze.Remove(maszynaRolnicza);
            }
            idxMaszyna = (idxMaszyna + idxMaszyna - 1) % maxLiczbaMaszyn;
        }
    }
}
