using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace TraktorProj.Algorithms
{
    class MainClass
    {


        static int[,] Map = {
	            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
	            { -1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,-1 },
	            { -1, 2, 2,-1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,-1 },
	            { -1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,-1 },
	            { -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1 },
	            { -1, 2, 2, 2,-1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,-1 },
	            { -1, 2, 2, 2,-1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 },
	            { -1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 },
	            { -1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 },
	            { -1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 },
	            { -1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 }
            };


        static public int GetMap(int x, int y)
        {
            if ((x < 1) || (x > 16))
                return (-1);
            if ((y < 1) || (y > 10))
                return (-1);
            return (Map[y, x]);
        }
    }
}
