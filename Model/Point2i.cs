using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public struct Point2i : IEqualityComparer<Point2i>, Algebra.IArithmetic<Point2i>
    {
        public Point2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        private int x;

        public int X
        {
            get { return x; }
            set { x = value; }
        }
        
        private int y;

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public bool Equals(Point2i x, Point2i y)
        {
            return x.X.Equals(y.X) && y.Y.Equals(y.Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        public int GetHashCode(Point2i obj)
        {
            return obj.X.GetHashCode() ^ obj.Y.GetHashCode();
        }

        public bool Equals(Point2i other)
        {
            return x.Equals(other.x) && y.Equals(other.y);
        }

        public int CompareTo(Point2i other)
        {
            if (x == other.x && y == other.y)
                return 0;
            else if (x > other.x && y > other.y)
                return 1;
            else if (x < other.x && y < other.y)
                return -1;
            else
                return 0;
        }

        public Point2i Add(Point2i other)
        {
            return new Point2i(x + other.x, y + other.y);
        }

        public Point2i Sub(Point2i other)
        {
            return new Point2i(x - other.x, y - other.y);
        }

        public Point2i Mul(Point2i other)
        {
            return new Point2i(x * other.x, y * other.y);
        }

        public Point2i Div(Point2i other)
        {
            return new Point2i(x / other.x, y / other.y);
        }
    }
}
