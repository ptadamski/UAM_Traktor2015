using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
using TraktorProj.Algorithms;
using TraktorProj.Interface;

namespace TraktorProj.Commons
{                                                                     
    public class ImageControler
    {

        OnNotifyDrawEvent onNotifyDrawEvent;     

        public ImageControler(OnNotifyDrawEvent onNotifyDrawEvent)
        {

            //this.window = window;
            this.onNotifyDrawEvent = onNotifyDrawEvent;
            //ChwastP = new ChwastPos();
        }

        /// <summary>
        /// Poruszanie się między "blokami" na planszy
        /// </summary>
        /// <param name="newX">Na którą pozycję X ma przejśc kelner</param>
        /// <param name="newY">Na którą pozycję Y ma przejśc kelner</param>
        /// top: move=1, right: move=2, down: move=3, left: move=4
        public bool Display(string imageSource, int newX, int newY, Orientation orient)
        {
            //window.ConsoleOutTextBlock.Text += "\r\n> " + newX + " " + newY;

            ImageFactoryArgs imgFactoryArgs = new ImageFactoryArgs() { Top = newY, Left = newX, FileName = imageSource };
            switch (orient)
            {
                case Orientation.Normal://0
                    break;
                case Orientation.Up://1                                    
                    imgFactoryArgs.Rotation = Rotation.Rotate270;
                    //TraktorBitmap.Rotation = Rotation.Rotate270;
                    break;
                case Orientation.Down://3                                              
                    imgFactoryArgs.Rotation = Rotation.Rotate90;
                    break;
                case Orientation.Left://4                                               
                    imgFactoryArgs.Rotation = Rotation.Rotate180;
                    break;
                case Orientation.Right://2
                    break;
            }
            ImageFactory imgFactory = new ImageFactory(imgFactoryArgs);

            var image = imgFactory.Create();

            if (onNotifyDrawEvent != null) 
            {     
                onNotifyDrawEvent(newX, newY, image);
                return true;
            }

            //posX = newX;
            //posY = newY;
            return false;
        }
        
        /*public bool createItem(string bitmap)
        {
            MainClass main = new MainClass();
            int chwastId = ChwastP.getAvaiableId();
            ChwastP.getPositions(chwastId);
            int posx = ChwastP.positions.X;
            int posy = ChwastP.positions.Y;
            ChwastP.addChwast(chwastId, posx, posy, 30);
                Window window = Application.Current.Windows[0];
                (window as MainWindow).ConsoleOutTextBlock.Text += "\r\n Id" + chwastId;
                if (window.GetType() == typeof(MainWindow))
                { 
                    Image chw = new Image();
                    switch(chwastId)
                    {
                        case 1:
                            chw = (window as MainWindow).chwast1;
                            break;
                        case 2:
                            chw = (window as MainWindow).chwast2;
                            break;
                        case 3:
                            chw = (window as MainWindow).chwast3;
                            break;
                        case 4:
                            chw = (window as MainWindow).chwast4;
                            break;
                        case 5:
                            chw = (window as MainWindow).chwast5;
                            break;
                        case 6:
                            chw = (window as MainWindow).chwast6;
                            break;
                        case 7:
                            chw = (window as MainWindow).chwast7;
                            break;
                        case 8:
                            chw = (window as MainWindow).chwast8;
                            break;
                        case 9:
                            chw = (window as MainWindow).chwast9;
                            break;
                        case 10:
                            chw = (window as MainWindow).chwast10;
                            break;
                        case 11:
                            chw = (window as MainWindow).chwast11;
                            break;
                        case 12:
                            chw = (window as MainWindow).chwast12;
                            break;
                        case 13:
                            chw = (window as MainWindow).chwast13;
                            break;
                        case 14:
                            chw = (window as MainWindow).chwast14;
                            break;
                        case 15:
                            chw = (window as MainWindow).chwast15;
                            break;
                        case 16:
                            chw = (window as MainWindow).chwast16;
                            break;
                        case 17:
                            chw = (window as MainWindow).chwast17;
                            break;
                        case 18:
                            chw = (window as MainWindow).chwast18;
                            break;
                        case 19:
                            chw = (window as MainWindow).chwast19;
                            break;
                        case 20:
                            chw = (window as MainWindow).chwast20;
                            break;
                    }
                    
                    (window as MainWindow).ConsoleOutTextBlock.Text += "\r\n Item " + bitmap + " " + posx + " " + posy;

                    Grid.SetColumn(chw, posx);
                    Grid.SetRow(chw, posy);
                   
                    ItemImage = new Image();
                    ItemBitmap = new BitmapImage();
                    ItemBitmap.BeginInit();
                    ItemBitmap.UriSource = new Uri("/Images/" + bitmap + ".png", UriKind.Relative);



                    ItemBitmap.EndInit();
                    ItemImage.Stretch = Stretch.UniformToFill;
                    ItemImage.Source = ItemBitmap;


                    chw.Source = ItemImage.Source;

                }
           
            return true;
        }             */
    }
}
