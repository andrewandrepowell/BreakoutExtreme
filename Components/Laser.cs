using MonoGame.Extended.ECS;
using Microsoft.Xna.Framework;
using BreakoutExtreme.Utility;
using MonoGame.Extended;
using System.Diagnostics;
using System;

namespace BreakoutExtreme.Components
{
    public partial class Laser
    {
        private static readonly RectangleF _bounds = new Rectangle(0, 0, 1, 2).ToBounds();
        private static readonly Action<Collider.CollideNode> _collideAction = (Collider.CollideNode node) => ((Laser)node.Current.Parent).ServiceCollision(node);
        private bool _initialized;
        private Entity _entity;
        private readonly Collider _collider;
        private readonly Animater _animater;
        private readonly Features.Appear _appear;
        private readonly Features.Vanish _vanish;
        private Glower _thinGlower;
        private Glower _thickGlower;
        private States _state = States.Active;
        private Vector2 _acceleration = new(0, -6000);
        public enum States { Active, Destroying, Destroyed }
        public States State => _state;
        public Collider GetCollider() => _collider;
        public Animater GetAnimater() => _animater;
        public void Reset(Entity entity)
        {
            Debug.Assert(!_initialized);
            _entity = entity;
            _thickGlower = Globals.Runner.CreateGlower(
                parent: _animater,
                color: Color.Orange,
                minVisibility: 0.1f,
                maxVisibility: 1f,
                pulsePeriod: 0.5f,
                appearVanishPeriod: 0.25f);
            _thinGlower = Globals.Runner.CreateGlower(
                parent: _animater,
                color: Color.Red,
                minVisibility: 0.1f,
                maxVisibility: 0.5f,
                pulsePeriod: 0.5f,
                appearVanishPeriod: 0.25f);
            _appear.Start();
            _state = States.Active;
            _initialized = true;
        }
        public void Destroy()
        {
            Debug.Assert(_initialized);
            _collider.Acceleration = Vector2.Zero;
            _collider.Velocity = Vector2.Zero;
            _thickGlower.VanishStart();
            _thinGlower.VanishStart();
            _vanish.Start();
            _state = States.Destroying;
        }
        public void RemoveEntity()
        {
            Debug.Assert(_initialized);
            _thinGlower.RemoveEntity();
            _thickGlower.RemoveEntity();
            Globals.Runner.RemoveEntity(_entity);
            _initialized = false;
        }
        public void Update()
        {
            if (_state == States.Active)
                _collider.Acceleration += _acceleration;

            if (_state == States.Destroying && !_thickGlower.VanishRunning && !_thickGlower.VanishRunning && !_vanish.Running)
                _state = States.Destroyed;
        }
        public Laser()
        {
            _initialized = false;
            _animater = new();
            _animater.Play(Animater.Animations.Laser);
            _appear = new() { Period = 0.25f };
            _animater.ShaderFeatures.Add(_appear);
            _vanish = new() { Period = 0.25f };
            _animater.ShaderFeatures.Add(_vanish);
            _collider = new(_bounds, this, _collideAction);
        }
    }
}
