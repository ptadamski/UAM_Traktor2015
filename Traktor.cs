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
using TraktorProj.ID3Algorithm;
using TraktorProj.Interface;

namespace TraktorProj
{
    public enum Orientation { Normal = 0, Up, Down, Left, Right }     
     
    class Traktor   //:  Drawable
    {
        private static Random random = new Random();

        private Map<Point2i, IRoslina> whereToSeed;

        private Point2i targetPosition;

        private Point2i position;

        public Point2i Position
        {
            get { return position; }
            set { position = value; }
        }


        private ImageControler imageControler;
        private string pora;
        private string image;
        private int order = 0;
        Parametry zboze = new Parametry(TraktorProj.Parametry.RodzajUprawy.zboze);
        Parametry warzywo = new Parametry(TraktorProj.Parametry.RodzajUprawy.warzywo);
                                     

        public Traktor(ImageControler imageControler, string image, int posx, int posy)
            //: base(onDraw, image, new Point2i(posx,posy))
        {
            this.image = image;
            this.imageControler = imageControler;
            this.targetPosition = this.position = new Point2i(posx, posy);
        }

        public void StartTraktor(int x, int y)
        {
            targetPosition.X = x;
            targetPosition.Y = y;

            Thread startThread = new Thread(TraktorThread);
            startThread.IsBackground = true;
            startThread.Start();
        }

        public void StartTraktor(string p)
        {
            pora = p;

            Thread startThread = new Thread(TraktorThread);
            startThread.IsBackground = true;
            startThread.Start();
        }

        private void TraktorThread()
        {
            // while(true){
            // generateParam();

            if (position.X > 1 && position.Y > 1)
            {
               // targetY = targetX = 1;
                Pathfind(targetPosition.X, targetPosition.Y);
                Thread.Sleep(6000);
            }

            Pathfind(targetPosition.X, targetPosition.Y);
            
            RunID3();

            Thread.Sleep(1000);
         
        }

        public void generateParam()
        {
            //string[] val = { "tak", "nie" };

            //Random
            //random = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (random.Next(1, 123) % 2 == 1) zboze.bronowane = "nie";
            else zboze.bronowane = "tak";

            //random = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (random.Next(1, 168) % 2 == 1) zboze.mineraly = "nie";
            else zboze.mineraly = "tak";

            //random = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (random.Next(1, 111) % 2 == 1) zboze.susza = "nie";
            else zboze.susza = "tak";

            //random = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (random.Next(1, 57) % 2 == 1) zboze.zaorane = "nie";
            else zboze.zaorane = "tak";

            //random = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (random.Next(1, 42) % 2 == 1) zboze.zasiane = "nie";
            else zboze.zasiane = "tak";

            //random = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (random.Next(1, 79) % 2 == 1) zboze.zbior = "nie";
            else zboze.zbior = "tak";



            //random = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (random.Next(1, 12) % 2 == 1) warzywo.bronowane = "tak";
            else warzywo.bronowane = "nie";

            //random = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (random.Next(1, 156) % 2 == 1) warzywo.mineraly = "nie";
            else warzywo.mineraly = "tak";

            //random = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (random.Next(1, 161) % 2 == 1) warzywo.susza = "tak";
            else warzywo.susza = "nie";

            //random = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (random.Next(1, 149) % 2 == 1) warzywo.zaorane = "nie";
            else warzywo.zaorane = "tak";

            //random = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (random.Next(1, 169) % 2 == 1) warzywo.zasiane = "tak";
            else warzywo.zasiane = "nie";

            //random = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (random.Next(1, 1698) % 2 == 1) warzywo.zbior = "nie";
            else warzywo.zbior = "tak";


        }

        //public void Decide() 
        //{
        //}

        public void Move(Orientation orient) 
        {
            switch (orient)
            {
                case Orientation.Normal:
                    break;
                case Orientation.Up:
                    position.Y -= 1;
                    break;
                case Orientation.Down:
                    position.Y += 1;
                    break;
                case Orientation.Left:
                    position.X -= 1;
                    break;
                case Orientation.Right:
                    position.X += 1;
                    break;
            }

            imageControler.Display(image, position.X, position.Y, orient);
        }

