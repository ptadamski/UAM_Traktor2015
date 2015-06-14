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
        private Controls controls;

        //Imagelist<Image> allTiles = new Imagelist<Image>();
        private ArrayList allTiles;
        private ChwastPos ChwP;
        private int maxTiles;
        private int[,] posTiles = new int[20, 20];
        
        private List<string> comandsList;

        public MainWindow()
        {
            InitializeComponent();
            maxTiles = 0;
          
            //Image[] allTiles = new Image[1000];
            allTiles = new ArrayList();

            controls = new Controls();
            ChwP = new ChwastPos();
        

            comandsList = new List<string>();
            comandsList.Add("clear - clear console");
            comandsList.Add("start - start");
            comandsList.Add("help - display help");
        }


        public void setTile(int posx, int posy, string sprite)
        {
            posTiles[posx, posy] = maxTiles;
            ConsoleOutTextBlock.Text += "\r\n> " + "nowe pole "+posx+" "+posy;
            
            BitmapImage ItemBitmap;
            Image ItemTemp = new Image();
          

            ItemTemp.Width = 60;
            ItemTemp.Height = 60;
            Window window = Application.Current.Windows[0];
          
            ItemBitmap = new BitmapImage();
            ItemBitmap.BeginInit();
            ItemBitmap.UriSource = new Uri("/Images/"+sprite+".png", UriKind.Relative);

            ItemBitmap.EndInit();
            ItemTemp.Stretch = Stretch.UniformToFill;
            ItemTemp.Source = ItemBitmap;
            (window as MainWindow).MainGrid.Children.Add(ItemTemp);
            Grid.SetRow(ItemTemp, posy);
            Grid.SetColumn(ItemTemp, posx);

            allTiles.Add((Image)ItemTemp);

            maxTiles++;

        }
        public void clearTile(int posx, int posy)
        {
            Window window = Application.Current.Windows[0];
            int find = posTiles[posx, posy];
            Image ItemTemp = new Image();
            ItemTemp = (Image)allTiles[find];
            (window as MainWindow).MainGrid.Children.Remove(ItemTemp);
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
                    Traktor.Instance.StartTraktor(s[1]);
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
                    Random random = new Random();
                    int posx = random.Next(1, 13);
                    int posy = random.Next(1, 10);
                    setTile(posx, posy,"field3");
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
                    controls.TractorMooveLeft("tractor",1,1,"");
                }
                else if (ConsoleInTextBox.Text == "right")
                {
                    controls.TractorMooveRight("tractor",1,1,"");
                }
                else if (ConsoleInTextBox.Text == "down")    
                {
                    controls.TractorMooveDown("tractor",1,1,"");
                }
                else if (ConsoleInTextBox.Text == "up")
                {
                    controls.TractorMooveUp("tractor", 1, 1, "");
                }

                else if (ConsoleInTextBox.Text == "generate")
                {
                    ConsoleOutTextBlock.Text += "\r\n> " + "generated";
                    Traktor.Instance.generateParam();
                    
                }
                else if (ConsoleInTextBox.Text == "rozpocznij")
                {
                    Traktor.Instance.PoryRokuStart();
                    
                }
                else if (ConsoleInTextBox.Text.Contains("chwast"))
                {
                   
                    ChwP.StartSzkodnik();
                    //controls.createItem("chwast");
                }
                else if (ConsoleInTextBox.Text.Contains("losuj"))
                {
                    ConsoleOutTextBlock.Text += "\r\nlosuje";
                    Traktor.Instance.LosujPos();
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
                            Traktor.Instance.StartTraktor(Int32.Parse(words[1]), Int32.Parse(words[2]));
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

            if (e.Key == Key.Right)
            {
                controls.TractorMooveRight("tractor", 1, 1, "");
            }
            if (e.Key == Key.Left)
            {
                controls.TractorMooveLeft("tractor", 1, 1, "");
            }
            if (e.Key == Key.Up)
            {
                controls.TractorMooveUp("tractor", 1, 1, "");
            }
            if (e.Key == Key.Down)
            {
                controls.TractorMooveDown("tractor", 1, 1, "");
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
