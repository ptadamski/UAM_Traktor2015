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
using TraktorProj.Interface;

namespace TraktorProj.Model
{
    public class DrawManager : IDrawManager<Pos2>
    { //IFactory<Image>

        public DrawManager(UIProxy uiProxy,  Args args)
        {
            this.args = args;
            this.uiProxy = uiProxy;
        }

        #region IDrawManager<Pos2>

        public void BindImage(object sender, string name)
        {
            uiProxy.BeginCreateImage(sender, args.width, args.height, args.path, name, args.ext);
        }

        public void Add(IDrawableObject<Pos2> child) 
        {
            uiProxy.Add(child, child.Location.X, child.Location.Y);
        }

        public void Remove(IDrawableObject<Pos2> child) 
        {
            uiProxy.Remove(child);
        }

        public void Update(IDrawableObject<Pos2> child) 
        {
            uiProxy.Update(child, child.Location.X, child.Location.Y);
        }

        #endregion

        public class Args
        {
            public Args(int width, int height, string path, string ext)
            {
                this.width = width;
                this.height = height;
                this.path = path;
                this.ext = ext;
            }

            public int height;
            public int width;
            public string path;
            public string ext;
        }

        private Args args;

        private UIProxy uiProxy;
    }
}
