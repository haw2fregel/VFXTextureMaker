using UnityEditor;
using UnityEngine;

namespace VFXTextureMaker
{
    public class OpGlowDrawer
    {
        public static void DrawProperty(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Glow"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var glowIteration = property.FindPropertyRelative("_glowIteration");
                CustomGUIUtility.IntField(rect, glowIteration, new GUIContent("Iteration"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var glowSize = property.FindPropertyRelative("_glowSize");
                CustomGUIUtility.FloatField(rect, glowSize, new GUIContent("Size"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var glowThreshold = property.FindPropertyRelative("_glowThreshold");
                CustomGUIUtility.FloatField(rect, glowThreshold, new GUIContent("Threshold"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var glowThresholdSmooth = property.FindPropertyRelative("_glowThresholdSmooth");
                CustomGUIUtility.FloatField(rect, glowThresholdSmooth, new GUIContent("Smooth"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var glowIntensity = property.FindPropertyRelative("_glowIntensity");
                CustomGUIUtility.FloatField(rect, glowIntensity, new GUIContent("Intensity"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var glowColor = property.FindPropertyRelative("_glowColor");
                CustomGUIUtility.PropertyField(rect, glowColor, new GUIContent("Color"));

                rect.xMin -= 10;
            }
            EditorGUI.EndProperty();
        }
        public static void DrawPropertyAnim(Rect rect, SerializedProperty property, int currentFrame, GUIContent label)
        {

            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Glow"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var glowIteration = property.FindPropertyRelative("_glowIteration");
                CustomGUIUtility.IntAnimField(rect, glowIteration, currentFrame, new GUIContent("Iteration"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var glowSize = property.FindPropertyRelative("_glowSize");
                CustomGUIUtility.FloatAnimField(rect, glowSize, currentFrame, new GUIContent("Size"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var glowThreshold = property.FindPropertyRelative("_glowThreshold");
                CustomGUIUtility.FloatAnimField(rect, glowThreshold, currentFrame, new GUIContent("Threshold"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var glowThresholdSmooth = property.FindPropertyRelative("_glowThresholdSmooth");
                CustomGUIUtility.FloatAnimField(rect, glowThresholdSmooth, currentFrame, new GUIContent("Smooth"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var glowIntensity = property.FindPropertyRelative("_glowIntensity");
                CustomGUIUtility.FloatAnimField(rect, glowIntensity, currentFrame, new GUIContent("Intensity"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var glowColor = property.FindPropertyRelative("_glowColor");
                CustomGUIUtility.PropertyField(rect, glowColor, new GUIContent("Color"));

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
                height += CustomGUIUtility.PropertyHeight * 6;
            }

            height += CustomGUIUtility.LayerSpaceHeight;

            return height;
        }
    }
}