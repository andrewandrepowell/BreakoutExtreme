using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class Paddle
    {
        private class LaserGlower(Paddle parent)
        {
            private const float _pulsePeriod = 1f;
            private const float _minGlowVisibility = 0.1f;
            private const float _maxThickGlowVisibility = 1f;
            private const float _maxThinGlowVisibility = 0.50f;
            private PulseGlower _thickPulseGlower;
            private PulseGlower _thinPulseGlower;
            private Paddle _parent = parent;
            private bool _initialized = false;
            private bool _empowered = false;
            private void UpdatePulseGlowerColors()
            {
                if (_empowered)
                {
                    _thickPulseGlower.GetTexturer().Color = new Color(251, 213, 218);
                    _thinPulseGlower.GetTexturer().Color = new Color(201, 59, 205);
                }
                else
                {
                    _thickPulseGlower.GetTexturer().Color = Color.Orange;
                    _thinPulseGlower.GetTexturer().Color = Color.Red;
                }
            }
            public bool Empowered
            {
                get => _empowered;
                set
                {
                    Debug.Assert(_initialized);
                    _empowered = value;
                    UpdatePulseGlowerColors();
                }
            }
            public void Reset()
            {
                Debug.Assert(!_initialized);
                _thickPulseGlower = Globals.Runner.CreatePulseGlower(
                    parent: _parent._animater,
                    color: Color.Orange,
                    minVisibility: _minGlowVisibility,
                    maxVisibility: _maxThickGlowVisibility,
                    pulsePeriod: _pulsePeriod);
                _thinPulseGlower = Globals.Runner.CreatePulseGlower(
                    parent: _parent._animater,
                    color: Color.Red,
                    minVisibility: _minGlowVisibility,
                    maxVisibility: _maxThinGlowVisibility,
                    pulsePeriod: _pulsePeriod);
                _empowered = false;
                _initialized = true;
            }
            public void Start()
            {
                Debug.Assert(_initialized);
                _thickPulseGlower.Start();
                _thinPulseGlower.Start();
            }
            public void RemoveEntity()
            {
                Debug.Assert(_initialized);
                _thickPulseGlower.RemoveEntity();
                _thinPulseGlower.RemoveEntity();
                _initialized = false;
            }
        }
    }
}
