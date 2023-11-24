using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace Utility
{
    public class ScreenCollider : MonoBehaviour
    {   
        void Awake()
        {
            CalculateScreenColliders();
        }

        void CalculateScreenColliders()
        {
            float screenRatio = (float)Screen.width / (float)Screen.height;

            float heightExtend = GameSession.Instance.MainCamera.orthographicSize;
            float widthExtend = heightExtend * screenRatio;

            float totalWidth = widthExtend * 2 + 2;
            float totalHeight = heightExtend * 2 + 2;
            
            Vector3 horizontalSize = new Vector3(totalWidth,1,1);
            Vector3 verticalSize = new Vector3(1,1,totalHeight);

            BoxCollider rightColl = gameObject.AddComponent<BoxCollider>();
            BoxCollider leftColl = gameObject.AddComponent<BoxCollider>();
            BoxCollider topColl = gameObject.AddComponent<BoxCollider>();
            BoxCollider botColl = gameObject.AddComponent<BoxCollider>();


            rightColl.size = verticalSize;
            rightColl.center = new Vector3(widthExtend + 0.5f, 0, 0);
            leftColl.size = verticalSize;
            leftColl.center = new Vector3(-widthExtend - 0.5f, 0, 0);
            topColl.size = horizontalSize;
            topColl.center = new Vector3(0, 0, heightExtend + 0.5f);
            botColl.size = horizontalSize;
            botColl.center = new Vector3(0, 0, -heightExtend - 0.5f);
        }
    }
}
