using UnityEditor;
using UnityEngine;

namespace VFXTextureMaker
{
    public class OpBlurDrawer
    {
        public static void DrawProperty(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Blur"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var blurType = property.FindPropertyRelative("_blurType");
                CustomGUIUtility.PropertyField(rect, blurType, new GUIContent("Blur Type"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var blurSampleCount = property.FindPropertyRelative("_blurSampleCount");
                CustomGUIUtility.IntField(rect, blurSampleCount, new GUIContent("Sample Count"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var blurSize = property.FindPropertyRelative("_blurSize");
                CustomGUIUtility.IntField(rect, blurSize, new GUIContent("Blur Size"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var blurGausianSigma = property.FindPropertyRelative("_blurGausianSigma");
                CustomGUIUtility.FloatField(rect, blurGausianSigma, new GUIContent("Gausian Sigma"));
                rect.y += CustomGUIUtility.PropertyHeight;

                if (blurType.enumValueIndex == 1)
                {
                    var blurDirection = property.FindPropertyRelative("_blurDirection");
                    CustomGUIUtility.PropertyValueField(rect, blurDirection, new GUIContent("Direction"));
                    rect.y += CustomGUIUtility.PropertyHeight;
                }

                if (blurType.enumValueIndex == 2 || blurType.enumValueIndex == 3)
                {
                    var blurCenter = property.FindPropertyRelative("_blurCenter");
                    CustomGUIUtility.PropertyValueField(rect, blurCenter, new GUIContent("Center"));
                    rect.y += CustomGUIUtility.PropertyHeight;
                }

                var blurRepeat = property.FindPropertyRelative("_blurRepeat");
                CustomGUIUtility.PropertyValueField(rect, blurRepeat, new GUIContent("Repeat"));
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
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Blur"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var blurType = property.FindPropertyRelative("_blurType");
                CustomGUIUtility.PropertyField(rect, blurType, new GUIContent("Blur Type"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var blurSampleCount = property.FindPropertyRelative("_blurSampleCount");
                CustomGUIUtility.IntAnimField(rect, blurSampleCount, currentFrame, new GUIContent("Sample Count"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var blurSize = property.FindPropertyRelative("_blurSize");
                CustomGUIUtility.IntAnimField(rect, blurSize, currentFrame, new GUIContent("Blur Size"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var blurGausianSigma = property.FindPropertyRelative("_blurGausianSigma");
                CustomGUIUtility.FloatAnimField(rect, blurGausianSigma, currentFrame, new GUIContent("Gausian Sigma"));
                rect.y += CustomGUIUtility.PropertyHeight;

                if (blurType.enumValueIndex == 1)
                {
                    var blurDirection = property.FindPropertyRelative("_blurDirection");
                    CustomGUIUtility.Vector2AnimField(rect, blurDirection, currentFrame, new GUIContent("Direction"));
                    rect.y += CustomGUIUtility.PropertyHeight;
                }
                else if (blurType.enumValueIndex == 2)
                {
                    var blurCenter = property.FindPropertyRelative("_blurCenter");
                    CustomGUIUtility.Vector2AnimField(rect, blurCenter, currentFrame, new GUIContent("Center"));
                    rect.y += CustomGUIUtility.PropertyHeight;
                }

                var blurRepeat = property.FindPropertyRelative("_blurRepeat");
                CustomGUIUtility.BoolAnimField(rect, blurRepeat, currentFrame, new GUIContent("Repeat"));
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
                var blurType = property.FindPropertyRelative("_blurType");
                height += CustomGUIUtility.PropertyHeight * 6;

                if (blurType.enumValueIndex == 1)
                {
                    height += CustomGUIUtility.PropertyHeight;
                }
                else if (blurType.enumValueIndex == 2)
                {
                    height += CustomGUIUtility.PropertyHeight;
                }
            }

            height += CustomGUIUtility.LayerSpaceHeight;

            return height;
        }
    }
}