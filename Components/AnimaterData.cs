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
            { Spriters.PaddleLarge, "animations/paddle_1" },
            { Spriters.BrickSmall, "animations/brick_0" },
            { Spriters.BrickLarge, "animations/brick_2" },
            { Spriters.Cracks, "animations/cracks_0" },
            { Spriters.Spike, "animations/spike_0" },
            { Spriters.Laser, "animations/laser_0" },
            { Spriters.EmpoweredLaser, "animations/laser_1" },
            { Spriters.Cannon, "animations/cannon_0" },
            { Spriters.Bomb, "animations/bomb_0" },
            { Spriters.Cleared, "animations/cleared_0" },
            { Spriters.GameEnd, "animations/game_end_0" },
            { Spriters.GameStart, "animations/game_start_0" },
            { Spriters.Powers, "animations/powers_0" },
        });
        private static readonly ReadOnlyDictionary<Spriters, Size> _spriterRegionSizes = new(new Dictionary<Spriters, Size>
        {
            { Spriters.Ball, new Size(80, 80) },
            { Spriters.Paddle, new Size(144, 80) },
            { Spriters.PaddleLarge, new Size(192, 80) },
            { Spriters.BrickSmall, new Size(80, 80) },
            { Spriters.BrickLarge, new Size(112, 80) },
            { Spriters.Cracks, new Size(16, 16) },
            { Spriters.Spike, new Size(80, 80) },
            { Spriters.Laser, new Size(80, 96) },
            { Spriters.EmpoweredLaser, new Size(80, 96) },
            { Spriters.Cannon, new Size(96, 96) },
            { Spriters.Bomb, new Size(80, 80) },
            { Spriters.Cleared, new Size(208, 96) },
            { Spriters.GameEnd, new Size(240, 96) },
            { Spriters.GameStart, new Size(256, 96) },
            { Spriters.Powers, new Size(48, 48) },
        });
        private static readonly ReadOnlyDictionary<Animations, string> _animationNames = new(new Dictionary<Animations, string>
        {
            { Animations.Ball, "ball_0" },
            { Animations.BallDead, "ball_1" },
            { Animations.Paddle, "paddle_0" },
            { Animations.PaddleDead, "paddle_1" },
            { Animations.PaddleLarge, "paddle_0" },
            { Animations.PaddleLargeDead, "paddle_1" },
            { Animations.BrickSmall, "brick_0" },
            { Animations.BrickSmallDead, "brick_1" },
            { Animations.BrickLarge, "brick_0" },
            { Animations.BrickLargeDead, "brick_1" },
            { Animations.CrackSmall, "crack_0" },
            { Animations.CrackMedium, "crack_1" },
            { Animations.CrackLarge, "crack_2" },
            { Animations.Spike, "spike_0" },
            { Animations.SpikeSolidifying, "spike_1" },
            { Animations.SpikeSolid, "spike_2" },
            { Animations.SpikeEdgeSolidifying, "spike_3" },
            { Animations.SpikeEdgeSolid, "spike_4" },
            { Animations.Laser, "laser_0" },
            { Animations.EmpoweredLaser, "laser_0" },
            { Animations.Cannon, "cannon_0" },
            { Animations.CannonDead, "cannon_1" },
            { Animations.CannonFire, "cannon_2" },
            { Animations.Bomb, "bomb_0" },
            { Animations.BombDead, "bomb_1" },
            { Animations.Cleared, "cleared_0" },
            { Animations.GameEnd, "game_end_0" },
            { Animations.GameStart, "game_start_0" },
            { Animations.PowerBackpane, "powers_0" },
            { Animations.PowerProtection, "powers_1" },
            { Animations.PowerProtectionDead, "powers_2" },
            { Animations.PowerNewBall, "powers_3" },
            { Animations.PowerNewBallDead, "powers_4" },
            { Animations.PowerEnlargePaddle, "powers_5" },
            { Animations.PowerEnlargePaddleDead, "powers_6" },
            { Animations.PowerEmpoweredLaser, "powers_7" },
            { Animations.PowerEmpoweredLaserDead, "powers_8" },
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
                Spriters.PaddleLarge,
                delegate(Spriter spriter)
                {
                    spriter.Add(_animationNames[Animations.PaddleLarge], [0]);
                    spriter.Add(_animationNames[Animations.PaddleLargeDead], [1]);
                }
            },
            {
                Spriters.BrickSmall,
                delegate(Spriter spriter)
                {
                    spriter.Add(_animationNames[Animations.BrickSmall], [0]);
                    spriter.Add(_animationNames[Animations.BrickSmallDead], [1]);
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
                    spriter.Add(_animationNames[Animations.SpikeSolidifying], [4, 5, 6, 7], 0.5f);
                    spriter.Add(_animationNames[Animations.SpikeSolid], [7]);
                    spriter.Add(_animationNames[Animations.SpikeEdgeSolidifying], [8, 9, 10, 11], 0.5f);
                    spriter.Add(_animationNames[Animations.SpikeEdgeSolid], [11]);
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
                Spriters.EmpoweredLaser,
                delegate(Spriter spriter)
                {
                    spriter.Add(_animationNames[Animations.EmpoweredLaser], [3, 4, 5], 0.15f, true);
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
            },
            {
                Spriters.GameStart,
                delegate(Spriter spriter)
                {
                    spriter.Add(_animationNames[Animations.GameStart], [0]);
                }
            },
            {
                Spriters.Powers,
                delegate(Spriter spriter)
                {
                    spriter.Add(_animationNames[Animations.PowerBackpane], [0]);
                    spriter.Add(_animationNames[Animations.PowerProtection], [2]);
                    spriter.Add(_animationNames[Animations.PowerProtectionDead], [3]);
                    spriter.Add(_animationNames[Animations.PowerNewBall], [6]);
                    spriter.Add(_animationNames[Animations.PowerNewBallDead], [7]);
                    spriter.Add(_animationNames[Animations.PowerEnlargePaddle], [10]);
                    spriter.Add(_animationNames[Animations.PowerEnlargePaddleDead], [11]);
                    spriter.Add(_animationNames[Animations.PowerEmpoweredLaser], [14]);
                    spriter.Add(_animationNames[Animations.PowerEmpoweredLaserDead], [15]);
                }
            }
        });
        private static readonly ReadOnlyDictionary<Animations, Spriters> _animationSpriters = new(new Dictionary<Animations, Spriters>()
        {
            { Animations.Ball, Spriters.Ball },
            { Animations.BallDead, Spriters.Ball },
            { Animations.Paddle, Spriters.Paddle },
            { Animations.PaddleDead, Spriters.Paddle },
            { Animations.PaddleLarge, Spriters.PaddleLarge },
            { Animations.PaddleLargeDead, Spriters.PaddleLarge },
            { Animations.BrickSmall, Spriters.BrickSmall },
            { Animations.BrickSmallDead, Spriters.BrickSmall },
            { Animations.BrickLarge, Spriters.BrickLarge },
            { Animations.BrickLargeDead, Spriters.BrickLarge },
            { Animations.CrackSmall, Spriters.Cracks },
            { Animations.CrackMedium, Spriters.Cracks },
            { Animations.CrackLarge, Spriters.Cracks },
            { Animations.Spike, Spriters.Spike },
            { Animations.SpikeSolidifying, Spriters.Spike },
            { Animations.SpikeSolid, Spriters.Spike },
            { Animations.SpikeEdgeSolidifying, Spriters.Spike },
            { Animations.SpikeEdgeSolid, Spriters.Spike },
            { Animations.Laser, Spriters.Laser },
            { Animations.EmpoweredLaser, Spriters.EmpoweredLaser },
            { Animations.Cannon, Spriters.Cannon },
            { Animations.CannonFire, Spriters.Cannon },
            { Animations.CannonDead, Spriters.Cannon },
            { Animations.Bomb, Spriters.Bomb },
            { Animations.BombDead, Spriters.Bomb },
            { Animations.Cleared, Spriters.Cleared },
            { Animations.GameEnd, Spriters.GameEnd },
            { Animations.GameStart, Spriters.GameStart },
            { Animations.PowerBackpane, Spriters.Powers },
            { Animations.PowerProtection, Spriters.Powers },
            { Animations.PowerProtectionDead, Spriters.Powers },
            { Animations.PowerNewBall, Spriters.Powers },
            { Animations.PowerNewBallDead, Spriters.Powers },
            { Animations.PowerEnlargePaddle, Spriters.Powers },
            { Animations.PowerEnlargePaddleDead, Spriters.Powers },
            { Animations.PowerEmpoweredLaser, Spriters.Powers },
            { Animations.PowerEmpoweredLaserDead, Spriters.Powers },
        });
        private readonly Dictionary<Spriters, Spriter> _spriters = [];
    }
}
