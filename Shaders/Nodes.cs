using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Shaders
{
    public class SilhouetteNode
    {
        public Effect Effect { get; }
        public EffectParameter OverlayColor { get; }
        public SilhouetteNode()
        {
            Effect = Globals.ContentManager.Load<Effect>($"effects/silhouette_0");
            OverlayColor = Effect.Parameters["OverlayColor"];
            OverlayColor.SetValue(Color.White.ToVector4());
        }
    }
    public class BlurNode
    {
        public Effect Effect { get; }
        public BlurNode()
        {
        }
    }
}
