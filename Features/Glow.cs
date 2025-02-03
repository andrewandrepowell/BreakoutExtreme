using BreakoutExtreme.Components;
using BreakoutExtreme.Shaders;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Diagnostics;
using System;

namespace BreakoutExtreme.Features
{
    public class Glow(Animater parent) : Feature
    {
        private Animater _parent = parent;
        private bool _gray = true;
        private bool _running = false;
        public override Scripts? Script => _running ? Scripts.Blur : null;
        public Animater Parent { get => _parent; set => _parent = value; }
        public void Start()
        {
            _running = true;
        }
        public void Stop()
        {
            _running = false;
        }
        public override void UpdateShaderNode(BlurNode node)
        {
            Debug.Assert(_running);
            node.Configure(textureSize: _parent.Texture.Bounds.Size, gray: _gray);
        }
    }
}
