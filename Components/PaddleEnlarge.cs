using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using BreakoutExtreme.Utility;

namespace BreakoutExtreme.Components
{
    public partial class Paddle
    {
        private static readonly ReadOnlyDictionary<Sizes, SizeConfig> _sizeConfigs = new(new Dictionary<Sizes, SizeConfig>()
        {
            {
                Sizes.Normal,
                new(Animater.Animations.Paddle,
                    Animater.Animations.PaddleDead,
                    new Rectangle(Globals.PlayAreaBlockBounds.Center.X, Globals.PlayAreaBlockBounds.Center.Y, 5, 1).ToBounds())
            },
            {
                Sizes.Large,
                new(Animater.Animations.PaddleLarge,
                    Animater.Animations.PaddleLargeDead,
                    new Rectangle(Globals.PlayAreaBlockBounds.Center.X, Globals.PlayAreaBlockBounds.Center.Y, 8, 1).ToBounds())
            }
        });
        private float _sizePeriod = 20;
        private float _sizeTime;
        private Sizes _size;
        private SizeConfig _sizeConfig;
        private class SizeConfig(
            Animater.Animations activeAnimation,
            Animater.Animations deadAnimation,
            RectangleF bounds)
        {
            public readonly Animater.Animations ActiveAnimation = activeAnimation;
            public readonly Animater.Animations DeadAnimation = deadAnimation;
            public readonly RectangleF Bounds = bounds;
        }
        private void UpdateSizeConfig()
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            _sizeConfig = _sizeConfigs[_size];
            if (_collider.Bounds == null)
            {
                _collider.Bounds = _sizeConfig.Bounds;
            }
            else
            {
                var prevBounds = (RectangleF)_collider.Bounds;
                _collider.Bounds = new RectangleF(
                    position: prevBounds.Position - (Vector2)((_sizeConfig.Bounds.Size - prevBounds.Size) / 2),
                    size: _sizeConfig.Bounds.Size);
            }
            _animater.Play(_sizeConfig.ActiveAnimation);
        }
        private void UpdateSize()
        {
            if (_state == States.Active && _size == Sizes.Large)
            {
                if (_sizeTime > 0)
                    _sizeTime -= Globals.GameTime.GetElapsedSeconds();
                else
                {
                    _size = Sizes.Normal;
                    UpdateSizeConfig();
                }
            }
        }
        public enum Sizes { Normal, Large }
        public Sizes Size => _size;
        public Vector2 DisplacementSizeIncrease
        {
            get
            {
                var normalSize = _sizeConfigs[Sizes.Normal].Bounds.Size;
                var currentSize = _sizeConfig.Bounds.Size;
                return (currentSize - normalSize) / 2;
            }
        }
        public float SizePeriod
        {
            get => _sizePeriod;
            set
            {
                Debug.Assert(_size == Sizes.Normal);
                Debug.Assert(value >= 0);
                _sizePeriod = value;
            }
        }
        public void Enlarge()
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            _sizeTime = _sizePeriod;
            _size = Sizes.Large;
            _limitedFlash.Start();
            UpdateSizeConfig();
        }
        public void ReleaseEnlarge()
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            _size = Sizes.Normal;
            _limitedFlash.Start();
            UpdateSizeConfig();
        }
    }
}
