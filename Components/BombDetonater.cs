using System.Diagnostics;
using Microsoft.Xna.Framework;
using System;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.Particles.Modifiers.Containers;
using MonoGame.Extended;

namespace BreakoutExtreme.Components
{
    public partial class Bomb
    {
        private class Detonater(Bomb parent)
        {
            private readonly static float _detonationHeight = Globals.PlayAreaBounds.Bottom - Globals.PlayAreaBounds.Height * 0.25f;
            private readonly static Vector2 _soaringAcceleration = new Vector2(x: 0, y: -5000);
            private readonly static float _detonationRadius = Globals.PlayAreaBounds.Width * 1 / 6;
            private readonly static CircleF _detonationBounds = new CircleF(Vector2.Zero, _detonationRadius);
            private const float _detonationThreshold = Globals.GameBlockSize;
            private const float _detonationPeriod = 2;
            private readonly Bomb _parent = parent;
            private States _state = States.Waiting;
            public enum States { Waiting, Soaring, Detonating, Finished }
            public States State => _state;
            public void Reset()
            {
                Debug.Assert(!_parent._initialized);
                _state = States.Waiting;
            }
            public void Start()
            {
                Debug.Assert(_parent._initialized);
                Debug.Assert(_state == States.Waiting);
                Debug.Assert(_parent._state == Bomb.States.Active);
                {
                    var particler = _parent._particler;
                    particler.Play(Particler.Particles.BombBlast);
                    particler.Stop();
                    var particleEffect = _parent._particler.GetParticleEffect();
                    Debug.Assert(particleEffect.Emitters.Count == 1);
                    var emitter = particleEffect.Emitters[0];
                    ((CircleProfile)emitter.Profile).Radius = _detonationRadius;
                    Debug.Assert(emitter.Modifiers.Count == 3 && emitter.Modifiers[1] is CircleContainerModifier);
                    ((CircleContainerModifier)emitter.Modifiers[1]).Radius = _detonationRadius;
                }
                _state = States.Soaring;
            }
            public void Update()
            {
                Debug.Assert(_parent._initialized);

                if (_state == States.Soaring)
                {
                    Debug.Assert(_parent._state == Bomb.States.Detonating);
                    _parent._collider.Acceleration += _soaringAcceleration;
                }

                if (_state == States.Soaring && Math.Abs(_parent._collider.Position.Y - _detonationHeight) <= _detonationThreshold)
                {
                    Debug.Assert(_parent._state == Bomb.States.Detonating);
                    _parent._collider.Bounds = new CircleF(_parent._collider.Position, _detonationBounds.Radius);
                    _parent._animater.Play(_parent._configNode.Dead);
                    _parent._particler.Start();
                    _parent._shake.Period = _detonationPeriod;
                    _parent._shake.Start();
                    _state = States.Detonating;
                }

                if (_state == States.Detonating && !_parent._shake.Running)
                {
                    Debug.Assert(_parent._state == Bomb.States.Detonating);
                    _parent._collider.Bounds = new CircleF(_parent._collider.Position, Bomb._bounds.Radius);
                    _parent._animater.Play(_parent._configNode.Active);
                    _parent._particler.Stop();
                    _state = States.Finished;
                }
            }
        }
    }
}
