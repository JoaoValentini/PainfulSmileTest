using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public static class GameUtility
    {
        public static bool IsInSight(Vector3 origin, Vector3 destination, LayerMask layerMask)
        {
            return !Physics.Linecast(origin,destination,layerMask);
        }
        
        public static bool IsInCameraView(Vector3 position, Camera camera, float offset = 1)
        {
            float screenRatio = (float)Screen.width / (float)Screen.height;

            float heightExtend = camera.orthographicSize;
            float widthExtend = heightExtend * screenRatio;

            bool xFit = position.x >= -widthExtend + offset && position.x <= widthExtend - offset;
            bool zFit = position.z >= -heightExtend + offset && position.z <= heightExtend - offset;

            return xFit && zFit;
        }


    }
}
