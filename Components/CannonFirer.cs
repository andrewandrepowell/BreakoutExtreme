using MonoGame.Extended;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class Cannon
    {
        private class Firer(Cannon parent)
        {
            private const float _delayPeriodConstant = 10;
            private bool _initialized = false;
            private bool _firing = false;
            private readonly Cannon _parent = parent;
            private float _delayTime;
            private const float _period = 10;
            private float _time;
            public void Fire()
            {
                Debug.Assert(_initialized);
                Debug.Assert(_parent._state == States.Active);
                Debug.Assert(_parent._animater.Animation == _parent._configNode.Active);
                Debug.Assert(_parent._particler.Particle == Particler.Particles.CannonBlast);
                _parent._animater.Play(_parent._configNode.Fire);
                _parent._particler.Trigger();
                Globals.Runner.CreateBomb(Bomb.Bombs.Normal, _parent._collider.Position);
                _firing = true;
            }
            public void Reset()
            {
                Debug.Assert(!_initialized);
                _delayTime = Globals.Random.NextSingle() * _delayPeriodConstant;
                _firing = false;
                _initialized = true;
            }
            public void RemoveEntity()
            {
                Debug.Assert(_initialized);
                _initialized = false;
            }
            public void Update()
            {
                Debug.Assert(_initialized);

                if (_parent._state != States.Active)
                    return;

                if (_firing && !_parent._animater.Running)
                {
                    _parent._animater.Play(_parent._configNode.Active);
                    _firing = false;
                }

                while (!_firing && _time <= 0 && _delayTime <= 0)
                {
                    Debug.Assert(_period > 0);
                    Fire();
                    _time += _period;
                }

                var timeElapsed = Globals.GameTime.GetElapsedSeconds();
                if (_delayTime <= 0)
                    _time -= timeElapsed;
                else 
                    _delayTime -= timeElapsed;
            }
        }
    }
}
