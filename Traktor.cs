using System;
using System.Collections.Generic;
using System.Data;
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
using TraktorProj.Model;
using TraktorProj.Interface;

namespace TraktorProj
{

    public enum MoveEnum { None=-1, Up=0, Right=1, Down=2, Left=3 }

    public class Traktor : DrawableObject
    {

        private Controls controls;
        private Pos2 targetLocation;
                                        
        private string imageName = "";
        private string fieldImageName = "";
        private string pora;
        private MainClass main = new MainClass();
        private int order = 0;
        int i = 0;
        Parametry zboze = new Parametry(TraktorProj.Parametry.RodzajUprawy.zboze);
        Parametry warzywo = new Parametry(TraktorProj.Parametry.RodzajUprawy.warzywo);

        Queue<MoveEnum> movementQueue;

        public Traktor(IDrawManager<Pos2> drawManager, Pos2 location, string name) :
            base(drawManager, location, name)
        {
            this.controls = new Controls();
            this.targetLocation = new Pos2(location.X, location.Y);//nowy obiekt, chyba ze Pos2 bedzie struktura
        }

        public void PoryRokuStart()
        {
            Window window = Application.Current.Windows[0];
            (window as MainWindow).ConsoleOutTextBlock.Text += "\r\n> " + "wiosna";
            Thread.Sleep(15000);
            (window as MainWindow).ConsoleOutTextBlock.Text += "\r\n> " + "lato";
            Thread.Sleep(15000);
            (window as MainWindow).ConsoleOutTextBlock.Text += "\r\n> " + "jesien";
            Thread.Sleep(15000);

        }

        public void StartTraktor(int x, int y)
        {
            targetLocation.X = x;
            targetLocation.Y = y;
            InitThread();
        }

        public void StartTraktor(string p)
        {
            pora = p;
            InitThread();
        }

        private void InitThread()
        {
            Thread startThread = new Thread(TraktorThread);
            startThread.IsBackground = true;
            startThread.SetApartmentState(ApartmentState.STA);
            startThread.Start();
        }

        private void TraktorThread()
        {
            i++;

            if (i % 3 == 0)
            {
                generateHeight();
                i = 0;
            }

            RunID3();
            if (imageName == "kombajn")
            {
                main.SetMap3(targetLocation.X, targetLocation.Y, 1);
                fieldImageName = "zamlocone";
            }
            if (imageName == "brona")
            {
                main.SetMap3(targetLocation.X, targetLocation.Y, 2);
                fieldImageName = "zabronowane";
            }
            if (imageName == "plug")
            {
                main.SetMap3(targetLocation.X, targetLocation.Y, 3);
                fieldImageName = "zaorane";
            }
            if (imageName == "deszczownia")
            {
                main.SetMap3(targetLocation.X, targetLocation.Y, 4);
                fieldImageName = "zapryskane";
            }
            if (imageName == "sadzarka")
            {
                main.SetMap3(targetLocation.X, targetLocation.Y, 5);
                fieldImageName = "zasiane";
            }
            if (imageName == "rozrzutnik")
            {
                main.SetMap3(targetLocation.X, targetLocation.Y, 6);
                fieldImageName = "rozsiane";
            }
            if (warzywo.wzrost < 0 && warzywo.wzrost > -1)
            {
                imageName = "sadzarka";
                main.SetMap3(targetLocation.X, targetLocation.Y, 6);
                fieldImageName = "zasiane";
            }
            if (zboze.wzrost < 0 && zboze.wzrost > -1)
            {
                imageName = "siewnik";
                main.SetMap3(targetLocation.X, targetLocation.Y, 6);
                fieldImageName = "zasiane";
            }

            if (warzywo.wzrost >= 1)
            {
                imageName = "kopaczka";
                main.SetMap3(targetLocation.X, targetLocation.Y, 6);
                fieldImageName = "zamlocone";
                warzywo.wzrost = -1;
            }
            if (zboze.wzrost >= 1)
            {
                imageName = "kombajn";
                main.SetMap3(targetLocation.X, targetLocation.Y, 6);
                fieldImageName = "zamlocone";
                zboze.wzrost = -1;
            }

            findPath();
            // Traktor.Instance.zmienPole(targetLocation.X, targetLocation.Y, fieldImageName);


            Thread.Sleep(1000);
            //targetLocation.Y = targetLocation.X = 1;
            //go();

            // }          
        }


