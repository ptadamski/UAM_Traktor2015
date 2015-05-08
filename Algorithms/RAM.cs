using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorProj.Algorithms
{
    class RAM
    {
        private MyStruct myStruct;
        private Parametr parametr;
        private Gleba gleba;
        private Warzywo warzywo;
        private Zboze zboze;
        private Roslina roslina;
        private Uprawa uprawa;
        private Chwast chwast;
        private Podpole podpole;
        private Pole pole;
        private Maszyna maszyna;
        private Traktor traktor;
        private Siewnik siewnik;
        private Sadzarka sadzarka;
        private Plug plug;
        private RozrzutnikNawozu rozrzutnikNawozu;
        private Deszczownia desczownia;
        private Brony brony;

        public RAM()
        {
            myStruct = new MyStruct();
            parametr = new Parametr();
            gleba = new Gleba();
            warzywo = new Warzywo();
            zboze = new Zboze();
            roslina = new Roslina();
            uprawa = new Uprawa();
            chwast =  new Chwast();
            podpole = new Podpole();
            pole =  new Pole();
            maszyna = new Maszyna();
            traktor = new Traktor();
            siewnik = new Siewnik();
            sadzarka = new Sadzarka();
            plug = new Plug();
            rozrzutnikNawozu = new RozrzutnikNawozu();
            desczownia = new Deszczownia();
            brony = new Brony();
        }

        public void posadz(float f)
        {
            myStruct.sadzonki += f;
        }

        public void zbierz()
        {
            myStruct.zebranych += 1;
        }

        public void zarob(float f)
        {
            myStruct.finanse += f;
        }

        private struct MyStruct
        {
            MyStruct(float f, int i, float ff)
            {
                sadzonki = f;
                zebranych = i;
                finanse = ff;
            }
            public float sadzonki;
            public int zebranych;
            public float finanse;
        }

        private struct Parametr
        {
            Parametr(DateTime pz, int coip,int piw, int pin,
                DateTime pss, float sz, bool wn, int cin, Gleba wg){
            
                poraZbioru = pz;
                coIlePodlewac = coip;
                potrzebnaIloscWody = piw;
                potrzebnaIloscNawozu = pin;
                poraSadzeniaSiewu = pss;
                stopienZachwaszczenia = sz;
                wymaganeNaslonecznienie = wn;
                coIleNawozic = cin;
                wymaganaGleba = wg;
            }
            public DateTime poraZbioru;
            public int coIlePodlewac;
            public int potrzebnaIloscWody;
            public int potrzebnaIloscNawozu;
            public DateTime poraSadzeniaSiewu;
            public float stopienZachwaszczenia;
            public bool wymaganeNaslonecznienie;
            public int coIleNawozic;
            public Gleba wymaganaGleba;
        }

        private struct Gleba
        {
            Gleba(String r)
            {
                rodzajGleby = r;
            }
            public String rodzajGleby;
        }

        private struct Warzywo
        {
            Warzywo(RodzajWarzywo r)
            {
                rodzajWazywo = r;
            }
            public enum RodzajWarzywo
            {
                Ziemniak, Ogorek, Burak, Marchew
            }
            public RodzajWarzywo rodzajWazywo;
        }

        private struct Zboze
        {
            Zboze(RodzajZboze r)
            {
                rodzaj = r;
            }
            public enum RodzajZboze
            {
                Zyto, Pszenica, Jeczmien
            }
            public RodzajZboze rodzaj;
        }

        private struct Uprawa
        {
            Uprawa(Parametr p, Zboze z, Warzywo w)
            {
                parametr = p;
                zboze = z;
                warzywo = w;
            }
            public Parametr parametr;
            public Zboze zboze;
            public Warzywo warzywo;
        }

        private struct Roslina
        {

            Roslina(Chwast ch, Uprawa up)
            {
                uprawa = up;
                chwast = ch;
            }
            public Uprawa uprawa;
            public Chwast chwast;
        }

        private struct Chwast
        {
            Chwast(String r)
            {
                rodzaj = r;
            }
            public String rodzaj;
        }

        private struct Podpole
        {
            Podpole(Roslina r, float n, Gleba g)
            {
                roslina = r;
                naslonecznienie = n;
                gleba = g;
            }
            public Roslina roslina;
            public float naslonecznienie;
            public Gleba gleba;
        }

        private struct Pole
        {
            Pole(Podpole pp)
            {
                podpole = pp;
            }
            public Podpole podpole;
        }
        private struct Maszyna
        {
            Maszyna(int cp, RozrzutnikNawozu r, Brony b, Deszczownia d, Siewnik s, Sadzarka sa, Plug pl)
            {
                czasPracy = cp;
                rozrzutnikNawozu = r;
                brony = b;
                deszczownia = d;
                siewnik = s;
                sadzarka = sa;
                plug = pl;
            }
            public int czasPracy;
            public RozrzutnikNawozu rozrzutnikNawozu;
            public Brony brony;
            public Deszczownia deszczownia;
            public Siewnik siewnik;
            public Sadzarka sadzarka;
            public Plug plug;


        }

        private struct Traktor
        {
            Traktor(Maszyna m, int mcp, float pal)
            {
                maszyna = m;
                maxCzasPracy = mcp;
                paliwo = pal;
            }
            public Maszyna maszyna;
            public int maxCzasPracy;
            public float paliwo;
        }
        private struct Siewnik
        {
            Siewnik(Zboze z)
            {
                siejeZboze = z;
            }
            public Zboze siejeZboze;
        }

        private struct Sadzarka
        {
            Sadzarka(Warzywo w)
            {
                sadziWarzywo = w;
            }
            public Warzywo sadziWarzywo;
        }

        private struct Plug
        {
            Plug(Gleba g)
            {
                spulchniaGlebe = g;
            }
            public Gleba spulchniaGlebe;
        }

        private struct RozrzutnikNawozu
        {
            RozrzutnikNawozu(Podpole np)
            {
                nawoziPodpole = np;
            }
            public Podpole nawoziPodpole;
        }

        private struct Deszczownia
        {
            Deszczownia(Podpole pp, int jw)
            {
                podlewapodpole = pp;
                jednostkiWody = jw;
            }
            public Podpole podlewapodpole;
            public int jednostkiWody;
        }

        private struct Brony
        {
            Brony(Gleba g)
            {
                rozdrabniaGlebe = g;
            }
            public Gleba rozdrabniaGlebe;
        }

    }
}
