using Microsoft.Xna.Framework.Graphics;

namespace BreakoutExtreme.Shaders
{
    public class Controller
    {
        private readonly SilhouetteNode _silhouetteNode = new SilhouetteNode();
        private readonly BlurNode _blurNode = new BlurNode();
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
            }
            Globals.SpriteBatch.Begin(effect: effect, samplerState: SamplerState.PointClamp);
        }
    }
}
