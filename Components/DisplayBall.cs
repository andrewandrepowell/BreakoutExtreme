using BreakoutExtreme.Utility;
using MonoGame.Extended;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public class DisplayBall
    {
        private readonly Animater _animater;
        private readonly Features.Float _float;
        private readonly Features.FloatAway _floatAway;
        private readonly Features.Vanish _vanish;
        private readonly Features.Appear _appear;
        private float _floatStartTime;
        private RunningStates _state = RunningStates.Waiting;
        public Animater GetAnimater() => _animater;
        public DisplayBall(float floatStartTime = 0)
        {
            Debug.Assert(floatStartTime >= 0);
            _floatStartTime = floatStartTime;
            _animater = new Animater();
            _animater.Visibility = 0;
            _float = new();
            _animater.ShaderFeatures.Add(_float);
            _floatAway = new();
            _floatAway.ForceStart();
            _animater.ShaderFeatures.Add(_floatAway);
            _vanish = new();
            _animater.ShaderFeatures.Add(_vanish);
            _appear = new();
            _animater.ShaderFeatures.Add(_appear);
        }
        public bool Running => _state == RunningStates.Starting || _state == RunningStates.Running;
        public void Start()
        {
            _animater.Play(Animater.Animations.Ball);
            _animater.Visibility = 1;
            _floatAway.Stop();
            _appear.Start();
            _state = RunningStates.Starting;
        }
        public void Stop()
        {
            _animater.Play(Animater.Animations.BallDead);
            _animater.Visibility = 1;
            _floatAway.Start();
            _vanish.Start();
            _state = RunningStates.Stopping;
        }
        public void Update()
        {
            if (!_float.Running)
            {
                if (_floatStartTime <= 0)
                    _float.Start();
                _floatStartTime -= Globals.GameTime.GetElapsedSeconds();
            }

            if (_state == RunningStates.Starting && _floatAway.State == RunningStates.Waiting && !_appear.Running)
            {
                _state = RunningStates.Running;
            }

            if (_state == RunningStates.Stopping && _floatAway.State == RunningStates.Running && !_vanish.Running)
            {
                _animater.Visibility = 0;
                _state = RunningStates.Waiting;
            }
        }
    }
}
