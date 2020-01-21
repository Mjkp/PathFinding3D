// ===============================
// AUTHOR          : Justin Moon
// CREATE DATE     : 17th of January 2020
// PURPOSE         : A static class for a star pathfinding in 3d grid environment
// SPECIAL NOTES   : This code was created by Justin Moon. Please mention the author if you use part or the totality of the code.
// ===============================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;


namespace PathFinding3D 
{
    public static class PathFinder
    {

        public static List<Node> AstarPathFinder(Grid3D grid, Node startNode, Node goalNode, ref List<Node> path)
        {
        

            Heap<Node> openSet = new Heap<Node>(grid.MaxSize); // use List when not using heap

            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count>0)
            {

                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if(currentNode == goalNode)
                {
                    path = RetracePath(startNode, goalNode);
                    break;
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

                        if (!openSet.Contains(neighbour))
                        { 
                        openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }

                }
            }
            return path;
        }
        //TODO this is 
        /*
        IEnumerable AstarPathFinder(Grid3D grid, Vector3 _startPos, Vector3 _goalPos)
        {
            Vector3[] waypoints = new Vector3[0];
            bool pathSuccess = false;


            Node startNode = grid.NodeFromWorldPoints(_startPos);
            Node goalNode = grid.NodeFromWorldPoints(_goalPos);

            if( !startNode.isObstacle && !goalNode.isObstacle)
            {
                Heap<Node> openSet = new Heap<Node>(grid.MaxSize); // use List when not using heap
                HashSet<Node> closedSet = new HashSet<Node>();
                openSet.Add(startNode);

                while (openSet.Count > 0)
                {

                    Node currentNode = openSet.RemoveFirst();
                    closedSet.Add(currentNode);

                    if (currentNode == goalNode)
                    {
                        pathSuccess = true;
                        break;
                    }

                    foreach (Node neighbour in grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.isObstacle || closedSet.Contains(neighbour)) continue;

                        int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                        if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMovementCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, goalNode);
                            neighbour.parent = currentNode;

                            if (!openSet.Contains(neighbour))
                            {
                                openSet.Add(neighbour);
                            }
                            else
                            {
                                openSet.UpdateItem(neighbour);
                            }
                        }

                    }
                }
            }
            yield return null;
            if (pathSuccess) waypoints = RetraceVectorPath(startNode, goalNode);
            //RequestPathManager.RequestPath();
        }
        */

        public static Vector3[] RetraceVectorPath(Node start, Node end)
        {
            List<Node> path = new List<Node>();
            Node currentNode = end;

            while(currentNode != start)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            Vector3[] wayPoints = SimplifyPath(path);
            Array.Reverse(wayPoints);
            return wayPoints;
        }
        public static List<Node> RetracePath(Node start, Node end)
        {
            List<Node> path = new List<Node>();
            Node currentNode = end;

            while (currentNode != start)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            return path;
        }

        public static Vector3[] SimplifyPath(List<Node> path)
        {
            List<Vector3> waypoints = new List<Vector3>();
            Vector3 prevDirection = Vector3.zero;
            for(int i = 1; i<path.Count;i++)
            {
                Vector3 currentDirection = path[i - 1].worldPos - path[i].worldPos;
                if( prevDirection == currentDirection)
                {
                    waypoints.Add(path[i].worldPos);
                }
                prevDirection = currentDirection;
            }
            return waypoints.ToArray();
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

