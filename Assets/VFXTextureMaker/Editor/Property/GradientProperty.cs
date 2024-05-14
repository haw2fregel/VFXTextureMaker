using System;
using UnityEngine;

namespace VFXTextureMaker
{
    [Serializable]
    public class GradientProperty
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
        [SerializeField] Gradient _value;
        public Gradient AnimationCurve { get => _value; }
        public Vector4[] Array
        {
            get
            {
                var array = new Vector4[1024];
                for (int i = 0; i < 1024; i++)
                {
                    array[i] = _value.Evaluate((float)i / (float)1024);
                }
                return array;
            }
        }

        public GradientProperty(string name)
        {
            _name = name;
            _value = new Gradient();
        }

        public GradientProperty(string name, Gradient value)
        {
            _name = name;
            _value = value;
        }

    }
}