// ===============================
// AUTHOR          : Justin Moon
// CREATE DATE     : 17th of January 2020
// PURPOSE         : A static class for a star pathfinding in 3d grid environment
// SPECIAL NOTES   : This code was created by Justin Moon. Please mention the author if you use part or the totality of the code.
// ===============================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PathFinding3D
{
    public class Grid3D : MonoBehaviour
    {
        public Vector3 startPosIndex;
        public Vector3 goalPosIndex;
        int startCol { get { return (int)startPosIndex.x; } }
        int startRow { get { return (int)startPosIndex.z; } }
        int startArray { get { return (int)startPosIndex.y; } }

        int goalCol { get { return (int)goalPosIndex.x; } }
        int goalRow { get { return (int)goalPosIndex.z; } }
        int goalArray { get { return (int)goalPosIndex.y; } }


        public LayerMask obstacle;
        public Vector3 gridWorldSize;
        public float nodeRadius;
        Node[,,] grid3D;

        int colNum, rowNum, arrayNum;
        float nodeDiameter;
        public List<Node> path;
        public int MaxSize { get { return colNum * rowNum * arrayNum; } }

        private void Start()
        {
            nodeDiameter = nodeRadius * 2;
            colNum = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            rowNum = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);
            arrayNum = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            Create3dGrid();

        }

        public void Update()
        {

            path = PathFinder.AstarPathFinder(this, grid3D[startCol, startRow, startArray],grid3D[goalCol, goalRow, goalArray]);
        }

        void Create3dGrid()
        {
            grid3D = new Node[colNum, rowNum, arrayNum];
            Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 
                                                         - Vector3.forward * gridWorldSize.z / 2 
                                                         - Vector3.up * gridWorldSize.y / 2;

            Debug.Log(worldBottomLeft);
            for (int i = 0; i < colNum; i++)
            {
                for (int j = 0; j < colNum; j++)
                {
                    for (int k = 0; k < colNum; k++)
                    {
                        Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius)
                                                             + Vector3.forward * (k * nodeDiameter + nodeRadius)
                                                             + Vector3.up * (j * nodeDiameter + nodeRadius);
                        bool notObstacle = !(Physics.CheckSphere(worldPoint, nodeRadius,obstacle));
                        grid3D[i, k, j] = new Node(this, notObstacle, worldPoint,i,j,k);
                                                                        
                    }
                }
            }

        }


        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for( int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int k = -1; k <= 1; k++)
                    {
                        if (i == 0 && j == 0 && k == 0) continue;

                        int checkX = node.gridX + i;
                        int checkY = node.gridY + j;
                        int checkZ = node.gridZ + k;

                        if( checkX>=0 && checkX < colNum && checkZ>=0 && checkZ<rowNum && checkY>=0&&checkY<arrayNum)
                        {
                            neighbours.Add(grid3D[checkX, checkZ, checkY]);
                        }

                    }
                }
            }

            return neighbours;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, gridWorldSize);

            if (grid3D != null)
            {
                foreach(Node n in grid3D)
                {
                    Gizmos.color = (n.isObstacle) ? Color.grey : Color.blue;
                    //if (path != null) if (path.Contains(n)) Gizmos.color = Color.red;
                    if (path != null)
                    {
                        if (path.Contains(n))
                        {
                            Gizmos.color = Color.red;
                            Gizmos.DrawCube(n.worldPos, (Vector3.one * 0.5f) * (nodeDiameter));
                        }
                        else
                        {
                            Gizmos.DrawWireCube(n.worldPos, (Vector3.one * 0.1f) * (nodeDiameter));

                        }
                    }

                }
            }
        }
    }
}
