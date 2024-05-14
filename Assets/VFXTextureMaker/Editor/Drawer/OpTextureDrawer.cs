using UnityEditor;
using UnityEngine;

namespace VFXTextureMaker
{
    public class OpTextureDrawer
    {
        public static void DrawProperty(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Texture"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var drawTex = property.FindPropertyRelative("_drawTex");
                CustomGUIUtility.PropertyField(rect, drawTex, new GUIContent("Texture"));

                rect.y += CustomGUIUtility.PropertyHeight;
                var textureFilterMode = property.FindPropertyRelative("_textureFilterMode");
                CustomGUIUtility.PropertyField(rect, textureFilterMode, new GUIContent("FilterMode"));

                rect.xMin -= 10;
            }
            EditorGUI.EndProperty();
        }
        public static void DrawPropertyAnim(Rect rect, SerializedProperty property, GUIContent label)
        {
            DrawProperty(rect, property, label);
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