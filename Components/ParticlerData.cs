using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Containers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;
using System.Collections.Generic;

namespace BreakoutExtreme.Components
{
    public partial class Particler
    {
        private readonly static ReadOnlyDictionary<Particles, string> _particleAssetNames = new(new Dictionary<Particles, string>()
        {
            { Particles.BallTrail, "animations/particle_0" }
        });
        private readonly static ReadOnlyDictionary<Particles, Rectangle> _particleRegions = new(new Dictionary<Particles, Rectangle>()
        {
            { Particles.BallTrail, new Rectangle(0, 0, 2, 2) }
        });
        private readonly static ReadOnlyDictionary<Particles, Func<Texture2DRegion, ParticleEffect>> _particleCreateActions = new(new Dictionary<Particles, Func<Texture2DRegion, ParticleEffect>>() 
        {
            {
                Particles.BallTrail,
                delegate(Texture2DRegion textureRegion)
                {
                    var particleEffect = new ParticleEffect()
                    {
                        Emitters = new List<ParticleEmitter>
                        {
                            new ParticleEmitter(
                                textureRegion: textureRegion, 
                                capacity: 500, 
                                lifeSpan: TimeSpan.FromSeconds(0.15f),
                                profile: Profile.Circle(4, Profile.CircleRadiation.In))
                            {
                                Parameters = new ParticleReleaseParameters
                                {
                                    Quantity = 6,
                                    Speed = new Range<float>(1, 1),
                                    Scale = new Range<float>(1.0f, 2.0f),
                                    Color = new Color(117, 36, 56).ToHsl()
                                },
                                Modifiers =
                                {
                                    new OpacityFastFadeModifier(),
                                }
                            }
                        }
                    };
                    return particleEffect;
                }
            }
        });
        private readonly Dictionary<Particles, ParticleEffect> _particleEffects = new();
    }
}
