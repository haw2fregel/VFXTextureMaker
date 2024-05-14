using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class IntAnimProperty
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
        [SerializeField] int _value;
        public int Value { get => _value; }
        [SerializeField] bool _isAnim;
        public bool IsAnim { get => _isAnim; }
        [SerializeField] bool _isCurve;
        public bool IsCurve { get => _isCurve; }
        [SerializeField] AnimationCurve _curve;
        public AnimationCurve Curve { get => _curve; }

        public IntAnimProperty(string name)
        {
            _name = name;
            _value = 0;
            _isAnim = false;
            _isCurve = false;
            _curve = new AnimationCurve(new Keyframe(0, 0));
        }

        public IntAnimProperty(string name, int value)
        {
            _name = name;
            _value = value;
            _isAnim = false;
            _isCurve = false;
            _curve = new AnimationCurve(new Keyframe(0, value));
        }

    }
}