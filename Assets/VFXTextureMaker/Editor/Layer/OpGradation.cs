using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class OpGradation : LayerOption
    {
        [SerializeField] GradationReference _gradationReference;
        readonly int GradationReferenceID = Shader.PropertyToID("_GradationReference");
        [SerializeField] GradientProperty _gradient;

        public OpGradation()
        {
            _gradationReference = GradationReference.Luminance;
            _gradient = new GradientProperty("_GradientArray", new Gradient());
        }
        public override void SetComputeShaderProperty(ComputeShader cs, int kernel)
        {
            cs.SetInt(GradationReferenceID, (int)_gradationReference);
            cs.SetVectorArray(_gradient.ID, _gradient.Array);
        }
        public override void SetComputeShaderPropertyAnim(ComputeShader cs, int kernel, int currentFrame)
        {
            cs.SetInt(GradationReferenceID, (int)_gradationReference);
            cs.SetVectorArray(_gradient.ID, _gradient.Array);
        }
    }

    [Serializable]
    public enum GradationReference
    {
        R,
        G,
        B,
        A,
        Luminance,
        U,
        V
    }
}
