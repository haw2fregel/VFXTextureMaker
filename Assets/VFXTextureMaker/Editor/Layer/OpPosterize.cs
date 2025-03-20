using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class OpPosterize : LayerOption
    {
        [SerializeField] IntAnimProperty _steps;

        public OpPosterize()
        {
            _steps = new IntAnimProperty("_PosterizeSteps", 3);
        }
        public override void SetComputeShaderProperty(ComputeShader cs, int kernel)
        {
            cs.SetInt(_steps.ID, _steps.Value);
        }
        public override void SetComputeShaderPropertyAnim(ComputeShader cs, int kernel, int currentFrame)
        {
            if (_steps.IsAnim)
            {
                cs.SetInt(_steps.ID, (int)_steps.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetInt(_steps.ID, _steps.Value);
            }

        }
    }
}