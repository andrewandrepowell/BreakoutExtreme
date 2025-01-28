using MonoGame.Extended;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public class DisplayBall
    {
        private readonly Animater _animater;
        private readonly Features.Float _float;
        private float _floatStartTime;
        public Animater GetAnimater() => _animater;
        public DisplayBall(float floatStartTime = 0)
        {
            Debug.Assert(floatStartTime >= 0);
            _floatStartTime = floatStartTime;
            _animater = new Animater();
            _float = new();
            _animater.ShaderFeatures.Add(_float);
        }
        public void Update()
        {
            if (!_float.Running)
            {
                if (_floatStartTime <= 0)
                    _float.Start();
                _floatStartTime -= Globals.GameTime.GetElapsedSeconds();
            }
        }
    }
}
