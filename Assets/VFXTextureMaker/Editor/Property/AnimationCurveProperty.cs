using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class AnimationCurveProperty
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
        [SerializeField] AnimationCurve _value;
        public AnimationCurve AnimationCurve { get => _value; }
        public float[] Array 
        {
            get
            {
                var array = new float[2048];
                for (int i = 0; i < 2048; i++)
                {
                    array[i] = _value.Evaluate((float)i / (float)2048);
                }
                return array;
            }
        }

        public AnimationCurveProperty(string name)
        {
            _name = name;
            _value = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        }

        public AnimationCurveProperty(string name, AnimationCurve value)
        {
            _name = name;
            _value = value;
        }
    }
}