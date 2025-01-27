using BreakoutExtreme.Utility;

namespace BreakoutExtreme.Shaders
{
    public abstract class Feature
    {
        public abstract Scripts Script { get; }
        public abstract RunningStates RunningState { get; }
        public virtual void Update(SilhouetteNode node)
        {
        }
        public virtual void Update(BlurNode node)
        {
        }
    }
}
