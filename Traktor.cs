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
        private int targetX = 1, targetY = 1;
        private string pora;
        private string imageName;
        private int order = 0;
        Parametry zboze = new Parametry(TraktorProj.Parametry.RodzajUprawy.zboze);
        Parametry warzywo = new Parametry(TraktorProj.Parametry.RodzajUprawy.warzywo);


        public static Traktor Instance
        {
            get
            {
                return traktor = traktor ?? new Traktor("tractor");
            }
        }

        public Traktor(string imageName)
        {
            this.controls = new Controls();
            this.imageName = imageName;
        }






        public void StartTraktor(int x, int y)
        {
            targetX = x;
            targetY = y;
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
            if (controls.posX > 1 && controls.posY > 1)
            {
                targetY = targetX = 1;
                go();
                Thread.Sleep(6000);
            }

            //
            //zboze.zaorane = "tak";

            //Thread.Sleep(2000); 
            go();
            RunID3();

            Thread.Sleep(1000);
            //targetY = targetX = 1;
            //go();

            // }          
        }

        public void generateParam()
        {
            Random
            losuj = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (losuj.Next(1, 123) % 2 == 1) zboze.bronowane = "nie";
            else zboze.bronowane = "tak";

            losuj = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (losuj.Next(1, 168) % 2 == 1) zboze.mineraly = "nie";
            else zboze.mineraly = "tak";

            losuj = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (losuj.Next(1, 111) % 2 == 1) zboze.susza = "nie";
            else zboze.susza = "tak";

            losuj = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (losuj.Next(1, 57) % 2 == 1) zboze.zaorane = "nie";
            else zboze.zaorane = "tak";

            losuj = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (losuj.Next(1, 42) % 2 == 1) zboze.zasiane = "nie";
            else zboze.zasiane = "tak";

            losuj = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (losuj.Next(1, 79) % 2 == 1) zboze.zbior = "nie";
            else zboze.zbior = "tak";



            losuj = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (losuj.Next(1, 12) % 2 == 1) warzywo.bronowane = "tak";
            else warzywo.bronowane = "nie";

            losuj = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (losuj.Next(1, 156) % 2 == 1) warzywo.mineraly = "nie";
            else warzywo.mineraly = "tak";

            losuj = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (losuj.Next(1, 161) % 2 == 1) warzywo.susza = "tak";
            else warzywo.susza = "nie";

            losuj = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (losuj.Next(1, 149) % 2 == 1) warzywo.zaorane = "nie";
            else warzywo.zaorane = "tak";

            losuj = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (losuj.Next(1, 169) % 2 == 1) warzywo.zasiane = "tak";
            else warzywo.zasiane = "nie";

            losuj = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (losuj.Next(1, 1698) % 2 == 1) warzywo.zbior = "nie";
            else warzywo.zbior = "tak";


        }

        private void go()
        {

            /* AStar */
            AStar astar = new AStar();

            AStarNode2D GoalNode = new AStarNode2D(null, null, 1, targetX, targetY, 0);
            AStarNode2D StartNode = new AStarNode2D(null, GoalNode, 1, controls.posX, controls.posY, 1);
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
                  new Action(() => controls.TractorMooveLeft(imageName)));
                }
                else if (dir == 1)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() => controls.TractorMooveRight(imageName)));
                }
                else if (dir == 2)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() => controls.TractorMooveDown(imageName)));
                }
                else if (dir == 0)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() => controls.TractorMooveUp(imageName)));

                }




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
            string mPora = pora;
            //treeList = new ID3Sample().GenerateTree("maszyny", "maszyna");
            var tree = new ID3Sample().GenerateTree(@"..\..\maszyny", "maszyna");

            if (targetX != 1 || targetY != 1)
            {
                Thread.Sleep(1000);
                targetY = 1;
                targetX = 1;
                return;
            }
            if (order < orderList.Count - 1)
                order++;
            else
                order = 0;

            //powinna byc lista parametrow okreslajaca pole...
            //za kazdym przejsciem przez wezel, 1 pole znika z listy
            //jezeli lista sie wyczerpie, lub nie zmieni, tzn ze trzeba przerwac


            ID3Attrib[] attribs = new ID3Attrib[] { new ID3Attrib("") };

            tree.decide(attribs);


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
                                            targetX = orderList[order].poleX;
                                            targetY = orderList[order].poleY;
                                            string maszyna = treeList[zz].Split(':')[1];
                                            orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                            imageName = maszyna.Substring(0, maszyna.Length - 1);
                                            changeParameter(imageName, orderList[order]);
                                            return;
                                        }

                                        if (treeList[zz].Contains("bronowane"))
                                        {
                                            if (treeList[zz + 1].Contains(orderList[order].bronowane))
                                            {
                                                if (treeList[zz + 2].Contains(":"))
                                                {
                                                    targetX = orderList[order].poleX;
                                                    targetY = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 2].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, orderList[order]);
                                                    return;
                                                }
                                            }

                                            if (treeList[zz + 3].Contains(orderList[order].bronowane))
                                            {
                                                if (treeList[zz + 4].Contains(":"))
                                                {
                                                    targetX = orderList[order].poleX;
                                                    targetY = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 4].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, orderList[order]);
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
                                                    targetX = orderList[order].poleX;
                                                    targetY = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 2].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, orderList[order]);
                                                    return;
                                                }
                                            }

                                            if (treeList[zz + 3].Contains(orderList[order].zasiane))
                                            {
                                                if (treeList[zz + 4].Contains(":"))
                                                {
                                                    targetX = orderList[order].poleX;
                                                    targetY = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 4].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, orderList[order]);
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
                                                    targetX = orderList[order].poleX;
                                                    targetY = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 2].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, orderList[order]);
                                                    return;
                                                }
                                            }

                                            if (treeList[zz + 3].Contains(orderList[order].zaorane))
                                            {
                                                if (treeList[zz + 4].Contains(":"))
                                                {
                                                    targetX = orderList[order].poleX;
                                                    targetY = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 4].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, orderList[order]);
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
                                                    targetX = orderList[order].poleX;
                                                    targetY = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 2].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, orderList[order]);
                                                    return;
                                                }
                                            }

                                            if (treeList[zz + 3].Contains(orderList[order].susza))
                                            {
                                                if (treeList[zz + 4].Contains(":"))
                                                {
                                                    targetX = orderList[order].poleX;
                                                    targetY = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 4].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, orderList[order]);
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
                                                    targetX = orderList[order].poleX;
                                                    targetY = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 2].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, orderList[order]);
                                                    return;
                                                }
                                            }

                                            if (treeList[zz + 3].Contains(orderList[order].mineraly))
                                            {
                                                if (treeList[zz + 4].Contains(":"))
                                                {
                                                    targetX = orderList[order].poleX;
                                                    targetY = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 4].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, orderList[order]);
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
                                                    targetX = orderList[order].poleX;
                                                    targetY = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 2].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, orderList[order]);
                                                    return;
                                                }
                                            }

                                            if (treeList[zz + 3].Contains(orderList[order].zbior))
                                            {
                                                if (treeList[zz + 4].Contains(":"))
                                                {
                                                    targetX = orderList[order].poleX;
                                                    targetY = orderList[order].poleY;
                                                    string maszyna = treeList[zz + 4].Split(':')[1];
                                                    orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, orderList[order]);
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
                                                            targetX = orderList[order].poleX;
                                                            targetY = orderList[order].poleY;
                                                            string maszyna = treeList[z].Split(':')[1];
                                                            orderList[order].maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                            imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                            changeParameter(imageName, orderList[order]);
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


        public class Dane
        {
            public int zbozeX = 3;
            public int zbozeY = 4;

            public int warzywoX = 6;
            public int warzywoY = 6;



        }
    }
}




