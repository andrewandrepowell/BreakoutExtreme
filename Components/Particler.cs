using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles;
using System;
using System.Diagnostics;
using BreakoutExtreme.Utility;


namespace BreakoutExtreme.Components
{
    public partial class Particler
    {
        private bool _disposed = false;
        private Particles _particle = Particles.BallTrail;
        private ParticleEffect _particleEffect;
        private Vector2 _position, _drawPosition;
        private bool _running = true;
        private void UpdateParticleEffectAutoTrigger()
        {
            for (var i = 0; i < _particleEffect.Emitters.Count; i++)
            {
                _particleEffect.Emitters[i].AutoTrigger = _running;
            }
        }
        private void UpdateParticleEffectPosition()
        {
            _particleEffect.Position = new Vector2(
                x: (float)Math.Floor(_position.X), 
                y: (float)Math.Floor(_position.Y));
        }
        private void UpdateParticleEffects()
        {
            var assetName = _particleAssetNames[_particle];
            var texture = Globals.ContentManager.Load<Texture2D>(assetName);
            var region = _particleRegions[_particle];
            var textureRegion = new Texture2DRegion(texture, region);
            var particleEffect = _particleCreateActions[_particle](textureRegion);
            _particleEffects.Add(_particle, particleEffect);
        }
        private void UpdateParticleEffect()
        {
            _particleEffect = _particleEffects[_particle];
        }
        public bool Disposed => _disposed;
        public Particles Particle => _particle;
        public Vector2 Position
        {
            get => _position;
            set
            {
                Debug.Assert(!_disposed);
                if (_position == value)
                    return;
                _position = value;
                UpdateParticleEffectPosition();
            }
        }
        public Layers Layer = Layers.Ground;
        public bool Running => _running;
        public void Start()
        {
            Debug.Assert(!_disposed);
            if (_running)
                return;
            _running = true;
            UpdateParticleEffectAutoTrigger();
        }
        public void Stop()
        {
            Debug.Assert(!_disposed);
            if (!_running)
                return;
            _running = false;
            UpdateParticleEffectAutoTrigger();
        }
        public void Play(Particles particle)
        {
            Debug.Assert(!_disposed);
            if (_particle == particle)
                return;
            _particle = particle;
            if (!_particleEffects.ContainsKey(_particle))
                UpdateParticleEffects();
            UpdateParticleEffect();
            UpdateParticleEffectAutoTrigger();
            UpdateParticleEffectPosition();
        }
        public void Dispose()
        {
            Debug.Assert(!_disposed);
            _disposed = true;
            foreach (var particleEffect in _particleEffects.Values)
                particleEffect.Dispose();
        }
        public void Update()
        {
            Debug.Assert(!_disposed);
            _particleEffect.Update(Globals.GameTime.GetElapsedSeconds());
        }
        public void Draw()
        {
            Debug.Assert(!_disposed);
            Globals.SpriteBatch.Draw(_particleEffect);
        }
        public Particler(Particles particle = Particles.BallTrail)
        {
            _particle = particle;
            UpdateParticleEffects();
            UpdateParticleEffect();
            UpdateParticleEffectAutoTrigger();
            UpdateParticleEffectPosition();
        }

    }
}
