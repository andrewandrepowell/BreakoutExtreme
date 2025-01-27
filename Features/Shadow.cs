using BreakoutExtreme.Shaders;
using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Features
{
    public class Shadow : Feature
    {
        public override Scripts? Script => Scripts.Silhouette;
        public override void UpdateShaderNode(SilhouetteNode node)
        {
            node.OverlayColor.SetValue(Color.Black.ToVector4());
        }
    }
}
