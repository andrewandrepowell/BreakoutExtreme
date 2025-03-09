using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Shaders
{
    public abstract class Feature
    {
        public virtual Scripts? Script => null;
        public virtual void Update()
        {
        }
        public virtual bool UpdateDrawOffset(ref Vector2 drawPosition)
        {
            return false;
        }
        public virtual bool UpdateVisibility(ref float visibility) 
        { 
            return false; 
        }
        public virtual bool UpdateScale(ref float scale)
        {
            return false;
        }
        public virtual bool UpdateRotation(ref float rotation)
        {
            return false;
        }
        public virtual void UpdateShaderNode(SilhouetteNode node)
        {
        }
        public virtual void UpdateShaderNode(BlurNode node)
        {
        }
        public virtual void UpdateShaderNode(PatternNode node)
        {

        }
        public virtual void UpdateShaderNode(HighlightCanvasItemNode node)
        {

        }
        public virtual void UpdateShaderNode(MaskBlurNode node)
        {

        }
        public virtual void UpdateShaderNode(SurroundBlurNode node)
        {

        }
        public virtual void UpdateShaderNode(AlterHSVNode node)
        {

        }
    }
}
