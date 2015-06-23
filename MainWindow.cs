using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using TraktorProj.Model;
using System.Windows.Media;
using TraktorProj.Commons;
using System.Collections;

namespace TraktorProj    
{

    public interface IImageFactoryInvoker
    {
        void Invoke(UIProxy.ImageFactoryInvokeEvent invoker, object sender, int width, int height,
            string path, string name, string ext);
    }

    public interface IImageBoundry
    {
        void Add(object sender);
        void Remove(object sender);     
        void Update(object sender, int posx, int posy);
    }

    public partial class MainWindow : IImageFactoryInvoker, IImageBoundry
    {

        void IImageFactoryInvoker.Invoke(UIProxy.ImageFactoryInvokeEvent invoker, object sender, int width, int height,
            string path, string name, string ext)
        {
            Image img;
            invoker(sender, width, height, path, name, ext, out img);
            images.Add(sender, img);
        }

        private IDictionary<object, Image> images = new Dictionary<object, Image>();


        private static string AgentImageName = @"tractor";
                           
        private Traktor agent;
        private static Pos2 startPosition = new Pos2(1, 1);

        private UIProxy uiProxy;

        private DrawManager drawManager;

        private Controls controls;

        private ArrayList allTiles;

        private ChwastPos ChwP;

        private int maxTiles;

        private int[,] posTiles = new int[20, 20];

        private List<string> comandsList;

        private void Init()
        {
            this.uiProxy = new UIProxy(this as IImageFactoryInvoker, this as IImageBoundry);                       
            this.drawManager = new DrawManager(this.uiProxy, new DrawManager.Args(60, 60, @"/Images", "png"));                         
            this.agent = new Traktor(drawManager, startPosition, AgentImageName);   
  
            this.comandsList = new List<string>();
            this.comandsList.Add("clear - clear console");
            this.comandsList.Add("start - start");
            this.comandsList.Add("help - display help");

            maxTiles = 0;

            //Image[] allTiles = new Image[1000];
            allTiles = new ArrayList();

            controls = new Controls();
            ChwP = new ChwastPos(drawManager, new Pos2(8,8), "pokrzywa"); //TO DO : smiechuchy => dodac random czy cos
        }

        #region Nie chce tego widziec

        public void clearTile(int posx, int posy)
        {
            int find = posTiles[posx, posy];
            Image ItemTemp = new Image();
            ItemTemp = (Image)allTiles[find];
            this.MainGrid.Children.Remove(ItemTemp);
            ConsoleOutTextBlock.Text += "\r\n> " + "usunieto pole " + posx + " " + posy; 
        }

        /*public void setTile(int posx, int posy, string sprite)
        {
            posTiles[posx, posy] = maxTiles;
            ConsoleOutTextBlock.Text += "\r\n> " + "nowe pole " + posx + " " + posy;

            BitmapImage ItemBitmap;
            Image ItemTemp = new Image();

            ItemTemp.Width = 60;
            ItemTemp.Height = 60;

            ItemBitmap = new BitmapImage();
            ItemBitmap.BeginInit();
            ItemBitmap.UriSource = new Uri("/Images/" + sprite + ".png", UriKind.Relative);

            ItemBitmap.EndInit();
            ItemTemp.Stretch = Stretch.UniformToFill;
            ItemTemp.Source = ItemBitmap;
            this.MainGrid.Children.Add(ItemTemp);
            Grid.SetRow(ItemTemp, posy);
            Grid.SetColumn(ItemTemp, posx);

            allTiles.Add((Image)ItemTemp);

            maxTiles++;

        }//*/
        #endregion

        void IImageBoundry.Add(object sender)
        {
            this.MainGrid.Children.Add(images[sender]);
        }

        void IImageBoundry.Remove(object sender)
        {
            this.MainGrid.Children.Remove(images[sender]);
            images.Remove(sender);
        }

        void IImageBoundry.Update(object sender, int posx, int posy)
        {
            var e = images[sender];
            Grid.SetColumn(e, posx);
            Grid.SetRow(e, posy);
        }
    }
}
