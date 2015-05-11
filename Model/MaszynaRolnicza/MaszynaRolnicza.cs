using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraktorProj.Interface;

namespace TraktorProj.Model
{
    public abstract class MaszynaRolnicza : IMaszynaRolnicza
    {
        public MaszynaRolnicza()
        {
            this.traktor = null;
        }

        public MaszynaRolnicza(Traktor traktor)
        {
            this.traktor = traktor;
        }

        protected Traktor traktor;

        public Traktor Traktor
        {
            get { return traktor; }
            set { traktor = value; }
        }

        public abstract void pracuj();
    }
}
