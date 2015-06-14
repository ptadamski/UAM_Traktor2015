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

        private Image TraktorImage;
        private BitmapImage TraktorBitmap;
        private ChwastPos ChwastP;

        private Image ItemImage;
        private BitmapImage ItemBitmap;

        public int posX;
        public int posY;

        /// <summary>
        /// inicjalizacja
        /// </summary>
        public Controls()
        {

            ChwastP = new ChwastPos();
            posX = 1;
            posY = 1;
            try
            {
              
              //tu mozemy cos wykonywac, jakies generowanie ruchow itd


            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        /// <summary>
        /// Poruszanie się między "blokami" na planszy
        /// </summary>
        /// <param name="newX">Na którą pozycję X ma przejśc kelner</param>
        /// <param name="newY">Na którą pozycję Y ma przejśc kelner</param>
        /// top: move=1, right: move=2, down: move=3, left: move=4
        public bool Move(int newX, int newY, int move, String bitmap)
        {
            Window window = Application.Current.Windows[0];
            

                if (window.GetType() == typeof (MainWindow))
                {


                    (window as MainWindow).ConsoleOutTextBlock.Text += "\r\n> " + newX + " " + newY;
                    Grid.SetColumn((window as MainWindow).TraktorImg, newX);
                    Grid.SetRow((window as MainWindow).TraktorImg, newY);

                    TraktorImage = new Image();
                    TraktorBitmap = new BitmapImage();
                    TraktorBitmap.BeginInit();
                    TraktorBitmap.UriSource = new Uri("/Images/"+bitmap+".png", UriKind.Relative);
               //     TraktorImage.UpdateLayout();
                    if (move == 1)
                    {
                        TraktorBitmap.Rotation = Rotation.Rotate270;
                    }
                    else if (move == 2)
                    {
                        //TraktorBitmap.Rotation = Rotation.Rotate90;

                    }
                    else if (move == 3)
                    {
                        TraktorBitmap.Rotation = Rotation.Rotate90;

                    }
                    else if (move == 4)
                    {
                        TraktorBitmap.Rotation = Rotation.Rotate180;    
                    }
                    
                    TraktorBitmap.EndInit();
                    TraktorImage.Stretch = Stretch.UniformToFill;
                    TraktorImage.Source = TraktorBitmap;


                    (window as MainWindow).TraktorImg.Source = TraktorImage.Source;

                    

                   
                   
                }
            
            posX = newX;
            posY = newY;


            return true;
        }

        public void changeBitmap()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {


                    TraktorImage = new Image();
                    TraktorBitmap = new BitmapImage();
                    TraktorBitmap.BeginInit();
                    TraktorBitmap.UriSource = new Uri("/Images/field1.png", UriKind.Relative);
                   

                    TraktorBitmap.EndInit();
                    TraktorImage.Stretch = Stretch.UniformToFill;
                    TraktorImage.Source = TraktorBitmap;


                    (window as MainWindow).TraktorImg.Source = TraktorImage.Source;

                }
            }
           
        }

        public bool createItem(string bitmap)
        {
            MainClass main = new MainClass();
            int chwastId = ChwastP.getAvaiableId();
            ChwastP.getPositions(chwastId);
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

        /// <summary>
        /// Niech traktor porusza się w prawo o jedno pole
        /// </summary>
        public void TractorMooveRight(string bitmap)
        {
           
            if (posX < 16)
            {
                Move(posX + 1, posY,2, bitmap);
                
                        
            }
        }

        public void TractorMoveTo(int targetX, int targetY)
        {
            //AStar.goTo(posX, posY, 5, 5);
        }

        /// <summary>
        /// Niech traktor porusza się w lewo o jedno pole
        /// </summary>
        public void TractorMooveLeft(string bitmap)
        {

            if (posX > 0)
            {
                Move(posX - 1, posY, 4,bitmap);

              
            }
        }

        /// <summary>
        /// Niech traktor porusza się w górę o jedno pole
        /// </summary>
        public void TractorMooveUp(string bitmap)
        {

            if (posY > 1)
            {
                Move(posX, posY - 1, 1,bitmap);

            }
        }
        
        /// <summary>
        /// Niech traktor porusza się w dół o jedno pole
        /// </summary>
        public void TractorMooveDown(string bitmap)
        {

            if (posY < 10)
            {
                Move(posX, posY + 1,3,bitmap); 
            }
        }
    }
}
