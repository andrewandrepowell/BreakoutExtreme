using MonoGame.Extended.ECS;
using Microsoft.Xna.Framework;
using BreakoutExtreme.Utility;
using MonoGame.Extended;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public class Laser
    {
        private static readonly RectangleF _bounds = new Rectangle(0, 0, 1, 2).ToBounds();
        private bool _initialized;
        private Entity _entity;
        private readonly Collider _collider;
        private readonly Animater _animater;
        private readonly Features.Appear _appear;
        private readonly Features.Vanish _vanish;
        private Glower _thinGlower;
        private Glower _thickGlower;
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
            _initialized = true;
        }
        public void RemoveEntity()
        {
            Debug.Assert(_initialized);
            _thinGlower.RemoveEntity();
            _thickGlower.RemoveEntity();
            Globals.Runner.RemoveEntity(_entity);
            _initialized = false;
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
            _collider = new(_bounds, this);
        }
    }
}
