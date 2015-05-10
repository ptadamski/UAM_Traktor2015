using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorProj.Model
{
    //composite: leaf
    public class RozrzutnikNawozu : MaszynaRolnicza
    {
        //zasobem sa zasoby naturalne
        //nie ma wymagan
        public override void pracuj()
        {
            throw new NotImplementedException();
        }
    }
}
