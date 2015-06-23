using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorProj.Model
{
    public class Pos2 : IEqualityComparer<Pos2>, IEquatable<Pos2>
    {
        private int posX;

        public int X
        {
            get { return posX; }
            set { posX = value; }
        }

        private int posY;

        public int Y
        {
            get { return posY; }
            set { posY = value; }
        }

        public Pos2()
        {
            this.posX = 0;
            this.posY = 0;

        }

        public Pos2(int posX, int posY)
        {
            this.posX = posX;
            this.posY = posY;
        }

        public bool Equals(Pos2 x, Pos2 y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Pos2 obj)
        {
            return posX ^ posY;
        }

        public bool Equals(Pos2 other)
        {
            return other.X.Equals(X) && other.Y.Equals(Y);
        }
    }
}
