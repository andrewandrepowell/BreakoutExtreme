using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles;
using System;
using System.Diagnostics;
using BreakoutExtreme.Utility;
using MonoGame.Extended.Collections;


namespace BreakoutExtreme.Components
{
    public partial class Particler
    {
        private bool _disposed = false;
        private Particles _particle = Particles.BallTrail;
        private ParticleEffect _particleEffect;
        private Vector2 _position;
        private bool _running;
        private void UpdateRunning()
        {
            _running = _particleEffect.Emitters[0].AutoTrigger;
#if DEBUG
            for (var i = 0; i < _particleEffect.Emitters.Count; i++)
            {
                Debug.Assert(_particleEffect.Emitters[i].AutoTrigger == _running);
            }
#endif
        }
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
            var regionSize = _particleRegionSizes[_particle];
            Debug.Assert(texture.Width % regionSize.Width == 0);
            Debug.Assert(texture.Height % regionSize.Height == 0);
            var xRegions = texture.Width / regionSize.Width;
            var yRegions = texture.Height / regionSize.Height;
            var totalRegions = xRegions * yRegions;
            var textureRegions = new Texture2DRegion[totalRegions];
            for (var y = 0; y < yRegions; y++)
            {
                for (var x = 0; x < xRegions; x++)
                {
                    textureRegions[x + y * xRegions] = new Texture2DRegion(texture, new Rectangle(
                        x * regionSize.Width,
                        y * regionSize.Height,
                        regionSize.Width,
                        regionSize.Height));
                }
            }
            var particleEffect = _particleCreateActions[_particle](textureRegions);
            _particleEffects.Add(_particle, particleEffect);
            _particles.Add(_particle);
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
        public bool Disposable = true; // prevents the runner from disposing the particler if false. Used for pulled components.
        public static void Load()
        {
            var particler = new Particler();
            var particles = Enum.GetValues<Particles>();
            foreach (ref var particle in particles.AsSpan())
            {
                particler.Play(particle);
            }
        }
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
        public void Trigger()
        {
            Debug.Assert(!_disposed);
            _particleEffect.Trigger();
        }
        public void Dispose()
        {
            Debug.Assert(!_disposed);
            _disposed = true;
            for (var i = 0; i < _particles.Count; i++)
                _particleEffects[_particles[i]].Dispose();
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
            UpdateRunning();
            UpdateParticleEffectAutoTrigger();
            UpdateParticleEffectPosition();
        }

    }
}
