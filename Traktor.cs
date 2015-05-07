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
        private List<Point> orderList, orderLists, way; 


        private Controls controls;
        private static Traktor traktor;
        private List<string> Times; 



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
       
        
  

 
        }






        public void StartTraktor()
        {
            Thread startThread = new Thread(TraktorThread);
            startThread.IsBackground = true;

            startThread.Start();


        }

        private void TraktorThread()
        {

            while (true)
            {
                
                //tutaj dzialanie
            }
        }




    }
}
