using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class FloatAnimProperty
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
        [SerializeField] float _value;
        public float Value { get => _value; }
        [SerializeField] bool _isAnim;
        public bool IsAnim { get => _isAnim; }
        [SerializeField] bool _isCurve;
        public bool IsCurve { get => _isCurve; }
        [SerializeField] AnimationCurve _curve;
        public AnimationCurve Curve { get => _curve; }

        public FloatAnimProperty(string name)
        {
            _name = name;
            _value = 0f;
            _isAnim = false;
            _isCurve = false;
            _curve = new AnimationCurve(new Keyframe(0, 0));
        }

        public FloatAnimProperty(string name, float value)
        {
            _name = name;
            _value = value;
            _isAnim = false;
            _isCurve = false;
            _curve = new AnimationCurve(new Keyframe(0, value));
        }

    }
}