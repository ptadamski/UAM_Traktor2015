using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TraktorProj.Algorithms;
using TraktorProj.Commons;
using TraktorProj.ID3Algorithm;

namespace TraktorProj
{
    public delegate void OnTileDrawEvent(int posx, int posy, string sprite, Rotation rot);

    class Traktor
    {


        MainWindow window = null;
        Random random = new System.Random();
        private Controls controls;
        private int targetX = 1, targetY = 1;
        private string pora;
        private string imageName;
        private string fieldImageName;
        private MainClass main = new MainClass();
        private int order = 0;
        int i = 0;                                       
        Queue<Parametry> zadania = new Queue<Parametry>();
        IList<Parametry> uprawy = new List<Parametry>();
        //event OnTileDrawEvent tileDrawer;

        //Parametry zboze = new Parametry(TraktorProj.Parametry.RodzajUprawy.zboze);
        //Parametry warzywo = new Parametry(TraktorProj.Parametry.RodzajUprawy.warzywo);


        //public static Traktor Instance
        //{
        //    get
        //    {
        //        return traktor = traktor ?? new Traktor("tractor");
        //    }
        //}

        public Traktor(MainWindow window, string imageName)
        {
            this.window = window;
            //this.tileDrawer += tileDrawer;
            this.controls = new Controls();
            this.imageName = imageName;
            this.Update(true);                              
        }

        public void Update(bool initFromFile = false)
        {
            if (initFromFile)
            {
                uprawy.Clear();
                zadania.Clear();

                DataTable tabUprawy = new CSV(@"..\..\MapaUpraw", ',', true).Table;
                DataTable tabRynek = new CSV(@"..\..\KsiegaRoslin", ',', true).Table;

                IDictionary<string, string> katalog = new Dictionary<string, string>();

                var wyliczenieProduktow = tabRynek.AsEnumerable();

                foreach (var produkt in wyliczenieProduktow)
                    katalog.Add(produkt["uprawa"] as string, produkt["rodzaj"] as string);

                Parametry param = null;
                for (int i = 0; i < tabUprawy.Rows.Count && i<5; i++)
                {
                    for (int j = 0; j < tabUprawy.Columns.Count && j<5; j++)
                    {
                        var uprawa = tabUprawy.Rows[i].ItemArray[j] as string;

                        if (katalog[uprawa].Equals("warzywo"))
                            param = new Parametry("warzywo", j+1, i+1);
                        else if (katalog[uprawa].Equals("zboze"))
                            param = new Parametry("zboze", j+1 ,i+1);
                        uprawy.Add(param);
                        //param.Randomize();
                    }
                }
            }

            foreach (var param in uprawy)
                zadania.Enqueue(param);
        }

        public void PoryRokuStart()
        {
            //Window window = Application.Current.Windows[0];
            (window as MainWindow).ConsoleOutTextBlock.Text += "\r\n> " + "wiosna";
            Thread.Sleep(15000);
            (window as MainWindow).ConsoleOutTextBlock.Text += "\r\n> " + "lato";
            Thread.Sleep(15000);
            (window as MainWindow).ConsoleOutTextBlock.Text += "\r\n> " + "jesien";
            Thread.Sleep(15000);
            
        }



        public void StartTraktor(int x, int y)
        {
            targetX = x;
            targetY = y;

            Update();

            
        Thread startThread = new Thread(TraktorThread);
            startThread.IsBackground = true;

            startThread.Start();


        }

        public void StartTraktor(string p)
        {
            pora = p;

            Update();
        Thread startThread = new Thread(TraktorThread);
            startThread.IsBackground = true;

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
           //  while(true){
            // generateParam();
           
            if (controls.posX > 1 && controls.posY > 1)
            {
               // targetY = targetX = 1;
                go();
                Thread.Sleep(1000);
            }

            //
            //zboze.zaorane = "tak";


            RunID3(@"..\..\maszyny", "maszyna", zadania);
            if (imageName == "kombajn")
            {
                main.SetMap3(targetX, targetY, 1);
                fieldImageName = "zamlocone";
            }
            if (imageName == "brona")
            {
                main.SetMap3(targetX, targetY, 2);
                fieldImageName = "zabronowane";
            }
            if (imageName == "plug")
            {
                main.SetMap3(targetX, targetY, 3);
                fieldImageName = "zaorane";
            }
            if (imageName == "deszczownia")
            {
                main.SetMap3(targetX, targetY, 4);
                fieldImageName = "zapryskane";
            }
            if (imageName == "sadzarka")
            {
                main.SetMap3(targetX, targetY, 5);
                fieldImageName = "zasiane";
            }
            if (imageName == "rozrzutnik")
            {
                main.SetMap3(targetX, targetY, 6);
                fieldImageName = "rozsiane";
            }

            foreach (var param in uprawy)
            {
                if (param.rodzaj == "warzywo")
                {
                    if (param.wzrost <= 0)
                    {
                        imageName = "sadzarka";
                        main.SetMap3(targetX, targetY, 6);
                        fieldImageName = "zasiane";
                    }
                    else if (param.wzrost >= 1)
                    {
                        imageName = "kopaczka";
                        main.SetMap3(targetX, targetY, 6);
                        fieldImageName = "zamlocone";
                        param.wzrost = 1.1;
                    }

                }
                else if (param.rodzaj == "zboze")
                {
                    if (param.wzrost <= 0)
                    {
                        imageName = "siewnik";
                        main.SetMap3(targetX, targetY, 6);
                        fieldImageName = "zasiane";
                    }
                    else if (param.wzrost >= 1)
                    {
                        imageName = "kombajn";
                        main.SetMap3(targetX, targetY, 6);
                        fieldImageName = "zamlocone";
                        param.wzrost = 1.1;
                    }

                }

            }

            go();
           // Traktor.Instance.zmienPole(targetX, targetY, fieldImageName);
           

            
            Thread.Sleep(1000);

            if (zadania.Count!=0)
                TraktorThread();
            //targetY = targetX = 1;
            //go();

            // }      
        }


