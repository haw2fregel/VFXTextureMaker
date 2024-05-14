using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class OpColorBalance : LayerOption
    {
        [SerializeField] FloatAnimProperty _temperature;
        [SerializeField] FloatAnimProperty _tint;
        [SerializeField] FloatAnimProperty _shiftHue;
        [SerializeField] FloatAnimProperty _shiftSaturation;
        [SerializeField] FloatAnimProperty _shiftValue;

        public OpColorBalance()
        {
            _temperature = new FloatAnimProperty("_Temperature", 0);
            _tint = new FloatAnimProperty("_Tint", 0);
            _shiftHue = new FloatAnimProperty("_ShiftHue", 0);
            _shiftSaturation = new FloatAnimProperty("_ShiftSaturation", 0);
            _shiftValue = new FloatAnimProperty("_ShiftValue", 0);
        }

        public override void SetComputeShaderProperty(ComputeShader cs, int kernel)
        {
            cs.SetFloat(_temperature.ID, _temperature.Value);
            cs.SetFloat(_tint.ID, _tint.Value);
            cs.SetFloat(_shiftHue.ID, _shiftHue.Value);
            cs.SetFloat(_shiftSaturation.ID, _shiftSaturation.Value);
            cs.SetFloat(_shiftValue.ID, _shiftValue.Value);
        }

        public override void SetComputeShaderPropertyAnim(ComputeShader cs, int kernel, int currentFrame)
        {
            if (_temperature.IsAnim)
            {
                cs.SetFloat(_temperature.ID, _temperature.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_temperature.ID, _temperature.Value);
            }

            if (_tint.IsAnim)
            {
                cs.SetFloat(_tint.ID, _tint.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_tint.ID, _tint.Value);
            }
            
            if (_shiftHue.IsAnim)
            {
                cs.SetFloat(_shiftHue.ID, _shiftHue.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_shiftHue.ID, _shiftHue.Value);
            }

            if (_shiftSaturation.IsAnim)
            {
                cs.SetFloat(_shiftSaturation.ID, _shiftSaturation.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_shiftSaturation.ID, _shiftSaturation.Value);
            }

            if (_shiftValue.IsAnim)
            {
                cs.SetFloat(_shiftValue.ID, _shiftValue.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_shiftValue.ID, _shiftValue.Value);
            }
        }
    }

}
