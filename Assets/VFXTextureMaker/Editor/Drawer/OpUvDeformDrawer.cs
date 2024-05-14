using UnityEditor;
using UnityEngine;

namespace VFXTextureMaker
{
    public class OpUvDeformDrawer
    {
        public static void DrawProperty(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("UV Deform"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var uvPivot = property.FindPropertyRelative("_uvPivot");
                CustomGUIUtility.PropertyValueField(rect, uvPivot, new GUIContent("Pivot"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var uvScale = property.FindPropertyRelative("_uvScale");
                CustomGUIUtility.PropertyValueField(rect, uvScale, new GUIContent("Scale"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var uvRotate = property.FindPropertyRelative("_uvRotate");
                CustomGUIUtility.FloatField(rect, uvRotate, new GUIContent("Rotation"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var uvBend = property.FindPropertyRelative("_uvBend");
                CustomGUIUtility.PropertyValueField(rect, uvBend, new GUIContent("Bend"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var uvPolar = property.FindPropertyRelative("_uvPolar");
                CustomGUIUtility.PropertyValueField(rect, uvPolar, new GUIContent("Polar"));

                rect.xMin -= 10;
            }
            EditorGUI.EndProperty();
        }

        public static void DrawPropertyAnim(Rect rect, SerializedProperty property, int currentFrame, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("UV Deform"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var uvPivot = property.FindPropertyRelative("_uvPivot");
                CustomGUIUtility.Vector2AnimField(rect, uvPivot, currentFrame, new GUIContent("Pivot"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var uvScale = property.FindPropertyRelative("_uvScale");
                CustomGUIUtility.Vector2AnimField(rect, uvScale, currentFrame, new GUIContent("Scale"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var uvRotate = property.FindPropertyRelative("_uvRotate");
                CustomGUIUtility.FloatAnimField(rect, uvRotate, currentFrame, new GUIContent("Rotation"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var uvBend = property.FindPropertyRelative("_uvBend");
                CustomGUIUtility.Vector2AnimField(rect, uvBend, currentFrame, new GUIContent("Bend"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var uvPolar = property.FindPropertyRelative("_uvPolar");
                CustomGUIUtility.BoolAnimField(rect, uvPolar, currentFrame, new GUIContent("Polar"));

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
                height += CustomGUIUtility.PropertyHeight * 5;
            }
            height += CustomGUIUtility.LayerSpaceHeight;

            return height;
        }
    }
}