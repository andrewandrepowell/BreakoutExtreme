using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using BreakoutExtreme.Utility;
using System.Diagnostics;


namespace BreakoutExtreme.Components
{
    public class Shadow
    {
        private const float _displacement = 8;
        private const float _visibility = 0.5f;
        private readonly Texturer _texturer;
        private readonly Features.Vanish _vanish;
        private Animater _parent;
        private Entity _entity;
        private bool _initialized;
        public Texturer GetTexturer() => _texturer;
        public bool VanishRunning => _vanish.Running;
        public void VanishStart() => _vanish.Start();
        public void Reset(Entity entity, Animater parent)
        {
            Debug.Assert(!_initialized);
            _entity = entity;
            _parent = parent;
            _initialized = true;
            _texturer.Parent = parent;
            _texturer.Position = _parent.Position + new Vector2(0, _displacement);
            _parent.GetAttacher().Attach(_texturer);
        }
        public Shadow()
        {
            _initialized = false;
            _texturer = new(null)
            {
                Layer = Layers.Shadow,
                ShowBase = false,
                Visibility = _visibility,
            };
            _texturer.ShaderFeatures.Add(new Features.Shadow());
            _vanish = new();
            _texturer.ShaderFeatures.Add(_vanish);
        }
        public void RemoveEntity()
        {
            Debug.Assert(_initialized);
            Globals.Runner.RemoveEntity(_entity);
            _parent.GetAttacher().Detach(_texturer);
            _initialized = false;
        }
    }
}
