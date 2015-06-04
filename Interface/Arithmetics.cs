using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algebra
{
    public interface IArithmetic<T> : IEquatable<T>, IComparable<T>
    {
        T Add(T other);
        T Sub(T other);
        T Mul(T other);
        T Div(T other);
    }
}
