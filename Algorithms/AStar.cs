using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TraktorProj.Algorithms
{
    public class AStar
    {
      
        public static void goTo(int myX, int myY, int targetX, int targetY)
        {
            Stack<Point> OL;
            Stack<Point> CL;
            OL = new Stack<Point>();
            CL = new Stack<Point>();
            OL.Push(new Point(myX,myY));
            while (OL.Count != 0) {
                Point last = OL.Pop();




                foreach (Window window in Application.Current.Windows)
                {
                     if (window.GetType() == typeof (MainWindow))
                     {

                         (window as MainWindow).ConsoleOutTextBlock.Text += "elo" + last.X;

                     }
                }
                
                
                
            }
          /*  while (lista OL nie jest pusta) {  
    wybierz ze zbioru OL pole o najmniejszej wartości F (nazwijmy je Q)  
    umieść pole Q na liście CL  
    if (Q jest węzłem docelowym)  
        znaleziono najkrótszą ścieżkę, wyjście z funkcji  
    for (każdy z 8 sąsiadów Q) {  
        if (sąsiad jest na CL lub sąsiad jest zabronionym polem)  
            nic nie rób  
        else if (sąsiad nie znajduje się na OL) {  
            przenieś go do OL  
            Q staje się rodzicem sąsiada  
            oblicz wartości G, H, F sąsiada  
        }  
        else {  
            oblicz nową wartość G sąsiada  
            if ( nowaG < G) {  
                Q staje się rodzicem sąsiada  
                G = nowaG  
                oblicz nową wartość F sąsiada (F=nowaF)  
            }  
        }  
    }    
}  */


        }
    }
}
