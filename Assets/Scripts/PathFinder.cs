using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding3D
{
    public static class PathFinder
    {
        public static List<Node> AstarPathFinder(Grid3D grid, Node startNode, Node goalNode)
        {
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while(openSet.Count>0)
            {
                Node currentNode = openSet[0];
                for(int i = 1; i< openSet.Count;i++) // starts with 1 bc 0th is current Node
                {
                    if(openSet[i].FCost<currentNode.FCost || openSet[i].FCost == currentNode.FCost || openSet[i].hCost < currentNode.hCost)
                   {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if(currentNode == goalNode)
                {
                    return RetracePath(startNode, goalNode);
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.isObstacle || closedSet.Contains(neighbour)) continue;

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if(newMovementCostToNeighbour<neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, goalNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                    }

                }
            }
            List<Node> path = RetracePath(startNode, goalNode);
            return path;
        }

        public static List<Node> RetracePath(Node start, Node end)
        {
            List<Node> path = new List<Node>();
            Node currentNode = end;

            while(currentNode != start)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            return path;
        }


        public static int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
            int dstZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);
           
            List<int> ManhattanDist = new List<int> { dstX , dstY , dstZ };
            ManhattanDist.Sort();
            return ManhattanDist[0] * 14 + ManhattanDist[1] * 14 + ManhattanDist[2] * 10;
        }

        private static float GetEuclideanHeuristicCost(Node current, Node target)
        {
            float heuristicCost = (current.ToVector3() - target.ToVector3()).magnitude;
            return heuristicCost;
        }

    }

}

