using UnityEditor;
using UnityEngine;

namespace VFXTextureMaker
{
    public class OpTileAndOffsetDrawer
    {
        public static void DrawProperty(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("TileAndOffset"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var count = property.FindPropertyRelative("_count");
                CustomGUIUtility.PropertyValueField(rect, count, new GUIContent("Count"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var posMinMaxX = property.FindPropertyRelative("_posMinMaxX");
                CustomGUIUtility.PropertyValueField(rect, posMinMaxX, new GUIContent("Pos MinMax X"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var posMinMaxY = property.FindPropertyRelative("_posMinMaxY");
                CustomGUIUtility.PropertyValueField(rect, posMinMaxY, new GUIContent("Pos MinMax Y"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var scaleMinMax = property.FindPropertyRelative("_scaleMinMax");
                CustomGUIUtility.PropertyValueField(rect, scaleMinMax, new GUIContent("Scale MinMax"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var scaleMinMaxX = property.FindPropertyRelative("_scaleMinMaxX");
                CustomGUIUtility.PropertyValueField(rect, scaleMinMaxX, new GUIContent("Scale MinMax X"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var scaleMinMaxY = property.FindPropertyRelative("_scaleMinMaxY");
                CustomGUIUtility.PropertyValueField(rect, scaleMinMaxY, new GUIContent("Scale MinMax Y"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var rotateMinMax = property.FindPropertyRelative("_rotateMinMax");
                CustomGUIUtility.PropertyValueField(rect, rotateMinMax, new GUIContent("Rotate MinMax"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var opacityMinMax = property.FindPropertyRelative("_opacityMinMax");
                CustomGUIUtility.PropertyValueField(rect, opacityMinMax, new GUIContent("Opacity MinMax"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var randomSeed = property.FindPropertyRelative("_randomSeed");
                CustomGUIUtility.IntField(rect, randomSeed, new GUIContent("Random Seed"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var blend = property.FindPropertyRelative("_blend");
                CustomGUIUtility.PropertyField(rect, blend, new GUIContent("Blend Mode"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var backColor = property.FindPropertyRelative("_backColor");
                CustomGUIUtility.PropertyField(rect, backColor, new GUIContent("BackGround Color"));
                

                rect.xMin -= 10;
            }
            EditorGUI.EndProperty();
        }
        public static void DrawPropertyAnim(Rect rect, SerializedProperty property, int currentFrame, GUIContent label)
        {

            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Blur"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var count = property.FindPropertyRelative("_count");
                CustomGUIUtility.Vector2AnimField(rect, count, currentFrame, new GUIContent("Count"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var posMinMaxX = property.FindPropertyRelative("_posMinMaxX");
                CustomGUIUtility.Vector2AnimField(rect, posMinMaxX, currentFrame, new GUIContent("Pos MinMax X"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var posMinMaxY = property.FindPropertyRelative("_posMinMaxY");
                CustomGUIUtility.Vector2AnimField(rect, posMinMaxY, currentFrame, new GUIContent("Pos MinMax Y"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var scaleMinMax = property.FindPropertyRelative("_scaleMinMax");
                CustomGUIUtility.Vector2AnimField(rect, scaleMinMax, currentFrame, new GUIContent("Scale MinMax"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var scaleMinMaxX = property.FindPropertyRelative("_scaleMinMaxX");
                CustomGUIUtility.Vector2AnimField(rect, scaleMinMaxX, currentFrame, new GUIContent("Scale MinMax X"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var scaleMinMaxY = property.FindPropertyRelative("_scaleMinMaxY");
                CustomGUIUtility.Vector2AnimField(rect, scaleMinMaxY, currentFrame, new GUIContent("Scale MinMax Y"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var rotateMinMax = property.FindPropertyRelative("_rotateMinMax");
                CustomGUIUtility.Vector2AnimField(rect, rotateMinMax, currentFrame, new GUIContent("Rotate MinMax"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var opacityMinMax = property.FindPropertyRelative("_opacityMinMax");
                CustomGUIUtility.Vector2AnimField(rect, opacityMinMax, currentFrame, new GUIContent("Opacity MinMax"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var randomSeed = property.FindPropertyRelative("_randomSeed");
                CustomGUIUtility.IntAnimField(rect, randomSeed, currentFrame, new GUIContent("Random Seed"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var blend = property.FindPropertyRelative("_blend");
                CustomGUIUtility.PropertyField(rect, blend, new GUIContent("Blend Mode"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var backColor = property.FindPropertyRelative("_backColor");
                CustomGUIUtility.PropertyField(rect, backColor, new GUIContent("BackGround Color"));

                rect.xMin -= 10;
            }
            EditorGUI.EndProperty();

        }
        public static float GetPropertyHeight(SerializedProperty property)
        {
            var height = CustomGUIUtility.FoldoutHeight;

            var showOption = property.FindPropertyRelative("showOption");
            if (showOption.boolValue)
            {
                height += CustomGUIUtility.PropertyHeight * 11;
            }

            height += CustomGUIUtility.LayerSpaceHeight;

            return height;
        }
    }
}