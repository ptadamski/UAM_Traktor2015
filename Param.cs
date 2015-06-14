using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorProj
{
    class Param
    {
        public enum PoraRoku { wiosna, lato, jesien };

        public PoraRoku pora;
        public string rozrost = "nie";
        public string chodnik = "nie";
        public string trawa = "nie";
        public string pole = "nie";
        public string uprawa = "nie";
        public string szkodnik= "";
        public int czas = 0;
        public string okres = "";

        public Param(PoraRoku p)
        {
            this.pora = p;
            if (p.Equals(PoraRoku.wiosna)){
                czas = 20;
                okres = "wiosna";
            }
            if (p.Equals(PoraRoku.lato)){
                czas = 10;
                okres = "lato";
            }
            if (p.Equals(PoraRoku.jesien)){
                czas = 30;
                okres = "jesien";
            }
        }
    }
}
