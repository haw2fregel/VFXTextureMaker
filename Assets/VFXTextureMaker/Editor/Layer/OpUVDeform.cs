using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class OpUVDeform : LayerOption
    {
        [SerializeField] Vector2AnimProperty _uvPivot;
        [SerializeField] Vector2AnimProperty _uvScale;
        [SerializeField] FloatAnimProperty _uvRotate;
        [SerializeField] Vector2AnimProperty _uvBend;
        [SerializeField] BoolAnimProperty _uvPolar;

        public OpUVDeform()
        {
            _uvPivot = new Vector2AnimProperty("_UVPivot", new Vector2(0.5f, 0.5f));
            _uvScale = new Vector2AnimProperty("_UVScale", new Vector2(1, 1));
            _uvRotate = new FloatAnimProperty("_UVRotate", 0);
            _uvBend = new Vector2AnimProperty("_UVBend", new Vector2(0, 0));
            _uvPolar = new BoolAnimProperty("_UVPolar", false);
        }
        public override void SetComputeShaderProperty(ComputeShader cs, int kernel)
        {
            cs.SetVector(_uvPivot.ID, new Vector4(_uvPivot.Value.x, _uvPivot.Value.y, 0, 0));
            cs.SetVector(_uvScale.ID, new Vector4(_uvScale.Value.x, _uvScale.Value.y, 0, 0));
            cs.SetFloat(_uvRotate.ID, _uvRotate.Value);
            cs.SetVector(_uvBend.ID, new Vector4(_uvBend.Value.x, _uvBend.Value.y, 0, 0));
            cs.SetInt(_uvPolar.ID, _uvPolar.Value ? 1 : 0);
        }
        public override void SetComputeShaderPropertyAnim(ComputeShader cs, int kernel, int currentFrame)
        {
            if (_uvPivot.IsAnim)
            {
                var valueX = _uvPivot.CurveX.Evaluate(currentFrame);
                var valueY = _uvPivot.CurveY.Evaluate(currentFrame);
                cs.SetVector(_uvPivot.ID, new Vector4(valueX, valueY, 0, 0));
            }
            else
            {
                cs.SetVector(_uvPivot.ID, new Vector4(_uvPivot.Value.x, _uvPivot.Value.y, 0, 0));
            }

            if (_uvScale.IsAnim)
            {
                var valueX = _uvScale.CurveX.Evaluate(currentFrame);
                var valueY = _uvScale.CurveY.Evaluate(currentFrame);
                cs.SetVector(_uvScale.ID, new Vector4(valueX, valueY, 0, 0));
            }
            else
            {
                cs.SetVector(_uvScale.ID, new Vector4(_uvScale.Value.x, _uvScale.Value.y, 0, 0));
            }

            if (_uvRotate.IsAnim)
            {
                cs.SetFloat(_uvRotate.ID, _uvRotate.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_uvRotate.ID, _uvRotate.Value);
            }

            if (_uvBend.IsAnim)
            {
                var valueX = _uvBend.CurveX.Evaluate(currentFrame);
                var valueY = _uvBend.CurveY.Evaluate(currentFrame);
                cs.SetVector(_uvBend.ID, new Vector4(valueX, valueY, 0, 0));
            }
            else
            {
                cs.SetVector(_uvBend.ID, new Vector4(_uvBend.Value.x, _uvBend.Value.y, 0, 0));
            }

            if (_uvPolar.IsAnim)
            {
                cs.SetInt(_uvPolar.ID, (int)_uvPolar.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetInt(_uvPolar.ID, _uvPolar.Value ? 1 : 0);
            }
        }
    }
}