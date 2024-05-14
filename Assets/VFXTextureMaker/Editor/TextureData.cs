using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace VFXTextureMaker
{
    [Serializable]
    public class TextureData : ScriptableObject
    {
        [SerializeField] Texture2D _targetTexture;
        public Texture2D TargetTexture { get => _targetTexture; set => _targetTexture = value; }
        Texture2D _previewTexture;
        public Texture2D PreviewTexture => _previewTexture;

        [SerializeField] List<Layer> _layerList;
        public List<Layer> LayerList => _layerList;
        [SerializeField] bool _animated;
        [SerializeField] int _animLength;
        [SerializeField] int _currentFrame;
        [SerializeField] Vector2Int _textureSize;
        public Vector2Int TextureSize => _textureSize;

        public TextureData()
        {
            _layerList = new List<Layer>();
            _layerList.Capacity = 100;
            _textureSize = new Vector2Int(256, 256);
            _animLength = 1;
        }

        public Texture GetSingleTexture(int index)
        {
            return _layerList[index].SingleTexture;
        }

        public void SetSingleTexture(RenderTexture texture, int index)
        {
            _layerList[index].SingleTexture = texture;
        }

        public int GetMaskedLayerAtIndex(int index)
        {
            return _layerList[index].MaskedLayer;
        }

        public bool GetVisibleAtIndex(int index)
        {
            return _layerList[index].Visible;
        }

        public void SetComputeShaderProperty(ComputeShader cs, int kernel, int index)
        {
            if (_animated)
            {
                _layerList[index].SetComputeShaderPropertyAnim(cs, kernel, _currentFrame);
            }
            else
            {
                _layerList[index].SetComputeShaderProperty(cs, kernel);
            }
        }

        public void SetComputeShaderProperty(ComputeShader cs, int kernel, int renderFrame, int index)
        {
            if (_animated)
            {
                _layerList[index].SetComputeShaderPropertyAnim(cs, kernel, renderFrame);
            }
            else
            {
                _layerList[index].SetComputeShaderProperty(cs, kernel);
            }
        }

        public ShaderPass GetShaderPass(int index)
        {
            return _layerList[index].ShaderPass;
        }

        public string GetShaderPassName(int index)
        {
            return _layerList[index].ShaderPassName;
        }

        public void UpdateTexture(RenderTexture renderTexture)
        {
            var pathTex = AssetDatabase.GetAssetPath(_targetTexture);
            if (string.IsNullOrWhiteSpace(pathTex))
            {
                pathTex = EditorUtility.SaveFilePanelInProject("Save Texture", "Texture", "png", "", "Assets");
                if (string.IsNullOrWhiteSpace(pathTex)) return;
            }

            RenderTexture.active = renderTexture;
            _targetTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBAFloat, false);
            _targetTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            _targetTexture.Apply();
            RenderTexture.active = null;

            byte[] pngData = _targetTexture.EncodeToPNG();
            File.WriteAllBytes(pathTex, pngData);
            AssetDatabase.Refresh();
            DestroyImmediate(_targetTexture);
            _targetTexture = AssetDatabase.LoadAssetAtPath(pathTex, typeof(Texture2D)) as Texture2D;
        }


        public void OnDisable()
        {
            if (_previewTexture != null) DestroyImmediate(_previewTexture);
            if (_resultRT != null) _resultRT.Release();
            if (_seetRT != null) _seetRT.Release();
            if (_bufferRT != null) _bufferRT.Release();
            if (_resultRT != null) DestroyImmediate(_resultRT);
            if (_seetRT != null) DestroyImmediate(_seetRT);
            if (_bufferRT != null) DestroyImmediate(_bufferRT);
        }

        public void OnEnable()
        {
            _resultRT = new RenderTexture(_textureSize.x, _textureSize.y, 0, RenderTextureFormat.ARGBFloat);
            _resultRT.enableRandomWrite = true;
            _resultRT.Create();
            _seetRT = new RenderTexture(_textureSize.x, _textureSize.y, 0, RenderTextureFormat.ARGBFloat);
            _seetRT.enableRandomWrite = true;
            _seetRT.Create();
            _bufferRT = new RenderTexture(_textureSize.x, _textureSize.y, 0, RenderTextureFormat.ARGBFloat);

            foreach (var layer in LayerList)
            {
                layer.OnEnable();
            }
        }

        public void DestroyLayer(int index)
        {
            LayerList[index].OnDestroy();
        }

        RenderTexture _resultRT;
        RenderTexture _bufferRT;
        RenderTexture _seetRT;
        const int ThreadCount = 32;
        readonly int ResolutionID = Shader.PropertyToID("_Resolution");
        readonly int ResultID = Shader.PropertyToID("_Result");
        readonly int BufferID = Shader.PropertyToID("_Buffer");
        readonly int MaskTexID = Shader.PropertyToID("_MaskTex");
        readonly int MaskableID = Shader.PropertyToID("_Maskable");
        readonly int DrawSizeID = Shader.PropertyToID("_DrawSize");
        readonly int SeetCountID = Shader.PropertyToID("_SeetCount");
        readonly int IndexID = Shader.PropertyToID("_Index");
        readonly int DrawTexID = Shader.PropertyToID("_DrawTex");
        public void Blit(ComputeShader cs, bool convert = false)
        {
            var groupCount = new Vector2Int((int)Mathf.Ceil((float)_textureSize.x / (float)ThreadCount), (int)Mathf.Ceil((float)_textureSize.y / (float)ThreadCount));
            var cs_instance = Instantiate(cs);

            if (_resultRT.width != _textureSize.x || _resultRT.height != _textureSize.y)
            {
                _resultRT.Release();
                _resultRT = new RenderTexture(_textureSize.x, _textureSize.y, 0, RenderTextureFormat.ARGBFloat);
                _resultRT.enableRandomWrite = true;
            }

            if (_bufferRT.width != _textureSize.x || _bufferRT.height != _textureSize.y)
            {
                _bufferRT.Release();
                _bufferRT = new RenderTexture(_textureSize.x, _textureSize.y, 0, RenderTextureFormat.ARGBFloat);
            }

            cs_instance.SetVector(ResolutionID, new Vector4(_textureSize.x, _textureSize.y, 0, 0));

            cs_instance.SetTexture(cs_instance.FindKernel("Clear"), ResultID, _resultRT);
            cs_instance.Dispatch(cs_instance.FindKernel("Clear"), groupCount.x, groupCount.y, 1);
            Graphics.CopyTexture(_resultRT, 0, 0, _bufferRT, 0, 0);

            int kernelID = 0;
            for (int i = 0; i < _layerList.Count; i++)
            {
                if (GetShaderPassName(i) == ShaderPass.CustomShader.ToString())
                {
                    if (_layerList[i].CustomShader.CustomMaterial != null)
                    {
                        var customMaterial = Instantiate(_layerList[i].CustomShader.CustomMaterial);
                        if (GetMaskedLayerAtIndex(i) != -1)
                        {
                            customMaterial.SetTexture(MaskTexID, GetSingleTexture(GetMaskedLayerAtIndex(i)));
                        }
                        else
                        {
                            customMaterial.SetTexture(MaskTexID, Texture2D.whiteTexture);
                        }
                        Graphics.Blit(_bufferRT, _resultRT, customMaterial, 0);
                        DestroyImmediate(customMaterial);
                    }
                    else
                    {
                        Graphics.Blit(_bufferRT, _resultRT);
                    }

                }
                else
                {
                    kernelID = cs_instance.FindKernel(GetShaderPassName(i));
                    cs_instance.SetTexture(kernelID, ResultID, _resultRT);
                    cs_instance.SetTexture(kernelID, BufferID, _bufferRT);

                    if (GetMaskedLayerAtIndex(i) != -1)
                    {
                        cs_instance.SetTexture(kernelID, MaskTexID, GetSingleTexture(GetMaskedLayerAtIndex(i)));
                        cs_instance.SetBool(MaskableID, true);
                    }
                    else
                    {
                        cs_instance.SetTexture(kernelID, MaskTexID, Texture2D.whiteTexture);
                        cs_instance.SetBool(MaskableID, false);
                    }

                    SetComputeShaderProperty(cs_instance, kernelID, i);

                    cs_instance.Dispatch(kernelID, groupCount.x, groupCount.y, 1);
                }

                SetSingleTexture(_resultRT, i);

                if (!GetVisibleAtIndex(i)) continue;

                Graphics.CopyTexture(_resultRT, 0, 0, _bufferRT, 0, 0);
            }

            if (!_previewTexture)
            {
                _previewTexture = new Texture2D(_textureSize.x, _textureSize.y, TextureFormat.RGBAFloat, false);
            }
            else if (_previewTexture.width != _textureSize.x || _previewTexture.height != _textureSize.y)
            {
                DestroyImmediate(_previewTexture);
                _previewTexture = new Texture2D(_textureSize.x, _textureSize.y, TextureFormat.RGBAFloat, false);
            }
            RenderTexture.active = _bufferRT;
            _previewTexture.ReadPixels(new Rect(0, 0, _textureSize.x, _textureSize.y), 0, 0);
            _previewTexture.Apply();
            RenderTexture.active = null;

            if (convert) UpdateTexture(_bufferRT);

            DestroyImmediate(cs_instance);
        }

        public void AnimBlit(ComputeShader cs, ComputeShader seetCs, bool convert = false)
        {
            var groupCount = new Vector2Int((int)Mathf.Ceil((float)_textureSize.x / (float)ThreadCount), (int)Mathf.Ceil((float)_textureSize.y / (float)ThreadCount));
            var cs_instance = Instantiate(cs);

            if (_resultRT.width != _textureSize.x || _resultRT.height != _textureSize.y)
            {
                _resultRT.Release();
                _resultRT = new RenderTexture(_textureSize.x, _textureSize.y, 0, RenderTextureFormat.ARGBFloat);
                _resultRT.enableRandomWrite = true;
            }

            if (_bufferRT.width != _textureSize.x || _bufferRT.height != _textureSize.y)
            {
                _bufferRT.Release();
                _bufferRT = new RenderTexture(_textureSize.x, _textureSize.y, 0, RenderTextureFormat.ARGBFloat);
            }

            cs_instance.SetVector(ResolutionID, new Vector4(_textureSize.x, _textureSize.y, 0, 0));

            var width = (int)Mathf.Ceil(Mathf.Sqrt(_animLength));
            var height = (int)Mathf.Ceil((float)_animLength / (float)width);
            var seetSize = new Vector2Int(_textureSize.x * width, _textureSize.y * height);
            var seetGroupCount = new Vector2Int((int)Mathf.Ceil((float)seetSize.x / (float)ThreadCount), (int)Mathf.Ceil((float)seetSize.y / (float)ThreadCount));
            if (_seetRT.width != seetSize.x || _seetRT.height != seetSize.y)
            {
                _seetRT.Release();
                _seetRT = new RenderTexture(seetSize.x, seetSize.y, 0, RenderTextureFormat.ARGBFloat);
                _seetRT.enableRandomWrite = true;
            }

            var seetcs_instance = Instantiate(seetCs);
            var SeetKernelID = seetcs_instance.FindKernel("CsMain");
            seetcs_instance.SetTexture(SeetKernelID, ResultID, _seetRT);
            seetcs_instance.SetTexture(SeetKernelID, DrawTexID, _bufferRT);
            seetcs_instance.SetVector(ResolutionID, new Vector4(seetSize.x, seetSize.y, 0, 0));
            seetcs_instance.SetVector(DrawSizeID, new Vector4(_textureSize.x, _textureSize.y, 0, 0));
            seetcs_instance.SetVector(SeetCountID, new Vector4(width, height, 0, 0));

            cs_instance.SetTexture(cs_instance.FindKernel("Clear"), ResultID, _resultRT);
            

            seetcs_instance.SetTexture(seetcs_instance.FindKernel("Clear"), ResultID, _seetRT);
            seetcs_instance.Dispatch(seetcs_instance.FindKernel("Clear"), seetGroupCount.x, seetGroupCount.y, 1);

            int kernelID = 0;
            for (int i = 0; i < _animLength; i++)
            {
                cs_instance.Dispatch(cs_instance.FindKernel("Clear"), groupCount.x, groupCount.y, 1);
                Graphics.CopyTexture(_resultRT, 0, 0, _bufferRT, 0, 0);
                for (int j = 0; j < _layerList.Count; j++)
                {
                    if (GetShaderPassName(j) == ShaderPass.CustomShader.ToString())
                    {
                        if (_layerList[j].CustomShader.CustomMaterial != null)
                        {
                            var customMaterial = Instantiate(_layerList[j].CustomShader.CustomMaterial);
                            if (GetMaskedLayerAtIndex(j) != -1)
                            {
                                customMaterial.SetTexture(MaskTexID, GetSingleTexture(GetMaskedLayerAtIndex(j)));
                            }
                            else
                            {
                                customMaterial.SetTexture(MaskTexID, Texture2D.whiteTexture);
                            }
                            Graphics.Blit(_bufferRT, _resultRT, customMaterial, 0);
                            DestroyImmediate(customMaterial);
                        }
                        else
                        {
                            Graphics.Blit(_bufferRT, _resultRT);
                        }
                    }
                    else
                    {
                        kernelID = cs_instance.FindKernel(GetShaderPassName(j));
                        cs_instance.SetTexture(kernelID, ResultID, _resultRT);
                        cs_instance.SetTexture(kernelID, BufferID, _bufferRT);

                        if (GetMaskedLayerAtIndex(j) != -1)
                        {
                            cs_instance.SetTexture(kernelID, MaskTexID, GetSingleTexture(GetMaskedLayerAtIndex(j)));
                            cs_instance.SetBool(MaskableID, true);
                        }
                        else
                        {
                            cs_instance.SetTexture(kernelID, MaskTexID, Texture2D.whiteTexture);
                            cs_instance.SetBool(MaskableID, false);
                        }

                        SetComputeShaderProperty(cs_instance, kernelID, i, j);

                        cs_instance.Dispatch(kernelID, groupCount.x, groupCount.y, 1);
                    }
                    SetSingleTexture(_resultRT, j);

                    if (!GetVisibleAtIndex(j)) continue;

                    Graphics.CopyTexture(_resultRT, 0, 0, _bufferRT, 0, 0);
                }
                seetcs_instance.SetInt(IndexID, i + 1);
                seetcs_instance.Dispatch(SeetKernelID, groupCount.x, groupCount.y, 1);
            }

            RenderTexture.active = _seetRT;
            if (_previewTexture.width != seetSize.x || _previewTexture.height != seetSize.y)
            {
                _previewTexture = new Texture2D(seetSize.x, seetSize.y, TextureFormat.RGBAFloat, false);
            }
            _previewTexture.ReadPixels(new Rect(0, 0, seetSize.x, seetSize.y), 0, 0);
            _previewTexture.Apply();
            RenderTexture.active = null;

            if (convert) UpdateTexture(_seetRT);

            DestroyImmediate(cs_instance);
            DestroyImmediate(seetcs_instance);
        }
    }
}