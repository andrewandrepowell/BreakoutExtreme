﻿using MonoGame.Extended.ECS;
using MonoGame.Extended;
using System;
using Microsoft.Xna.Framework;
using BreakoutExtreme.Utility;
using System.Collections.ObjectModel;
using System.Collections.Generic;


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
        private static readonly Action<Collider.CollideNode> _collideAction = (Collider.CollideNode node) => ((Brick)node.Current.Parent).ServiceCollision(node);
        private readonly Animater _animater;
        private readonly Collider _collider;
        private readonly Entity _entity;
        private readonly Bricks _brick;
        private readonly Shadow _shadow;
        private readonly Features.Shake _shake;
        private readonly Features.Cracks _cracks;
        private void ServiceCollision(Collider.CollideNode node)
        {
        }
        public enum Bricks
        {
            ThickBrick
        }
        public Bricks GetBrick() => _brick;
        public Animater GetAnimater() => _animater;
        public Collider GetCollider() => _collider;
        public void Damage()
        {
            _shake.Start(0.5f);
        }
        public Brick(Entity entity, Bricks brick)
        {
            _animater = new();
            _animater.Play(_brickAnimations[brick]);
            _collider = new(bounds: _brickBounds[brick], parent: this, action: _collideAction);
            _entity = entity;
            _brick = brick;
            _shadow = Globals.Runner.CreateShadow(_animater, new Vector2(_animater.Position.X, _animater.Position.Y + Globals.ShadowDisplacement));
            _shake = new();
            _animater.ShaderFeatures.Add(_shake);
            _cracks = new(_animater) { Degree = Features.Cracks.Degrees.Large };
            _animater.ShaderFeatures.Add(_cracks);
        }
        public void RemoveEntity()
        {
            Globals.Runner.RemoveEntity(_entity);
            _shadow.RemoveEntity();
        }
    }
}
