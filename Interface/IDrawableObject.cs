using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TraktorProj.Interface
{
    public interface IDrawableObject<_Location>
    {
        void Draw();

        _Location Location { get; set; }
    }
}
