﻿using BreakoutExtreme.Shaders;
using MonoGame.Extended;
using System.Diagnostics;
using System;

namespace BreakoutExtreme.Features
{
    public class Vanish : Feature
    {
        private float _period = 1;
        private float _time = 0;
        private float _delayPeriod = 0;
        private float _delayTime;
        private bool _running = false;
        public override bool UpdateVisibility(ref float visibility)
        {
            if (!_running)
                return false;
            visibility *= (_time / _period);
            return true;
        }
        public bool Running => _running;
        public float Period
        {
            get => _period;
            set
            {
                Debug.Assert(!Running);
                Debug.Assert(value > 0);
                _period = value;
            }
        }
        public float DelayPeriod
        {
            get => _delayPeriod;
            set
            {
                Debug.Assert(!Running);
                Debug.Assert(_delayPeriod >= 0);
                _delayPeriod = value;
            }
        }
        public void Start()
        {
            _time = _period;
            _delayTime = _delayPeriod;
            _running = true;
        }
        public void Stop()
        {
            _running = false;
        }
        public override void Update()
        {
            if (_running)
            {
                if (_time <= 0)
                    Stop();
                var timeElapsed = Globals.GameTime.GetElapsedSeconds();
                if (_delayTime > 0)
                    _delayTime -= timeElapsed;
                else
                    _time -= timeElapsed;
            }
        }
    }
}
