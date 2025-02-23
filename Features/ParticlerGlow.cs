using BreakoutExtreme.Components;
using BreakoutExtreme.Shaders;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Features
{
    public class ParticlerGlow(Particler parent) : Feature
    {
        private Particler _parent = parent;
        private bool _gray = true;
        private bool _running = false;
        private Color _color = Color.White;
        public override Scripts? Script => _running ? Scripts.SurroundBlur : null;
        public Particler Parent { get => _parent; set => _parent = value; }
        public Color Color { get => _color; set => _color = value; }
        public void Start()
        {
            _running = true;
        }
        public void Stop()
        {
            _running = false;
        }
        public override void UpdateShaderNode(SurroundBlurNode node)
        {
            Debug.Assert(_running);
            var texture = _parent.GetParticleEffect().Emitters[0].TextureRegion.Texture;
            node.Configure(textureSize: texture.Bounds.Size, color: _color);
        }
    }
}
