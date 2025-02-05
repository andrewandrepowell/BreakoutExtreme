using MonoGame.Extended.ECS;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public class Glower
    {
        private readonly Texturer _texturer;
        private readonly Features.Glow _glow;
        private readonly Features.Pulse _pulse;
        private readonly Features.Appear _appear;
        private readonly Features.Vanish _vanish;
        private Entity _entity;
        private Animater _parent;
        private bool _initialized;
        public Texturer GetTexturer() => _texturer;
        public bool VanishRunning => _vanish.Running;
        public void VanishStart() => _vanish.Start();
        public void Reset(
            Entity entity, Animater parent, Color color, 
            float minVisibility, float maxVisibility, float pulsePeriod, bool pulseRepeating, float appearVanishPeriod)
        {
            Debug.Assert(!_initialized);
            _entity = entity;
            _parent = parent;
            _texturer.Parent = parent;
            _texturer.Color = color;
            _texturer.Position = parent.Position;
            _parent.GetAttacher().Attach(_texturer);
            _glow.Parent = parent;
            _glow.Start();
            _pulse.MinVisibility = minVisibility;
            _pulse.MaxVisibility = maxVisibility;
            _pulse.Period = pulsePeriod;
            _pulse.Repeating = pulseRepeating;
            _pulse.Start();
            _appear.Period = appearVanishPeriod;
            _appear.Start();
            _vanish.Period = appearVanishPeriod;
            _initialized = true;
        }
        public Glower()
        {
            _initialized = false;
            _texturer = new(null) { ShowBase = false };
            _glow = new(null);
            _texturer.ShaderFeatures.Add(_glow);
            _pulse = new();
            _texturer.ShaderFeatures.Add(_pulse);
            _appear = new();
            _texturer.ShaderFeatures.Add(_appear);
            _vanish = new();
            _texturer.ShaderFeatures.Add(_vanish);
        }
        public void RemoveEntity()
        {
            Debug.Assert(_initialized);
            _parent.GetAttacher().Detach(_texturer);
            _glow.Stop();
            _pulse.Stop();
            _appear.Stop();
            _vanish.Stop();
            Globals.Runner.RemoveEntity(_entity);
            _initialized = false;
        }
    }
}
