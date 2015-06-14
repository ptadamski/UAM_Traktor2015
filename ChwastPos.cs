using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TraktorProj.Algorithms;
using TraktorProj.Commons;
using TraktorProj.ID3Algorithm;
using System.Threading;


namespace TraktorProj
{
    class ChwastPos
    {
        Param wiosna = new Param(TraktorProj.Param.PoraRoku.wiosna);
        Param lato = new Param(TraktorProj.Param.PoraRoku.lato);
        Param jesien = new Param(TraktorProj.Param.PoraRoku.jesien);
        private int[,] istChwast = new int[21, 3];
        private int order = 0;
        public int targetX;
        public int targetY;
        public string pora;
        public Point positions;
        private MainClass main;
        public ChwastPos()
        {
            main = new MainClass();
            for (int i = 0; i <= 20; i++)
            {
                for (int j = 0; j < 3; j++)
                    istChwast[i, j] = 0;
            }
            // this.controls = new Controls();
            // istChwast = new int [20][3];
            //this.istChwast = istChwast;
        }
        public int getAvaiableId()
        {
            for (int i = 1; i <= 20; i++)
            {

                if (istChwast[i, 2] == 0)
                {

                    return i;
                }
            }

            return 0;
        }

        public void addChwast(int id, int posx, int posy, int zycie)
        {
            istChwast[id, 0] = posx;
            istChwast[id, 1] = posy;
            istChwast[id, 2] = zycie;
        }

        public void getPositions(int id)
        {

            int cnt = 0;
            //Point[] pozycje = new Point []{};
            if (istChwast[1, 2] == 0)
            {
                Random random = new Random();
                int posx = random.Next(1, 15);
                int posy = random.Next(1, 10);
                Point a = new Point(posx, posy);
                positions = a;

            }
            else
            {

                Point[] pozycje = Enumerable.Repeat<Point>(new Point(0, 0), 1000).ToArray();
                for (int i = 1; i <= 20; i++)
                {
                    if (istChwast[i, 2] > 0)
                    {
                        int x = istChwast[i, 0];
                        int y = istChwast[i, 1];

                        for (int x1 = x - 1; x1 <= x + 1; x1++)
                        {
                            for (int y1 = y - 1; y1 <= y + 1; y1++)
                            {

                                if (x1 != x || y1 != y)
                                {
                                    int TypField = main.GetMap2(x1, y1);
                                    if (TypField != -1 && TypField != 7)
                                    {
                                        //Point p = new Point(x1, y1);
                                        pozycje[cnt] = new Point(x1, y1);
                                        cnt++;
                                    }

                                }
                            }


                        }
                    }
                }

                Random random = new Random();
                int idpos = random.Next(0, cnt - 1);

                positions = pozycje[idpos];
            }
            main.SetMap2(positions.X, positions.Y);


        }

         public void StartSzkodnik(int x, int y)
        {
            targetX = x;
            targetY = y;
            main = new MainClass();
            Thread startThread = new Thread(SzkodnikThread);
            startThread.IsBackground = true;

            startThread.Start();


        }

        public void StartSzkodnik()
        {
            //pora = p;
            Thread startThread = new Thread(SzkodnikThread);
            startThread.IsBackground = true;

            startThread.Start();


        }

        private void SzkodnikThread()
        {
             while(true){
                generateParam();
            /*
            if (controls.posX > 1 && controls.posY > 1)
            {
                // targetY = targetX = 1;
                go();
                Thread.Sleep(6000);
            }

            //
            //zboze.zaorane = "tak";

            */
            RunID3();
           
            /*
            if (imageName == "kombajn")
            {
                //main.SetMap3(targetX, targetY, 1);
                fieldImageName = "zamlocone";
            }
            if (imageName == "brona")
            {
                //main.SetMap3(targetX, targetY, 1);
                fieldImageName = "zabronowane";
            }
            if (imageName == "plug")
            {
                //main.SetMap3(targetX, targetY, 1);
                fieldImageName = "zaorane";
            }
            if (imageName == "deszczownia")
            {
                //main.SetMap3(targetX, targetY, 1);
                fieldImageName = "zapryskane";
            }
            if (imageName == "sadzarka")
            {
                //main.SetMap3(targetX, targetY, 1);
                fieldImageName = "zasiane";
            }

            go();
            // Traktor.Instance.zmienPole(targetX, targetY, fieldImageName);


            
            targetY = targetX = 1;
            go();
            */
            Thread.Sleep(1000);
             }          
        }
        
        public void generateParam()
        {
            Random
            losuj = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (losuj.Next(1, 123) % 2 == 1) wiosna.trawa = "nie";
            else wiosna.trawa = "tak";

            losuj = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (losuj.Next(1, 168) % 2 == 1) lato.pole = "nie";
            else lato.pole = "tak";

            losuj = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (losuj.Next(1, 111) % 2 == 1) lato.uprawa = "nie";
            else lato.uprawa = "tak";

            losuj = new System.Random(DateTime.Now.Millisecond); // 1 przypadek
            if (losuj.Next(1, 57) % 2 == 1) jesien.chodnik = "nie";
            else jesien.chodnik = "tak";
        }

