using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using BreakoutExtreme.Utility;


namespace BreakoutExtreme.Components
{
    public class Shadow
    {
        private const float _displacement = 8;
        private const float _visibility = 0.5f;
        private readonly Entity _entity;
        private readonly Animater _animater;
        private readonly Animater _parent;
        private readonly Features.Vanish _vanish;
        public Animater GetAnimater() => _animater;
        public bool VanishRunning => _vanish.Running;
        public void VanishStart() => _vanish.Start();
        public Shadow(Entity entity, Animater parent)
        {
            _entity = entity;
            _parent = parent;
            _animater = new();
            _animater.Position = _parent.Position + new Vector2(0, _displacement);
            _animater.Layer = Layers.Shadow;
            _animater.ShowBase = false;
            _animater.Visibility = _visibility;
            _animater.ShaderFeatures.Add(new Features.Shadow());
            _vanish = new();
            _animater.ShaderFeatures.Add(_vanish);
            _animater.Play(parent.Animation);
            _parent.GetAttacher().Attach(_animater);
        }
        public void RemoveEntity()
        {
            Globals.Runner.RemoveEntity(_entity);
            _parent.GetAttacher().Detach(_animater);
        }
    }
}
