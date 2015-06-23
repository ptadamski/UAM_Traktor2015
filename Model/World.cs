using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraktorProj.Interface;

namespace TraktorProj.Model
{

    public class World
    {

        //odpowiada za 
            //przechowanie wszystkich obiektow w swiecie
            //symulacje srodowiska
            //detekcje kolizji     

        public static Terrain2 mapa1 = new Terrain2(@".\..\..\mapa1");
        public static Terrain2 mapa2 = new Terrain2(@".\..\..\mapa2");
        public static Terrain2 mapa3 = new Terrain2(@".\..\..\mapa3");
             
        public void Update()
        {
        }

        protected IDictionary<Pos2, IList<IDrawableObject<Pos2>>> items = new Dictionary<Pos2, IList<IDrawableObject<Pos2>>>();
           
        public IList<IDrawableObject<Pos2>> this[Pos2 position]
        {
            get
            {
                IList<IDrawableObject<Pos2>> obj;
                if (!items.TryGetValue(position, out obj))
                    throw new IndexOutOfRangeException();
                return obj;
            }
            set
            {
                IList<IDrawableObject<Pos2>> obj;
                if (items.TryGetValue(position, out obj))
                    items[position] = value;
            }
        }    
    }
}
