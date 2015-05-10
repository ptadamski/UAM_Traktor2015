using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorProj.Interface
{
    public enum Kierunek { Prawo, Lewo, Gora, Dol }

    //composite: component
    public interface ICiagnik : IMaszynaRolnicza
    {
        void idz(Kierunek kierunek);
        void wez(IMaszynaRolnicza maszynaRolnicza);
        void odstaw(IMaszynaRolnicza maszynaRolnicza);
    }
}
