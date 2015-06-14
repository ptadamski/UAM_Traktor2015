using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TraktorProj.Commons;

namespace TraktorProj.Interface
{
    public delegate void OnNotifyDrawEvent(int posx, int posy, BitmapImage sprite);

    public interface IDrawContext : IDisposable
    {
        void Draw(int posx, int posy, BitmapImage sprite);
    }

    public interface IDrawable : IDisposable
    {
        void Draw();
    }
}
