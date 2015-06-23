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
        public DrawableObject(IDrawManager<Pos2> drawManager, Pos2 location, string name)
        {
            this.drawManager = drawManager;
            this.name = name;
            this.location = location;
            drawManager.BindImage(this,name);
            drawManager.Add(this);
        }

        public DrawableObject(IDrawManager<Pos2> drawManager, int positionX, int positionY, string name)
        {
            this.drawManager = drawManager;
            this.name = name;
            this.location = new Pos2(positionX, positionY);
            drawManager.BindImage(this, name);
            drawManager.Add(this);
        }

        private string name;

        private IDrawManager<Pos2> drawManager;

        #region IDrawable impl

        public void Draw()  {}

        private Pos2 location;

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
