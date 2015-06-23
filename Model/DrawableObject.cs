using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TraktorProj.Interface;

namespace TraktorProj.Model
{
    public class DrawableObject : IDrawableObject<Pos2>, IDisposable
    {
        public DrawableObject(IDrawManager<Pos2> drawManager, Pos2 location, string image)
        {
            this.drawManager = drawManager;
            this.image = image;
            this.location = location;
            drawManager.BindImage(this,image);
            drawManager.Add(this);
        }

        public DrawableObject(IDrawManager<Pos2> drawManager, int positionX, int positionY, string name)
        {
            this.drawManager = drawManager;
            this.image = name;
            this.location = new Pos2(positionX, positionY);
            drawManager.BindImage(this, name);
            drawManager.Add(this);
        }

        protected string image;

        protected IDrawManager<Pos2> drawManager;

        #region IDrawable impl

        public void Draw()  {}

        protected Pos2 location;

        public Pos2 Location
        {
            get { return location; }
            set { location = value; }
        }

        #endregion

        #region IDisposable impl

        public void Dispose()
        {
            drawManager.Remove(this);
        }

        #endregion
    }
}
