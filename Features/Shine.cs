using BreakoutExtreme.Shaders;
using MonoGame.Extended;
using System.Diagnostics;

namespace BreakoutExtreme.Features
{
    public class Shine : Feature
    {
        private bool _running = false;
        private float _initialGameTimeSeconds;
        public bool Running => _running;
        public override Scripts? Script => (_running) ? Scripts.HighlightCanvasItem : null;
        public void Start()
        {
            _initialGameTimeSeconds = (float)Globals.GameTime.TotalGameTime.TotalSeconds;
            _running = true;
        }
        public void Stop()
        {
            _running = false;
        }
        public override void UpdateShaderNode(HighlightCanvasItemNode node)
        {
            Debug.Assert(_running);
            node.Configure(
                //positionMin: 0, 
                //positionMax: 1, 
                speed: 0.1f,
                initialGameTimeSeconds: _initialGameTimeSeconds);
        }
    }
}
