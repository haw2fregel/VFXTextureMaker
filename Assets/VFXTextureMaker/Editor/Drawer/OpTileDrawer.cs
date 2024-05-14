using UnityEditor;
using UnityEngine;

namespace VFXTextureMaker
{
    public class OpTileDrawer
    {
        public static void DrawProperty(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Bend"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var tile = property.FindPropertyRelative("_tile");
                CustomGUIUtility.PropertyValueField(rect, tile, new GUIContent("Tile"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var tileOffset = property.FindPropertyRelative("_tileOffset");
                CustomGUIUtility.PropertyValueField(rect, tileOffset, new GUIContent("Offset"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var tileRandomOffsetX = property.FindPropertyRelative("_tileRandomOffsetX");
                CustomGUIUtility.PropertyValueField(rect, tileRandomOffsetX, new GUIContent("Random Offset X"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var tileRandomOffsetY = property.FindPropertyRelative("_tileRandomOffsetY");
                CustomGUIUtility.PropertyValueField(rect, tileRandomOffsetY, new GUIContent("Random Offset Y"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var tileRandomScale = property.FindPropertyRelative("_tileRandomScale");
                CustomGUIUtility.PropertyValueField(rect, tileRandomScale, new GUIContent("Random Scale"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var tileRandomScaleX = property.FindPropertyRelative("_tileRandomScaleX");
                CustomGUIUtility.PropertyValueField(rect, tileRandomScaleX, new GUIContent("Random Scale X"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var tileRandomScaleY = property.FindPropertyRelative("_tileRandomScaleY");
                CustomGUIUtility.PropertyValueField(rect, tileRandomScaleY, new GUIContent("Random Scale Y"));

                rect.xMin -= 10;
            }
            EditorGUI.EndProperty();
        }

        public static void DrawPropertyAnim(Rect rect, SerializedProperty property, int currentFrame, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Bend"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var tile = property.FindPropertyRelative("_tile");
                CustomGUIUtility.Vector2AnimField(rect, tile, currentFrame, new GUIContent("Tile"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var tileOffset = property.FindPropertyRelative("_tileOffset");
                CustomGUIUtility.Vector2AnimField(rect, tileOffset, currentFrame, new GUIContent("Offset"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var tileRandomOffsetX = property.FindPropertyRelative("_tileRandomOffsetX");
                CustomGUIUtility.Vector2AnimField(rect, tileRandomOffsetX, currentFrame, new GUIContent("Random Offset X"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var tileRandomOffsetY = property.FindPropertyRelative("_tileRandomOffsetY");
                CustomGUIUtility.Vector2AnimField(rect, tileRandomOffsetY, currentFrame, new GUIContent("Random Offset Y"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var tileRandomScale = property.FindPropertyRelative("_tileRandomScale");
                CustomGUIUtility.Vector2AnimField(rect, tileRandomScale, currentFrame, new GUIContent("Random Scale"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var tileRandomScaleX = property.FindPropertyRelative("_tileRandomScaleX");
                CustomGUIUtility.Vector2AnimField(rect, tileRandomScaleX, currentFrame, new GUIContent("Random Scale X"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var tileRandomScaleY = property.FindPropertyRelative("_tileRandomScaleY");
                CustomGUIUtility.Vector2AnimField(rect, tileRandomScaleY, currentFrame, new GUIContent("Random Scale Y"));

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

    }
}