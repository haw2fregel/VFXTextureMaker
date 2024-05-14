using UnityEditor;
using UnityEngine;

namespace VFXTextureMaker
{
    public class OpDrawDrawer
    {
        public static void DrawProperty(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Draw"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var drawColor = property.FindPropertyRelative("_drawColor");
                CustomGUIUtility.PropertyField(rect, drawColor, new GUIContent("Draw Color"));

                rect.xMin -= 10;
            }
            EditorGUI.EndProperty();
        }
        public static void DrawPropertyAnim(Rect rect, SerializedProperty property, int currentFrame, GUIContent label)
        {

            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Draw"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var drawColor = property.FindPropertyRelative("_drawColor");
                CustomGUIUtility.PropertyField(rect, drawColor, new GUIContent("Draw Color"));

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
                height += CustomGUIUtility.PropertyHeight ;
            }

            height += CustomGUIUtility.LayerSpaceHeight;

            return height;
        }
    }
}