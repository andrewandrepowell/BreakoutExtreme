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
        private readonly MaskBlurNode _maskBlurNode = new();
        public void Begin(Feature feature)
        {
            Effect effect = null;
            switch (feature.Script)
            {
                case Scripts.Silhouette:
                    feature.UpdateShaderNode(_silhouetteNode);
                    effect = _silhouetteNode.Effect;
                    break;
                case Scripts.Blur:
                    feature.UpdateShaderNode(_blurNode);
                    effect = _blurNode.Effect;
                    break;
                case Scripts.Pattern:
                    feature.UpdateShaderNode(_patternNode);
                    effect = _patternNode.Effect;
                    break;
                case Scripts.HighlightCanvasItem:
                    feature.UpdateShaderNode(_highlightCanvasItemNode);
                    effect = _highlightCanvasItemNode.Effect;
                    break;
            }
            Globals.SpriteBatch.Begin(effect: effect, samplerState: SamplerState.PointClamp);
        }
    }
}
