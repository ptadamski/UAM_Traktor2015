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
        public MainWindow()
        {
            InitializeComponent();
            Init();
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
                    agent.StartTraktor(s[1]);
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
                    agent.MoveBy(MoveEnum.Left);
                }
                else if (ConsoleInTextBox.Text == "right")
                {
                    agent.MoveBy(MoveEnum.Right);
                }
                else if (ConsoleInTextBox.Text == "down")
                {
                    agent.MoveBy(MoveEnum.Down);
                }
                else if (ConsoleInTextBox.Text == "up")
                {
                    agent.MoveBy(MoveEnum.Up);
                }

                else if (ConsoleInTextBox.Text == "generate")
                {
                    ConsoleOutTextBlock.Text += "\r\n> " + "generated";
                    agent.generateParam();
                    
                }
                else if (ConsoleInTextBox.Text == "rozpocznij")
                {
                    agent.PoryRokuStart();
                    
                }
                else if (ConsoleInTextBox.Text.Contains("chwast"))
                {
                   
                    ChwP.StartSzkodnik();
                    //controls.createItem("chwast");
                }
                else if (ConsoleInTextBox.Text.Contains("losuj"))
                {
                    ConsoleOutTextBlock.Text += "\r\nlosuje";
                    agent.LosujPos();
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
                            agent.StartTraktor(Int32.Parse(words[1]), Int32.Parse(words[2]));
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
                 agent.MoveBy(MoveEnum.Right);
            }
            if (e.Key == Key.Left)
            {
                agent.MoveBy(MoveEnum.Left);
            }
            if (e.Key == Key.Up)
            {
                agent.MoveBy(MoveEnum.Up);
            }
            if (e.Key == Key.Down)
            {
                agent.MoveBy(MoveEnum.Down);
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
