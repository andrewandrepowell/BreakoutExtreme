using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public class Spike
    {
        private readonly static ReadOnlyDictionary<Edges, EdgeConfig> _edgeConfigs = new(new Dictionary<Edges, EdgeConfig>()
        {
            { Edges.None, new(Animater.Animations.SpikeSolidifying, SpriteEffects.None) },
            { Edges.Left, new(Animater.Animations.SpikeEdgeSolidifying, SpriteEffects.None) },
            { Edges.Right, new(Animater.Animations.SpikeEdgeSolidifying, SpriteEffects.FlipHorizontally) }
        });
        private readonly Animater _animater;
        private readonly Features.FloatDown _floatDown;
        private readonly Features.LimitedFlash _flash;
        private readonly Edges _edge;
        private readonly EdgeConfig _edgeConfig;
        private States _state;
        private class EdgeConfig(
            Animater.Animations solidifyingAnimation,
            SpriteEffects spriteEffect)
        {
            public readonly Animater.Animations SolidifyingAnimation = solidifyingAnimation;
            public readonly SpriteEffects SpriteEffect = spriteEffect;
        }
        public Animater GetAnimater() => _animater;
        public enum Edges { None, Left, Right }
        public enum States { Waiting, Active, Protecting }
        public States State => _state;
        public void Start()
        {
            Debug.Assert(_state == States.Waiting);
            _floatDown.Stop();
            _state = States.Active;
        }
        public void Stop()
        {
            Debug.Assert(_state == States.Active);
            _floatDown.Start();
            _state = States.Waiting;
        }
        public void Protect()
        {
            Debug.Assert(_state == States.Active);
            _animater.SpriteEffect = _edgeConfig.SpriteEffect;
            _animater.Play(_edgeConfig.SolidifyingAnimation);
            _state = States.Protecting;
        }
        public void ReleaseProtect()
        {
            Debug.Assert(_state == States.Protecting);
            _flash.Start();
            _animater.SpriteEffect = SpriteEffects.None;
            _animater.Play(Animater.Animations.Spike);
            _state = States.Active;
        }
        public Spike(Vector2 position, Edges edge)
        {
            _edge = edge;
            _edgeConfig = _edgeConfigs[edge];
            _state = States.Waiting;
            _animater = new() { Pausable = false };
            _flash = new();
            _floatDown = new();
            _floatDown.Period = 2;
            _floatDown.MinHeight = Globals.GameBlockSize;
            _floatDown.ForceStart();
            _animater.ShaderFeatures.Add(_flash);
            _animater.ShaderFeatures.Add(_floatDown);
            _animater.Play(Animater.Animations.Spike);
            _animater.Position = position;
        }
    }
}
