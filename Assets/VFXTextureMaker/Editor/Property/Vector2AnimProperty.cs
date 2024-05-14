using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class Vector2AnimProperty
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
        [SerializeField] Vector2 _value;
        public Vector2 Value { get => _value; }
        [SerializeField] bool _isAnim;
        public bool IsAnim { get => _isAnim; }
        [SerializeField] bool _isCurve;
        public bool IsCurve { get => _isCurve; }
        [SerializeField] AnimationCurve _curveX;
        public AnimationCurve CurveX { get => _curveX; }
        [SerializeField] AnimationCurve _curveY;
        public AnimationCurve CurveY { get => _curveY; }
        public Vector2AnimProperty(string name)
        {
            _name = name;
            _value = new Vector2(0, 0);
            _isAnim = false;
            _isCurve = false;
            _curveX = new AnimationCurve(new Keyframe(0, 0));
            _curveY = new AnimationCurve(new Keyframe(0, 0));
        }

        public Vector2AnimProperty(string name, Vector2 value)
        {
            _name = name;
            _value = value;
            _isAnim = false;
            _isCurve = false;
            _curveX = new AnimationCurve(new Keyframe(0, value.x));
            _curveY = new AnimationCurve(new Keyframe(0, value.y));
        }

    }
}