using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding3D
{
    public class Agent : MonoBehaviour
    {
        public Transform target;
        private Vector3[] path;
        float velocity = 15;
        int targetIndex;

        private void Start()
        {
            RequestPathManager.RequestPath(transform.position,target.position,OnPathFound);
        }

        Vector3[] NodeToPath(List<Node> _path)
        {
            List<Vector3> Vectorpath = new List<Vector3>();
            foreach(Node n in _path)
            {
                Vectorpath.Add(n.worldPos);
            }
            return Vectorpath.ToArray(); 
        }

        public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
        {
            if(pathSuccessful)
            {
                path = newPath;
                targetIndex = 0;
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        IEnumerator FollowPath()
        {
            Vector3 currentWaypoint = path[0];

            while (true)
            {
                if(transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if(targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, velocity * Time.deltaTime);
                yield return null;
           }
        }

        public void OnDrawGizmos()
        {
            if (path != null)
            {
                for (int i = targetIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(path[i], Vector3.one);

                    if (i == targetIndex)
                    {
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
            }
        }

    }
}