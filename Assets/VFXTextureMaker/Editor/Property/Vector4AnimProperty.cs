using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class Vector4AnimProperty
    {
        int _id;
        public int ID
        {
            get
            {
                if (_id == 0)
                {
                    _id = Shader.PropertyToID(_name);
                }
                return _id;
            }
        }
        [SerializeField] string _name;
        [SerializeField] Vector4 _value;
        public Vector4 Value { get => _value; }
        [SerializeField] bool _isAnim;
        public bool IsAnim { get => _isAnim; }
        [SerializeField] bool _isCurve;
        public bool IsCurve { get => _isCurve; }
        [SerializeField] AnimationCurve _curveX;
        public AnimationCurve CurveX { get => _curveX; }
        [SerializeField] AnimationCurve _curveY;
        public AnimationCurve CurveY { get => _curveY; }
        [SerializeField] AnimationCurve _curveZ;
        public AnimationCurve CurveZ { get => _curveZ; }
        [SerializeField] AnimationCurve _curveW;
        public AnimationCurve CurveW { get => _curveW; }

        public Vector4AnimProperty(string name)
        {
            _name = name;
            _value = new Vector4(0, 0, 0, 0);
            _isAnim = false;
            _isCurve = false;
            _curveX = new AnimationCurve(new Keyframe(0, 0));
            _curveY = new AnimationCurve(new Keyframe(0, 0));
            _curveZ = new AnimationCurve(new Keyframe(0, 0));
            _curveW = new AnimationCurve(new Keyframe(0, 0));
        }

        public Vector4AnimProperty(string name, Vector4 value)
        {
            _name = name;
            _value = value;
            _isAnim = false;
            _isCurve = false;
            _curveX = new AnimationCurve(new Keyframe(0, value.x));
            _curveY = new AnimationCurve(new Keyframe(0, value.y));
            _curveZ = new AnimationCurve(new Keyframe(0, value.z));
            _curveW = new AnimationCurve(new Keyframe(0, value.w));
        }

    }
}