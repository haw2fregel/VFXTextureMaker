using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class OpDraw : LayerOption
    {
        [SerializeField] Color _drawColor;
        readonly int DrawColorID = Shader.PropertyToID("_DrawColor");

        public OpDraw()
        {
            _drawColor = new Color(1, 1, 1, 1);
        }

        public override void SetComputeShaderProperty(ComputeShader cs, int kernel)
        {
            cs.SetVector(DrawColorID, _drawColor);
        }

        public override void SetComputeShaderPropertyAnim(ComputeShader cs, int kernel, int currentFrame)
        {
            cs.SetVector(DrawColorID, _drawColor);
        }
    }
}