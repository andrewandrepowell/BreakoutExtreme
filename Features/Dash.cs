using BreakoutExtreme.Shaders;
using BreakoutExtreme.Utility;
using MonoGame.Extended;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace BreakoutExtreme.Features
{
    public class Dash : Feature
    {
        private readonly static ReadOnlyDictionary<Directions, float[]> _directionMasks = new(new Dictionary<Directions, float[]>() 
        {
            {
                Directions.Left,
                new float[]
                {
                    0, 0, 0, 0, 0,
                    0, 0, 0, 0, 0,
                    1, 2, 3, 0, 0,
                    0, 0, 0, 0, 0,
                    0, 0, 0, 0, 0,
                }
                .Select(x => x * 0.25f)
                .ToArray()
            },
            {
                Directions.Right,
                new float[]
                {
                    0, 0, 0, 0, 0,
                    0, 0, 0, 0, 0,
                    0, 0, 3, 2, 1,
                    0, 0, 0, 0, 0,
                    0, 0, 0, 0, 0,
                }
                .Select(x => x * 0.25f)
                .ToArray()
            }
        });
        private ITexture _parent;
        private Directions _direction = Directions.Left;
        private float[] _mask;
        private bool _running = false;
        private float _delayPeriod = 0;
        private float _delayTime;
        private float _spread = 4;
        private bool _initialized;
        private void UpdateMask()
        {
            _mask = _directionMasks[_direction];
        }
        public bool Running => _running;
        public float Spread
        {
            get => _spread;
            set
            {
                Debug.Assert(_initialized);
                Debug.Assert(!_running);
                Debug.Assert(value > 0);
                _spread = value;
            }
        }
        public void Start()
        {
            Debug.Assert(_initialized);
            _delayTime = _delayPeriod;
            _running = true;
        }
        public void Stop()
        {
            Debug.Assert(_initialized);
            _running = false;
        }
        public float DelayPeriod
        {
            get => _delayPeriod;
            set
            {
                Debug.Assert(!_running);
                Debug.Assert(value >= 0);
                _delayPeriod = value;
            }
        }
        public Directions Direction
        {
            get => _direction;
            set
            {
                Debug.Assert(value == Directions.Left || value == Directions.Right);
                if (_direction == value) return;
                _direction = value;
                UpdateMask();
            }
        }
        public override Scripts? Script => (_running && _delayTime <= 0) ? Scripts.MaskBlur : null;
        public override void UpdateShaderNode(MaskBlurNode node)
        {
            node.Configure(
                textureSize: _parent.Texture.Bounds.Size,
                spread: _spread,
                mask: _mask);
        }
        public override void Update()
        {
            if (_running)
            {
                if (_delayTime > 0)
                    _delayTime -= Globals.GameTime.GetElapsedSeconds();
            }
        }
        public void Reset(ITexture parent)
        {
            _parent = parent;
            _initialized = true;
        }
        public Dash()
        {
            _initialized = false;
            UpdateMask();
        }
    }
}
