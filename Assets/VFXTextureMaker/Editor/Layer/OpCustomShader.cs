using System;
using UnityEngine;
using UnityEditor;

namespace VFXTextureMaker
{
    [Serializable]
    public class OpCustomShader : LayerOption
    {
        [SerializeField] Shader _customShader;
        [SerializeField] Material _customMaterial;
        public Material CustomMaterial => _customMaterial;

        public OpCustomShader()
        {
        }
        public override void SetComputeShaderProperty(ComputeShader cs, int kernel)
        {

        }
        public override void SetComputeShaderPropertyAnim(ComputeShader cs, int kernel, int currentFrame)
        {
        }

        public void OnDestroy()
        {
            if(_customMaterial != null) UnityEngine.Object.DestroyImmediate(_customMaterial, true);
            AssetDatabase.RenameAsset(TextureDataEditor.TextureDataPath, TextureDataEditor.TextureDataPath);
        }
    }
}
