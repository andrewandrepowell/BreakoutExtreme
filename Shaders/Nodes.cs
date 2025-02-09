using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using BreakoutExtreme.Utility;
using System.Linq;

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
        public readonly EffectParameter SpriteTextureDimensions;
        public readonly EffectParameter Gray;
        public void Configure(Size textureSize, bool gray = false)
        {
            SpriteTextureDimensions.SetValue(textureSize.ToVector2());
            Gray.SetValue((gray) ? 1 : 0);
        }
        public BlurNode()
        {
            Effect = Globals.ContentManager.Load<Effect>($"effects/blur_0");
            SpriteTextureDimensions = Effect.Parameters["SpriteTextureDimensions"];
            Gray = Effect.Parameters["Gray"];
        }
    }
    public class HighlightCanvasItemNode
    {
        public readonly Effect Effect;
        public readonly EffectParameter LineSmoothness; //: hint_range(0, 0.1) = 0.045;
        public readonly EffectParameter LineWidth; //: hint_range(0, 0.2) = 0.09;
        public readonly EffectParameter Brightness; // = 3.0;
        public readonly EffectParameter Rotation; //: hint_range(-90, 90) = 30;
        public readonly EffectParameter Distortion; //: hint_range(1, 2) = 1.8;
        public readonly EffectParameter Speed; //= 0.7;
        public readonly EffectParameter Position; //: hint_range(0, 1) = 0;
        public readonly EffectParameter PositionMin; //= 0.25;
        public readonly EffectParameter PositionMax; //= 0.5;
        public readonly EffectParameter Alpha; //: hint_range(0, 1) = 1;
        public readonly EffectParameter GameTimeSeconds;
        public void Configure(
            float lineSmoothness = 0.045f,
            float lineWidth = 0.09f,
            float brightness = 3.0f,
            float rotation = 0.5235987755982988f,
            float distortion = 1.8f,
            float speed = 0.7f,
            float position = 0,
            float positionMin = 0.25f,
            float positionMax = 0.5f,
            float alpha = 1f,
            float initialGameTimeSeconds = 0f)
        {
            LineSmoothness.SetValue(lineSmoothness);
            LineWidth.SetValue(lineWidth);
            Brightness.SetValue(brightness);
            Rotation.SetValue(rotation);
            Distortion.SetValue(distortion);
            Speed.SetValue(speed);
            Position.SetValue(position);
            PositionMin.SetValue(positionMin);
            PositionMax.SetValue(positionMax);
            Alpha.SetValue(alpha);
            GameTimeSeconds.SetValue((float)Globals.GameTime.TotalGameTime.TotalSeconds - initialGameTimeSeconds);
        }
        public HighlightCanvasItemNode()
        {
            Effect = Globals.ContentManager.Load<Effect>($"effects/hightlight_canvasitem_0");
            LineSmoothness = Effect.Parameters["LineSmoothness"];
            LineWidth = Effect.Parameters["LineWidth"];
            Brightness = Effect.Parameters["Brightness"];
            Rotation = Effect.Parameters["Rotation"];
            Distortion = Effect.Parameters["Distortion"];
            Speed = Effect.Parameters["Speed"];
            Position = Effect.Parameters["Position"];
            PositionMin = Effect.Parameters["PositionMin"];
            PositionMax = Effect.Parameters["PositionMax"];
            Alpha = Effect.Parameters["Alpha"];
            GameTimeSeconds = Effect.Parameters["GameTimeSeconds"];
        }
    }
    public class MaskBlurNode
    {
        public const int MaskSize = 9;
        public readonly static float[] DefaultMask = Enumerable
            .Range(0, MaskSize)
            .Select( x => (float)1 / MaskSize)
            .ToArray();
        public readonly Effect Effect;
        public readonly EffectParameter SpriteTextureDimensions;
        public readonly EffectParameter Spread;
        public readonly EffectParameter Mask;
        public void Configure(Size textureSize, float spread = 5.0f, float[] mask = null)
        {
            if (mask == null)
                mask = DefaultMask;
            SpriteTextureDimensions.SetValue(textureSize.ToVector2());
            Spread.SetValue(spread);
            Mask.SetValue(mask);
        }
        public MaskBlurNode()
        {
            Effect = Globals.ContentManager.Load<Effect>($"effects/mask_blur_0");
            SpriteTextureDimensions = Effect.Parameters["SpriteTextureDimensions"];
            Spread = Effect.Parameters["Spread"];
            Mask = Effect.Parameters["Mask"];
        }
    }
}
