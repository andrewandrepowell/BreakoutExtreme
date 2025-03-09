using BreakoutExtreme.Shaders;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Diagnostics;

namespace BreakoutExtreme.Features
{
    public class Flash : Feature
    {
        private float _period = (float)1 / 15;
        private float _time;
        public bool _running = false;
        public bool _active = false;
        public bool Running => _running;
        public Color Color = Color.White;
        public override Scripts? Script => (_running && _active) ? Scripts.Silhouette : null;
        public float Period
        {
            get => _period;
            set
            {
                Debug.Assert(!_running);
                Debug.Assert(value > 0);
                _period = value;
            }
        }
        public void Start()
        {
            _time = _period;
            _running = true;
        }
        public void Stop()
        {
            _running = false;
        }
        public override void UpdateShaderNode(SilhouetteNode node)
        {
            node.OverlayColor.SetValue(Color.ToVector4());
        }
        public override void Update()
        {
            if (_running)
            {
                while (_time <= 0)
                {
                    _active = !_active;
                    _time += _period;
                }
                _time -= Globals.GameTime.GetElapsedSeconds();
            }
        }
    }
}
