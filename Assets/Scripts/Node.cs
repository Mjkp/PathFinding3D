using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding3D
{
    public class Node : IHeapItem<Node>
    {

        public Grid3D Grid { get; private set; }
        public bool isObstacle;
        public Vector3 worldPos;
        public Node parent;
        public int heapIndex;

        public int gCost;
        public int hCost;
        public int FCost { get { return gCost + hCost; } }
        public int gridX;
        public int gridY;
        public int gridZ;

        public int movementPenaly;



        public Node(Grid3D _grid, bool _isObstacle, Vector3 _pos, int _gridX, int _gridY, int _gridZ, int _penalty)
        {
            gridX = _gridX;
            gridY = _gridY;
            gridZ = _gridZ;
            isObstacle = _isObstacle;
            worldPos = _pos;
            Grid = _grid;
            movementPenaly = _penalty;

        }

        public Vector3 ToVector3()
        {
            return new Vector3(gridX, gridZ, gridY);
        }

        public int HeapIndex
        {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = FCost.CompareTo(nodeToCompare.FCost);
            if(compare ==0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }
            return -compare;
        }
    }

}
