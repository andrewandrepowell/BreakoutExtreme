using System.Collections.Generic;
using System.Collections.ObjectModel;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using BreakoutExtreme.Utility;

namespace BreakoutExtreme.Components
{
    public partial class Brick
    {
        private static readonly ReadOnlyDictionary<Bricks, RectangleF> _brickBounds = new(new Dictionary<Bricks, RectangleF>()
        {
            { Bricks.ThickBrick, new Rectangle(Globals.PlayAreaBlockBounds.X, Globals.PlayAreaBlockBounds.Y, 3, 1).ToBounds() }
        });
        private static readonly ReadOnlyDictionary<Bricks, Animater.Animations> _brickAnimations = new(new Dictionary<Bricks, Animater.Animations>()
        {
            { Bricks.ThickBrick, Animater.Animations.BrickLarge }
        });
        private static readonly ReadOnlyDictionary<Bricks, Animater.Animations> _brickDeadAnimations = new(new Dictionary<Bricks, Animater.Animations>()
        {
            { Bricks.ThickBrick, Animater.Animations.BrickLargeDead }
        });
        private static readonly ReadOnlyDictionary<Bricks, int> _brickTotalHPs = new(new Dictionary<Bricks, int>()
        {
            { Bricks.ThickBrick, 3 }
        });
    }
}
