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


namespace TraktorProj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Controls controls;
       
        private List<string> comandsList; 
        public MainWindow()
        {
            InitializeComponent();

            controls = new Controls();

        

            comandsList = new List<string>();
            comandsList.Add("clear - clear console");
            comandsList.Add("start - start");
            comandsList.Add("help - display help");
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
                else if (ConsoleInTextBox.Text == "left")
                {
                    controls.TractorMooveLeft("tractor");
                }
                else if (ConsoleInTextBox.Text == "right")
                {
                    controls.TractorMooveRight("tractor");
                }
                else if (ConsoleInTextBox.Text == "down")    
                {
                    controls.TractorMooveDown("tractor");
                }
                else if (ConsoleInTextBox.Text == "up")
                {
                    controls.TractorMooveUp("tractor");
                }

                else if (ConsoleInTextBox.Text == "generate")
                {
                    ConsoleOutTextBlock.Text += "\r\n> " + "generated";
                    Traktor.Instance.generateParam();
                }
                else if (ConsoleInTextBox.Text.Contains("chwast"))
                {
                    
                    controls.createItem("chwast");
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
                controls.TractorMooveRight("tractor");
            }
            if (e.Key == Key.Left)
            {
                controls.TractorMooveLeft("tractor");
            }
            if (e.Key == Key.Up)
            {
                controls.TractorMooveUp("tractor");
            }
            if (e.Key == Key.Down)
            {
                controls.TractorMooveDown("tractor");
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
