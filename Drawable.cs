using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TraktorProj.Commons;
using TraktorProj.Interface;

namespace TraktorProj
{                                       
    public class Drawable : IDrawable
    {
        //IDrawContext context;
        static DrawManager manager;
        event OnNotifyDrawEvent onNotifyDrawEvent;
        BitmapImage image;

        protected Point2i position;

        public Point2i Position
        {
            get { return position; }
            set { position = value; }
        }

        public Drawable(IDrawContext context, BitmapImage image, Point2i position)
        {
            this.onNotifyDrawEvent += context.Draw;
            this.image = image;
            this.position = position;
            manager.AddChild(this);
        }

        public Drawable(OnNotifyDrawEvent onNotifyDrawEvent, BitmapImage image, Point2i position)
        {
            this.onNotifyDrawEvent += onNotifyDrawEvent;
            this.image = image;
            this.position = position;
            manager.AddChild(this);
        }

        private bool isVisible;

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        public virtual void Draw()
        {
            if (isVisible && onNotifyDrawEvent != null)
                onNotifyDrawEvent(position.X, position.Y, image);
        }

        public virtual void Dispose()
        {
            manager.RemoveChild(this);
        }
    }


    public class DrawManager : IDrawable
    {
        HashSet<IDrawable> items = new HashSet<IDrawable>();
              
        public void AddChild(IDrawable obj)
        {
            items.Add(obj);
        }

        public void RemoveChild(IDrawable obj)
        {
            items.Remove(obj);
        }

        public void Draw()
        {
            foreach (var item in items)
                item.Draw();
        }

        public void Dispose()
        {
            items.Clear();
        }
    }
}
