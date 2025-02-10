using MonoGame.Extended.ECS;
using Microsoft.Xna.Framework;
using BreakoutExtreme.Utility;
using MonoGame.Extended;
using System.Diagnostics;
using System;

namespace BreakoutExtreme.Components
{
    public partial class Laser : IUpdate, IRemoveEntity, IDestroyed
    {
        private const float _appearVanishPeriod = 0.25f;
        private const float _pulsePeriod = 0.5f;
        private const float _minGlowVisibility = 0.1f;
        private const float _maxThickGlowVisibility = 1f;
        private const float _maxThinGlowVisibility = 0.5f;
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
        private PlayArea _parent;
        private Vector2 _acceleration = new(0, -6000);
        public enum States { Active, Destroying, Destroyed }
        public States State => _state;
        public bool Destroyed => _state == States.Destroyed;
        public Collider GetCollider() => _collider;
        public Animater GetAnimater() => _animater;
        public void Reset(Entity entity, PlayArea parent)
        {
            Debug.Assert(!_initialized);
            _entity = entity;
            _parent = parent;
            _thickGlower = Globals.Runner.CreateGlower(
                parent: _animater,
                color: Color.Orange,
                minVisibility: _minGlowVisibility,
                maxVisibility: _maxThickGlowVisibility,
                pulsePeriod: _pulsePeriod,
                pulseRepeating: true,
                appearVanishPeriod: _appearVanishPeriod);
            _thinGlower = Globals.Runner.CreateGlower(
                parent: _animater,
                color: Color.Red,
                minVisibility: _minGlowVisibility,
                maxVisibility: _maxThinGlowVisibility,
                pulsePeriod: _pulsePeriod,
                pulseRepeating: true,
                appearVanishPeriod: _appearVanishPeriod);
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
            if (Globals.Paused)
                return;
            if (!_initialized)
                return;
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
            _appear = new() { Period = _appearVanishPeriod };
            _animater.ShaderFeatures.Add(_appear);
            _vanish = new() { Period = _appearVanishPeriod };
            _animater.ShaderFeatures.Add(_vanish);
            _collider = new(_bounds, this, _collideAction);
        }
    }
}
