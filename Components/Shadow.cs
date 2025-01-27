using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using System;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public class Shadow
    {
        private readonly Entity _entity;
        private readonly Animater _animater;
        private readonly Animater _parent;
        public Animater GetAnimater() => _animater;
        public Shadow(Entity entity, Animater parent, Vector2 shadowPosition)
        {
            _entity = entity;
            _parent = parent;
            _animater = new();
            _animater.Position = shadowPosition;
            _animater.Layer = Animater.Layers.Shadow;
            _animater.ShowBase = false;
            _animater.Visibility = Globals.ShadowVisibility;
            _animater.ShaderFeatures.Add(new Features.Shadow());
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
