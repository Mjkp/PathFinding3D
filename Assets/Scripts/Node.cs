using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding3D
{
    public class Node
    {

        public Grid3D Grid { get; private set; }
        public bool isObstacle;
        public Vector3 worldPos;
        public Node parent;

        public int gCost;
        public int hCost;
        public int FCost { get { return gCost + hCost; } }
        public int gridX;
        public int gridY;
        public int gridZ;


        public Node(Grid3D _grid, bool _isObstacle, Vector3 _pos, int _gridX, int _gridY, int _gridZ)
        {
            gridX = _gridX;
            gridY = _gridY;
            gridZ = _gridZ;
            isObstacle = _isObstacle;
            worldPos = _pos;
            Grid = _grid;

        }

        public Vector3 ToVector3()
        {
            return new Vector3(gridX, gridZ, gridY);
        }




    }

}
