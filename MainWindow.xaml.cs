using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TraktorProj.Commons;
using TraktorProj.Algorithms;
using System.Collections;


namespace TraktorProj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ImageControler imageControler;

        //Imagelist<Image> allTiles = new Imagelist<Image>();
        private ArrayList allTiles;
        private int maxTiles;
        private int[,] posTiles = new int[20, 20];

        private Traktor traktor;
        
        private List<string> comandsList;

        public MainWindow()
        {
            InitializeComponent();
            maxTiles = 0;
          
            //Image[] allTiles = new Image[1000];
            allTiles = new ArrayList();


        

            comandsList = new List<string>();
            comandsList.Add("clear - clear console");
            comandsList.Add("start - start");
            comandsList.Add("help - display help");


            imageControler = new ImageControler(setTile);
            traktor = new Traktor(imageControler, "tractor", 1,1);
        }


        public void setTile(int posx, int posy, BitmapImage bitmap)
        {
            posTiles[posx, posy] = maxTiles;
            ConsoleOutTextBlock.Text += "\r\n> " + "nowe pole " + posx + " " + posy;

            Image ItemTemp = new Image();

            ItemTemp.Width = 60;
            ItemTemp.Height = 60;
            ItemTemp.Stretch = Stretch.UniformToFill;
            ItemTemp.Source = bitmap;                         
            MainGrid.Children.Add(ItemTemp);
            Grid.SetRow(ItemTemp, posy);
            Grid.SetColumn(ItemTemp, posx);

            //allTiles.Add(ItemTemp);

            maxTiles++;  
        }

        public void clearTile(int posx, int posy)
        {
            int find = posTiles[posx, posy];
            Image ItemTemp = new Image();
            ItemTemp = (Image)allTiles[find];
            this.MainGrid.Children.Remove(ItemTemp);
            ConsoleOutTextBlock.Text += "\r\n> " + "usunieto pole " + posx + " " + posy;
           // (window as MainWindow).MainGrid.Children.Remove(allTiles[find]);
            

        }


        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {           
            if (e.Key == Key.Enter)
            {                                     
               // ConsoleOutTextBlock.Text += "siema";
                    
                if (ConsoleInTextBox.Text == "clear")
                {                                              
                    ConsoleOutTextBlock.Text = "";
                }
                else if (ConsoleInTextBox.Text.ToLower().StartsWith("start:") || ConsoleInTextBox.Text == "Start")
                {
                    String[] s = ConsoleInTextBox.Text.Split(':');
                    ConsoleOutTextBlock.Text += "\r\n> " + "started";
                    traktor.StartTraktor(s[1]);
                }
                else if(ConsoleInTextBox.Text == "help")
                {
                    foreach (var VARIABLE in comandsList)
                    {
                        ConsoleOutTextBlock.Text += "\r\n> " + VARIABLE;
                    }
                }
                else if (ConsoleInTextBox.Text == "add")
                {
                    //Random random = new Random();
                    //int posx = random.Next(1, 13);
                    //int posy = random.Next(1, 10);
                    //ImageFactory factory = new ImageFactory(new ImageFactoryArgs());
                    //var item = factory.Create();
                    //setTile(posx, posy,"field3");
                }
                else if (ConsoleInTextBox.Text.Contains("rem"))
                {

                    string[] words = ConsoleInTextBox.Text.Split(' ');
                    if (words.Length == 3) {
                        int tarX = Int32.Parse(words[1]);
                        int tarY = Int32.Parse(words[2]);
                        clearTile(tarX, tarY);

                        
                    }
                    else {
                        ConsoleOutTextBlock.Text += "\r\nWrong parameter";
                    }
                }
                else if (ConsoleInTextBox.Text == "left")
                {
                    traktor.Move(Orientation.Left);
                }
                else if (ConsoleInTextBox.Text == "right")
                {
                    traktor.Move(Orientation.Right);
                }
                else if (ConsoleInTextBox.Text == "down")    
                {
                    traktor.Move(Orientation.Down);
                }
                else if (ConsoleInTextBox.Text == "up")
                {
                    traktor.Move(Orientation.Up);
                }

                else if (ConsoleInTextBox.Text == "generate")
                {
                    ConsoleOutTextBlock.Text += "\r\n> " + "generated";
                    traktor.generateParam();
                }
                else if (ConsoleInTextBox.Text.Contains("go"))
                {

                    string[] words = ConsoleInTextBox.Text.Split(' ');
                    if (words.Length == 3)
                    {
                        int tarX = Int32.Parse(words[1]);
                        int tarY = Int32.Parse(words[2]);

                        if (MainClass.GetMap(tarX, tarY) > 0)
                        {
                            ConsoleOutTextBlock.Text += "\r\nOn my way";
                            traktor.StartTraktor(Int32.Parse(words[1]), Int32.Parse(words[2]));
                        }
                        else
                        {
                            ConsoleOutTextBlock.Text += "\r\nPosition not avaiable";

                        }

                    }
                    else
                    {
                        ConsoleOutTextBlock.Text += "\r\nWrong parameter";

                    }
                 
                    
                   
                    
                }
                else
                {
                    ConsoleOutTextBlock.Text += "\r\n>Unknown command";
                }

            }
            //Keyboard.Focus(MainGrid);
        }

        

        private void MainWindowKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Right:
                    traktor.Move(Orientation.Right);
                    break;
                case Key.Left:
                    traktor.Move(Orientation.Left);
                    break;
                case Key.Up:
                    traktor.Move(Orientation.Up);
                    break;
                case Key.Down:
                    traktor.Move(Orientation.Down);
                    break;
            }
        }

        private void ConsoleOutputTextChanged(object sender, TextChangedEventArgs e)
        {
            ConsoleOutTextBlock.ScrollToEnd();
        }

        private void ConsoleInMouseDown(object sender, MouseButtonEventArgs e)
        {
            ConsoleInTextBox.Focusable = true;
        }

        private void CloseApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ConsoleInTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