        public void generateHeight()
        {
            zboze.wzrost += 0.2;
            warzywo.wzrost += 0.2;
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

        private void findPath()
        {
            /* AStar */
            AStar astar = new AStar();

            AStarNode2D GoalNode = new AStarNode2D(null, null, 1, targetLocation.X, targetLocation.Y, 0);
            AStarNode2D StartNode = new AStarNode2D(null, GoalNode, 1, Location.X, Location.Y, MoveEnum.Right);
            StartNode.GoalNode = GoalNode;

            astar.FindPath(StartNode, GoalNode);
            /* AStar */
            AStarNode2D point0 = (AStarNode2D)astar.Solution[0];
            astar.Solution.RemoveAt(0);

            foreach (AStarNode2D point in astar.Solution)
            {
                Thread.Sleep(600);
                MoveBy(point.DIR);
            }
        }

        /// <summary>
        /// pobierając liste klientów zwróci Point kolejnego do którego ma się udać
        /// </summary>
        private void RunID3()
        {
            return;
            List<string> treeList = new List<string>();
            List<Parametry> orderList = new List<Parametry>();
            orderList.Clear();

            orderList.Add(zboze);
            orderList.Add(warzywo);
            // image = "tractor";




            ID3Sample id3Sample = new ID3Sample("");//tu musi byc jakis load z pliku...
            string mPora = pora;


            treeList = id3Sample.GenerateTree();
            //if (targetLocation.X != 1 || targetLocation.Y != 1)
            //{
            //    Thread.Sleep(1000);
            //    targetLocation.Y = 1;
            //    targetLocation.X = 1;
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
                                            targetLocation.X = orderList[order].poleX;
                                            targetLocation.Y = orderList[order].poleY;

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
                                                    targetLocation.X = orderList[order].poleX;
                                                    targetLocation.Y = orderList[order].poleY;
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
                                                    targetLocation.X = orderList[order].poleX;
                                                    targetLocation.Y = orderList[order].poleY;
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
                                                    targetLocation.X = orderList[order].poleX;
                                                    targetLocation.Y = orderList[order].poleY;
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
                                                    targetLocation.X = orderList[order].poleX;
                                                    targetLocation.Y = orderList[order].poleY;
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
                                                    targetLocation.X = orderList[order].poleX;
                                                    targetLocation.Y = orderList[order].poleY;
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
                                                    targetLocation.X = orderList[order].poleX;
                                                    targetLocation.Y = orderList[order].poleY;
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
                                                    targetLocation.X = orderList[order].poleX;
                                                    targetLocation.Y = orderList[order].poleY;
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
                                                    targetLocation.X = orderList[order].poleX;
                                                    targetLocation.Y = orderList[order].poleY;
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
                                                    targetLocation.X = orderList[order].poleX;
                                                    targetLocation.Y = orderList[order].poleY;
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
                                                    targetLocation.X = orderList[order].poleX;
                                                    targetLocation.Y = orderList[order].poleY;
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
                                                    targetLocation.X = orderList[order].poleX;
                                                    targetLocation.Y = orderList[order].poleY;
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
                                                    targetLocation.X = orderList[order].poleX;
                                                    targetLocation.Y = orderList[order].poleY;
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
                                                            targetLocation.X = orderList[order].poleX;
                                                            targetLocation.Y = orderList[order].poleY;
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

        public void LosujPos()
        {
            Random random = new Random();
            int posx = random.Next(1, 15);
            int posy = random.Next(1, 10);
            Window window = Application.Current.Windows[0];


            if (window.GetType() == typeof(MainWindow))
            {


                (window as MainWindow).ConsoleOutTextBlock.Text += "\r\n>Wylosowana pozycja: " + posx + " " + posy;
            }
        }


        public void MoveBy(MoveEnum e)
        {
            //powinno przejsc przez srodowisko, zeby sprawdzic czy ruch jest dopuszczalny
            switch (e)
            {
                case MoveEnum.Up: Location.Y -= 1;
                    break;
                case MoveEnum.Down: Location.Y += 1;
                    break;
                case MoveEnum.Left: Location.X -= 1;
                    break;
                case MoveEnum.Right: Location.X += 1;
                    break;
            }
            drawManager.Update(this);
        }


    }
}




