using MonoGame.Extended;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

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
            { Spriters.Spike, "animations/spike_0" },
            { Spriters.Laser, "animations/laser_0" },
            { Spriters.Cannon, "animations/cannon_0" },
            { Spriters.Bomb, "animations/bomb_0" },
            { Spriters.Cleared, "animations/cleared_0" },
            { Spriters.GameEnd, "animations/game_end_0" },
        });
        private static readonly ReadOnlyDictionary<Spriters, Size> _spriterRegionSizes = new(new Dictionary<Spriters, Size>
        {
            { Spriters.Ball, new Size(80, 80) },
            { Spriters.Paddle, new Size(144, 80) },
            { Spriters.BrickLarge, new Size(112, 80) },
            { Spriters.Cracks, new Size(16, 16) },
            { Spriters.Spike, new Size(80, 80) },
            { Spriters.Laser, new Size(80, 96) },
            { Spriters.Cannon, new Size(96, 96) },
            { Spriters.Bomb, new Size(80, 80) },
            { Spriters.Cleared, new Size(208, 96) },
            { Spriters.GameEnd, new Size(240, 96) },
        });
        private static readonly ReadOnlyDictionary<Animations, string> _animationNames = new(new Dictionary<Animations, string>
        {
            { Animations.Ball, "ball_0" },
            { Animations.BallDead, "ball_1" },
            { Animations.Paddle, "paddle_0" },
            { Animations.PaddleDead, "paddle_dead_0" },
            { Animations.BrickLarge, "brick_0" },
            { Animations.BrickLargeDead, "brick_1" },
            { Animations.CrackSmall, "crack_0" },
            { Animations.CrackMedium, "crack_1" },
            { Animations.CrackLarge, "crack_2" },
            { Animations.Spike, "spike_0" },
            { Animations.Laser, "laser_0" },
            { Animations.Cannon, "cannon_0" },
            { Animations.CannonDead, "cannon_1" },
            { Animations.CannonFire, "cannon_2" },
            { Animations.Bomb, "bomb_0" },
            { Animations.BombDead, "bomb_1" },
            { Animations.Cleared, "cleared_0" },
            { Animations.GameEnd, "game_end_0" },
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
                    spriter.Add(_animationNames[Animations.PaddleDead], [1]);
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
            },
            {
                Spriters.Spike,
                delegate(Spriter spriter)
                {
                    spriter.Add(_animationNames[Animations.Spike], [0, 1, 2, 3, 2, 1], 1, true);
                }
            },
            {
                Spriters.Laser,
                delegate(Spriter spriter)
                {
                    spriter.Add(_animationNames[Animations.Laser], [0]);
                }
            },
            {
                Spriters.Cannon,
                delegate(Spriter spriter)
                {
                    spriter.Add(_animationNames[Animations.Cannon], [0]);
                    spriter.Add(_animationNames[Animations.CannonFire], [1, 2, 3, 4], 0.25f);
                    spriter.Add(_animationNames[Animations.CannonDead], [5]);
                }
            },
            {
                Spriters.Bomb,
                delegate(Spriter spriter)
                {
                    spriter.Add(_animationNames[Animations.Bomb], [0]);
                    spriter.Add(_animationNames[Animations.BombDead], [1]);
                }
            },
            {
                Spriters.Cleared,
                delegate(Spriter spriter)
                {
                    spriter.Add(_animationNames[Animations.Cleared], [0]);
                }
            },
            {
                Spriters.GameEnd,
                delegate(Spriter spriter)
                {
                    spriter.Add(_animationNames[Animations.GameEnd], [0]);
                }
            }
        });
        private static readonly ReadOnlyDictionary<Animations, Spriters> _animationSpriters = new(new Dictionary<Animations, Spriters>()
        {
            { Animations.Ball, Spriters.Ball },
            { Animations.BallDead, Spriters.Ball },
            { Animations.Paddle, Spriters.Paddle },
            { Animations.PaddleDead, Spriters.Paddle },
            { Animations.BrickLarge, Spriters.BrickLarge },
            { Animations.BrickLargeDead, Spriters.BrickLarge },
            { Animations.CrackSmall, Spriters.Cracks },
            { Animations.CrackMedium, Spriters.Cracks },
            { Animations.CrackLarge, Spriters.Cracks },
            { Animations.Spike, Spriters.Spike },
            { Animations.Laser, Spriters.Laser },
            { Animations.Cannon, Spriters.Cannon },
            { Animations.CannonFire, Spriters.Cannon },
            { Animations.CannonDead, Spriters.Cannon },
            { Animations.Bomb, Spriters.Bomb },
            { Animations.BombDead, Spriters.Bomb },
            { Animations.Cleared, Spriters.Cleared },
            { Animations.GameEnd, Spriters.GameEnd },
        });
        private readonly Dictionary<Spriters, Spriter> _spriters = [];
    }
}
