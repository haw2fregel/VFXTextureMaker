using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class OpTexture : LayerOption
    {
        [SerializeField] Texture2D _drawTex;
        [SerializeField] TextureFilterMode _textureFilterMode;

        static readonly int DrawTexId = Shader.PropertyToID("_DrawTex");
        static readonly int DrawTexSizeId = Shader.PropertyToID("_DrawTexSize");
        static readonly int TextureFilterModeId = Shader.PropertyToID("_TextureFilterMode");

        public OpTexture()
        {
            _textureFilterMode = TextureFilterMode.SmoothFilter;
        }
        public override void SetComputeShaderProperty(ComputeShader cs, int kernel)
        {
            if (_drawTex == null)
            {
                var tex = Texture2D.whiteTexture;
                cs.SetTexture(kernel, DrawTexId, tex);
                cs.SetVector(DrawTexSizeId, new Vector4(1, 1, 0, 0));
                cs.SetInt(TextureFilterModeId, 0);
            }
            else
            {
                cs.SetTexture(kernel, DrawTexId, _drawTex);
                cs.SetVector(DrawTexSizeId, new Vector4(_drawTex.width, _drawTex.height, 0, 0));
                cs.SetInt(TextureFilterModeId, (int)_textureFilterMode);
            }
        }

        public override void SetComputeShaderPropertyAnim(ComputeShader cs, int kernel, int currentFrame)
        {
            SetComputeShaderProperty(cs, kernel);
        }

        [Serializable]
        public enum TextureFilterMode
        {
            NonFilter,
            SmoothFilter
        }
    }
}