using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class LayerOption
    {
        [SerializeField] protected bool showOption;
        [SerializeField] bool active;
        public bool Active { get => active; set => active = value; }
        public virtual void SetComputeShaderProperty(ComputeShader cs, int kernel) { }
        public virtual void SetComputeShaderPropertyAnim(ComputeShader cs, int kernel, int currentFrame) { }
    }
}