using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TraktorProj.Algorithms;

namespace TraktorProj.Commons
{
    public class Controls
    {
        /// <summary>
        /// NIE TYKAC!!!
        /// </summary>

        //private Image TraktorImage;
        //private BitmapImage TraktorBitmap;
        private ChwastPos ChwastP;

        private Image ItemImage;
        private BitmapImage ItemBitmap;

        //public int posX;
       // public int posY;

        /// <summary>
        /// inicjalizacja
        /// </summary>
        public Controls()
        {

            //ChwastP = new ChwastPos(null,null,null);
            //posX = 1;
            //posY = 1;
            try
            {
              
              //tu mozemy cos wykonywac, jakies generowanie ruchow itd


            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }
        
        public bool createItem(string bitmap)
        {
            //MainClass main = new MainClass();
            int chwastId = ChwastP.getAvaiableId();
            ChwastP.getPositions(chwastId);
            //ChwastP.generateParam();
            int posx = Convert.ToInt32(ChwastP.positions.X);
            int posy = Convert.ToInt32(ChwastP.positions.Y);
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
        }   
    }
}
