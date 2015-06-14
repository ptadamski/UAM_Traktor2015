using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TraktorProj.Algorithms;
using TraktorProj.Commons;
using TraktorProj.ID3Algorithm;


namespace TraktorProj
{
    class ChwastPos
    {
        private int[,] istChwast = new int[21,3];
        public Point positions;
        public ChwastPos()
        {
            for (int i = 0; i <= 20 ; i++)
            {
                for (int j = 0; j < 3; j++)
                    istChwast[i,j] = 0;
            }
           // this.controls = new Controls();
            // istChwast = new int [20][3];
            //this.istChwast = istChwast;
        }
        public int getAvaiableId()
        {
            for (int i=1;i<=20;i++){

                if (istChwast[i,2] == 0){

                    return i;
                }
            }

            return 0;
        }

        public void addChwast(int id, int posx, int posy, int zycie)
        {
            istChwast[id, 0] =  posx;
            istChwast[id, 1] = posy;
            istChwast[id, 2] = zycie;
        }

        public void getPositions(int id)
        {
            MainClass main = new MainClass();
            //Point[] pozycje = new Point []{};
            if (istChwast[1, 2] == 0)
            {
                Random random = new Random();
                int posx = random.Next(1, 15);
                int posy = random.Next(1, 10);
                Point a = new Point ( posx, posy );
                positions = a;

            }
            else
            {
                 int cnt = 0;
                 Point[] pozycje = Enumerable.Repeat<Point>(new Point(0,0),1000).ToArray();
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
                int idpos = random.Next(0, cnt);

                positions = pozycje[idpos];
            }

        }
    }
}
