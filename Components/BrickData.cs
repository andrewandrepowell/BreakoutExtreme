using System.Collections.Generic;
using System.Collections.ObjectModel;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using BreakoutExtreme.Utility;

namespace BreakoutExtreme.Components
{
    public partial class Brick
    {
        private static readonly ReadOnlyDictionary<Powers, PowerConfig> _powerConfigs = new(new Dictionary<Powers, PowerConfig>() 
        {
            { Powers.NewBall, new(Tint: new(0xcf573cff)) },
            { Powers.MultiBall, new(Tint: new(0x411d31ff)) },
            { Powers.Empowered, new(Tint: new(0xdf2c95ff)) },
            { Powers.Protection, new(Tint: new(0xa1bbe6ff)) },
            { Powers.EnlargePaddle, new(Tint: new(0x788c8cff)) },
        });
        private static readonly ReadOnlyDictionary<Bricks, BrickConfig> _brickConfigs = new(new Dictionary<Bricks, BrickConfig>() 
        {
            {
                Bricks.Power,
                new(bounds: new Rectangle(Globals.PlayAreaBlockBounds.X, Globals.PlayAreaBlockBounds.Y, 1, 1).ToBounds(),
                    activeAnimation: Animater.Animations.BrickSmall,
                    deadAnimation: Animater.Animations.BrickSmallDead,
                    totalHP: 1,
                    glow: Color.LightCyan)
            },
            {
                Bricks.Small,
                new(bounds: new Rectangle(Globals.PlayAreaBlockBounds.X, Globals.PlayAreaBlockBounds.Y, 1, 1).ToBounds(),
                    activeAnimation: Animater.Animations.BrickSmall,
                    deadAnimation: Animater.Animations.BrickSmallDead,
                    totalHP: 1)
            },
            {
                Bricks.Medium,
                new(bounds: new Rectangle(Globals.PlayAreaBlockBounds.X, Globals.PlayAreaBlockBounds.Y, 2, 1).ToBounds(),
                    activeAnimation: Animater.Animations.BrickMedium,
                    deadAnimation: Animater.Animations.BrickMediumDead,
                    totalHP: 2)
            },
            {
                Bricks.Large,
                new(bounds: new Rectangle(Globals.PlayAreaBlockBounds.X, Globals.PlayAreaBlockBounds.Y, 3, 1).ToBounds(),
                    activeAnimation: Animater.Animations.BrickLarge,
                    deadAnimation: Animater.Animations.BrickLargeDead,
                    totalHP: 3)
            }
        });
    }
}
