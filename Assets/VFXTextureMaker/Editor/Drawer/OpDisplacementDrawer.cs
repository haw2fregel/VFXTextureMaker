using UnityEditor;
using UnityEngine;

namespace VFXTextureMaker
{
    public class OpDisplacementDrawer
    {
        public static void DrawProperty(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Displacement"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var displacementChannel = property.FindPropertyRelative("_displacementChannel");
                DisplacementChannelField(rect, displacementChannel, new GUIContent("Channel"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var displacementStrength = property.FindPropertyRelative("_displacementStrength");
                CustomGUIUtility.FloatField(rect, displacementStrength, new GUIContent("Strength"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var displacementWeight = property.FindPropertyRelative("_displacementWeight");
                CustomGUIUtility.Vector4Field(rect, displacementWeight, new GUIContent("Weight"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var displacementRepeat = property.FindPropertyRelative("_displacementRepeat");
                CustomGUIUtility.PropertyValueField(rect, displacementRepeat, new GUIContent("Repeat"));
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

                var displacementChannel = property.FindPropertyRelative("_displacementChannel");
                DisplacementChannelField(rect, displacementChannel, new GUIContent("Channel"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var displacementStrength = property.FindPropertyRelative("_displacementStrength");
                CustomGUIUtility.FloatAnimField(rect, displacementStrength, currentFrame, new GUIContent("Strength"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var displacementWeight = property.FindPropertyRelative("_displacementWeight");
                CustomGUIUtility.Vector4AnimField(rect, displacementWeight, currentFrame, new GUIContent("Weight"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var displacementRepeat = property.FindPropertyRelative("_displacementRepeat");
                CustomGUIUtility.BoolAnimField(rect, displacementRepeat, currentFrame, new GUIContent("Repeat"));
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
                height += CustomGUIUtility.PropertyHeight * 4;
            }

            height += CustomGUIUtility.LayerSpaceHeight;

            return height;
        }

        static readonly string[] DisplacementChannel =
        {
            "X",
            "Y",
            "None"
        };
        static void DisplacementChannelField(Rect rect, SerializedProperty property, GUIContent label)
        {
            var value = property.vector2Value;

            var rectBG = rect;
            rectBG.width = 105;
            EditorGUI.DrawRect(rectBG, new Color(1, 1, 1, 0.1f));

            var rectLabel = rect;
            rectLabel.width = 100;
            EditorGUI.LabelField(rectLabel, label, EditorStyles.boldLabel);

            var lineRect = rect;
            lineRect.width = 2;
            lineRect.x = rect.x + rectLabel.width + 5;
            EditorGUI.DrawRect(lineRect, new Color(0.5f, 0.5f, 0.5f, 0.5f));

            rect.y += EditorGUIUtility.singleLineHeight;
            rect.xMin += 10;
            EditorGUI.DrawRect(rect, new Color(1, 1, 1, 0.02f));

            var rectLabelX = rect;
            rectLabelX.width = 16;
            EditorGUI.LabelField(rectLabelX, "X");

            var rectValueX = rect;
            rectValueX.width *= 0.5f;
            rectValueX.width -= 16;
            rectValueX.x += 16;
            value.x = (float)EditorGUI.Popup(rectValueX, (int)value.x, DisplacementChannel);

            var rectLabelY = rect;
            rectLabelY.x += rect.width * 0.5f + 2;
            rectLabelY.width = 16;
            EditorGUI.LabelField(rectLabelY, "Y");

            var rectValueY = rect;
            rectValueY.width *= 0.5f;
            rectValueY.width -= 16;
            rectValueY.x += rect.width * 0.5f + 16;
            value.y = (float)EditorGUI.Popup(rectValueY, (int)value.y, DisplacementChannel);

            property.vector2Value = value;

        }

    }
}