using UnityEditor;
using UnityEngine;

namespace VFXTextureMaker
{
    [CustomPropertyDrawer(typeof(Layer))]
    public class LayerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            var animated = property.FindPropertyRelative("_animated");
            if (animated.boolValue)
            {
                DrawPropertyAnim(rect, property, label);
            }
            else
            {
                DrawProperty(rect, property, label);
            }
        }

        public void DrawProperty(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var index = property.FindPropertyRelative("_index");
            var shaderPassProp = property.FindPropertyRelative("_shaderPass");
            var shaderPassNames = shaderPassProp.enumNames;
            var shaderPassIndex = shaderPassProp.enumValueIndex;
            EditorGUI.LabelField(rect, "" + index.intValue + " : " + shaderPassNames[shaderPassIndex]);

            var rocked = property.FindPropertyRelative("_rocked");
            using (new EditorGUI.DisabledScope(rocked.boolValue))
            {
                rect.y += EditorGUIUtility.singleLineHeight;

                var draw = property.FindPropertyRelative("_draw");
                var drawActive = draw.FindPropertyRelative("active");
                if (drawActive.boolValue)
                {
                    OpDrawDrawer.DrawProperty(rect, draw, label);
                    rect.y += OpDrawDrawer.GetPropertyHeight(draw);
                }

                var texture = property.FindPropertyRelative("_texture");
                var textureActive = texture.FindPropertyRelative("active");
                if (textureActive.boolValue)
                {
                    OpTextureDrawer.DrawProperty(rect, texture, label);
                    rect.y += OpTextureDrawer.GetPropertyHeight(texture);
                }

                var noise = property.FindPropertyRelative("_noise");
                var noiseActive = noise.FindPropertyRelative("active");
                if (noiseActive.boolValue)
                {
                    OpNoiseDrawer.DrawProperty(rect, noise, label);
                    rect.y += OpNoiseDrawer.GetPropertyHeight(noise);
                }

                var blur = property.FindPropertyRelative("_blur");
                var blurActive = blur.FindPropertyRelative("active");
                if (blurActive.boolValue)
                {
                    OpBlurDrawer.DrawProperty(rect, blur, label);
                    rect.y += OpBlurDrawer.GetPropertyHeight(blur);
                }

                var shape = property.FindPropertyRelative("_shape");
                var shapeActive = shape.FindPropertyRelative("active");
                if (shapeActive.boolValue)
                {
                    OpShapeDrawer.DrawProperty(rect, shape, label);
                    rect.y += OpShapeDrawer.GetPropertyHeight(shape);
                }

                var gradient = property.FindPropertyRelative("_gradient");
                var gradientActive = gradient.FindPropertyRelative("active");
                if (gradientActive.boolValue)
                {
                    OpGradientDrawer.DrawProperty(rect, gradient, label);
                    rect.y += OpGradientDrawer.GetPropertyHeight(gradient);
                }

                var glow = property.FindPropertyRelative("_glow");
                var glowActive = glow.FindPropertyRelative("active");
                if (glowActive.boolValue)
                {
                    OpGlowDrawer.DrawProperty(rect, glow, label);
                    rect.y += OpGlowDrawer.GetPropertyHeight(glow);
                }

                var uvDeform = property.FindPropertyRelative("_uvDeform");
                var uvDeformActive = uvDeform.FindPropertyRelative("active");
                if (uvDeformActive.boolValue)
                {
                    OpUvDeformDrawer.DrawProperty(rect, uvDeform, label);
                    rect.y += OpUvDeformDrawer.GetPropertyHeight(uvDeform);
                }

                var opacity = property.FindPropertyRelative("_opacity");
                var opacityActive = opacity.FindPropertyRelative("active");
                if (opacityActive.boolValue)
                {
                    OpOpacityDrawer.DrawProperty(rect, opacity, label);
                    rect.y += OpOpacityDrawer.GetPropertyHeight(opacity);
                }

                var displacement = property.FindPropertyRelative("_displacement");
                var displacementActive = displacement.FindPropertyRelative("active");
                if (displacementActive.boolValue)
                {
                    OpDisplacementDrawer.DrawProperty(rect, displacement, label);
                    rect.y += OpDisplacementDrawer.GetPropertyHeight(displacement);
                }

                var colorBalance = property.FindPropertyRelative("_colorBalance");
                var colorBalanceActive = colorBalance.FindPropertyRelative("active");
                if (colorBalanceActive.boolValue)
                {
                    OpColorBalanceDrawer.DrawProperty(rect, colorBalance, label);
                    rect.y += OpColorBalanceDrawer.GetPropertyHeight(colorBalance);
                }

                var customShader = property.FindPropertyRelative("_customShader");
                var customShaderActive = customShader.FindPropertyRelative("active");
                if (customShaderActive.boolValue)
                {
                    OpCustomShaderDrawer.DrawProperty(rect, customShader, label);
                    rect.y += OpCustomShaderDrawer.GetPropertyHeight(customShader);
                }

            }
            EditorGUI.EndProperty();
        }

        public void DrawPropertyAnim(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            var index = property.FindPropertyRelative("_index");
            var shaderPassProp = property.FindPropertyRelative("_shaderPass");
            var shaderPassNames = shaderPassProp.enumNames;
            var shaderPassIndex = shaderPassProp.enumValueIndex;
            EditorGUI.LabelField(rect, "" + index.intValue + " : " + shaderPassNames[shaderPassIndex]);

            var currentFrame = property.FindPropertyRelative("_currentFrame");

            var rocked = property.FindPropertyRelative("_rocked");
            using (new EditorGUI.DisabledScope(rocked.boolValue))
            {
                rect.y += EditorGUIUtility.singleLineHeight;

                var draw = property.FindPropertyRelative("_draw");
                var drawActive = draw.FindPropertyRelative("active");
                if (drawActive.boolValue)
                {
                    OpDrawDrawer.DrawPropertyAnim(rect, draw, currentFrame.intValue, label);
                    rect.y += OpDrawDrawer.GetPropertyHeight(draw);
                }

                var noise = property.FindPropertyRelative("_noise");
                var noiseActive = noise.FindPropertyRelative("active");
                if (noiseActive.boolValue)
                {
                    OpNoiseDrawer.DrawPropertyAnim(rect, noise, currentFrame.intValue, label);
                    rect.y += OpNoiseDrawer.GetPropertyHeight(noise); ;
                }

                var texture = property.FindPropertyRelative("_texture");
                var textureActive = texture.FindPropertyRelative("active");
                if (textureActive.boolValue)
                {
                    OpTextureDrawer.DrawPropertyAnim(rect, texture, label);
                    rect.y += OpTextureDrawer.GetPropertyHeight(texture);
                }

                var blur = property.FindPropertyRelative("_blur");
                var blurActive = blur.FindPropertyRelative("active");
                if (blurActive.boolValue)
                {
                    OpBlurDrawer.DrawPropertyAnim(rect, blur, currentFrame.intValue, label);
                    rect.y += OpBlurDrawer.GetPropertyHeight(blur);
                }

                var shape = property.FindPropertyRelative("_shape");
                var shapeActive = shape.FindPropertyRelative("active");
                if (shapeActive.boolValue)
                {
                    OpShapeDrawer.DrawPropertyAnim(rect, shape, currentFrame.intValue, label);
                    rect.y += OpShapeDrawer.GetPropertyHeight(shape);
                }

                var gradient = property.FindPropertyRelative("_gradient");
                var gradientActive = gradient.FindPropertyRelative("active");
                if (gradientActive.boolValue)
                {
                    OpGradientDrawer.DrawPropertyAnim(rect, gradient, currentFrame.intValue, label);
                    rect.y += OpGradientDrawer.GetPropertyHeight(gradient);
                }

                var glow = property.FindPropertyRelative("_glow");
                var glowActive = glow.FindPropertyRelative("active");
                if (glowActive.boolValue)
                {
                    OpGlowDrawer.DrawPropertyAnim(rect, glow, currentFrame.intValue, label);
                    rect.y += OpGlowDrawer.GetPropertyHeight(glow);
                }

                var uvDeform = property.FindPropertyRelative("_uvDeform");
                var uvDeformActive = uvDeform.FindPropertyRelative("active");
                if (uvDeformActive.boolValue)
                {
                    OpUvDeformDrawer.DrawPropertyAnim(rect, uvDeform, currentFrame.intValue, label);
                    rect.y += OpUvDeformDrawer.GetPropertyHeight(uvDeform);
                }

                var opacity = property.FindPropertyRelative("_opacity");
                var opacityActive = opacity.FindPropertyRelative("active");
                if (opacityActive.boolValue)
                {
                    OpOpacityDrawer.DrawPropertyAnim(rect, opacity, currentFrame.intValue, label);
                    rect.y += OpOpacityDrawer.GetPropertyHeight(opacity);
                }

                var displacement = property.FindPropertyRelative("_displacement");
                var displacementActive = displacement.FindPropertyRelative("active");
                if (displacementActive.boolValue)
                {
                    OpDisplacementDrawer.DrawPropertyAnim(rect, displacement, currentFrame.intValue, label);
                    rect.y += OpDisplacementDrawer.GetPropertyHeight(displacement);
                }

                var colorBalance = property.FindPropertyRelative("_colorBalance");
                var colorBalanceActive = colorBalance.FindPropertyRelative("active");
                if (colorBalanceActive.boolValue)
                {
                    OpColorBalanceDrawer.DrawPropertyAnim(rect, colorBalance, currentFrame.intValue, label);
                    rect.y += OpColorBalanceDrawer.GetPropertyHeight(colorBalance);
                }

                var customShader = property.FindPropertyRelative("_customShader");
                var customShaderActive = customShader.FindPropertyRelative("active");
                if (customShaderActive.boolValue)
                {
                    OpCustomShaderDrawer.DrawPropertyAnim(rect, customShader, currentFrame.intValue, label);
                    rect.y += OpCustomShaderDrawer.GetPropertyHeight(customShader);
                }
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;

            var draw = property.FindPropertyRelative("_draw");
            var drawActive = draw.FindPropertyRelative("active");
            if (drawActive.boolValue)
            {
                height += OpDrawDrawer.GetPropertyHeight(draw);
            }

            var texture = property.FindPropertyRelative("_texture");
            var textureActive = texture.FindPropertyRelative("active");
            if (textureActive.boolValue)
            {
                height += OpTextureDrawer.GetPropertyHeight(texture);
            }

            var noise = property.FindPropertyRelative("_noise");
            var noiseActive = noise.FindPropertyRelative("active");
            if (noiseActive.boolValue)
            {
                height += OpNoiseDrawer.GetPropertyHeight(noise);
            }

            var blur = property.FindPropertyRelative("_blur");
            var blurActive = blur.FindPropertyRelative("active");
            if (blurActive.boolValue)
            {
                height += OpBlurDrawer.GetPropertyHeight(blur);
            }

            var shape = property.FindPropertyRelative("_shape");
            var shapeActive = shape.FindPropertyRelative("active");
            if (shapeActive.boolValue)
            {
                height += OpShapeDrawer.GetPropertyHeight(shape);
            }

            var gradient = property.FindPropertyRelative("_gradient");
            var gradientActive = gradient.FindPropertyRelative("active");
            if (gradientActive.boolValue)
            {
                height += OpGradientDrawer.GetPropertyHeight(gradient);
            }

            var glow = property.FindPropertyRelative("_glow");
            var glowActive = glow.FindPropertyRelative("active");
            if (glowActive.boolValue)
            {
                height += OpGlowDrawer.GetPropertyHeight(glow);
            }

            var uvDeform = property.FindPropertyRelative("_uvDeform");
            var uvDeformActive = uvDeform.FindPropertyRelative("active");
            if (uvDeformActive.boolValue)
            {
                height += OpUvDeformDrawer.GetPropertyHeight(uvDeform);
            }

            var opacity = property.FindPropertyRelative("_opacity");
            var opacityActive = opacity.FindPropertyRelative("active");
            if (opacityActive.boolValue)
            {
                height += OpOpacityDrawer.GetPropertyHeight(opacity);
            }

            var displacement = property.FindPropertyRelative("_displacement");
            var displacementActive = displacement.FindPropertyRelative("active");
            if (displacementActive.boolValue)
            {
                height += OpDisplacementDrawer.GetPropertyHeight(displacement);
            }

            var colorBalance = property.FindPropertyRelative("_colorBalance");
            var colorBalanceActive = colorBalance.FindPropertyRelative("active");
            if (colorBalanceActive.boolValue)
            {
                height += OpColorBalanceDrawer.GetPropertyHeight(colorBalance);
            }

            var customShader = property.FindPropertyRelative("_customShader");
            var customShaderActive = customShader.FindPropertyRelative("active");
            if (customShaderActive.boolValue)
            {
                height += OpCustomShaderDrawer.GetPropertyHeight(customShader);
            }

            return height;
        }

    }
}