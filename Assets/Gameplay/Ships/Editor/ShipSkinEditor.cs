using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Gameplay.Ships
{
    [CustomEditor(typeof(ShipSkin))]
    public class ShipSkinEditor : Editor
    {
        ShipSkin _shipSkin;
        SerializedProperty _hull;
        SerializedProperty _mainSail;
        SerializedProperty _smallSail;
        SerializedProperty _mast;
        SerializedProperty _mastPole;
        float _shipSizeMult = 3;

        void OnEnable()
        {
            _hull = serializedObject.FindProperty("_hull");
            _mainSail = serializedObject.FindProperty("_mainSail");
            _smallSail = serializedObject.FindProperty("_smallSail");
            _mast = serializedObject.FindProperty("_mast");
            _mastPole = serializedObject.FindProperty("_mastPole");
            _shipSkin = target as ShipSkin;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_hull, GUILayout.Height(30));
            EditorGUILayout.PropertyField(_mainSail, GUILayout.Height(30));
            EditorGUILayout.PropertyField(_smallSail, GUILayout.Height(30));
            EditorGUILayout.PropertyField(_mast, GUILayout.Height(30));
            EditorGUILayout.PropertyField(_mastPole, GUILayout.Height(30));

            serializedObject.ApplyModifiedProperties();

            GUILayout.Space(25);
            GUILayout.Label("");
            Rect rect = GUILayoutUtility.GetLastRect();
            float size = 125 * _shipSizeMult;
            rect.position = new Vector2(GetXPosition(size), rect.position.y);
            rect.size = new Vector2(size, size);
            GUI.Box(rect,"");
            
            DrawShipPreview(rect);
        }

        void DrawShipPreview(Rect rect)
        {
            DrawShipPart(rect, _shipSkin.Hull, 0);
            DrawShipPart(rect, _shipSkin.MainSail, -12.9f);
            DrawShipPart(rect, _shipSkin.SmallSail, 32);
            DrawShipPart(rect, _shipSkin.Mast, -36.4f);
            DrawShipPart(rect, _shipSkin.MastPole, -48.2f);
        }

        void DrawShipPart(Rect baseRect, Sprite sprite, float yOffset)
        {
            if(!sprite)
                return;

            Texture2D texture = AssetPreview.GetAssetPreview(sprite);
            Vector2 size = sprite.textureRect.size * _shipSizeMult;
            float xPos = GetXPosition(size.x);
            float yPos = GetYPosition(baseRect, size.y, yOffset);
            Rect texRect = new Rect(new Vector2(xPos, yPos), size);
            GUI.DrawTexture(texRect, texture);
        }
        
        float GetYPosition(Rect baseRect, float height, float offset)
        {
            float centeredPos = baseRect.position.y + baseRect.size.y /2f - height/2f + offset * _shipSizeMult;
            return centeredPos;
        }
        float GetXPosition(float width)
        {
            return Screen.width/2f - width /2f;
        }


    }
}
