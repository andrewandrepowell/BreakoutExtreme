using Microsoft.Xna.Framework.Graphics;
using System;

namespace BreakoutExtreme.Shaders
{
    public class Controller
    {
        private readonly SilhouetteNode _silhouetteNode = new();
        private readonly BlurNode _blurNode = new();
        private readonly PatternNode _patternNode = new();
        private readonly HighlightCanvasItemNode _highlightCanvasItemNode = new();
        public void Begin(Feature feature)
        {
            Effect effect = null;
            BlendState blendState = null;
            switch (feature.Script)
            {
                case Scripts.Silhouette:
                    feature.UpdateShaderNode(_silhouetteNode);
                    effect = _silhouetteNode.Effect;
                    blendState = BlendState.AlphaBlend;
                    break;
                case Scripts.Blur:
                    feature.UpdateShaderNode(_blurNode);
                    effect = _blurNode.Effect;
                    blendState = BlendState.AlphaBlend;
                    break;
                case Scripts.Pattern:
                    feature.UpdateShaderNode(_patternNode);
                    effect = _patternNode.Effect;
                    blendState = BlendState.AlphaBlend;
                    break;
                case Scripts.HighlightCanvasItem:
                    feature.UpdateShaderNode(_highlightCanvasItemNode);
                    effect = _highlightCanvasItemNode.Effect;
                    blendState = BlendState.Additive;
                    break;
            }
            Globals.SpriteBatch.Begin(effect: effect, samplerState: SamplerState.PointClamp, blendState: blendState);
        }
    }
}
