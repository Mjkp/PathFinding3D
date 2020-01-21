using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PathFinding3D
{
    public class FindPath : MonoBehaviour
    {
        RequestPathManager requestManager;
        Grid3D grid;

        private void Awake()
        {
            requestManager = GetComponent<RequestPathManager>();
            grid = GetComponent<Grid3D>();
        }

        public void StartFindPath(Vector3 startPos, Vector3 targetPos)
        {
            StartCoroutine(PathFinding(startPos, targetPos));
        }

        IEnumerator PathFinding(Vector3 startPos, Vector3 targetPos)
        {
            List<Node> path = new List<Node>();
            Vector3[] waypoints = new Vector3[0];
            bool pathFound = false;
            Node startNode = grid.NodeFromWorldPoints(startPos);
            Node targetNode = grid.NodeFromWorldPoints(targetPos);

            PathFinder.AstarPathFinder(grid, startNode, targetNode,ref path,ref pathFound);
            if(pathFound)
            {
                waypoints = PathFinder.RetraceVectorPath(startNode, targetNode);
            }
            requestManager.FinishedProcessingPath(waypoints, pathFound);
            yield return null;
        }

    }
}
