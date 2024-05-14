using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class OpNoise : LayerOption
    {
        [SerializeField] NoiseType _noiseType;
        public string NoiseTypeName { get => _noiseType.ToString(); }
        [SerializeField] FloatAnimProperty _noiseLengthPow;
        [SerializeField] FloatAnimProperty _voronoiEdgeSize;
        [SerializeField] Vector2AnimProperty _noiseSelfWarp;
        [SerializeField] Vector2AnimProperty _noiseSelfWarpTile;
        [SerializeField] Vector2AnimProperty _noiseSelfWarpOffset;
        [SerializeField] Vector2AnimProperty _noiseTile;
        [SerializeField] Vector2AnimProperty _noiseOffset;
        [SerializeField] BoolAnimProperty _noiseSeamless;
        [SerializeField] Vector4AnimProperty _noiseWeight;
        [SerializeField] IntAnimProperty _noiseRandomSeed;

        public OpNoise()
        {
            _noiseType = NoiseType.ValueNoise;
            _noiseLengthPow = new FloatAnimProperty("_NoiseLengthPow", 2);
            _voronoiEdgeSize = new FloatAnimProperty("_VoronoiEdgeSize", 1);
            _noiseSelfWarp = new Vector2AnimProperty("_NoiseSelfWarp", new Vector2(2, 2));
            _noiseSelfWarpTile = new Vector2AnimProperty("_NoiseSelfWarpTile", new Vector2(1, 1));
            _noiseSelfWarpOffset = new Vector2AnimProperty("_NoiseSelfWarpOffset", new Vector2(0, 0));
            _noiseTile = new Vector2AnimProperty("_NoiseTile", new Vector2(10, 10));
            _noiseOffset = new Vector2AnimProperty("_NoiseOffset", new Vector2(0, 0));
            _noiseSeamless = new BoolAnimProperty("_NoiseSeamless", true);
            _noiseWeight = new Vector4AnimProperty("_NoiseWeight", new Vector4(1, 0, 0, 0));
            _noiseRandomSeed = new IntAnimProperty("_NoiseRandomSeed", 0);
        }
        public override void SetComputeShaderProperty(ComputeShader cs, int kernel)
        {
            cs.SetFloat(_noiseLengthPow.ID, _noiseLengthPow.Value);
            cs.SetFloat(_voronoiEdgeSize.ID, _voronoiEdgeSize.Value);
            cs.SetVector(_noiseSelfWarp.ID, new Vector4(_noiseSelfWarp.Value.x, _noiseSelfWarp.Value.y, 0, 0));
            cs.SetVector(_noiseSelfWarpTile.ID, new Vector4(_noiseSelfWarpTile.Value.x, _noiseSelfWarpTile.Value.y, 0, 0));
            cs.SetVector(_noiseSelfWarpOffset.ID, new Vector4(_noiseSelfWarpOffset.Value.x, _noiseSelfWarpOffset.Value.y, 0, 0));
            cs.SetVector(_noiseTile.ID, new Vector4(_noiseTile.Value.x, _noiseTile.Value.y, 0, 0));
            cs.SetVector(_noiseOffset.ID, new Vector4(_noiseOffset.Value.x, _noiseOffset.Value.y, 0, 0));
            cs.SetBool(_noiseSeamless.ID, _noiseSeamless.Value);
            cs.SetVector(_noiseWeight.ID, _noiseWeight.Value);
            cs.SetInt(_noiseRandomSeed.ID, _noiseRandomSeed.Value);
        }
        public override void SetComputeShaderPropertyAnim(ComputeShader cs, int kernel, int currentFrame)
        {
            if (_noiseLengthPow.IsAnim)
            {
                cs.SetFloat(_noiseLengthPow.ID, _noiseLengthPow.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_noiseLengthPow.ID, _noiseLengthPow.Value);
            }

            if (_voronoiEdgeSize.IsAnim)
            {
                cs.SetFloat(_voronoiEdgeSize.ID, _voronoiEdgeSize.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetFloat(_voronoiEdgeSize.ID, _voronoiEdgeSize.Value);
            }

            if (_noiseSelfWarp.IsAnim)
            {
                var valueX = _noiseSelfWarp.CurveX.Evaluate(currentFrame);
                var valueY = _noiseSelfWarp.CurveY.Evaluate(currentFrame);
                cs.SetVector(_noiseSelfWarp.ID, new Vector4(valueX, valueY, 0, 0));
            }
            else
            {
                cs.SetVector(_noiseSelfWarp.ID, new Vector4(_noiseSelfWarp.Value.x, _noiseSelfWarp.Value.y, 0, 0));
            }

            if (_noiseSelfWarpTile.IsAnim)
            {
                var valueX = _noiseSelfWarpTile.CurveX.Evaluate(currentFrame);
                var valueY = _noiseSelfWarpTile.CurveY.Evaluate(currentFrame);
                cs.SetVector(_noiseSelfWarpTile.ID, new Vector4(valueX, valueY, 0, 0));
            }
            else
            {
                cs.SetVector(_noiseSelfWarpTile.ID, new Vector4(_noiseSelfWarpTile.Value.x, _noiseSelfWarpTile.Value.y, 0, 0));
            }

            if (_noiseSelfWarpOffset.IsAnim)
            {
                var valueX = _noiseSelfWarpOffset.CurveX.Evaluate(currentFrame);
                var valueY = _noiseSelfWarpOffset.CurveY.Evaluate(currentFrame);
                cs.SetVector(_noiseSelfWarpOffset.ID, new Vector4(valueX, valueY, 0, 0));
            }
            else
            {
                cs.SetVector(_noiseSelfWarpOffset.ID, new Vector4(_noiseSelfWarpOffset.Value.x, _noiseSelfWarpOffset.Value.y, 0, 0));
            }

            if (_noiseTile.IsAnim)
            {
                var valueX = _noiseTile.CurveX.Evaluate(currentFrame);
                var valueY = _noiseTile.CurveY.Evaluate(currentFrame);
                cs.SetVector(_noiseTile.ID, new Vector4(valueX, valueY, 0, 0));
            }
            else
            {
                cs.SetVector(_noiseTile.ID, new Vector4(_noiseTile.Value.x, _noiseTile.Value.y, 0, 0));
            }

            if (_noiseOffset.IsAnim)
            {
                var valueX = _noiseOffset.CurveX.Evaluate(currentFrame);
                var valueY = _noiseOffset.CurveY.Evaluate(currentFrame);
                cs.SetVector(_noiseOffset.ID, new Vector4(valueX, valueY, 0, 0));
            }
            else
            {
                cs.SetVector(_noiseOffset.ID, new Vector4(_noiseOffset.Value.x, _noiseOffset.Value.y, 0, 0));
            }

            if (_noiseSeamless.IsAnim)
            {
                cs.SetBool(_noiseSeamless.ID, _noiseSeamless.Curve.Evaluate(currentFrame) >= 1);
            }
            else
            {
                cs.SetBool(_noiseSeamless.ID, _noiseSeamless.Value);
            }

            if (_noiseWeight.IsAnim)
            {
                var valueX = _noiseWeight.CurveX.Evaluate(currentFrame);
                var valueY = _noiseWeight.CurveY.Evaluate(currentFrame);
                var valueZ = _noiseWeight.CurveZ.Evaluate(currentFrame);
                var valueW = _noiseWeight.CurveW.Evaluate(currentFrame);
                cs.SetVector(_noiseWeight.ID, new Vector4(valueX, valueY, valueZ, valueW));
            }
            else
            {
                cs.SetVector(_noiseWeight.ID, new Vector4(_noiseWeight.Value.x, _noiseWeight.Value.y, _noiseWeight.Value.z, _noiseWeight.Value.w));
            }

            if (_noiseRandomSeed.IsAnim)
            {
                cs.SetInt(_noiseRandomSeed.ID, (int)_noiseRandomSeed.Curve.Evaluate(currentFrame));
            }
            else
            {
                cs.SetInt(_noiseRandomSeed.ID, _noiseRandomSeed.Value);
            }
        }

        [Serializable]
        public enum NoiseType
        {
            ValueNoise,
            ValueNoiseGrad,
            PerlinNoise,
            PerlinNoiseGrad,
            DistanceNoise,
            Voronoi,
            VoronoiEdge
        }
    }
}