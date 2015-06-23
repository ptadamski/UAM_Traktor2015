using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TraktorProj.Interface
{
    public interface IDrawManager<_Location>
    {
        void BindImage(object sender, string name);

        void Add(IDrawableObject<_Location> child);

        void Remove(IDrawableObject<_Location> child);
    }
}
