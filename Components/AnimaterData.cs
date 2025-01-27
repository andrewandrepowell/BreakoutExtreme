using MonoGame.Extended.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class Animater
    {
        private static readonly ReadOnlyDictionary<Atlases, string> _atlasAssetNames = new(new Dictionary<Atlases, string>
        {
            { Atlases.Ball, "animations/ball_0" },
            { Atlases.Paddle, "animations/paddle_0" },
            { Atlases.BrickLarge, "animations/brick_2" },
            { Atlases.Cracks, "animations/cracks_0" },
        });
        private static readonly ReadOnlyDictionary<Atlases, Size> _atlasRegionSizes = new(new Dictionary<Atlases, Size>
        {
            { Atlases.Ball, new Size(80, 80) },
            { Atlases.Paddle, new Size(144, 80) },
            { Atlases.BrickLarge, new Size(112, 80) },
            { Atlases.Cracks, new Size(16, 16) },
        });
        private static readonly ReadOnlyDictionary<Animations, string> _animationNames = new(new Dictionary<Animations, string>
        {
            { Animations.Ball, "ball_0" },
            { Animations.Paddle, "paddle_0" },
            { Animations.BrickLarge, "brick_0" },
            { Animations.BrickLargeDead, "brick_1" },
            { Animations.CrackSmall, "crack_0" },
            { Animations.CrackMedium, "crack_1" },
            { Animations.CrackLarge, "crack_2" },
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
                    spriteSheet.DefineAnimation(_animationNames[Animations.BrickLarge], builder => builder.AddFrame(1, TimeSpan.FromSeconds(0.1)));
                    spriteSheet.DefineAnimation(_animationNames[Animations.BrickLargeDead], builder => builder.AddFrame(0, TimeSpan.FromSeconds(0.1)));
                }
            },
            {
                Atlases.Cracks,
                delegate(SpriteSheet spriteSheet)
                {
                    spriteSheet.DefineAnimation(_animationNames[Animations.CrackSmall], builder => builder.AddFrame(0, TimeSpan.Zero));
                    spriteSheet.DefineAnimation(_animationNames[Animations.CrackMedium], builder => builder.AddFrame(1, TimeSpan.Zero));
                    spriteSheet.DefineAnimation(_animationNames[Animations.CrackLarge], builder => builder.AddFrame(2, TimeSpan.Zero));
                }
            }
        });
        private static readonly ReadOnlyDictionary<Animations, Atlases> _animationAtlases = new(new Dictionary<Animations, Atlases>()
        {
            { Animations.Ball, Atlases.Ball },
            { Animations.Paddle, Atlases.Paddle },
            { Animations.BrickLarge, Atlases.BrickLarge },
            { Animations.BrickLargeDead, Atlases.BrickLarge },
            { Animations.CrackSmall, Atlases.Cracks },
            { Animations.CrackMedium, Atlases.Cracks },
            { Animations.CrackLarge, Atlases.Cracks },
        });
        private readonly Dictionary<Atlases, Node> _atlasNodes = [];
    }
}
