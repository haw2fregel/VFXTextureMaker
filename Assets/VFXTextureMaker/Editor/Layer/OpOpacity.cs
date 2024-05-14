using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class OpOpacity : LayerOption
    {
        [SerializeField] Blend _blend;
        [SerializeField] Vector4 _drawChannel;
        [SerializeField] FloatAnimProperty _opacity;
        [SerializeField] FloatAnimProperty _opacityPower;
        [SerializeField] BoolAnimProperty _isMultiplyAlpha;
        [SerializeField] BoolAnimProperty _isOneMinus;
        [SerializeField] FloatAnimProperty _contrast;

        readonly int BlendID = Shader.PropertyToID("_Blend");
        readonly int DrawChannelID = Shader.PropertyToID("_DrawChannel");

        public OpOpacity()
        {
            _blend = Blend.Overwrite;
            _drawChannel = new Vector4(0, 1, 2, 3);
            _opacity = new FloatAnimProperty("_Opacity", 1);
            _opacityPower = new FloatAnimProperty("_OpacityPower", 1);
            _isMultiplyAlpha = new BoolAnimProperty("_IsMultiplyAlpha", false);
            _isOneMinus = new BoolAnimProperty("_IsOneMinus", false);
            _contrast = new FloatAnimProperty("_Contrast", 1);
        }

        public override void SetComputeShaderProperty(ComputeShader cs, int kernel)
        {
            cs.SetInt(BlendID, (int)_blend);
            cs.SetVector(DrawChannelID, _drawChannel);
            cs.SetFloat(_opacity.ID, _opacity.Value);
            cs.SetFloat(_opacityPower.ID, _opacityPower.Value);
            cs.SetBool(_isMultiplyAlpha.ID, _isMultiplyAlpha.Value);
            cs.SetBool(_isOneMinus.ID, _isOneMinus.Value);
            cs.SetFloat(_contrast.ID, _contrast.Value);
        }

        public override void SetComputeShaderPropertyAnim(ComputeShader cs, int kernel, int currentFrame)
        {
            cs.SetInt(BlendID, (int)_blend);
            cs.SetVector(DrawChannelID, _drawChannel);

            if (_opacity.IsAnim)
            {
                cs.SetFloat(_opacity.ID, _opacity.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_opacity.ID, _opacity.Value);
            }

            if (_opacityPower.IsAnim)
            {
                cs.SetFloat(_opacityPower.ID, _opacityPower.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_opacityPower.ID, _opacityPower.Value);
            }

            if (_isMultiplyAlpha.IsAnim)
            {
                cs.SetBool(_isMultiplyAlpha.ID, _isMultiplyAlpha.Curve.Evaluate(currentFrame) >= 1);
            }
            else
            {
                cs.SetBool(_isMultiplyAlpha.ID, _isMultiplyAlpha.Value);
            }

            if (_isOneMinus.IsAnim)
            {
                cs.SetBool(_isOneMinus.ID, _isOneMinus.Curve.Evaluate(currentFrame) >= 1);
            }
            else
            {
                cs.SetBool(_isOneMinus.ID, _isOneMinus.Value);
            }

            if (_contrast.IsAnim)
            {
                cs.SetFloat(_contrast.ID, _contrast.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_contrast.ID, _contrast.Value);
            }
        }
    }

    [Serializable]
    public enum Blend
    {
        Overwrite,
        Add,
        Subtruct,
        Multiple,
        Divid,
        Max,
        Min,
        Screen,
        Overlay,
        Difference
    }
}
