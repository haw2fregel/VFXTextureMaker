using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class OpDisplacement : LayerOption
    {
        [SerializeField] Vector2 _displacementChannel;
        [SerializeField] FloatAnimProperty _displacementStrength;
        [SerializeField] Vector4AnimProperty _displacementWeight;
        [SerializeField] BoolAnimProperty _displacementRepeat;
        readonly int DisplacementChannelID = Shader.PropertyToID("_DisplacemantChannel");

        public OpDisplacement()
        {
            _displacementChannel = new Vector2(0, 1);
            _displacementStrength = new FloatAnimProperty("_DisplacemantStrength", 1);
            _displacementWeight = new Vector4AnimProperty("_DisplacementWeight", new Vector4(1, 1, 1, 1));
            _displacementRepeat = new BoolAnimProperty("_DisplacementRepeat", false);
        }

        public override void SetComputeShaderProperty(ComputeShader cs, int kernel)
        {
            cs.SetVector(DisplacementChannelID, new Vector4(_displacementChannel.x, _displacementChannel.y, 0, 0));
            cs.SetFloat(_displacementStrength.ID, _displacementStrength.Value);
            cs.SetVector(_displacementWeight.ID, _displacementWeight.Value);
            cs.SetBool(_displacementRepeat.ID, _displacementRepeat.Value);
        }

        public override void SetComputeShaderPropertyAnim(ComputeShader cs, int kernel, int currentFrame)
        {
            cs.SetVector(DisplacementChannelID, _displacementChannel);

            if (_displacementStrength.IsAnim)
            {
                cs.SetFloat(_displacementStrength.ID, _displacementStrength.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_displacementStrength.ID, _displacementStrength.Value);
            }

            if (_displacementWeight.IsAnim)
            {
                var valueX = _displacementWeight.CurveX.Evaluate(currentFrame);
                var valueY = _displacementWeight.CurveY.Evaluate(currentFrame);
                var valueZ = _displacementWeight.CurveZ.Evaluate(currentFrame);
                var valueW = _displacementWeight.CurveW.Evaluate(currentFrame);
                cs.SetVector(_displacementWeight.ID, new Vector4(valueX, valueY, valueZ, valueW));
            }
            else
            {
                cs.SetVector(_displacementWeight.ID, _displacementWeight.Value);
            }

            if (_displacementRepeat.IsAnim)
            {
                cs.SetBool(_displacementRepeat.ID, _displacementRepeat.Curve.Evaluate(currentFrame) >= 1);
            }
            else
            {
                cs.SetBool(_displacementRepeat.ID, _displacementRepeat.Value);
            }
        }
    }

}
