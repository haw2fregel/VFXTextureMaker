using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class OpShape : LayerOption
    {
        [SerializeField] ShapeType _shapeType;
        public string ShapeTypeName { get => _shapeType.ToString(); }
        [SerializeField] AnimationCurveProperty _shapeCurve;
        [SerializeField] Vector2AnimProperty _shapeCenter;
        [SerializeField] FloatAnimProperty _shapeDistancePower;
        [SerializeField] IntAnimProperty _polygonCount;
        [SerializeField] FloatAnimProperty _polygonBump;

        public OpShape()
        {
            _shapeType = ShapeType.DistancePower;
            _shapeCurve = new AnimationCurveProperty("_ShapeCurveArray", new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0)));
            _shapeCenter = new Vector2AnimProperty("_ShapeCenter", new Vector2(0.5f, 0.5f));
            _shapeDistancePower = new FloatAnimProperty("_ShapeDistancePower", 2);
            _polygonCount = new IntAnimProperty("_PolygonCount", 3);
            _polygonBump = new FloatAnimProperty("_PolygonBump", 0);
        }


        public override void SetComputeShaderProperty(ComputeShader cs, int kernel)
        {
            cs.SetFloats(_shapeCurve.ID, _shapeCurve.Array);
            cs.SetVector(_shapeCenter.ID, new Vector4(_shapeCenter.Value.x, _shapeCenter.Value.y, 0, 0));
            cs.SetFloat(_shapeDistancePower.ID, _shapeDistancePower.Value);
            cs.SetInt(_polygonCount.ID, _polygonCount.Value);
            cs.SetFloat(_polygonBump.ID, _polygonBump.Value);
        }

        public override void SetComputeShaderPropertyAnim(ComputeShader cs, int kernel, int currentFrame)
        {
            cs.SetFloats(_shapeCurve.ID, _shapeCurve.Array);

            if (_shapeCenter.IsAnim)
            {
                var valueX = _shapeCenter.CurveX.Evaluate(currentFrame);
                var valueY = _shapeCenter.CurveY.Evaluate(currentFrame);
                cs.SetVector(_shapeCenter.ID, new Vector4(valueX, valueY, 0, 0));
            }
            else
            {
                cs.SetVector(_shapeCenter.ID, new Vector4(_shapeCenter.Value.x, _shapeCenter.Value.y, 0, 0));
            }

            if (_shapeDistancePower.IsAnim)
            {
                cs.SetFloat(_shapeDistancePower.ID, _shapeDistancePower.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_shapeDistancePower.ID, _shapeDistancePower.Value);
            }

            if (_polygonCount.IsAnim)
            {
                cs.SetInt(_polygonCount.ID, (int)_polygonCount.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetInt(_polygonCount.ID, _polygonCount.Value);
            }

            if (_polygonBump.IsAnim)
            {
                cs.SetFloat(_polygonBump.ID, _polygonBump.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_polygonBump.ID, _polygonBump.Value);
            }
        }
    }

    [Serializable]
    public enum ShapeType
    {
        DistancePower,
        Polygon
    }
}
