using GeneticAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TraktorProj
{
    public class ImageFactoryArgs
    {
        public ImageFactoryArgs(string filename, int top, int left, Rotation rotation)
        {
            this.filename = filename;
            this.top = top;
            this.left = left;
            this.rotation = rotation;
        }

        public ImageFactoryArgs()
        {
        }

        private string filename;

        public string FileName
        {
            get { return filename; }
            set { filename = value; }
        }

        private int top;

        public int Top
        {
            get { return top; }
            set { top = value; }
        }

        private int left;

        public int Left
        {
            get { return left; }
            set { left = value; }
        }

        private Rotation rotation;

        public Rotation Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

    }

    public class ImageFactory : IFactory<BitmapImage, ImageFactoryArgs>
    {
        BitmapImage last;
        ImageFactoryArgs args;

        public ImageFactory(ImageFactoryArgs args)
        {
            this.args = args;
        }

        public BitmapImage Create()
        {
            return Create(args);
        }

        public BitmapImage Create(ImageFactoryArgs args)
        {                         
            last = new BitmapImage();
            last.BeginInit();
            last.UriSource = new Uri("/Images/"+args.FileName+".png", UriKind.Relative);
            last.Rotation = args.Rotation;
            last.EndInit();
            return last;
        }

        public ImageFactoryArgs Args
        {
            get
            {
                return args;
            }
            set
            {
                args = value;
            }
        }

        public BitmapImage Last
        {
            get { return last; }
        }
    }
}
