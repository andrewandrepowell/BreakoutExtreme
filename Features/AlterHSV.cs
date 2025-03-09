using BreakoutExtreme.Shaders;
using System.Diagnostics;

namespace BreakoutExtreme.Features
{
    public class AlterHSV : Feature
    {
        private float _hue = 0;
        private float _saturation = 0;
        private float _value = 0;
        private bool _running = false;
        public override Scripts? Script => _running ? Scripts.AlterHSV : null;
        public bool Running => _running;
        public float Hue
        {
            get => _hue;
            set
            {
                Debug.Assert(value >= -1 && value <= 1);
                _hue = value;
            }
        }
        public float Saturation
        {
            get => _saturation;
            set
            {
                Debug.Assert(value >= -1 && value <= 1);
                _saturation = value;
            }
        }
        public float Value
        {
            get => _value;
            set
            {
                Debug.Assert(value >= -1 && value <= 1);
                _value = value;
            }
        }
        public void Start()
        {
            _running = true;
        }
        public void Stop()
        {
            _running = false;
        }
        public override void UpdateShaderNode(AlterHSVNode node)
        {
            node.Configure(new(_hue, _saturation, _value));
        }
    }
}