        private void Pathfind(int targetPositionX, int targetPositionY)
        {
            /* AStar */
            AStar astar = new AStar();

            AStarNode2D GoalNode = new AStarNode2D(null, null, 1, targetPositionX, targetPositionY, 0);
            AStarNode2D StartNode = new AStarNode2D(null, GoalNode, 1, Position.X, Position.Y, 1);
            StartNode.GoalNode = GoalNode;

            astar.FindPath(StartNode, GoalNode);
            /* AStar */
            AStarNode2D point0 = (AStarNode2D)astar.Solution[0];
            astar.Solution.RemoveAt(0);

            foreach (AStarNode2D point in astar.Solution)
            {
                int dir = point.DIR;
                Thread.Sleep(600);

                Action action = null;

                switch (dir)
                {
                    case 0: action = new Action(() => Move(Orientation.Up));
                        break;
                    case 1: action = new Action(() => Move(Orientation.Right));
                        break;
                    case 2: action = new Action(() => Move(Orientation.Down));
                        break;
                    case 3: action = new Action(() => Move(Orientation.Left));
                        break;
                }

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, action);
            }
        }

        /// <summary>
        /// pobierając liste klientów zwróci Point kolejnego do którego ma się udać
        /// </summary>
        private void RunID3()
        {
            List<string> treeList = new List<string>();
            List<Parametry> orderList = new List<Parametry>();
            orderList.Clear();

            orderList.Add(zboze);
            orderList.Add(warzywo);
            // image = "tractor";




            ID3Sample id3Sample = new ID3Sample("");//tu musi byc jakis load z pliku...
            string mPora = pora;


            treeList = id3Sample.GenerateTree();
            //if (targetX != 1 || targetY != 1)
            //{
            //    Thread.Sleep(1000);
            //    targetY = 1;
            //    targetX = 1;
            //    return;
            //}
            if (order < orderList.Count - 1)
                order++;
            else
                order = 0;

            for (int i = 0; i < treeList.Count; i++)
            {
                if (treeList[i].Contains(mPora))
                {
                    for (int j = i + 1; j < treeList.Count; j++)
                    {
                        if (treeList[j].Contains("uprawa"))
                        {
                            for (int l = j + 1; l < treeList.Count; l++)
                            {
                                if (treeList[l].Contains(orderList[order].rodzaj))
                                {
                                    for (int zz = l + 1; zz < treeList.Count; zz++)
                                    {
                                        if (treeList[zz].Contains(":"))
                                        {
                                            targetPosition.X = orderList[order].poleX;
                                            targetPosition.Y = orderList[order].poleY;
                                            string maszyna = treeList[zz].Split(':')[1];
                                            orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                            image = maszyna.Substring(0, maszyna.Length - 1);
                                            changeParameter(image, orderList[order]);
                                            return;
                                        }

                                        if (treeList[zz].Contains("bronowane"))
                                        {
                                            if (treeList[zz + 1].Contains(orderList[order].bronowane))
                                            {
                                                if (treeList[zz + 2].Contains(":"))
                                                {
                                                    targetPosition.X = orderList[order].poleX;
                                                    targetPosition.Y = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 2].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    image = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(image, orderList[order]);
                                                    return;
                                                }
                                            }

                                            if (treeList[zz + 3].Contains(orderList[order].bronowane))
                                            {
                                                if (treeList[zz + 4].Contains(":"))
                                                {
                                                    targetPosition.X = orderList[order].poleX;
                                                    targetPosition.Y = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 4].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    image = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(image, orderList[order]);
                                                    return;
                                                }
                                            }
                                        }

                                        if (treeList[zz].Contains("zasiane"))
                                        {
                                            if (treeList[zz + 1].Contains(orderList[order].zasiane))
                                            {
                                                if (treeList[zz + 2].Contains(":"))
                                                {
                                                    targetPosition.X = orderList[order].poleX;
                                                    targetPosition.Y = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 2].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    image = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(image, orderList[order]);
                                                    return;
                                                }
                                            }

                                            if (treeList[zz + 3].Contains(orderList[order].zasiane))
                                            {
                                                if (treeList[zz + 4].Contains(":"))
                                                {
                                                    targetPosition.X = orderList[order].poleX;
                                                    targetPosition.Y = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 4].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    image = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(image, orderList[order]);
                                                    return;
                                                }
                                            }
                                        }

                                        if (treeList[zz].Contains("zaorane"))
                                        {
                                            if (treeList[zz + 1].Contains(orderList[order].zaorane))
                                            {
                                                if (treeList[zz + 2].Contains(":"))
                                                {
                                                    targetPosition.X = orderList[order].poleX;
                                                    targetPosition.Y = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 2].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    image = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(image, orderList[order]);
                                                    return;
                                                }
                                            }

                                            if (treeList[zz + 3].Contains(orderList[order].zaorane))
                                            {
                                                if (treeList[zz + 4].Contains(":"))
                                                {
                                                    targetPosition.X = orderList[order].poleX;
                                                    targetPosition.Y = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 4].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    image = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(image, orderList[order]);
                                                    return;
                                                }
                                            }
                                        }

                                        if (treeList[zz].Contains("susza"))
                                        {
                                            if (treeList[zz + 1].Contains(orderList[order].susza))
                                            {
                                                if (treeList[zz + 2].Contains(":"))
                                                {
                                                    targetPosition.X = orderList[order].poleX;
                                                    targetPosition.Y = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 2].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    image = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(image, orderList[order]);
                                                    return;
                                                }
                                            }

                                            if (treeList[zz + 3].Contains(orderList[order].susza))
                                            {
                                                if (treeList[zz + 4].Contains(":"))
                                                {
                                                    targetPosition.X = orderList[order].poleX;
                                                    targetPosition.Y = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 4].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    image = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(image, orderList[order]);
                                                    return;
                                                }
                                            }
                                        }

