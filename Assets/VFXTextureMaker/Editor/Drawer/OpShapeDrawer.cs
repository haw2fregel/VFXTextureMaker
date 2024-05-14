using UnityEditor;
using UnityEngine;

namespace VFXTextureMaker
{
    public class OpShapeDrawer
    {
        public static void DrawProperty(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Shape"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var shapeType = property.FindPropertyRelative("_shapeType");
                CustomGUIUtility.PropertyField(rect, shapeType, new GUIContent("Shape Type"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var shapeCurve = property.FindPropertyRelative("_shapeCurve");
                CustomGUIUtility.PropertyValueField(rect, shapeCurve, new GUIContent("Curve"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var shapeCenter = property.FindPropertyRelative("_shapeCenter");
                CustomGUIUtility.PropertyValueField(rect, shapeCenter, new GUIContent("Center"));
                rect.y += CustomGUIUtility.PropertyHeight;

                if (shapeType.enumValueIndex == 0)
                {
                    var shapeDistancePower = property.FindPropertyRelative("_shapeDistancePower");
                    CustomGUIUtility.FloatField(rect, shapeDistancePower, new GUIContent("Power"));
                }
                else if (shapeType.enumValueIndex == 1)
                {
                    var polygonCount = property.FindPropertyRelative("_polygonCount");
                    CustomGUIUtility.IntField(rect, polygonCount, new GUIContent("Polygon Count"));
                    rect.y += CustomGUIUtility.PropertyHeight;

                    var polygonBump = property.FindPropertyRelative("_polygonBump");
                    CustomGUIUtility.FloatField(rect, polygonBump, new GUIContent("Bump"));
                }

                rect.xMin -= 10;
            }
            EditorGUI.EndProperty();
        }
        public static void DrawPropertyAnim(Rect rect, SerializedProperty property, int currentFrame, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Shape"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var shapeType = property.FindPropertyRelative("_shapeType");
                CustomGUIUtility.PropertyField(rect, shapeType, new GUIContent("Shape Type"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var shapeCurve = property.FindPropertyRelative("_shapeCurve");
                CustomGUIUtility.PropertyValueField(rect, shapeCurve, new GUIContent("Curve"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var shapeCenter = property.FindPropertyRelative("_shapeCenter");
                CustomGUIUtility.Vector2AnimField(rect, shapeCenter, currentFrame, new GUIContent("Center"));
                rect.y += CustomGUIUtility.PropertyHeight;
                if (shapeType.enumValueIndex == 0)
                {
                    var shapeDistancePower = property.FindPropertyRelative("_shapeDistancePower");
                    CustomGUIUtility.FloatAnimField(rect, shapeDistancePower, currentFrame, new GUIContent("Power"));
                }
                else if (shapeType.enumValueIndex == 1)
                {
                    var polygonCount = property.FindPropertyRelative("_polygonCount");
                    CustomGUIUtility.IntAnimField(rect, polygonCount, currentFrame, new GUIContent("Polygon Count"));
                    rect.y += CustomGUIUtility.PropertyHeight;

                    var polygonBump = property.FindPropertyRelative("_polygonBump");
                    CustomGUIUtility.FloatAnimField(rect, polygonBump, currentFrame, new GUIContent("Bump"));
                }

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
                var shapeType = property.FindPropertyRelative("_shapeType");

                height += CustomGUIUtility.PropertyHeight * 3;

                if (shapeType.enumValueIndex == 0)
                {
                    height += CustomGUIUtility.PropertyHeight;
                }
                else if (shapeType.enumValueIndex == 1)
                {
                    height += CustomGUIUtility.PropertyHeight * 2;
                }
            }
            height += CustomGUIUtility.LayerSpaceHeight;
            return height;
        }
    }
}