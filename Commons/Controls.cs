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
    class Controls
    {
        /// <summary>
        /// NIE TYKAC!!!
        /// </summary>

        private Image TraktorImage;
        private BitmapImage TraktorBitmap;
        public int posX;
        public int posY;


        /// <summary>
        /// inicjalizacja
        /// </summary>
        public Controls()
        {


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
        public bool Move(int newX, int newY, int move)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof (MainWindow))
                {

                    Grid.SetColumn((window as MainWindow).TraktorImg, newX);
                    Grid.SetRow((window as MainWindow).TraktorImg, newY);

                    TraktorImage = new Image();
                    TraktorBitmap = new BitmapImage();
                    TraktorBitmap.BeginInit();
                    TraktorBitmap.UriSource = new Uri("/Images/tractor.png", UriKind.Relative);
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
            }
            posX = newX;
            posY = newY;


            return true;
        }

        /// <summary>
        /// Niech traktor porusza się w prawo o jedno pole
        /// </summary>
        public void TractorMooveRight()
        {
           
            if (posX < 16)
            {
                Move(posX + 1, posY,2);
                
                        
            }
        }

        public void TractorMoveTo(int targetX, int targetY)
        {
            AStar.goTo(posX, posY, 5, 5);
        }

        /// <summary>
        /// Niech traktor porusza się w lewo o jedno pole
        /// </summary>
        public void TractorMooveLeft()
        {

            if (posX > 0)
            {
                Move(posX - 1, posY, 4);

              
            }
        }

        /// <summary>
        /// Niech traktor porusza się w górę o jedno pole
        /// </summary>
        public void TractorMooveUp()
        {

            if (posY > 1)
            {
                Move(posX, posY - 1, 1);

            }
        }
        
        /// <summary>
        /// Niech traktor porusza się w dół o jedno pole
        /// </summary>
        public void TractorMooveDown()
        {

            if (posY < 10)
            {
                Move(posX, posY + 1,3); 
            }
        }
    }
}
