using System.Collections.Generic;
using System.Collections.ObjectModel;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using BreakoutExtreme.Utility;

namespace BreakoutExtreme.Components
{
    public partial class Brick
    {
        private static readonly ReadOnlyDictionary<Bricks, BrickConfig> _brickConfigs = new(new Dictionary<Bricks, BrickConfig>() 
        {
            {
                Bricks.Small,
                new(bounds: new Rectangle(Globals.PlayAreaBlockBounds.X, Globals.PlayAreaBlockBounds.Y, 1, 1).ToBounds(),
                    activeAnimation: Animater.Animations.BrickSmall,
                    deadAnimation: Animater.Animations.BrickSmallDead,
                    totalHP: 1)
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