                                        if (treeList[zz].Contains("mineraly"))
                                        {
                                            if (treeList[zz + 1].Contains(orderList[order].mineraly))
                                            {
                                                if (treeList[zz + 2].Contains(":"))
                                                {
                                                    targetPosition.X = orderList[order].poleX;
                                                    targetPosition.Y = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 2].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    image = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(image, orderList[order]);
                                                    return;
                                                }
                                            }

                                            if (treeList[zz + 3].Contains(orderList[order].mineraly))
                                            {
                                                if (treeList[zz + 4].Contains(":"))
                                                {
                                                    targetPosition.X = orderList[order].poleX;
                                                    targetPosition.Y = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 4].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    image = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(image, orderList[order]);
                                                    return;
                                                }
                                            }
                                        }
                                        if (treeList[zz].Contains("zbior"))
                                        {
                                            if (treeList[zz + 1].Contains(orderList[order].zbior))
                                            {
                                                if (treeList[zz + 2].Contains(":"))
                                                {
                                                    targetPosition.X = orderList[order].poleX;
                                                    targetPosition.Y = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 2].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    image = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(image, orderList[order]);
                                                    return;
                                                }
                                            }

                                            if (treeList[zz + 3].Contains(orderList[order].zbior))
                                            {
                                                if (treeList[zz + 4].Contains(":"))
                                                {
                                                    targetPosition.X = orderList[order].poleX;
                                                    targetPosition.Y = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 4].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    image = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(image, orderList[order]);
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                    //////

                                    /////
                                    for (int zz = l + 1; zz < treeList.Count; zz++)
                                    {
                                        if (treeList[zz].Contains("mineraly"))
                                        {
                                            for (int zzz = zz + 1; zzz < treeList.Count; zzz++)
                                            {
                                                if (treeList[zzz].Contains(orderList[order].mineraly))
                                                {

                                                    for (int z = zzz + 1; z < treeList.Count; z++)
                                                    {
                                                        if (treeList[z].Contains(":"))
                                                        {
                                                            targetPosition.X = orderList[order].poleX;
                                                            targetPosition.Y = orderList[order].poleY;
                                                            string maszyna = treeList[z].Split(':')[1];
                                                            orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                            image = maszyna.Substring(0, maszyna.Length - 1);
                                                            changeParameter(image, orderList[order]);
                                                            return;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //////
                                    return;
                                }
                            }
                        }
                        //}

                    }
                }
            }

        }

        private void changeParameter(string maszyna, Parametry p)
        {
            if (maszyna.Equals("plug")) { if (p.zaorane.Equals("tak")) { p.zaorane = "nie"; } else { p.zaorane = "tak"; } }
            if (maszyna.Equals("brona")) { if (p.bronowane.Equals("tak")) { p.bronowane = "nie"; } else { p.bronowane = "tak"; } }
            if (maszyna.Equals("kombajn")) { if (p.zbior.Equals("tak")) { p.zbior = "nie"; } else { p.zbior = "tak"; } }
            if (maszyna.Equals("deszczownia")) { if (p.susza.Equals("tak")) { p.susza = "nie"; } else { p.susza = "tak"; } }
            if (maszyna.Equals("siewnik")) { if (p.zasiane.Equals("tak")) { p.zasiane = "nie"; } else { p.zasiane = "tak"; } }
            if (maszyna.Equals("sadzarka")) { if (p.zasiane.Equals("tak")) { p.zasiane = "nie"; } else { p.zasiane = "tak"; } }
            if (maszyna.Equals("rozrzutnik"))
            {
                { if (p.mineraly.Equals("tak")) { p.mineraly = "nie"; } else { p.mineraly = "tak"; } }


            }
        }

    }
}