        public void generateHeight()
        {
            foreach (var param in uprawy)
            {
                param.wzrost += 0.4;
                //if (tileDrawer != null)
                //    tileDrawer(param.poleX, param.poleY, param.rodzaj, Rotation.Rotate0);
            }
        }

        public void generateParam()
        {
            foreach (var param in uprawy)
            {
                param.Randomize();
            }
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
                  new Action(() => controls.TractorMooveLeft(imageName,targetX,targetY,fieldImageName)));
                }
                else if (dir == 1)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() => controls.TractorMooveRight(imageName, targetX, targetY, fieldImageName)));
                }
                else if (dir == 2)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() => controls.TractorMooveDown(imageName, targetX, targetY, fieldImageName)));
                }
                else if (dir == 0)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() => controls.TractorMooveUp(imageName, targetX, targetY, fieldImageName)));

                }




            }
        }

        /// <summary>
        /// pobierając liste klientów zwróci Point kolejnego do którego ma się udać
        /// </summary>
        private void RunID3(string filepath, string attribName, 
            Queue<Parametry> orderList)
        {
            List<string> treeList = new List<string>();
            //List<Parametry> orderList = new List<Parametry>();
            //orderList.Clear();

            //orderList.Add(zboze);
            //orderList.Add(warzywo);
            // image = "tractor";

            ID3Sample id3Sample = new ID3Sample();//tu musi byc jakis load z pliku...
            string mPora = pora;  

            if (orderList.Count == 0)
                return;       

            if (mPora == null)
                return;


            treeList = id3Sample.GenerateTree(filepath, attribName);
            //if (targetX != 1 || targetY != 1)
            //{
            //    Thread.Sleep(1000);
            //    targetY = 1;
            //    targetX = 1;
            //    return;
            //}
            //if (order < orderList.Count - 1)
             //   order++;
            //else
            //    order = 0;


            var order = orderList.Dequeue();
            Console.WriteLine("x={0} y={1} roslina={2}", order.poleX, order.poleY, order.rodzaj);


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
                                if (treeList[l].Contains(order.rodzaj))
                                {
                                    for (int zz = l + 1; zz < treeList.Count; zz++)
                                    {
                                        if (treeList[zz].Contains(":"))
                                        {
                                            targetX = order.poleX;
                                            targetY = order.poleY;
                                           
                                            string maszyna = treeList[zz].Split(':')[1];
                                            order.maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                            imageName = maszyna.Substring(0, maszyna.Length - 1);
                                            changeParameter(imageName, order);
                                            return;
                                        }

                                        if (treeList[zz].Contains("bronowane"))
                                        {
                                            if (treeList[zz + 1].Contains(order.bronowane))
                                            {
                                                if (treeList[zz + 2].Contains(":"))
                                                {
                                                    targetX = order.poleX;
                                                    targetY = order.poleY;
                                                    string maszyna = treeList[zz + 2].Split(':')[1];
                                                    order.maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, order);   
                                                    return;
                                                }
                                            }

                                            if (treeList[zz + 3].Contains(order.bronowane))
                                            {
                                                if (treeList[zz + 4].Contains(":"))
                                                {
                                                    targetX = order.poleX;
                                                    targetY = order.poleY;
                                                    string maszyna = treeList[zz + 4].Split(':')[1];
                                                    order.maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, order);
                                                    return;
                                                }
                                            }
                                        }

                                        if (treeList[zz].Contains("zasiane"))
                                        {
                                            if (treeList[zz + 1].Contains(order.zasiane))
                                            {
                                                if (treeList[zz + 2].Contains(":"))
                                                {
                                                    targetX = order.poleX;
                                                    targetY = order.poleY;
                                                    string maszyna = treeList[zz + 2].Split(':')[1];
                                                    order.maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, order);
                                                    return;
                                                }
                                            }

                                            if (treeList[zz + 3].Contains(order.zasiane))
                                            {
                                                if (treeList[zz + 4].Contains(":"))
                                                {
                                                    targetX = order.poleX;
                                                    targetY = order.poleY;
                                                    string maszyna = treeList[zz + 4].Split(':')[1];
                                                    order.maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, order);
                                                    return;
                                                }
                                            }
                                        }

                                        if (treeList[zz].Contains("zaorane"))
                                        {
                                            if (treeList[zz + 1].Contains(order.zaorane))
                                            {
                                                if (treeList[zz + 2].Contains(":"))
                                                {
                                                    targetX = order.poleX;
                                                    targetY = order.poleY;
                                                    string maszyna = treeList[zz + 2].Split(':')[1];
                                                    order.maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, order);
                                                    return;
                                                }
                                            }

                                            if (treeList[zz + 3].Contains(order.zaorane))
                                            {
                                                if (treeList[zz + 4].Contains(":"))
                                                {
                                                    targetX = order.poleX;
                                                    targetY = order.poleY;
                                                    string maszyna = treeList[zz + 4].Split(':')[1];
                                                    order.maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, order);
                                                    return;
                                                }
                                            }
                                        }

                                        if (treeList[zz].Contains("susza"))
                                        {
                                            if (treeList[zz + 1].Contains(order.susza))
                                            {
                                                if (treeList[zz + 2].Contains(":"))
                                                {
                                                    targetX = order.poleX;
                                                    targetY = order.poleY;
                                                    string maszyna = treeList[zz + 2].Split(':')[1];
                                                    order.maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, order);
                                                    return;
                                                }
                                            }

                                            if (treeList[zz + 3].Contains(order.susza))
                                            {
                                                if (treeList[zz + 4].Contains(":"))
                                                {
                                                    targetX = order.poleX;
                                                    targetY = order.poleY;
                                                    string maszyna = treeList[zz + 4].Split(':')[1];
                                                    order.maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, order);
                                                    return;
                                                }
                                            }
                                        }

                                        if (treeList[zz].Contains("mineraly"))
                                        {
                                            if (treeList[zz + 1].Contains(order.mineraly))
                                            {
                                                if (treeList[zz + 2].Contains(":"))
                                                {
                                                    targetX = order.poleX;
                                                    targetY = order.poleY;
                                                    string maszyna = treeList[zz + 2].Split(':')[1];
                                                    order.maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, order);
                                                    return;
                                                }
                                            }

                                            if (treeList[zz + 3].Contains(order.mineraly))
                                            {
                                                if (treeList[zz + 4].Contains(":"))
                                                {
                                                    targetX = order.poleX;
                                                    targetY = order.poleY;
                                                    string maszyna = treeList[zz + 4].Split(':')[1];
                                                    order.maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, order);
                                                    return;
                                                }
                                            }
                                        }
                                        if (treeList[zz].Contains("zbior"))
                                        {
                                            if (treeList[zz + 1].Contains(order.zbior))
                                            {
                                                if (treeList[zz + 2].Contains(":"))
                                                {
                                                    targetX = order.poleX;
                                                    targetY = order.poleY;
                                                    string maszyna = treeList[zz + 2].Split(':')[1];
                                                    order.maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, order);
                                                    return;
                                                }
                                            }

                                            if (treeList[zz + 3].Contains(order.zbior))
                                            {
                                                if (treeList[zz + 4].Contains(":"))
                                                {
                                                    targetX = order.poleX;
                                                    targetY = order.poleY;
                                                    string maszyna = treeList[zz + 4].Split(':')[1];
                                                    order.maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                    imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                    changeParameter(imageName, order);
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
                                                if (treeList[zzz].Contains(order.mineraly))
                                                {

                                                    for (int z = zzz + 1; z < treeList.Count; z++)
                                                    {
                                                        if (treeList[z].Contains(":"))
                                                        {
                                                            targetX = order.poleX;
                                                            targetY = order.poleY;
                                                            string maszyna = treeList[z].Split(':')[1];
                                                            order.maszyna = maszyna.Substring(0, maszyna.Length - 1);
                                                            imageName = maszyna.Substring(0, maszyna.Length - 1);
                                                            changeParameter(imageName, order);
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
            Console.WriteLine("wybrano maszyne: {0}", maszyna);
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
            //Window window = Application.Current.Windows[0];


            if (window.GetType() == typeof(MainWindow))
            {


                (window as MainWindow).ConsoleOutTextBlock.Text += "\r\n>Wylosowana pozycja: " + posx + " " + posy;
            }
        }

        public void zmienPole(int x, int y, string field)
        {
            //Window window = Application.Current.Windows[0];

            if (window.GetType() == typeof(MainWindow))
            {
                (window as MainWindow).setTile(x, y, field);
            }
        }
    }
}




