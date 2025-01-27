using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Shaders
{
    public class SilhouetteNode
    {
        public readonly Effect Effect;
        public readonly EffectParameter OverlayColor;
        public SilhouetteNode()
        {
            Effect = Globals.ContentManager.Load<Effect>($"effects/silhouette_0");
            OverlayColor = Effect.Parameters["OverlayColor"];
            OverlayColor.SetValue(Color.White.ToVector4());
        }
    }
    public class PatternNode
    {
        public readonly Effect Effect;
        public readonly EffectParameter PatternTexture;
        public readonly EffectParameter SpriteTextureDimensions;
        public readonly EffectParameter SpriteRegionDimensions;
        public readonly EffectParameter PatternTextureDimensions;
        public readonly EffectParameter PatternRegionPosition;
        public readonly EffectParameter PatternRegionDimensions;
        public PatternNode()
        {
            Effect = Globals.ContentManager.Load<Effect>($"effects/pattern_0");
            PatternTexture = Effect.Parameters["PatternTexture"];
            SpriteTextureDimensions = Effect.Parameters["SpriteTextureDimensions"];
            SpriteRegionDimensions = Effect.Parameters["SpriteRegionDimensions"];
            PatternTextureDimensions = Effect.Parameters["PatternTextureDimensions"];
            PatternRegionPosition = Effect.Parameters["PatternRegionPosition"];
            PatternRegionDimensions = Effect.Parameters["PatternRegionDimensions"];
        }
    }
    public class BlurNode
    {
        public readonly Effect Effect;
        public BlurNode()
        {
        }
    }
}
