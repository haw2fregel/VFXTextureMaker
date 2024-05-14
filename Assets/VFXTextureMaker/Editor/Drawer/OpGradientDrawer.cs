using UnityEditor;
using UnityEngine;

namespace VFXTextureMaker
{
    public class OpGradientDrawer
    {
        public static void DrawProperty(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Gradient"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var gradationReference = property.FindPropertyRelative("_gradationReference");
                CustomGUIUtility.PropertyField(rect, gradationReference, new GUIContent("Reference"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var gradient = property.FindPropertyRelative("_gradient");
                CustomGUIUtility.PropertyValueField(rect, gradient, new GUIContent("Gradient"));

                rect.xMin -= 10;
            }
            EditorGUI.EndProperty();
        }
        public static void DrawPropertyAnim(Rect rect, SerializedProperty property, int currentFrame, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Gradient"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var gradationReference = property.FindPropertyRelative("_gradationReference");
                CustomGUIUtility.PropertyField(rect, gradationReference, new GUIContent("Reference"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var gradient = property.FindPropertyRelative("_gradient");
                CustomGUIUtility.PropertyValueField(rect, gradient, new GUIContent("Gradient"));

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
                height += CustomGUIUtility.PropertyHeight * 2;
            }
            height += CustomGUIUtility.LayerSpaceHeight;
            return height;
        }
    }
}