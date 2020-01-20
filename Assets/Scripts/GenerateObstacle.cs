using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding3D
{
    public class GenerateObstacle : MonoBehaviour
    {
        public Grid3D theGrid;
        public GameObject obstacleCube;
        public LayerMask obstacleLayer;
        private Vector3 cubeSize;
        private Vector3 bottomLeft;
        private float nodeDiameter;

        private int col;
        private int row;
        private int array;


        // Start is called before the first frame update
        void Start()
        {
            cubeSize = theGrid.gridWorldSize;
            bottomLeft = theGrid.worldBottomLeft;
            nodeDiameter = theGrid.nodeDiameter;
            col = theGrid.colNum;
            row = theGrid.rowNum;
            array = theGrid.arrayNum;

            GenerateObstacles();
            //Debug.Log(counter); // to check how many obstacles generated
        }



        // Update is called once per frame
        void Update()
        {
        }
        private int counter;

        void GenerateObstacles()
        {
            for(int i =0;i< col;i++)
            {
                for (int j = 0; j < row; j++)
                {
                    for (int k = 0; k < array; k++)
                    {
                        if(Random.Range(0,20)<1)
                        {
                            Vector3 pos = new Vector3(1 + i * nodeDiameter, 1 + j * nodeDiameter, 1 + k * nodeDiameter);
                            GameObject obstaclePrefab = Instantiate(obstacleCube, pos + bottomLeft, Quaternion.identity);
                            obstaclePrefab.transform.parent = this.transform;
                            obstaclePrefab.layer = 8;
                            //counter++; // to check how many obstacles generated
                        }
                        else
                        {
                            continue;
                        }

                    }
                }
            }
        }
    }
}