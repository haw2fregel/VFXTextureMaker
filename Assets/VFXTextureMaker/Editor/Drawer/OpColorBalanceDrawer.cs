using UnityEditor;
using UnityEngine;

namespace VFXTextureMaker
{
    public class OpColorBalanceDrawer
    {
        public static void DrawProperty(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Color Balance"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var temperature = property.FindPropertyRelative("_temperature");
                CustomGUIUtility.FloatField(rect, temperature, new GUIContent("Temperature"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var tint = property.FindPropertyRelative("_tint");
                CustomGUIUtility.FloatField(rect, tint, new GUIContent("Tint"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var shiftHue = property.FindPropertyRelative("_shiftHue");
                CustomGUIUtility.FloatField(rect, shiftHue, new GUIContent("Hue"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var shiftSaturation = property.FindPropertyRelative("_shiftSaturation");
                CustomGUIUtility.FloatField(rect, shiftSaturation, new GUIContent("Saturation"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var shiftValue = property.FindPropertyRelative("_shiftValue");
                CustomGUIUtility.FloatField(rect, shiftValue, new GUIContent("Value"));
                rect.y += CustomGUIUtility.PropertyHeight;

                rect.xMin -= 10;
            }
            EditorGUI.EndProperty();
        }

        public static void DrawPropertyAnim(Rect rect, SerializedProperty property, int currentFrame, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Displacement"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var temperature = property.FindPropertyRelative("_temperature");
                CustomGUIUtility.FloatAnimField(rect, temperature, currentFrame, new GUIContent("Temperature"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var tint = property.FindPropertyRelative("_tint");
                CustomGUIUtility.FloatAnimField(rect, tint, currentFrame, new GUIContent("Tint"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var shiftHue = property.FindPropertyRelative("_shiftHue");
                CustomGUIUtility.FloatAnimField(rect, shiftHue, currentFrame, new GUIContent("Hue"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var shiftSaturation = property.FindPropertyRelative("_shiftSaturation");
                CustomGUIUtility.FloatAnimField(rect, shiftSaturation, currentFrame, new GUIContent("Saturation"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var shiftValue = property.FindPropertyRelative("_shiftValue");
                CustomGUIUtility.FloatAnimField(rect, shiftValue, currentFrame, new GUIContent("Value"));
                rect.y += CustomGUIUtility.PropertyHeight;

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
                height += CustomGUIUtility.PropertyHeight * 3;
            }

            height += CustomGUIUtility.LayerSpaceHeight;

            return height;
        }

    }
}