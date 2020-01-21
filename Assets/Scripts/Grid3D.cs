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
        /// <summary>
        /// layer masks for nodes
        /// </summary>
        public LayerMask obstacle;
        public LayerMask semiWalkable;

        /// <summary>
        /// prefab for regions
        /// </summary>
        public GameObject obstacleRegion;
        public GameObject semiWalkableRegion;

        /// <summary>
        /// access the Rendere component for each region
        /// </summary>
        private Renderer visible;

        /// <summary>
        /// parent for each region
        /// </summary>
        public GameObject obstacleParent;
        public GameObject semiWalkableParent;


        public Vector3 startPosIndex;
        public Vector3 goalPosIndex;
        int startCol { get { return (int)startPosIndex.x; } }
        int startRow { get { return (int)startPosIndex.z; } }
        int startArray { get { return (int)startPosIndex.y; } }

        int goalCol { get { return (int)goalPosIndex.x; } }
        int goalRow { get { return (int)goalPosIndex.z; } }
        int goalArray { get { return (int)goalPosIndex.y; } }

        [HideInInspector]
        public Vector3 worldBottomLeft;

        public Vector3 gridWorldSize;
        public float nodeRadius;
        Node[,,] grid3D;

        [HideInInspector]
        public int colNum, rowNum, arrayNum;
        [HideInInspector]
        public float nodeDiameter;
        [HideInInspector]
        public List<Node> path;
        public int MaxSize { get { return colNum * rowNum * arrayNum; } }

        public int penaltyValue;

        private void Awake()
        {
            
            nodeDiameter = nodeRadius * 2;
            colNum = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            rowNum = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);
            arrayNum = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            Create3dGrid();

        }



        public void Update()
        {
            //PathFinder.AstarPathFinder(this, NodeFromWorldPoints(startPos.position), NodeFromWorldPoints(targetPos.position), ref path,ref isSuccess);

        }


        private void GenerateObstacles(int obstaclePercentage, int semiwalkablePercentage)
        {
            for (int i = 0; i < colNum; i++)
            {
                for (int j = 0; j < colNum; j++)
                {
                    for (int k = 0; k < colNum; k++)
                    {
                        int randomVal = Random.Range(0, 100);
                        if (randomVal < obstaclePercentage)
                        {
                            Vector3 pos = new Vector3(1 + i * nodeDiameter, 1 + j * nodeDiameter, 1 + k * nodeDiameter);
                            GameObject obstaclePrefab = Instantiate(obstacleRegion, pos + worldBottomLeft, Quaternion.identity);
                            obstaclePrefab.transform.parent = obstacleParent.transform;
                            ///<<summary>>
                            /// the layer number is 8. So it is ok to write just 8. But the Unity layer is by bits. so it is actually 2 to the power of 8. 
                            /// So I need to convert 10 digit num to 2 digit num by log(x,2);
                            /// </summary>
                            obstaclePrefab.layer = (int)Mathf.Log(obstacle.value,2); 
                            visible = obstaclePrefab.GetComponent<Renderer>();
                            visible.enabled = true;
                        }
                        else if(randomVal >= obstaclePercentage && randomVal <= obstaclePercentage + semiwalkablePercentage)
                        {
                            Vector3 pos = new Vector3(1 + i * nodeDiameter, 1 + j * nodeDiameter, 1 + k * nodeDiameter);
                            GameObject WalkablePrefab = Instantiate(semiWalkableRegion, pos + worldBottomLeft, Quaternion.identity);
                            WalkablePrefab.transform.parent = semiWalkableParent.transform;
                            WalkablePrefab.layer = (int)Mathf.Log(semiWalkable.value, 2);
                            visible = WalkablePrefab.GetComponent<Renderer>();
                            visible.enabled = true;
                        }

                    }
                }
            }

        }

        public Node NodeFromWorldPoints(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
            float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
            float percentZ = (worldPosition.z + gridWorldSize.z / 2) / gridWorldSize.z;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);
            percentZ = Mathf.Clamp01(percentZ);

            int x = Mathf.RoundToInt((colNum - 1) * percentX);
            int y = Mathf.RoundToInt((arrayNum - 1) * percentY);
            int z = Mathf.RoundToInt((rowNum - 1) * percentZ);

            return grid3D[x,z,y];

        }

        void Create3dGrid()
        {
            grid3D = new Node[colNum, rowNum, arrayNum];
            worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 
                                                         - Vector3.forward * gridWorldSize.z / 2 
                                                         - Vector3.up * gridWorldSize.y / 2;

            GenerateObstacles(3,20); // if there is too much obstacles it can not find a path and gives error

            for (int i = 0; i < colNum; i++)
            {
                for (int j = 0; j < rowNum; j++)
                {
                    for (int k = 0; k < arrayNum; k++)
                    {
                        Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius)
                                                             + Vector3.forward * (k * nodeDiameter + nodeRadius)
                                                             + Vector3.up * (j * nodeDiameter + nodeRadius);
                        bool notObstacle = !(Physics.CheckSphere(worldPoint, nodeRadius,obstacle));

                        int movementPenalty = 0 ; 

                        if(Physics.CheckSphere(worldPoint, nodeRadius, semiWalkable)) movementPenalty = penaltyValue;

                        grid3D[i, k, j] = new Node(this, notObstacle, worldPoint,i,j,k, movementPenalty);
                                                                        
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
                        int checkY = node.gridY + k;
                        int checkZ = node.gridZ + j;

                        if( checkX>=0 && checkX < colNum && checkZ>=0 && checkZ<rowNum && checkY>=0 && checkY<arrayNum)
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

        }

    }


}
