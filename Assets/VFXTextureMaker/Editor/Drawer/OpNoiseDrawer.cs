using UnityEditor;
using UnityEngine;

namespace VFXTextureMaker
{
    public class OpNoiseDrawer
    {
        public static void DrawProperty(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Noise"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var noiseType = property.FindPropertyRelative("_noiseType");
                CustomGUIUtility.PropertyField(rect, noiseType, new GUIContent("Noise Type"));
                rect.y += CustomGUIUtility.PropertyHeight;

                if (noiseType.enumValueIndex == 4 || noiseType.enumValueIndex == 5 || noiseType.enumValueIndex == 6)
                {
                    var noiseLengthPow = property.FindPropertyRelative("_noiseLengthPow");
                    CustomGUIUtility.FloatField(rect, noiseLengthPow, new GUIContent("Length Pow"));
                    rect.y += CustomGUIUtility.PropertyHeight;
                }

                if (noiseType.enumValueIndex == 6)
                {
                    var voronoiEdgeSize = property.FindPropertyRelative("_voronoiEdgeSize");
                    CustomGUIUtility.FloatField(rect, voronoiEdgeSize, new GUIContent("Edge Size"));
                    rect.y += CustomGUIUtility.PropertyHeight;
                }

                var noiseTile = property.FindPropertyRelative("_noiseTile");
                CustomGUIUtility.PropertyValueField(rect, noiseTile, new GUIContent("Tile"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var noiseOffset = property.FindPropertyRelative("_noiseOffset");
                CustomGUIUtility.PropertyValueField(rect, noiseOffset, new GUIContent("Offset"));
                rect.y += CustomGUIUtility.PropertyHeight;

                if (noiseType.enumValueIndex == 0 || noiseType.enumValueIndex == 1 || noiseType.enumValueIndex == 2 || noiseType.enumValueIndex == 3)
                {
                    var noiseSelfWarp = property.FindPropertyRelative("_noiseSelfWarp");
                    CustomGUIUtility.PropertyValueField(rect, noiseSelfWarp, new GUIContent("Self Warp"));
                    rect.y += CustomGUIUtility.PropertyHeight;

                    var noiseSelfWarpTile = property.FindPropertyRelative("_noiseSelfWarpTile");
                    CustomGUIUtility.PropertyValueField(rect, noiseSelfWarpTile, new GUIContent("Warp Tile"));
                    rect.y += CustomGUIUtility.PropertyHeight;

                    var noiseSelfWarpOffset = property.FindPropertyRelative("_noiseSelfWarpOffset");
                    CustomGUIUtility.PropertyValueField(rect, noiseSelfWarpOffset, new GUIContent("Warp Offset"));
                    rect.y += CustomGUIUtility.PropertyHeight;
                }

                if (noiseType.enumValueIndex != 5 && noiseType.enumValueIndex != 6)
                {
                    var noiseWeight = property.FindPropertyRelative("_noiseWeight");
                    CustomGUIUtility.Vector4Field(rect, noiseWeight, new GUIContent("Weight"));
                    rect.y += CustomGUIUtility.PropertyHeight;
                }

                var noiseSeamless = property.FindPropertyRelative("_noiseSeamless");
                CustomGUIUtility.PropertyValueField(rect, noiseSeamless, new GUIContent("Seamless"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var noiseRandomSeed = property.FindPropertyRelative("_noiseRandomSeed");
                CustomGUIUtility.IntField(rect, noiseRandomSeed, new GUIContent("RandomSeed"));

                rect.xMin -= 10;

            }
            EditorGUI.EndProperty();
        }
        public static void DrawPropertyAnim(Rect rect, SerializedProperty property, int currentFrame, GUIContent label)
        {

            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Noise"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var noiseType = property.FindPropertyRelative("_noiseType");
                CustomGUIUtility.PropertyField(rect, noiseType, new GUIContent("Noise Type"));
                rect.y += CustomGUIUtility.PropertyHeight;

                if (noiseType.enumValueIndex == 4 || noiseType.enumValueIndex == 5 || noiseType.enumValueIndex == 6)
                {
                    var noiseLengthPow = property.FindPropertyRelative("_noiseLengthPow");
                    CustomGUIUtility.FloatAnimField(rect, noiseLengthPow, currentFrame, new GUIContent("Length Pow"));
                    rect.y += CustomGUIUtility.PropertyHeight;
                }

                if (noiseType.enumValueIndex == 6)
                {
                    var voronoiEdgeSize = property.FindPropertyRelative("_voronoiEdgeSize");
                    CustomGUIUtility.FloatAnimField(rect, voronoiEdgeSize, currentFrame, new GUIContent("Edge Size"));
                    rect.y += CustomGUIUtility.PropertyHeight;
                }

                var noiseTile = property.FindPropertyRelative("_noiseTile");
                CustomGUIUtility.Vector2AnimField(rect, noiseTile, currentFrame, new GUIContent("Tile"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var noiseOffset = property.FindPropertyRelative("_noiseOffset");
                CustomGUIUtility.Vector2AnimField(rect, noiseOffset, currentFrame, new GUIContent("Offset"));
                rect.y += CustomGUIUtility.PropertyHeight;

                if (noiseType.enumValueIndex == 0 || noiseType.enumValueIndex == 1 || noiseType.enumValueIndex == 2 || noiseType.enumValueIndex == 3)
                {
                    var noiseSelfWarp = property.FindPropertyRelative("_noiseSelfWarp");
                    CustomGUIUtility.Vector2AnimField(rect, noiseSelfWarp, currentFrame, new GUIContent("Self Warp"));
                    rect.y += CustomGUIUtility.PropertyHeight;

                    var noiseSelfWarpTile = property.FindPropertyRelative("_noiseSelfWarpTile");
                    CustomGUIUtility.Vector2AnimField(rect, noiseSelfWarpTile, currentFrame, new GUIContent("Warp Tile"));
                    rect.y += CustomGUIUtility.PropertyHeight;

                    var noiseSelfWarpOffset = property.FindPropertyRelative("_noiseSelfWarpOffset");
                    CustomGUIUtility.Vector2AnimField(rect, noiseSelfWarpOffset, currentFrame, new GUIContent("Warp Offset"));
                    rect.y += CustomGUIUtility.PropertyHeight;
                }

                if (noiseType.enumValueIndex != 5 && noiseType.enumValueIndex != 6)
                {
                    var noiseWeight = property.FindPropertyRelative("_noiseWeight");
                    CustomGUIUtility.Vector4AnimField(rect, noiseWeight, currentFrame, new GUIContent("Weight"));
                    rect.y += CustomGUIUtility.PropertyHeight;
                }

                var noiseSeamless = property.FindPropertyRelative("_noiseSeamless");
                CustomGUIUtility.BoolAnimField(rect, noiseSeamless, currentFrame, new GUIContent("Seamless"));
                rect.y += CustomGUIUtility.PropertyHeight;

                var noiseRandomSeed = property.FindPropertyRelative("_noiseRandomSeed");
                CustomGUIUtility.IntAnimField(rect, noiseRandomSeed, currentFrame, new GUIContent("Random Seed"));

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
                var noiseType = property.FindPropertyRelative("_noiseType");
                height += CustomGUIUtility.PropertyHeight;

                if (noiseType.enumValueIndex == 4 || noiseType.enumValueIndex == 5 || noiseType.enumValueIndex == 6)
                {
                    height += CustomGUIUtility.PropertyHeight;
                }

                if (noiseType.enumValueIndex == 6)
                {
                    height += CustomGUIUtility.PropertyHeight;
                }

                height += CustomGUIUtility.PropertyHeight;
                height += CustomGUIUtility.PropertyHeight;

                if (noiseType.enumValueIndex == 0 || noiseType.enumValueIndex == 1 || noiseType.enumValueIndex == 2 || noiseType.enumValueIndex == 3)
                {
                    height += CustomGUIUtility.PropertyHeight;
                    height += CustomGUIUtility.PropertyHeight;
                    height += CustomGUIUtility.PropertyHeight;
                }
                if (noiseType.enumValueIndex != 5 && noiseType.enumValueIndex != 6)
                {

                    height += CustomGUIUtility.PropertyHeight;
                }

                height += CustomGUIUtility.PropertyHeight;

                height += CustomGUIUtility.PropertyHeight;


            }

            height += CustomGUIUtility.LayerSpaceHeight;

            return height;
        }
    }
}