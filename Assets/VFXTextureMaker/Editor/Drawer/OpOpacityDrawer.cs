using UnityEditor;
using UnityEngine;

namespace VFXTextureMaker
{
    public class OpOpacityDrawer
    {
        public static void DrawProperty(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Opacity"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var blend = property.FindPropertyRelative("_blend");
                CustomGUIUtility.PropertyField(rect, blend, new GUIContent("Blend Mode"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var drawChannel = property.FindPropertyRelative("_drawChannel");
                DrawChannelField(rect, drawChannel, new GUIContent("Draw Channel"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var opacity = property.FindPropertyRelative("_opacity");
                CustomGUIUtility.FloatField(rect, opacity, new GUIContent("Opacity"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var opacityPower = property.FindPropertyRelative("_opacityPower");
                CustomGUIUtility.FloatField(rect, opacityPower, new GUIContent("Power"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var isMultiplyAlpha = property.FindPropertyRelative("_isMultiplyAlpha");
                CustomGUIUtility.PropertyValueField(rect, isMultiplyAlpha, new GUIContent("Multiply Alpha"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var isOneMinus = property.FindPropertyRelative("_isOneMinus");
                CustomGUIUtility.PropertyValueField(rect, isOneMinus, new GUIContent("OneMinus"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var contrast = property.FindPropertyRelative("_contrast");
                CustomGUIUtility.FloatField(rect, contrast, new GUIContent("Contrast"));

                rect.xMin -= 10;
            }
            EditorGUI.EndProperty();
        }

        public static void DrawPropertyAnim(Rect rect, SerializedProperty property, int currentFrame, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Opacity"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var blend = property.FindPropertyRelative("_blend");
                CustomGUIUtility.PropertyField(rect, blend, new GUIContent("Blend Mode"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var drawChannel = property.FindPropertyRelative("_drawChannel");
                DrawChannelField(rect, drawChannel, new GUIContent("Draw Channel"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var opacity = property.FindPropertyRelative("_opacity");
                CustomGUIUtility.FloatAnimField(rect, opacity, currentFrame, new GUIContent("Opacity"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var opacityPower = property.FindPropertyRelative("_opacityPower");
                CustomGUIUtility.FloatAnimField(rect, opacityPower, currentFrame, new GUIContent("Power"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var isMultiplyAlpha = property.FindPropertyRelative("_isMultiplyAlpha");
                CustomGUIUtility.BoolAnimField(rect, isMultiplyAlpha, currentFrame, new GUIContent("Multiply Alpha"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var isOneMinus = property.FindPropertyRelative("_isOneMinus");
                CustomGUIUtility.BoolAnimField(rect, isOneMinus, currentFrame, new GUIContent("OneMinus"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var contrast = property.FindPropertyRelative("_contrast");
                CustomGUIUtility.FloatAnimField(rect, contrast, currentFrame, new GUIContent("Contrast"));

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
                height += CustomGUIUtility.PropertyHeight * 7;
            }

            height += CustomGUIUtility.LayerSpaceHeight;

            return height;
        }

        static readonly string[] DrawChannel =
        {
            "R",
            "G",
            "B",
            "A",
            "Luminance",
            "1",
            "0",
            "None"
        };
        static void DrawChannelField(Rect rect, SerializedProperty property, GUIContent label)
        {
            var value = property.vector4Value;

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

            var rectLabelR = rect;
            rectLabelR.width = 16;
            EditorGUI.LabelField(rectLabelR, "R");

            var rectValueR = rect;
            rectValueR.width *= 0.25f;
            rectValueR.width -= 16;
            rectValueR.x += 16;
            value.x = (float)EditorGUI.Popup(rectValueR, (int)value.x, DrawChannel);

            var rectLabelG = rect;
            rectLabelG.x += rect.width * 0.25f + 2;
            rectLabelG.width = 16;
            EditorGUI.LabelField(rectLabelG, "G");

            var rectValueG = rect;
            rectValueG.width *= 0.25f;
            rectValueG.width -= 16;
            rectValueG.x += rect.width * 0.25f + 16;
            value.y = (float)EditorGUI.Popup(rectValueG, (int)value.y, DrawChannel);

            var rectLabelB = rect;
            rectLabelB.x += rect.width * 0.5f + 2;
            rectLabelB.width = 16;
            EditorGUI.LabelField(rectLabelB, "B");

            var rectValueB = rect;
            rectValueB.width *= 0.25f;
            rectValueB.width -= 16;
            rectValueB.x += rect.width * 0.5f + 16;
            value.z = (float)EditorGUI.Popup(rectValueB, (int)value.z, DrawChannel);

            var rectLabelA = rect;
            rectLabelA.x += rect.width * 0.75f + 2;
            rectLabelA.width = 16;
            EditorGUI.LabelField(rectLabelA, "A");

            var rectValueA = rect;
            rectValueA.width *= 0.25f;
            rectValueA.width -= 16;
            rectValueA.x += rect.width * 0.75f + 16;
            value.w = (float)EditorGUI.Popup(rectValueA, (int)value.w, DrawChannel);

            property.vector4Value = value;

        }

    }
}