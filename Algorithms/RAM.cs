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

        public RAM()
        {
            myStruct = new MyStruct();
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
    }
}