        private void RunID3()
        {
            List<string> treeList = new List<string>();
            List<Param> orderList = new List<Param>();
            orderList.Clear();

            orderList.Add(wiosna);
            orderList.Add(lato);
            orderList.Add(jesien);
            // image = "tractor";
            ID3Sample id3Sample = new ID3Sample("");

            treeList = id3Sample.GenerateTree2();

            if (order < orderList.Count - 1)
                order++;
            else
                order = 0;

            
            for (int i = 0; i < treeList.Count; i++)
            {
                if (treeList[i].Contains("pora"))
                {
                    for (int j = i + 1; j < treeList.Count; j++)
                    {
                        if (treeList[j].Contains(orderList[order].okres))
                        {
                            for (int l = j + 1; l < treeList.Count; l++)
                            {
                                if (treeList[l].Contains(":"))
                                {
                                    string szkodnik = treeList[l].Split(':')[1];
                                    orderList[order].szkodnik = szkodnik.Substring(0, szkodnik.Length - 1);
                                    return;
                                }
                                if (treeList[l].Contains("trawa"))
                                {
                                    if (treeList[l + 1].Contains(orderList[order].trawa))
                                    {
                                        if (treeList[l + 2].Contains(":"))
                                        {
                                            string szkodnik = treeList[l + 2].Split(':')[1];
                                            orderList[order].szkodnik = szkodnik.Substring(0, szkodnik.Length - 1);
                                            return;
                                        }
                                    }
                                    if (treeList[l + 3].Contains(orderList[order].trawa))
                                    {
                                        if (treeList[l + 4].Contains(":"))
                                        {
                                            string szkodnik = treeList[l + 4].Split(':')[1];
                                            orderList[order].szkodnik = szkodnik.Substring(0, szkodnik.Length - 1);
                                            return;
                                        }
                                    }
                                }
                                if (treeList[l].Contains("pole"))
                                {
                                    if (treeList[l + 1].Contains(orderList[order].pole))
                                    {
                                        if (treeList[l + 2].Contains(":"))
                                        {
                                            string szkodnik = treeList[l + 2].Split(':')[1];
                                            orderList[order].szkodnik = szkodnik.Substring(0, szkodnik.Length - 1);
                                            return;
                                        }
                                    }
                                    if (treeList[l + 3].Contains(orderList[order].pole))
                                    {
                                        if (treeList[l + 4].Contains(":"))
                                        {
                                            string szkodnik = treeList[l + 4].Split(':')[1];
                                            orderList[order].szkodnik = szkodnik.Substring(0, szkodnik.Length - 1);
                                            return;
                                        }
                                    }
                                }
                                if (treeList[l].Contains("chodnik"))
                                {
                                    if (treeList[l + 1].Contains(orderList[order].chodnik))
                                    {
                                        if (treeList[l + 2].Contains(":"))
                                        {
                                            string szkodnik = treeList[l + 2].Split(':')[1];
                                            orderList[order].szkodnik = szkodnik.Substring(0, szkodnik.Length - 1);
                                            return;
                                        }
                                    }
                                    if (treeList[l + 3].Contains(orderList[order].chodnik))
                                    {
                                        if (treeList[l + 4].Contains(":"))
                                        {
                                            string szkodnik = treeList[l + 4].Split(':')[1];
                                            orderList[order].szkodnik = szkodnik.Substring(0, szkodnik.Length - 1);
                                            return;
                                        }
                                    }
                                }
                                if (treeList[l].Contains("uprawa"))
                                {
                                    if (treeList[l + 1].Contains(orderList[order].uprawa))
                                    {
                                        if (treeList[l + 2].Contains(":"))
                                        {
                                            string szkodnik = treeList[l + 2].Split(':')[1];
                                            orderList[order].szkodnik = szkodnik.Substring(0, szkodnik.Length - 1);
                                            return;
                                        }
                                    }
                                    if (treeList[l + 3].Contains(orderList[order].uprawa))
                                    {
                                        if (treeList[l + 4].Contains(":"))
                                        {
                                            string szkodnik = treeList[l + 4].Split(':')[1];
                                            orderList[order].szkodnik = szkodnik.Substring(0, szkodnik.Length - 1);
                                            return;
                                        }
                                    }
                                }

                            }
                            return;
                        }
                    }
                }
            }
        }
        private void changeParameter(string szkodnik, Param p)
        {
            if (szkodnik.Equals("kwiaty")) { if (p.trawa.Equals("tak")) { p.trawa = "nie"; } else { p.trawa = "tak"; } }
            if (szkodnik.Equals("piasek")) { if (p.chodnik.Equals("tak")) { p.chodnik = "nie"; } else { p.chodnik = "tak"; } }
            if (szkodnik.Equals("pokrzywa")) { if (p.pole.Equals("tak")) { p.pole = "nie"; } else { p.pole = "tak"; } }
            if (szkodnik.Equals("owad")) { if (p.uprawa.Equals("tak")) { p.uprawa = "nie"; } else { p.uprawa = "tak"; } }
        }


    }
}
