using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class OpTileAndOffset : LayerOption
    {
        [SerializeField] Vector2AnimProperty _count;
        [SerializeField] Vector2AnimProperty _posMinMaxX;
        [SerializeField] Vector2AnimProperty _posMinMaxY;
        [SerializeField] Vector2AnimProperty _scaleMinMax;
        [SerializeField] Vector2AnimProperty _scaleMinMaxX;
        [SerializeField] Vector2AnimProperty _scaleMinMaxY;
        [SerializeField] Vector2AnimProperty _rotateMinMax;
        [SerializeField] Vector2AnimProperty _opacityMinMax;
        [SerializeField] IntAnimProperty _randomSeed;

        [SerializeField] Blend _blend;
        readonly int BlendID = Shader.PropertyToID("_TileAndOffsetBlendMode");

        [SerializeField] Color _backColor;
        readonly int BackColorID = Shader.PropertyToID("_TileAndOffsetBackColor");

        public OpTileAndOffset()
        {
            _count = new Vector2AnimProperty("_TileOffsetCount", new Vector2(1, 1));
            _posMinMaxX = new Vector2AnimProperty("_TileOffsetPosMinMaxX", new Vector2(0, 0));
            _posMinMaxY = new Vector2AnimProperty("_TileOffsetPosMinMaxY", new Vector2(0, 0));
            _scaleMinMax = new Vector2AnimProperty("_TileOffsetScaleMinMax", new Vector2(1, 1));
            _scaleMinMaxX = new Vector2AnimProperty("_TileOffsetScaleMinMaxX", new Vector2(1, 1));
            _scaleMinMaxY = new Vector2AnimProperty("_TileOffsetScaleMinMaxY", new Vector2(1, 1));
            _rotateMinMax = new Vector2AnimProperty("_TileOffsetRotateMinMax", new Vector2(0, 0));
            _opacityMinMax = new Vector2AnimProperty("_TileOffsetOpacityMinMax", new Vector2(1, 1));
            _randomSeed = new IntAnimProperty("_TileAndOffsetRandomSeed", 1);
            _blend = Blend.Overwrite;
            _backColor = new Color(0, 0, 0, 0);
        }

        public override void SetComputeShaderProperty(ComputeShader cs, int kernel)
        {
            cs.SetVector(_count.ID, new Vector4(_count.Value.x, _count.Value.y, 0, 0));
            cs.SetVector(_posMinMaxX.ID, new Vector4(_posMinMaxX.Value.x, _posMinMaxX.Value.y, 0, 0));
            cs.SetVector(_posMinMaxY.ID, new Vector4(_posMinMaxY.Value.x, _posMinMaxY.Value.y, 0, 0));
            cs.SetVector(_scaleMinMax.ID, new Vector4(_scaleMinMax.Value.x, _scaleMinMax.Value.y, 0, 0));
            cs.SetVector(_scaleMinMaxX.ID, new Vector4(_scaleMinMaxX.Value.x, _scaleMinMaxX.Value.y, 0, 0));
            cs.SetVector(_scaleMinMaxY.ID, new Vector4(_scaleMinMaxY.Value.x, _scaleMinMaxY.Value.y, 0, 0));
            cs.SetVector(_rotateMinMax.ID, new Vector4(_rotateMinMax.Value.x, _rotateMinMax.Value.y, 0, 0));
            cs.SetVector(_opacityMinMax.ID, new Vector4(_opacityMinMax.Value.x, _opacityMinMax.Value.y, 0, 0));
            cs.SetInt(_randomSeed.ID, _randomSeed.Value);
            cs.SetInt(BlendID, (int)_blend);
            cs.SetVector(BackColorID, _backColor);
        }

        public override void SetComputeShaderPropertyAnim(ComputeShader cs, int kernel, int currentFrame)
        {
            if (_count.IsAnim)
            {
                var valueX = _count.CurveX.Evaluate(currentFrame);
                var valueY = _count.CurveY.Evaluate(currentFrame);
                cs.SetVector(_count.ID, new Vector4(valueX, valueY, 0, 0));
            }
            else
            {
                cs.SetVector(_count.ID, new Vector4(_count.Value.x, _count.Value.y, 0, 0));
            }

            if (_posMinMaxX.IsAnim)
            {
                var valueX = _posMinMaxX.CurveX.Evaluate(currentFrame);
                var valueY = _posMinMaxX.CurveY.Evaluate(currentFrame);
                cs.SetVector(_posMinMaxX.ID, new Vector4(valueX, valueY, 0, 0));
            }
            else
            {
                cs.SetVector(_posMinMaxX.ID, new Vector4(_posMinMaxX.Value.x, _posMinMaxX.Value.y, 0, 0));
            }

            if (_posMinMaxY.IsAnim)
            {
                var valueX = _posMinMaxY.CurveX.Evaluate(currentFrame);
                var valueY = _posMinMaxY.CurveY.Evaluate(currentFrame);
                cs.SetVector(_posMinMaxY.ID, new Vector4(valueX, valueY, 0, 0));
            }
            else
            {
                cs.SetVector(_posMinMaxY.ID, new Vector4(_posMinMaxY.Value.x, _posMinMaxY.Value.y, 0, 0));
            }

            if (_scaleMinMax.IsAnim)
            {
                var valueX = _scaleMinMax.CurveX.Evaluate(currentFrame);
                var valueY = _scaleMinMax.CurveY.Evaluate(currentFrame);
                cs.SetVector(_scaleMinMax.ID, new Vector4(valueX, valueY, 0, 0));
            }
            else
            {
                cs.SetVector(_scaleMinMax.ID, new Vector4(_scaleMinMax.Value.x, _scaleMinMax.Value.y, 0, 0));
            }

            if (_scaleMinMaxX.IsAnim)
            {
                var valueX = _scaleMinMaxX.CurveX.Evaluate(currentFrame);
                var valueY = _scaleMinMaxX.CurveY.Evaluate(currentFrame);
                cs.SetVector(_scaleMinMaxX.ID, new Vector4(valueX, valueY, 0, 0));
            }
            else
            {
                cs.SetVector(_scaleMinMaxX.ID, new Vector4(_scaleMinMaxX.Value.x, _scaleMinMaxX.Value.y, 0, 0));
            }

            if (_scaleMinMaxY.IsAnim)
            {
                var valueX = _scaleMinMaxY.CurveX.Evaluate(currentFrame);
                var valueY = _scaleMinMaxY.CurveY.Evaluate(currentFrame);
                cs.SetVector(_scaleMinMaxY.ID, new Vector4(valueX, valueY, 0, 0));
            }
            else
            {
                cs.SetVector(_scaleMinMaxY.ID, new Vector4(_scaleMinMaxY.Value.x, _scaleMinMaxY.Value.y, 0, 0));
            }

            if (_rotateMinMax.IsAnim)
            {
                var valueX = _rotateMinMax.CurveX.Evaluate(currentFrame);
                var valueY = _rotateMinMax.CurveY.Evaluate(currentFrame);
                cs.SetVector(_rotateMinMax.ID, new Vector4(valueX, valueY, 0, 0));
            }
            else
            {
                cs.SetVector(_rotateMinMax.ID, new Vector4(_rotateMinMax.Value.x, _rotateMinMax.Value.y, 0, 0));
            }

            if (_opacityMinMax.IsAnim)
            {
                var valueX = _opacityMinMax.CurveX.Evaluate(currentFrame);
                var valueY = _opacityMinMax.CurveY.Evaluate(currentFrame);
                cs.SetVector(_opacityMinMax.ID, new Vector4(valueX, valueY, 0, 0));
            }
            else
            {
                cs.SetVector(_opacityMinMax.ID, new Vector4(_opacityMinMax.Value.x, _opacityMinMax.Value.y, 0, 0));
            }

            if (_randomSeed.IsAnim)
            {
                cs.SetInt(_randomSeed.ID, (int)_randomSeed.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetInt(_randomSeed.ID, _randomSeed.Value);
            }

            cs.SetInt(BlendID, (int)_blend);
            cs.SetVector(BackColorID, _backColor);
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
}