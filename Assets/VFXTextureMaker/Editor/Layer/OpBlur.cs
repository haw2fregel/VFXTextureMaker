using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class OpBlur : LayerOption
    {
        [SerializeField] BlurType _blurType;
        public string BlurTypeName { get => _blurType.ToString(); }
        [SerializeField] IntAnimProperty _blurSampleCount;
        [SerializeField] IntAnimProperty _blurSize;
        [SerializeField] FloatAnimProperty _blurGausianSigma;
        [SerializeField] Vector2AnimProperty _blurDirection;
        [SerializeField] Vector2AnimProperty _blurCenter;
        [SerializeField] BoolAnimProperty _blurRepeat;

        public OpBlur()
        {
            _blurType = BlurType.Gausian;
            _blurSampleCount = new IntAnimProperty("_BlurSampleCount", 7);
            _blurSize = new IntAnimProperty("_BlurSize", 1);
            _blurGausianSigma = new FloatAnimProperty("_BlurGausianSigma", 3);
            _blurDirection = new Vector2AnimProperty("_BlurDirection", new Vector2(1, 1));
            _blurCenter = new Vector2AnimProperty("_BlurCenter", new Vector2(0.5f, 0.5f));
            _blurRepeat = new BoolAnimProperty("_BlurRepeat", false);
        }

        public override void SetComputeShaderProperty(ComputeShader cs, int kernel)
        {
            cs.SetFloat(_blurSampleCount.ID, _blurSampleCount.Value);
            cs.SetFloat(_blurSize.ID, _blurSize.Value);
            cs.SetFloat(_blurGausianSigma.ID, _blurGausianSigma.Value);
            cs.SetVector(_blurDirection.ID, new Vector4(_blurDirection.Value.x, _blurDirection.Value.y, 0, 0));
            cs.SetVector(_blurCenter.ID, new Vector4(_blurCenter.Value.x, _blurCenter.Value.y, 0, 0));
            cs.SetBool(_blurRepeat.ID, _blurRepeat.Value);
        }

        public override void SetComputeShaderPropertyAnim(ComputeShader cs, int kernel, int currentFrame)
        {
            if (_blurSampleCount.IsAnim)
            {
                cs.SetFloat(_blurSampleCount.ID, (int)_blurSampleCount.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_blurSampleCount.ID, _blurSampleCount.Value);
            }

            if (_blurSize.IsAnim)
            {
                cs.SetFloat(_blurSize.ID, (int)_blurSize.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_blurSize.ID, _blurSize.Value);
            }

            if (_blurGausianSigma.IsAnim)
            {
                cs.SetFloat(_blurGausianSigma.ID, _blurGausianSigma.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_blurGausianSigma.ID, _blurGausianSigma.Value);
            }

            if (_blurDirection.IsAnim)
            {
                var valueX = _blurDirection.CurveX.Evaluate(currentFrame);
                var valueY = _blurDirection.CurveY.Evaluate(currentFrame);
                cs.SetVector(_blurDirection.ID, new Vector4(valueX, valueY, 0, 0));
            }
            else
            {
                cs.SetVector(_blurDirection.ID, new Vector4(_blurDirection.Value.x, _blurDirection.Value.y, 0, 0));
            }

            if (_blurCenter.IsAnim)
            {
                var valueX = _blurCenter.CurveX.Evaluate(currentFrame);
                var valueY = _blurCenter.CurveY.Evaluate(currentFrame);
                cs.SetVector(_blurCenter.ID, new Vector4(valueX, valueY, 0, 0));
            }
            else
            {
                cs.SetVector(_blurCenter.ID, new Vector4(_blurCenter.Value.x, _blurCenter.Value.y, 0, 0));
            }

            if (_blurRepeat.IsAnim)
            {
                cs.SetBool(_blurRepeat.ID, _blurRepeat.Curve.Evaluate(currentFrame) >= 1);
            }
            else
            {
                cs.SetBool(_blurRepeat.ID, _blurRepeat.Value);
            }
        }

        [Serializable]
        public enum BlurType
        {
            Gausian,
            Direction,
            Zoom,
            Rotate
        }

    }
}