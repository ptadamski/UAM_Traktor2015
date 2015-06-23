using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraktorProj
{
    class Parametry
    {
        public enum RodzajUprawy { warzywo, zboze}


        public RodzajUprawy uprawa;
        public string pora="nie";
        public string susza="nie";
        public string mineraly="nie";
        public string zbior = "nie";
        public string zasiane = "nie";
        public string zaorane = "nie";
        public string bronowane = "nie";
        public string maszyna = "";
        public int poleX;
        public int poleY;
        public string rodzaj;
        public double wzrost=0;

      public  Parametry(RodzajUprawy r)
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

       


    }
}
