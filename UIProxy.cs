using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace TraktorProj
{
    public class UIProxy
    {
        public UIProxy(IImageFactoryInvoker imageFactoryInvoker, IImageBoundry imageBoundry)
        {
            this.imageFactoryInvoker = imageFactoryInvoker;
            this.imageBoundry = imageBoundry;
        }

        private IImageFactoryInvoker imageFactoryInvoker;

        private IImageBoundry imageBoundry;
                                                                 
        public void BeginCreateImage(object sender, int width, int height,
            string path, string name, string ext)
        {

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render,
            new Action(() => 
            {
                imageFactoryInvoker.Invoke(EndCreateImage, sender, width, height, path, name, ext);
            }));          
        }                                 

        public delegate void ImageFactoryInvokeEvent(object sender, int width, int height, 
            string path, string name, string ext, out Image image);

        private void EndCreateImage(object sender, int width, int height,
            string path, string name, string ext, out Image img)
        {
            var str = string.Format(@"{0}/{1}.{2}", path, name, ext); 
            var bmp = new BitmapImage(new Uri(str, UriKind.Relative));
            img = new Image();

            img.Width = width;
            img.Height = height;

            img.Stretch = Stretch.UniformToFill;
            img.Source = bmp;
        }

        public void Add(object e, int posx, int posy)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render,
            new Action(() =>
            {
                imageBoundry.Add(e);
                imageBoundry.Update(e, posx, posy);
            }));

        }

        public void Remove(object e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render,
            new Action(() => { imageBoundry.Remove(e); }));
        }

        public void Update(object e, int posx, int posy)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render,
            new Action(() =>
            {
                imageBoundry.Update(e, posx, posy);
            }));
        }
    }
}
