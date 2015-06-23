using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraktorProj.Model;

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


        private MoveEnum FDIR;

        public MoveEnum DIR
        {
            get
            {
                return FDIR;
            }
        }

        public AStarNode2D(AStarNode AParent, AStarNode AGoalNode, double ACost, int AX, int AY, MoveEnum ADIR)
            : base(AParent, AGoalNode, ACost)
        {
            FX = AX;
            FY = AY;
            FDIR = ADIR;
        }


        private void AddSuccessor(ArrayList ASuccessors, int AX, int AY, MoveEnum ADIR)
        {
            int CurrentCost = World.mapa1[new Pos2(AX, AY)];
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
            AddSuccessor(ASuccessors, FX - 1, FY, MoveEnum.Left);

            AddSuccessor(ASuccessors, FX, FY - 1, MoveEnum.Up);

            AddSuccessor(ASuccessors, FX + 1, FY, MoveEnum.Right);

            AddSuccessor(ASuccessors, FX, FY + 1, MoveEnum.Down);

        }


    }

}
