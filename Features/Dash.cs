using BreakoutExtreme.Shaders;
using BreakoutExtreme.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private void UpdateMask()
        {
            _mask = _directionMasks[_direction];
        }
        public bool Running => _running;
        public void Start()
        {
            _running = true;
        }
        public void Stop()
        {
            _running = false;
        }
        public Directions Direction
        {
            get => _direction;
            set
            {
                if (_direction == value) return;
                _direction = value;
                UpdateMask();
            }
        }
        public override Scripts? Script => (_running) ? Scripts.MaskBlur : null;
        public override void UpdateShaderNode(MaskBlurNode node)
        {
            node.Configure(
                textureSize: _parent.Texture.Bounds.Size,
                spread: 3,
                mask: _mask);
        }
        public Dash(ITexture parent)
        {
            _parent = parent;
            UpdateMask();
        }
    }
}
