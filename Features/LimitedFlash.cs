﻿using BreakoutExtreme.Shaders;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Diagnostics;

namespace BreakoutExtreme.Features
{
    public class LimitedFlash : Feature
    {
        private const float _flashPeriod = (float)1 / 15;
        private float _flashTime;
        private float _limitedTime;
        public bool _running = false;
        public bool _active = false;
        public bool Running => _running;
        public Color Color = Color.White;
        public override Scripts? Script => (_running && _active) ? Scripts.Silhouette : null;
        public void Start(float time = 1)
        {
            Debug.Assert(time >= 0);
            _limitedTime = time;
            _flashTime = _flashPeriod;
            _running = true;
        }
        public void Stop()
        {
            _running = false;
        }
        public override void UpdateShaderNode(SilhouetteNode node)
        {
            node.OverlayColor.SetValue(Color.ToVector4());
        }
        public override void Update()
        {
            if (_running)
            {
                while (_flashTime <= 0)
                {
                    _active = !_active;
                    _flashTime += _flashPeriod;
                }
                if (_limitedTime <= 0)
                {
                    _running = false;
                }
                var timeElasped = Globals.GameTime.GetElapsedSeconds();
                _flashTime -= timeElasped;
                _limitedTime -= timeElasped;
            }
        }
    }
}
