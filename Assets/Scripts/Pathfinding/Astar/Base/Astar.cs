using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using DataStructures.PriorityQueue;

namespace Pathfinding
{
    public class Node
    {
        // Change this depending on what the desired size is for each element in the grid
        public static int NODE_SIZE = 32;
        public Node Parent;
        public Vector2 Position;
        public Vector2 Center
        {
            get
            {
                return new Vector2(Position.X + NODE_SIZE / 2, Position.Y + NODE_SIZE / 2);
            }
        }
        public float DistanceToTarget;
        public float Cost;
        public float Weight;
        public float F
        {
            get
            {
                if (DistanceToTarget != -1 && Cost != -1)
                    return DistanceToTarget + Cost;
                else
                    return -1;
            }
        }
        private MapTile tile;

        public Node(Vector2 pos, MapTile t, float weight = 1)
        {
            Parent = null;
            Position = pos;
            DistanceToTarget = -1;
            Cost = 1;
            Weight = weight;
            tile = t;
        }

        public bool canWalk(MapTile[] ground)
        {
            return ground.Contains(tile);
        }
    }

    public class Astar
    {
        List<List<Node>> grid;
        Entity entity;
        int GridRows
        {
            get
            {
               return grid[0].Count;
            }
        }
        int GridCols
        {
            get
            {
                return grid.Count;
            }
        }

        public Astar(List<List<Node>> grid, Entity entity)
        {
            this.grid = grid;
            this.entity = entity;
        }

        public Stack<Node> run(Vector2 position)
        {
            Node start = new Node(new Vector2(entity.position.x, entity.position.y), entity.ground[0]);
            Node end = new Node(new Vector2(position.X, position.Y), entity.ground[0]);

            Stack<Node> Path = new Stack<Node>();
            PriorityQueue<Node, float> OpenList = new PriorityQueue<Node,float>(1);
            List<Node> ClosedList = new List<Node>();
            List<Node> adjacencies;
            Node current = start;
           
            // add start node to Open List
            OpenList.Insert(start, start.F);

            while(OpenList.isEmpty() && !ClosedList.Exists(x => x.Position == end.Position))
            {
                current = OpenList.Pop();
                ClosedList.Add(current);
                adjacencies = GetAdjacentNodes(current);

                foreach(Node n in adjacencies)
                {
                    if (!ClosedList.Contains(n) && n.canWalk(entity.ground))
                    {
                        bool isFound = false;
                        foreach (var oLNode in OpenList.elements())
                        {
                            if (oLNode == n)
                            {
                                isFound = true;
                            }
                        }
                        if (!isFound)
                        {
                            n.Parent = current;
                            n.DistanceToTarget = Math.Abs(n.Position.X - end.Position.X) + Math.Abs(n.Position.Y - end.Position.Y);
                            n.Cost = n.Weight + n.Parent.Cost;
                            OpenList.Insert(n, n.F);
                        }
                    }
                }
            }
            
            // construct path, if end was not closed return null
            if(!ClosedList.Exists(x => x.Position == end.Position))
            {
                return null;
            }

            // if all good, return path
            Node temp = ClosedList[ClosedList.IndexOf(current)];
            if (temp == null) return null;
            do
            {
                Path.Push(temp);
                temp = temp.Parent;
            } while (temp != start && temp != null) ;
            return Path;
        }
		
        private List<Node> GetAdjacentNodes(Node n)
        {
            List<Node> temp = new List<Node>();

            int row = (int)n.Position.Y;
            int col = (int)n.Position.X;

            if(row + 1 < GridRows)
            {
                temp.Add(grid[col][row + 1]);
            }
            if(row - 1 >= 0)
            {
                temp.Add(grid[col][row - 1]);
            }
            if(col - 1 >= 0)
            {
                temp.Add(grid[col - 1][row]);
            }
            if(col + 1 < GridCols)
            {
                temp.Add(grid[col + 1][row]);
            }

            return temp;
        }
    }
}