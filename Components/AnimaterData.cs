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
        private static readonly ReadOnlyDictionary<Spriters, string> _spriterAssetNames = new(new Dictionary<Spriters, string>
        {
            { Spriters.Ball, "animations/ball_0" },
            { Spriters.Paddle, "animations/paddle_0" },
            { Spriters.BrickLarge, "animations/brick_2" },
            { Spriters.Cracks, "animations/cracks_0" },
        });
        private static readonly ReadOnlyDictionary<Spriters, Size> _spriterRegionSizes = new(new Dictionary<Spriters, Size>
        {
            { Spriters.Ball, new Size(80, 80) },
            { Spriters.Paddle, new Size(144, 80) },
            { Spriters.BrickLarge, new Size(112, 80) },
            { Spriters.Cracks, new Size(16, 16) },
        });
        private static readonly ReadOnlyDictionary<Animations, string> _animationNames = new(new Dictionary<Animations, string>
        {
            { Animations.Ball, "ball_0" },
            { Animations.BallDead, "ball_1" },
            { Animations.Paddle, "paddle_0" },
            { Animations.BrickLarge, "brick_0" },
            { Animations.BrickLargeDead, "brick_1" },
            { Animations.CrackSmall, "crack_0" },
            { Animations.CrackMedium, "crack_1" },
            { Animations.CrackLarge, "crack_2" },
        });
        private static readonly ReadOnlyDictionary<Spriters, Action<Spriter>> _spriterConfigureAnimations = new(new Dictionary<Spriters, Action<Spriter>>
        {
            {
                Spriters.Ball,
                delegate(Spriter spriter)
                {
                    spriter.Add(_animationNames[Animations.Ball], [0]);
                    spriter.Add(_animationNames[Animations.BallDead], [1, 2, 3, 4, 5], 0.2f);
                }
            },
            {
                Spriters.Paddle,
                delegate(Spriter spriter)
                {
                    spriter.Add(_animationNames[Animations.Paddle], [0]);
                }
            },
            {
                Spriters.BrickLarge,
                delegate(Spriter spriter)
                {
                    spriter.Add(_animationNames[Animations.BrickLarge], [0]);
                    spriter.Add(_animationNames[Animations.BrickLargeDead], [1]);
                }
            },
            {
                Spriters.Cracks,
                delegate(Spriter spriter)
                {
                    spriter.Add(_animationNames[Animations.CrackSmall], [0]);
                    spriter.Add(_animationNames[Animations.CrackMedium], [1]);
                    spriter.Add(_animationNames[Animations.CrackLarge], [2]);
                }
            }
        });
        private static readonly ReadOnlyDictionary<Animations, Spriters> _animationSpriters = new(new Dictionary<Animations, Spriters>()
        {
            { Animations.Ball, Spriters.Ball },
            { Animations.BallDead, Spriters.Ball },
            { Animations.Paddle, Spriters.Paddle },
            { Animations.BrickLarge, Spriters.BrickLarge },
            { Animations.BrickLargeDead, Spriters.BrickLarge },
            { Animations.CrackSmall, Spriters.Cracks },
            { Animations.CrackMedium, Spriters.Cracks },
            { Animations.CrackLarge, Spriters.Cracks },
        });
        private readonly Dictionary<Spriters, Spriter> _spriters = [];
    }
}
