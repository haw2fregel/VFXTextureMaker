using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class Layer
    {
        [SerializeField] ShaderPass _shaderPass;
        public ShaderPass ShaderPass => _shaderPass;
        public string ShaderPassName
        {
            get
            {
                if (_shaderPass == ShaderPass.Draw_Noise || _shaderPass == ShaderPass.Displacement_Noise)
                {
                    return _shaderPass.ToString() + "_" + _noise.NoiseTypeName;
                }
                else if (_shaderPass == ShaderPass.Filter_Blur)
                {
                    return _shaderPass.ToString() + "_" + _blur.BlurTypeName;
                }
                else if (_shaderPass == ShaderPass.Draw_Shape)
                {
                    return _shaderPass.ToString() + "_" + _shape.ShapeTypeName;
                }
                return _shaderPass.ToString();
            }
        }

        RenderTexture _singleTexture;
        public RenderTexture SingleTexture
        {
            get
            {
                return _singleTexture;
            }
            set
            {
                if (_singleTexture == null)
                {
                    _singleTexture = new RenderTexture(value.width, value.height, 0, RenderTextureFormat.ARGBFloat);
                    _singleTexture.Create();
                }
                else if (value.width != _singleTexture.width || value.height != _singleTexture.height)
                {
                    _singleTexture.Release();
                    _singleTexture = new RenderTexture(value.width, value.height, 0, RenderTextureFormat.ARGBFloat);
                }

                Graphics.CopyTexture(value, 0, 0, _singleTexture, 0, 0);
            }
        }

        [SerializeField] bool _animated;
        [SerializeField] int _currentFrame;
        [SerializeField] int _index;
        [SerializeField] bool _visible;
        public bool Visible => _visible;
        [SerializeField] bool _rocked;
        public bool Rocked => _rocked;
        [SerializeField] bool _masked;
        public bool Masked => _masked;
        [SerializeField] int _maskedLayer;
        public int MaskedLayer => _maskedLayer;

        [SerializeField] OpDraw _draw;
        [SerializeField] OpTexture _texture;
        [SerializeField] OpOpacity _opacity;
        [SerializeField] OpDisplacement _displacement;
        [SerializeField] OpUVDeform _uvDeform;
        [SerializeField] OpNoise _noise;
        [SerializeField] OpBlur _blur;
        [SerializeField] OpShape _shape;
        [SerializeField] OpGradation _gradient;
        [SerializeField] OpGlow _glow;
        [SerializeField] OpColorBalance _colorBalance;
        [SerializeField] OpCustomShader _customShader;
        public OpCustomShader CustomShader => _customShader;

        public Layer()
        {
        }
        public Layer(int num, ShaderPass pass)
        {
            _index = num;

            _draw = new OpDraw();
            _opacity = new OpOpacity();
            _noise = new OpNoise();
            _texture = new OpTexture();
            _uvDeform = new OpUVDeform();
            _displacement = new OpDisplacement();
            _blur = new OpBlur();
            _shape = new OpShape();
            _gradient = new OpGradation();
            _glow = new OpGlow();
            _colorBalance ??= new OpColorBalance();
            _customShader ??= new OpCustomShader();

            _masked = false;
            _rocked = false;
            _visible = true;
            _maskedLayer = -1;

            SetPass(pass);
        }

        //アプデでオプション追加すると既存データで使えないので、読み込み時にNullチェックする
        public void OnEnable()
        {
            _draw ??= new OpDraw();
            _opacity ??= new OpOpacity();
            _noise ??= new OpNoise();
            _texture ??= new OpTexture();
            _uvDeform ??= new OpUVDeform();
            _displacement ??= new OpDisplacement();
            _blur ??= new OpBlur();
            _shape ??= new OpShape();
            _gradient ??= new OpGradation();
            _glow ??= new OpGlow();
            _colorBalance ??= new OpColorBalance();
            _customShader ??= new OpCustomShader();
            SetPass(_shaderPass);
        }

        public void OnDisable()
        {
            _singleTexture.Release();
            UnityEngine.Object.DestroyImmediate(_singleTexture);
        }

        public void OnDestroy()
        {
            _customShader.OnDestroy();
        }

        public void SetPass(ShaderPass pass)
        {
            _shaderPass = pass;

            switch (pass)
            {
                case ShaderPass.Clear:
                    _draw.Active = false;
                    _opacity.Active = false;
                    _displacement.Active = false;
                    _noise.Active = false;
                    _texture.Active = false;
                    _uvDeform.Active = false;
                    _blur.Active = false;
                    _shape.Active = false;
                    _gradient.Active = false;
                    _glow.Active = false;
                    _colorBalance.Active = false;
                    _customShader.Active = false;
                    break;
                case ShaderPass.Draw_Texture:
                    _draw.Active = true;
                    _opacity.Active = true;
                    _displacement.Active = false;
                    _noise.Active = false;
                    _texture.Active = true;
                    _uvDeform.Active = true;
                    _blur.Active = false;
                    _shape.Active = false;
                    _gradient.Active = false;
                    _glow.Active = false;
                    _colorBalance.Active = false;
                    _customShader.Active = false;
                    break;
                case ShaderPass.Displacement_Texture:
                    _draw.Active = true;
                    _opacity.Active = false;
                    _displacement.Active = true;
                    _noise.Active = false;
                    _texture.Active = true;
                    _uvDeform.Active = true;
                    _blur.Active = false;
                    _shape.Active = false;
                    _gradient.Active = false;
                    _glow.Active = false;
                    _colorBalance.Active = false;
                    _customShader.Active = false;
                    break;
                case ShaderPass.Displacement_NormalMap:
                    _draw.Active = false;
                    _opacity.Active = false;
                    _displacement.Active = true;
                    _noise.Active = false;
                    _texture.Active = true;
                    _uvDeform.Active = true;
                    _blur.Active = false;
                    _shape.Active = false;
                    _gradient.Active = false;
                    _glow.Active = false;
                    _colorBalance.Active = false;
                    _customShader.Active = false;
                    break;
                case ShaderPass.Draw_Noise:
                    _draw.Active = true;
                    _opacity.Active = true;
                    _displacement.Active = false;
                    _noise.Active = true;
                    _texture.Active = false;
                    _uvDeform.Active = true;
                    _blur.Active = false;
                    _shape.Active = false;
                    _gradient.Active = false;
                    _glow.Active = false;
                    _colorBalance.Active = false;
                    _customShader.Active = false;
                    break;
                case ShaderPass.Displacement_Noise:
                    _draw.Active = false;
                    _opacity.Active = false;
                    _displacement.Active = true;
                    _noise.Active = true;
                    _texture.Active = false;
                    _uvDeform.Active = true;
                    _blur.Active = false;
                    _shape.Active = false;
                    _gradient.Active = false;
                    _glow.Active = false;
                    _colorBalance.Active = false;
                    _customShader.Active = false;
                    break;
                case ShaderPass.Draw_Shape:
                    _draw.Active = true;
                    _opacity.Active = true;
                    _displacement.Active = false;
                    _noise.Active = false; ;
                    _texture.Active = false;
                    _uvDeform.Active = true;
                    _blur.Active = false;
                    _shape.Active = true;
                    _gradient.Active = false;
                    _glow.Active = false;
                    _colorBalance.Active = false;
                    _customShader.Active = false;
                    break;
                case ShaderPass.Filter_Blur:
                    _draw.Active = false;
                    _opacity.Active = false;
                    _displacement.Active = false;
                    _noise.Active = false; ;
                    _texture.Active = false;
                    _uvDeform.Active = false;
                    _blur.Active = true;
                    _shape.Active = false;
                    _gradient.Active = false;
                    _glow.Active = false;
                    _colorBalance.Active = false;
                    _customShader.Active = false;
                    break;
                case ShaderPass.Filter_GradationSample:
                    _draw.Active = false;
                    _opacity.Active = true;
                    _displacement.Active = false;
                    _noise.Active = false; ;
                    _texture.Active = false;
                    _uvDeform.Active = true;
                    _blur.Active = false;
                    _shape.Active = false;
                    _gradient.Active = true;
                    _glow.Active = false;
                    _colorBalance.Active = false;
                    _customShader.Active = false;
                    break;
                case ShaderPass.Filter_Glow:
                    _draw.Active = false;
                    _opacity.Active = true;
                    _displacement.Active = false;
                    _noise.Active = false; ;
                    _texture.Active = false;
                    _uvDeform.Active = false;
                    _blur.Active = false;
                    _shape.Active = false;
                    _gradient.Active = false;
                    _glow.Active = true;
                    _colorBalance.Active = false;
                    _customShader.Active = false;
                    break;
                case ShaderPass.Filter_ColorBalance:
                    _draw.Active = false;
                    _opacity.Active = false;
                    _displacement.Active = false;
                    _noise.Active = false; ;
                    _texture.Active = false;
                    _uvDeform.Active = false;
                    _blur.Active = false;
                    _shape.Active = false;
                    _gradient.Active = false;
                    _glow.Active = false;
                    _colorBalance.Active = true;
                    _customShader.Active = false;
                    break;
                case ShaderPass.CustomShader:
                    _draw.Active = false;
                    _opacity.Active = false;
                    _displacement.Active = false;
                    _noise.Active = false; ;
                    _texture.Active = false;
                    _uvDeform.Active = false;
                    _blur.Active = false;
                    _shape.Active = false;
                    _gradient.Active = false;
                    _glow.Active = false;
                    _colorBalance.Active = false;
                    _customShader.Active = true;
                    break;
                case ShaderPass.Draw_Draw:
                    _draw.Active = true;
                    _opacity.Active = true;
                    _displacement.Active = false;
                    _noise.Active = false; ;
                    _texture.Active = false;
                    _uvDeform.Active = false;
                    _blur.Active = false;
                    _shape.Active = false;
                    _gradient.Active = false;
                    _glow.Active = false;
                    _colorBalance.Active = false;
                    _customShader.Active = false;
                    break;
                case ShaderPass.Displacement_Displacement:
                    _draw.Active = false;
                    _opacity.Active = false;
                    _displacement.Active = false;
                    _noise.Active = false; ;
                    _texture.Active = false;
                    _uvDeform.Active = true;
                    _blur.Active = false;
                    _shape.Active = false;
                    _gradient.Active = false;
                    _glow.Active = false;
                    _colorBalance.Active = false;
                    _customShader.Active = false;
                    break;
            }
        }

        public void SetComputeShaderProperty(ComputeShader cs, int kernel)
        {
            if (_draw.Active) _draw.SetComputeShaderProperty(cs, kernel);
            if (_opacity.Active) _opacity.SetComputeShaderProperty(cs, kernel);
            if (_displacement.Active) _displacement.SetComputeShaderProperty(cs, kernel);
            if (_texture.Active) _texture.SetComputeShaderProperty(cs, kernel);
            if (_noise.Active) _noise.SetComputeShaderProperty(cs, kernel);
            if (_uvDeform.Active) _uvDeform.SetComputeShaderProperty(cs, kernel);
            if (_shape.Active) _shape.SetComputeShaderProperty(cs, kernel);
            if (_blur.Active) _blur.SetComputeShaderProperty(cs, kernel);
            if (_gradient.Active) _gradient.SetComputeShaderProperty(cs, kernel);
            if (_glow.Active) _glow.SetComputeShaderProperty(cs, kernel);
            if (_colorBalance.Active) _colorBalance.SetComputeShaderProperty(cs, kernel);
        }

        public void SetComputeShaderPropertyAnim(ComputeShader cs, int kernel, int currentFrame)
        {
            if (_draw.Active) _draw.SetComputeShaderPropertyAnim(cs, kernel, currentFrame);
            if (_opacity.Active) _opacity.SetComputeShaderPropertyAnim(cs, kernel, currentFrame);
            if (_displacement.Active) _displacement.SetComputeShaderPropertyAnim(cs, kernel, currentFrame);
            if (_texture.Active) _texture.SetComputeShaderPropertyAnim(cs, kernel, currentFrame);
            if (_noise.Active) _noise.SetComputeShaderPropertyAnim(cs, kernel, currentFrame);
            if (_uvDeform.Active) _uvDeform.SetComputeShaderPropertyAnim(cs, kernel, currentFrame);
            if (_shape.Active) _shape.SetComputeShaderPropertyAnim(cs, kernel, currentFrame);
            if (_blur.Active) _blur.SetComputeShaderPropertyAnim(cs, kernel, currentFrame);
            if (_gradient.Active) _gradient.SetComputeShaderPropertyAnim(cs, kernel, currentFrame);
            if (_glow.Active) _glow.SetComputeShaderPropertyAnim(cs, kernel, currentFrame);
            if (_colorBalance.Active) _colorBalance.SetComputeShaderPropertyAnim(cs, kernel, currentFrame);
        }
    }
}