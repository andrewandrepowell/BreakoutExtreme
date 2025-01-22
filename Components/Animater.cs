using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public class Animater
    {
        private static readonly ReadOnlyDictionary<Atlases, string> _atlasAssetNames = new(new Dictionary<Atlases, string>
        {
            { Atlases.Ball, "animations/ball_0" },
            { Atlases.Brick2, "animations/brick_2" },
        });
        private static readonly ReadOnlyDictionary<Atlases, Size> _atlasRegionSizes = new(new Dictionary<Atlases, Size>
        {
            { Atlases.Ball, new Size(80, 80) },
            { Atlases.Brick2, new Size(112, 80) },
        });
        private static readonly ReadOnlyDictionary<Animations, string> _animationNames = new(new Dictionary<Animations, string>
        {
            { Animations.Ball, "ball_0" },
            { Animations.Brick2, "brick_0" },
        });
        private static readonly ReadOnlyDictionary<Atlases, Action<SpriteSheet>> _atlasConfigureAnimations = new(new Dictionary<Atlases, Action<SpriteSheet>>
        {
            {
                Atlases.Ball,
                delegate(SpriteSheet spriteSheet)
                {
                    spriteSheet.DefineAnimation(_animationNames[Animations.Ball], builder => builder
                        .AddFrame(0, TimeSpan.Zero));
                }
            },
            {
                Atlases.Brick2,
                delegate(SpriteSheet spriteSheet)
                {
                    spriteSheet.DefineAnimation(_animationNames[Animations.Brick2], builder => builder
                        .AddFrame(0, TimeSpan.Zero));
                }
            },
        });
        private static readonly ReadOnlyDictionary<Animations, Atlases> _animationAtlases = new(new Dictionary<Animations, Atlases>()
        {
            { Animations.Ball, Atlases.Ball },
            { Animations.Brick2, Atlases.Brick2 },
        });
        private readonly Dictionary<Atlases, AnimatedSprite> _atlasAnimatedSprites = new();
        private AnimatedSprite _atlasAnimatedSprite;
        private Animations _animation = Animations.Ball;
        private Vector2 _position, _drawPosition;
        private float _scale = 1;
        private Vector2 _scaleVector;
        private void UpdateAtlasAnimatedSprites()
        {
            var atlas = _animationAtlases[Animation];
            var atlasAssetName = _atlasAssetNames[atlas];
            var atlasTexture = Globals.ContentManager.Load<Texture2D>(atlasAssetName);
            var atlasRegionSize = _atlasRegionSizes[atlas];
            var atlasObject = Texture2DAtlas.Create(atlasAssetName, atlasTexture, atlasRegionSize.Width, atlasRegionSize.Height);
            var spriteSheet = new SpriteSheet(atlasAssetName, atlasObject);
            _atlasConfigureAnimations[atlas](spriteSheet);
            var animatedSprite = new AnimatedSprite(spriteSheet, _animationNames[Animation]);
            Debug.Assert(!_atlasAnimatedSprites.ContainsKey(atlas));
            _atlasAnimatedSprites.Add(atlas, animatedSprite);
        }
        private void UpdateDrawPosition()
        {
            _drawPosition.X = (float)Math.Floor(_position.X);
            _drawPosition.Y = (float)Math.Floor(_position.Y);
        }
        private void UpdateScaleVector()
        {
            _scaleVector.X = _scale;
            _scaleVector.Y = _scale;
        }
        public static void Load()
        {
            var animater = new Animater();
            var animations = Enum.GetValues<Animations>();
            foreach (ref var animation in animations.AsSpan())
            {
                animater.Play(animation);
            }
        }
        public enum Atlases
        {
            Ball,
            Brick2
        }
        public enum Animations
        {
            Ball,
            Brick2
        }
        public Animations Animation => _animation;
        public Vector2 Position
        {
            get => _position;
            set
            {
                if (_position == value)
                    return;
                _position = value;
                UpdateDrawPosition();
            }
        }
        public float Scale
        {
            get => _scale;
            set
            {
                if (_scale == value)
                    return;
                _scale = value;
                UpdateScaleVector();
            }
        }
        public float Rotation = 0;
        public Animater()
        {
            UpdateAtlasAnimatedSprites();
            UpdateDrawPosition();
            UpdateScaleVector();
        }
        public void Play(Animations animation)
        {
            var differentAnimation = Animation != animation;
            _animation = animation;

            var atlas = _animationAtlases[Animation];
            if (differentAnimation && !_atlasAnimatedSprites.ContainsKey(atlas))
            {
                UpdateAtlasAnimatedSprites();
            }

            _atlasAnimatedSprites[atlas].SetAnimation(_animationNames[Animation]);
        }
        public void Update()
        {
            _atlasAnimatedSprites[_animationAtlases[Animation]].Update(Globals.GameTime);
        }
        public void Draw()
        {
            Globals.SpriteBatch.Draw(
                sprite: _atlasAnimatedSprites[_animationAtlases[Animation]], 
                position: _drawPosition, 
                rotation: Rotation, 
                scale: _scaleVector);
        }
    }
}
