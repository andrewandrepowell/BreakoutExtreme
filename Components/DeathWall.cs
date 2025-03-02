using MonoGame.Extended;
using Microsoft.Xna.Framework;
using BreakoutExtreme.Utility;
using System;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public class DeathWall : IUpdate
    {
        private static readonly Action<Collider.CollideNode> _collideAction = (Collider.CollideNode node) => ((DeathWall)node.Current.Parent).ServiceCollision(node);
        private Collider _collider;
        private States _state;
        private Spike[] _spikes;
        private bool _protectTimed = false;
        private float _protectPeriod = 10;
        private float _protectTime;
        private Sounder _sounder;
        private void ServiceCollision(Collider.CollideNode node)
        {
        }
        public enum States { Active, Protecting }
        public Collider GetCollider() => _collider;
        public States State => _state;
        public bool ProtectTimed
        {
            get => _protectTimed;
            set
            {
                Debug.Assert(_state != States.Protecting);
                _protectTimed = value;
            }
        }
        public float ProtectPeriod
        {
            get => _protectPeriod;
            set
            {
                Debug.Assert(_state != States.Protecting);
                Debug.Assert(_protectPeriod >= 0);
                _protectPeriod = value;
            }
        }
        public void Protect()
        {
            Debug.Assert(_state == States.Active);
            foreach (ref var spike in _spikes.AsSpan())
                spike.Protect();
            if (_protectTimed)
                _protectTime = _protectPeriod;
            _state = States.Protecting;
        }
        public void ReleaseProtect()
        {
            Debug.Assert(_state == States.Protecting);
            foreach (ref var spike in _spikes.AsSpan())
                spike.ReleaseProtect();
            _state = States.Active;
        }
        public void ResetProtect()
        {
            Debug.Assert(_state == States.Protecting);
            Debug.Assert(_protectTimed);
            _protectTime = _protectPeriod;
        }
        public void RunBounceEffects()
        {
            Debug.Assert(_state == States.Protecting);
            _sounder.Play(Sounder.Sounds.Wall);
        }
        public void Update()
        {
            if (Globals.Paused)
                return;
            if (_state == States.Protecting && _protectTimed)
            {
                if (_protectTime <= 0)
                    ReleaseProtect();
                else
                    _protectTime -= Globals.GameTime.GetElapsedSeconds();
            }
        }
        public DeathWall(RectangleF bounds)
        {
            Debug.Assert(bounds.Height == Globals.GameBlockSize);
            _state = States.Active;
            _collider = new(bounds, this, _collideAction);
            _sounder = Globals.Runner.GetSounder();

            {
                var spikesTotal = (int)Math.Floor(bounds.Width / Globals.GameBlockSize);
                _spikes = new Spike[spikesTotal];
                var startPosition = new Vector2(x: bounds .Center.X - (spikesTotal - 1) * Globals.GameBlockSize / 2, bounds.Center.Y);
                for (var i = 0; i < spikesTotal; i++)
                {
                    var spike = Globals.Runner.CreateSpike(
                        position: new Vector2(x: startPosition.X + i * Globals.GameBlockSize, y: startPosition.Y),
                        edge: (i == 0) ? Spike.Edges.Left : (i == spikesTotal - 1) ? Spike.Edges.Right : Spike.Edges.None);
                    spike.Start();
                    _spikes[i] = spike;
                }
            }
        }
    }
}
