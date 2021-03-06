﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;

namespace TraktorProj.Algorithms
{
    public class AStarNode : IComparable
    {

        private AStarNode FParent;

        public AStarNode Parent
        {
            get
            {
                return FParent;
            }
            set
            {
                FParent = value;
            }
        }

        public double Cost
        {
            set
            {
                FCost = value;
            }
            get
            {
                return FCost;
            }
        }
        private double FCost;

        public double GoalEstimate
        {
            set
            {
                FGoalEstimate = value;
            }
            get
            {
                Calculate();
                return (FGoalEstimate);
            }
        }
        private double FGoalEstimate;

        public double TotalCost
        {
            get
            {
                return (Cost + GoalEstimate);
            }
        }

        public AStarNode GoalNode
        {
            set
            {
                FGoalNode = value;
                Calculate();
            }
            get
            {
                return FGoalNode;
            }
        }
        private AStarNode FGoalNode;



        public AStarNode(AStarNode AParent, AStarNode AGoalNode, double ACost)
        {
            FParent = AParent;
            FCost = ACost;
            GoalNode = AGoalNode;
        }


        public bool IsGoal()
        {
            return IsSameState(FGoalNode);
        }


        public virtual bool IsSameState(AStarNode ANode)
        {
            return false;
        }

        public virtual void Calculate()
        {
            FGoalEstimate = 0.0f;
        }

        public virtual void GetSuccessors(ArrayList ASuccessors)
        {
        }


        public override bool Equals(object obj)
        {
            return IsSameState((AStarNode)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }



        public int CompareTo(object obj)
        {
            return (-TotalCost.CompareTo(((AStarNode)obj).TotalCost));
        }


    }

    public sealed class AStar
    {


        private AStarNode FStartNode;
        private AStarNode FGoalNode;
        private Heap Fringe;

        private ArrayList FSuccessors;



        public ArrayList Solution
        {
            get
            {
                return FSolution;
            }
        }
        private ArrayList FSolution;



        public AStar()
        {
            Fringe = new Heap();

            FSuccessors = new ArrayList();
            FSolution = new ArrayList();
        }


        public void FindPath(AStarNode AStartNode, AStarNode AGoalNode)
        {
            FStartNode = AStartNode;
            FGoalNode = AGoalNode;

            Fringe.Add(FStartNode);

            while (Fringe.Count > 0)
            {
                AStarNode NodeCurrent = (AStarNode)Fringe.Pop();

                if (NodeCurrent.IsGoal())
                {

                    while (NodeCurrent != null)
                    {
                        FSolution.Insert(0, NodeCurrent);

                        NodeCurrent = NodeCurrent.Parent;
                    }
                    break;
                }

                NodeCurrent.GetSuccessors(FSuccessors);

                foreach (AStarNode NodeSuccessor in FSuccessors)
                {
                    AStarNode NodeOpen = null;
                    if (Fringe.Contains(NodeSuccessor))
                        NodeOpen = (AStarNode)Fringe[Fringe.IndexOf(NodeSuccessor)];
                    if ((NodeOpen != null) && (NodeSuccessor.TotalCost > NodeOpen.TotalCost))
                        continue;





                    Fringe.Remove(NodeOpen);
                    Fringe.Push(NodeSuccessor);
                }

            }
        }

    }
}
