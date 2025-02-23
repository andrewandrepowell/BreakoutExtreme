using MonoGame.Extended.ECS;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using BreakoutExtreme.Utility;

namespace BreakoutExtreme.Components
{
    public class PulseGlower : IUpdate
    {
        private bool _initialized;
        private bool _running;
        private Entity _entity;
        private readonly Texturer _texturer;
        private Animater _parent;
        private readonly Features.Pulse _pulse;
        private readonly Features.Glow _glow;
        public bool Running => _running;
        public bool Initialized => _initialized;
        public Texturer GetTexturer() => _texturer;
        public void Reset(
            Entity entity, Animater parent, Color color,
            float minVisibility, float maxVisibility, float pulsePeriod)
        {
            Debug.Assert(!_initialized);
            _entity = entity;
            _parent = parent;
            _texturer.Parent = parent;
            _texturer.Color = color;
            _texturer.Position = parent.Position;
            _parent.GetAttacher().Attach(_texturer);
            _glow.Parent = parent;
            _pulse.MinVisibility = minVisibility;
            _pulse.MaxVisibility = maxVisibility;
            _pulse.Period = pulsePeriod;
            _initialized = true;
            _running = false;
        }
        public void RemoveEntity()
        {
            Debug.Assert(_initialized);
            _parent.GetAttacher().Detach(_texturer);
            _glow.Stop();
            _pulse.Stop();
            Globals.Runner.RemoveEntity(_entity);
            _initialized = false;
        }
        public void Start()
        {
            Debug.Assert(_initialized);
            _pulse.Start();
            _glow.Start();
            _running = true;
        }
        public void Stop()
        {
            Debug.Assert(_initialized);
            _pulse.Stop();
            _glow.Stop();
            _running = false;
        }
        public PulseGlower()
        {
            _initialized = false;
            _running = false;
            _texturer = new(null) { ShowBase = false };
            _glow = new(null);
            _texturer.ShaderFeatures.Add(_glow);
            _pulse = new()
            {
                StartDark = true,
                Repeating = false
            };
            _texturer.ShaderFeatures.Add(_pulse);
        }
        public void Update()
        {
            if (Globals.Paused)
                return;
            if (!_initialized)
                return;
            if (_running && !_pulse.Running)
                Stop();
        }
    }
}
