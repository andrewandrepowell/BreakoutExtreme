using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using MonoGame.Extended.ECS;

namespace BreakoutExtreme.Components
{
    public partial class Ball
    {
        private static readonly CircleF _bounds = new(Vector2.Zero, Globals.GameHalfBlockSize);
        private static readonly Action<Collider.CollideNode> _collideAction = (Collider.CollideNode node) => ((Ball)node.Current.Parent).ServiceCollision(node);
        private readonly Animater _animater;
        private readonly Collider _collider;
        private readonly Entity _entity;
        private readonly Launcher _launcher;
        private void ServiceCollision(Collider.CollideNode node)
        {
            if (node.Other.Parent is Wall || node.Other.Parent is Paddle)
            { 
                    node.CorrectPosition();
            }
            _launcher.ServiceCollision(node);
        }
        public Animater GetAnimater() => _animater;
        public Collider GetCollider() => _collider;
        public Launcher GetLauncher() => _launcher;
        public Ball(Entity entity)
        {
            _animater = new();
            _animater.Play(Animater.Animations.Ball);
            _collider = new(bounds: _bounds, parent: this, action: _collideAction);
            _entity = entity;
            _launcher = new Launcher(this);
        }
        public void RemoveEntity()
        {
            Globals.Runner.RemoveEntity(_entity);
        }
        public void Update()
        {
            _launcher.Update();
        }
    }
}
