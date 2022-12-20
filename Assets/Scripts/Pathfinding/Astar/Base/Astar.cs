using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using DataStructures.PriorityQueue;
using UnityEngine;

namespace Pathfinding
{
    public class Node
    {
        public static int NODE_SIZE = 32;
        public Node parent;
        public Vector3Int position;
        public Vector3Int Center
        {
            get
            {
                return new Vector3Int(position.x + NODE_SIZE / 2, position.y + NODE_SIZE / 2, 0);
            }
        }
        public float distance;
        public float cost;
        public float F
        {
            get
            {
                if (distance != -1 && cost != -1)
                    return distance + cost;
                else
                    return -1;
            }
        }
        private MapTile tile;

        public Node(Vector3Int pos, MapTile t)
        {
            parent = null;
            position = pos;
            distance = -1;
            cost = 1;
            tile = t;
        }

        public bool canWalk(MapTile[] ground)
        {
            return ground.Contains(tile);
        }

        public void update(MapTile t)
        {
            tile = t;
        }
    }

    public class Astar
    {
        List<List<Node>> grid;
        int xMin, yMin;
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

        public Astar()
        {

        }

        public Stack<Node> run(List<List<Node>> grid, UnitEntity entity, Vector3Int position, int xMin, int yMin)
        {
            this.grid = grid;
            this.xMin = xMin;
            this.yMin = yMin;

            Node start = new Node(new Vector3Int(entity.position.x, entity.position.y, 0), entity.ground[0]);
            Node end = new Node(new Vector3Int(position.x, position.y, 0), entity.ground[0]);

            Stack<Node> Path = new Stack<Node>();
            PriorityQueue<Node, float> OpenList = new PriorityQueue<Node,float>(1);
            List<Node> ClosedList = new List<Node>();
            List<Node> adjacencies;
            Node current = start;

            OpenList.Insert(start, start.F);

            int count = 0;

            while(!OpenList.isEmpty() && !ClosedList.Exists(x => x.position == end.position) && count < entity.moveLimit)
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
                            n.parent = current;
                            n.distance = Vector3Int.Distance(n.position, end.position); // (n.Position.X - end.Position.X) + Math.Abs(n.Position.Y - end.Position.Y);
                            n.cost = n.parent.cost + 1; // weight : 1
                            OpenList.Insert(n, n.F);
                            count++;
                        }
                    }
                }
            }
            
            if(!ClosedList.Exists(x => x.position == end.position))
            {
                return null;
            }

            Node temp = ClosedList[ClosedList.IndexOf(current)];
            if (temp == null) return null;
            do
            {
                Path.Push(temp);
                temp = temp.parent;
            } while (temp != start && temp != null) ;
            return Path;
        }
		
        private List<Node> GetAdjacentNodes(Node n)
        {
            List<Node> temp = new List<Node>();

            int row = n.position.y - yMin;
            int col = n.position.x - xMin;

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