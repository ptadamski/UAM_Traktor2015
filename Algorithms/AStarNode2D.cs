using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorProj.Algorithms
{             
    public class AStarNode2D : AStarNode
    {         
        public int X
        {
            get
            {
                return FX;
            }
        }
        private int FX;

        public int Y
        {
            get
            {
                return FY;
            }
        }
        private int FY;

        public int DIR
        {
            get
            {
                return FDIR;
            }
        }
        private int FDIR;



        public AStarNode2D(AStarNode AParent, AStarNode AGoalNode, double ACost, int AX, int AY, int ADIR)
            : base(AParent, AGoalNode, ACost)
        {
            FX = AX;
            FY = AY;
            FDIR = ADIR;
        }


        private void AddSuccessor(ArrayList ASuccessors, int AX, int AY, int ADIR)
        {
            int CurrentCost = MainClass.GetMap(AX, AY);
            if (CurrentCost == -1)
            {
                return;
            }
            AStarNode2D NewNode = new AStarNode2D(this, GoalNode, Cost + CurrentCost, AX, AY, ADIR);
            if (NewNode.IsSameState(Parent))
            {
                return;
            }
            ASuccessors.Add(NewNode);
        }


        public override bool IsSameState(AStarNode ANode)
        {
            if (ANode == null)
            {
                return false;
            }
            return ((((AStarNode2D)ANode).X == FX) &&
                (((AStarNode2D)ANode).Y == FY));
        }

        public override void Calculate()
        {
            if (GoalNode != null)
            {
                double xd = FX - ((AStarNode2D)GoalNode).X;
                double yd = FY - ((AStarNode2D)GoalNode).Y;
                GoalEstimate = (Math.Abs(xd) + Math.Abs(yd));
            }
            else
            {
                GoalEstimate = 0;
            }
        }

        public override void GetSuccessors(ArrayList ASuccessors)
        {
            ASuccessors.Clear();
            AddSuccessor(ASuccessors, FX - 1, FY, 3);

            AddSuccessor(ASuccessors, FX, FY - 1, 0);

            AddSuccessor(ASuccessors, FX + 1, FY, 1);

            AddSuccessor(ASuccessors, FX, FY + 1, 2);

        }


    }

}
