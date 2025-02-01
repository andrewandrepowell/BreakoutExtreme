using MonoGame.Extended.ECS;
using MonoGame.Extended;
using System;
using Microsoft.Xna.Framework;
using BreakoutExtreme.Utility;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;


namespace BreakoutExtreme.Components
{
    public class Brick
    {
        private static readonly ReadOnlyDictionary<Bricks, RectangleF> _brickBounds = new(new Dictionary<Bricks, RectangleF>() 
        {
            { Bricks.ThickBrick, new Rectangle(Globals.PlayAreaBlockBounds.X, Globals.PlayAreaBlockBounds.Y, 3, 1).ToBounds() }
        });
        private static readonly ReadOnlyDictionary<Bricks, Animater.Animations> _brickAnimations = new(new Dictionary<Bricks, Animater.Animations>() 
        {
            { Bricks.ThickBrick, Animater.Animations.BrickLarge }
        });
        private static readonly ReadOnlyDictionary<Bricks, Animater.Animations> _brickDeadAnimations = new(new Dictionary<Bricks, Animater.Animations>()
        {
            { Bricks.ThickBrick, Animater.Animations.BrickLargeDead }
        });
        private static readonly ReadOnlyDictionary<Bricks, int> _brickTotalHPs = new(new Dictionary<Bricks, int>() 
        {
            { Bricks.ThickBrick, 3 }
        });
        private static readonly Action<Collider.CollideNode> _collideAction = (Collider.CollideNode node) => ((Brick)node.Current.Parent).ServiceCollision(node);
        private const float _shakePeriod = 0.5f;
        private static readonly Vector2 _shineDirection = Vector2.Normalize(new Vector2(1, 1));
        private readonly Animater _animater;
        private readonly Collider _collider;
        private readonly Particler _particler;
        private readonly Entity _entity;
        private readonly Bricks _brick;
        private readonly Shadow _shadow;
        private readonly Features.Shake _shake;
        private readonly Features.Cracks _cracks;
        private readonly Features.Vanish _vanish;
        private readonly Features.Shine _shine;
        private void ServiceCollision(Collider.CollideNode node)
        {
        }
        public enum Bricks
        {
            ThickBrick
        }
        public enum States
        {
            Active,
            Destroying,
            Destroyed
        }
        public Bricks GetBrick() => _brick;
        public Animater GetAnimater() => _animater;
        public Collider GetCollider() => _collider;
        public Particler GetParticler() => _particler;
        public readonly int TotalHP;
        public int CurrentHP;
        public States State { get; private set; }
        public void Damage()
        {
            Debug.Assert(CurrentHP > 0 && State == States.Active);
            
            {
                
                CurrentHP -= 1;
                _shake.Start(_shakePeriod);
                _cracks.Degree = (Features.Cracks.Degrees)(TotalHP - CurrentHP);
                _particler.Trigger();
            }

            if (CurrentHP == 0)
            {
                Destroy();
            }

        }
        public void Destroy()
        {
            Debug.Assert(State == States.Active);
            Debug.Assert(!_vanish.Running);
            CurrentHP = 0;

            _shake.Start(_shakePeriod);
            _cracks.Degree = Features.Cracks.Degrees.None;
            _vanish.Start();
            _shadow.VanishStart();
            _animater.Play(_brickDeadAnimations[_brick]);

            State = States.Destroying;
        }
        public Brick(Entity entity, Bricks brick, Vector2 position)
        {
            _animater = new();
            _animater.Play(_brickAnimations[brick]);
            _collider = new(bounds: _brickBounds[brick], parent: this, action: _collideAction);
            _collider.Position = position;
            _particler = new(Particler.Particles.BrickBreak);
            _particler.Layer = Layers.Foreground;
            _entity = entity;
            _brick = brick;
            _shadow = Globals.Runner.CreateShadow(_animater);
            _shake = new();
            _animater.ShaderFeatures.Add(_shake);
            _cracks = new(_animater);
            _animater.ShaderFeatures.Add(_cracks);
            _vanish = new();
            _animater.ShaderFeatures.Add(_vanish);
            _shine = new();
            _shine.RepeatPeriod = 7.5f;
            _shine.DelayPeriod = _shineDirection.Dot(position) * 0.01f;
            _shine.Start();
            _animater.ShaderFeatures.Add(_shine);
            TotalHP = _brickTotalHPs[brick];
            CurrentHP = TotalHP;
            State = States.Active;
        }
        public void RemoveEntity()
        {
            Globals.Runner.RemoveEntity(_entity);
            _shadow.RemoveEntity();
        }
        public void Update()
        {
            if (State == States.Destroying && !_vanish.Running && !_shadow.VanishRunning)
            {
                State = States.Destroyed;
            }
        }
    }
}
