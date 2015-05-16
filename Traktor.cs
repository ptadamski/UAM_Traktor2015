using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TraktorProj.Algorithms;
using TraktorProj.Commons;

namespace TraktorProj
{
    class Traktor
    {



        private Controls controls;
        private static Traktor traktor;
        private int targetX, targetY;



        public static Traktor Instance
        {
            get
            {
                if (traktor == null)
                {
                    traktor = new Traktor();
                }

                return traktor;
            }
        }

        public Traktor()
        {

            controls = new Controls();
  

 
        }






        public void StartTraktor(int x, int y)
        {
            targetX = x;
            targetY = y;
            Thread startThread = new Thread(TraktorThread);
            startThread.IsBackground = true;

            startThread.Start();


        }
      
        private void TraktorThread()
        {

        

                 /* AStar */
                    AStar astar = new AStar();

                    AStarNode2D GoalNode = new AStarNode2D(null, null, 1, targetX, targetY,0);
                    AStarNode2D StartNode = new AStarNode2D(null, GoalNode, 1, controls.posX, controls.posY,1);
                    StartNode.GoalNode = GoalNode;

                    astar.FindPath(StartNode, GoalNode);
                    /* AStar */
                    AStarNode2D point0 = (AStarNode2D)astar.Solution[0];
                    astar.Solution.RemoveAt(0);

                        foreach (AStarNode2D point in astar.Solution)
                        {
                          
                            int dir = point.DIR;
                            Thread.Sleep(600);

               
                            if (dir == 3)
                            {
                                 Application.Current.Dispatcher.BeginInvoke(
                               DispatcherPriority.Background,
                               new Action(() => controls.TractorMooveLeft()));
                            }
                            else if (dir == 1)
                            {
                                 Application.Current.Dispatcher.BeginInvoke(
                               DispatcherPriority.Background,
                               new Action(() => controls.TractorMooveRight()));
                            }
                            else if (dir == 2)
                            {
                                 Application.Current.Dispatcher.BeginInvoke(
                               DispatcherPriority.Background,
                               new Action(() => controls.TractorMooveDown()));
                            }
                            else if (dir == 0)
                            {
                                 Application.Current.Dispatcher.BeginInvoke(
                               DispatcherPriority.Background,
                               new Action(() => controls.TractorMooveUp()));

                            }



                          
                        }

                       
            
                      
                

              
            }
     }


}
