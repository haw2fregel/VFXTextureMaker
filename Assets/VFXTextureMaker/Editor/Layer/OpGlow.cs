using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class OpGlow : LayerOption
    {
        [SerializeField] IntAnimProperty _glowIteration;
        [SerializeField] FloatAnimProperty _glowSize;
        [SerializeField] FloatAnimProperty _glowThreshold;
        [SerializeField] FloatAnimProperty _glowThresholdSmooth;
        [SerializeField] FloatAnimProperty _glowIntensity;
        [SerializeField] Color _glowColor;
        readonly int GlowColorID = Shader.PropertyToID("_GlowColor");

        public OpGlow()
        {
            _glowIteration = new IntAnimProperty("_GlowIteration", 5);
            _glowSize = new FloatAnimProperty("_GlowSize", 1.0f);
            _glowThreshold = new FloatAnimProperty("_GlowThreshold", 0.5f);
            _glowThresholdSmooth = new FloatAnimProperty("_GlowThresholdSmooth", 0.1f);
            _glowIntensity = new FloatAnimProperty("_GlowIntensity", 1.0f);
            _glowColor = new Color(1, 1, 1, 1);
        }

        public override void SetComputeShaderProperty(ComputeShader cs, int kernel)
        {
            cs.SetInt(_glowIteration.ID, _glowIteration.Value);
            cs.SetFloat(_glowSize.ID, _glowSize.Value);
            cs.SetFloat(_glowThreshold.ID, _glowThreshold.Value);
            cs.SetFloat(_glowThresholdSmooth.ID, _glowThresholdSmooth.Value);
            cs.SetFloat(_glowIntensity.ID, _glowIntensity.Value);
            cs.SetVector(GlowColorID, _glowColor);
        }

        public override void SetComputeShaderPropertyAnim(ComputeShader cs, int kernel, int currentFrame)
        {
            if (_glowIteration.IsAnim)
            {
                cs.SetInt(_glowIteration.ID, (int)_glowIteration.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetInt(_glowIteration.ID, _glowIteration.Value);
            }

            if (_glowSize.IsAnim)
            {
                cs.SetFloat(_glowSize.ID, _glowSize.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_glowSize.ID, _glowSize.Value);
            }

            if (_glowThreshold.IsAnim)
            {
                cs.SetFloat(_glowThreshold.ID, _glowThreshold.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_glowThreshold.ID, _glowThreshold.Value);
            }

            if (_glowThresholdSmooth.IsAnim)
            {
                cs.SetFloat(_glowThresholdSmooth.ID, _glowThresholdSmooth.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_glowThresholdSmooth.ID, _glowThresholdSmooth.Value);
            }

            if (_glowIntensity.IsAnim)
            {
                cs.SetFloat(_glowIntensity.ID, _glowIntensity.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_glowIntensity.ID, _glowIntensity.Value);
            }

            cs.SetVector(GlowColorID, _glowColor);
        }
    }
}