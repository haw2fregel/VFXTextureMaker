using UnityEditor;
using UnityEngine;

namespace VFXTextureMaker
{
    public class OpCustomShaderDrawer
    {
        public static void DrawProperty(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var showOption = property.FindPropertyRelative("showOption");
            showOption.boolValue = CustomGUIUtility.Foldout(rect, showOption.boolValue, new GUIContent("Custom Materail"));
            if (showOption.boolValue)
            {
                rect.xMin += 10;
                rect.y += CustomGUIUtility.FoldoutHeight;

                var customShader = property.FindPropertyRelative("_customShader");
                var customMaterial = property.FindPropertyRelative("_customMaterial");

                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    CustomGUIUtility.PropertyField(rect, customShader, new GUIContent("Shader"));
                    if (check.changed)
                    {
                        //設定したShaderからマテリアルを生成。
                        //TextureDataのサブアセットとして管理する。
                        if (customMaterial.objectReferenceValue != null) Object.DestroyImmediate(customMaterial.objectReferenceValue, true);
                        if (customShader.objectReferenceValue != null)
                        {
                            var material = new Material((Shader)customShader.objectReferenceValue);
                            AssetDatabase.AddObjectToAsset(material, TextureDataEditor.TextureData);
                            customMaterial.objectReferenceValue = material;

                            //ImportAssets()でReimportすると警告がでるので、RenameAsset()を利用
                            AssetDatabase.RenameAsset(TextureDataEditor.TextureDataPath, TextureDataEditor.TextureDataPath);
                        }
                    }
                }
                rect.y += CustomGUIUtility.PropertyHeight;

                if (customMaterial.objectReferenceValue != null)
                {
                    foreach (var materialProperty in MaterialEditor.GetMaterialProperties(new Object[] { customMaterial.objectReferenceValue }))
                    {
                        if (materialProperty.displayName == "MainTex" || materialProperty.displayName == "MaskTex"
                            || materialProperty.displayName == "unity_Lightmaps" || materialProperty.displayName == "unity_LightmapsInd"
                            || materialProperty.displayName == "unity_ShadowMasks") continue;

                        CustomGUIUtility.MaterialPropertyField(rect, materialProperty);
                        rect.y += CustomGUIUtility.PropertyHeight;
                    }
                }

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

                var customShader = property.FindPropertyRelative("_customShader");
                var customMaterial = property.FindPropertyRelative("_customMaterial");

                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    CustomGUIUtility.PropertyField(rect, customShader, new GUIContent("Shader"));
                    if (check.changed)
                    {
                        //設定したShaderからマテリアルを生成。
                        //TextureDataのサブアセットとして管理する。
                        if (customMaterial.objectReferenceValue != null) Object.DestroyImmediate(customMaterial.objectReferenceValue, true);
                        if (customShader.objectReferenceValue != null)
                        {
                            var material = new Material((Shader)customShader.objectReferenceValue);
                            AssetDatabase.AddObjectToAsset(material, TextureDataEditor.TextureData);
                            customMaterial.objectReferenceValue = material;

                            //ImportAssets()でReimportすると警告がでるので、RenameAsset()を利用
                            AssetDatabase.RenameAsset(TextureDataEditor.TextureDataPath, TextureDataEditor.TextureDataPath);
                        }
                    }
                }
                rect.y += CustomGUIUtility.PropertyHeight;

                if (customMaterial.objectReferenceValue != null)
                {
                    foreach (var materialProperty in MaterialEditor.GetMaterialProperties(new Object[] { customMaterial.objectReferenceValue }))
                    {
                        if (materialProperty.displayName == "MainTex" || materialProperty.displayName == "MaskTex"
                            || materialProperty.displayName == "unity_Lightmaps" || materialProperty.displayName == "unity_LightmapsInd"
                            || materialProperty.displayName == "unity_ShadowMasks") continue;

                        CustomGUIUtility.MaterialPropertyField(rect, materialProperty);
                        rect.y += CustomGUIUtility.PropertyHeight;
                    }
                }

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
                height += CustomGUIUtility.PropertyHeight;

                var customMaterial = property.FindPropertyRelative("_customMaterial");
                if (customMaterial.objectReferenceValue != null)
                {
                    foreach (var materialProperty in MaterialEditor.GetMaterialProperties(new Object[] { customMaterial.objectReferenceValue }))
                    {
                        if (materialProperty.displayName == "MainTex" || materialProperty.displayName == "MaskTex"
                            || materialProperty.displayName == "unity_Lightmaps" || materialProperty.displayName == "unity_LightmapsInd"
                            || materialProperty.displayName == "unity_ShadowMasks") continue;

                        height += CustomGUIUtility.PropertyHeight;
                    }
                }

            }

            height += CustomGUIUtility.LayerSpaceHeight;

            return height;
        }
    }
}