using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraktorProj.Model;

namespace TraktorProj.Interface
{
    public interface IRoslina
    {
        void zyj(TimeSpan czas, Podpole miejsce);
        void gin();
        void wprowadz(Dictionary<Resource, int> substancje);
        void rosnij(Podpole miejsce);
        void rozmnazaj(Podpole miejsce, IRoslina potomek);
        void wydzielaj(Podpole miejsce, out Dictionary<Resource, int> metabolity);//Piotr : bede tego potrzebowal
    }
}
