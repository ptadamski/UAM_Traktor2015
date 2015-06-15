using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraktorProj
{
    class Parametry
    {
        public enum RodzajUprawy { warzywo, zboze }

        static Random random = new Random();

        public RodzajUprawy uprawa;
        public string pora = "nie";
        public string susza = "nie";
        public string mineraly = "nie";
        public string zbior = "nie";
        public string zasiane = "nie";
        public string zaorane = "nie";
        public string bronowane = "nie";
        public string maszyna = "";
        public int poleX;
        public int poleY;
        public string rodzaj;
        public double wzrost = 0;

        public Parametry(RodzajUprawy r)
        {
            this.uprawa = r;
            if (r.Equals(RodzajUprawy.warzywo))
            {
                poleX = 3;
                poleY = 2;
                rodzaj = "warzywo";
            }
            if (r.Equals(RodzajUprawy.zboze))
            {
                poleX = 4;
                poleY = 6;
                rodzaj = "zboze";
            }

        }

        public Parametry(string rodzaj, int px, int py)
        {
            this.poleX = px;
            this.poleY = py;
            this.rodzaj = rodzaj;
            if (rodzaj == "warzywo")
            {
                this.uprawa = RodzajUprawy.warzywo;
            }
            else if (rodzaj == "zboze")
            {
                this.uprawa = RodzajUprawy.zboze;
            }
        }

        public void Randomize()
        {
            for (int i = 0; i < 6; i++)
            {
                var r = random.NextDouble() > 0;
                var s = r ? @"tak" : @"nie";

                switch (i)
                {
                    case 0: 
                        bronowane = s; //
                        break;
                    case 1:
                        susza = s;//
                        break;
                    case 2:
                        mineraly = s;//
                        break;
                    case 3:
                        zbior = s;//
                        break;
                    case 4:
                        zasiane = s; //
                        break;
                    case 5:
                        zaorane = s; //
                        break;
                }
            }
        }
    }
}
