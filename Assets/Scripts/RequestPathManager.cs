using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding3D
{
    public class RequestPathManager : MonoBehaviour
    {
        Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
        PathRequest currentPathRequest;

        static RequestPathManager instance;

        bool isProcessingPath;

        private void Awake()
        {
            instance = this;

        }

        public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
        {
            PathRequest newRequest = new PathRequest(pathStart, pathEnd,callback);
            instance.pathRequestQueue.Enqueue(newRequest);
            instance.TryProcessNext();
        }

        void TryProcessNext()
        {
            if(!isProcessingPath && pathRequestQueue.Count>0)
            {
                currentPathRequest = pathRequestQueue.Dequeue();
                isProcessingPath = true;
                //PathFinder.AstarPathFinderVector3(transform.GetComponent<Grid3D>(),currentPathRequest.pathStart, currentPathRequest.pathEnd);
            }
        }

        public void FinishedProcessingPath(Vector3[] path, bool success)
        {
            currentPathRequest.callback(path, success);
            isProcessingPath = false;
            TryProcessNext();
        }

        struct PathRequest
        {
            public Vector3 pathStart;
            public Vector3 pathEnd;
            public Action<Vector3[], bool> callback;

            public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
            {
                pathStart = _start;
                pathEnd = _end;
                callback = _callback;
            }
        }
    }
}