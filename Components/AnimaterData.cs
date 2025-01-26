using MonoGame.Extended.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace BreakoutExtreme.Components
{
    public partial class Animater
    {
        private static readonly ReadOnlyDictionary<Atlases, string> _atlasAssetNames = new(new Dictionary<Atlases, string>
        {
            { Atlases.Ball, "animations/ball_0" },
            { Atlases.Paddle, "animations/paddle_0" },
            { Atlases.BrickLarge, "animations/brick_2" },
        });
        private static readonly ReadOnlyDictionary<Atlases, Size> _atlasRegionSizes = new(new Dictionary<Atlases, Size>
        {
            { Atlases.Ball, new Size(80, 80) },
            { Atlases.Paddle, new Size(144, 80) },
            { Atlases.BrickLarge, new Size(112, 80) },
        });
        private static readonly ReadOnlyDictionary<Animations, string> _animationNames = new(new Dictionary<Animations, string>
        {
            { Animations.Ball, "ball_0" },
            { Animations.Paddle, "paddle_0" },
            { Animations.BrickLarge, "brick_0" },
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
                Atlases.Paddle,
                delegate(SpriteSheet spriteSheet)
                {
                    spriteSheet.DefineAnimation(_animationNames[Animations.Paddle], builder => builder
                        .AddFrame(0, TimeSpan.Zero));
                }
            },
            {
                Atlases.BrickLarge,
                delegate(SpriteSheet spriteSheet)
                {
                    spriteSheet.DefineAnimation(_animationNames[Animations.BrickLarge], builder => builder
                        .AddFrame(0, TimeSpan.Zero));
                }
            },
        });
        private static readonly ReadOnlyDictionary<Animations, Atlases> _animationAtlases = new(new Dictionary<Animations, Atlases>()
        {
            { Animations.Ball, Atlases.Ball },
            { Animations.Paddle, Atlases.Paddle },
            { Animations.BrickLarge, Atlases.BrickLarge },
        });
        private readonly Dictionary<Atlases, AnimatedSprite> _atlasAnimatedSprites = [];
    }
}
