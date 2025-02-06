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
using System.Linq;
using MonoGame.Extended.Collections;

namespace BreakoutExtreme.Components
{
    public partial class Particler
    {
        private readonly static ReadOnlyDictionary<Particles, string> _particleAssetNames = new(new Dictionary<Particles, string>()
        {
            { Particles.BallTrail, "animations/particle_0" },
            { Particles.BrickBreak, "animations/particle_1" },
            { Particles.CannonBlast, "animations/fireball_0" },
            { Particles.BombBlast, "animations/fireball_0" },
        });
        private readonly static ReadOnlyDictionary<Particles, Size> _particleRegionSizes = new(new Dictionary<Particles, Size>()
        {
            { Particles.BallTrail, new Size(1, 1) },
            { Particles.BrickBreak, new Size(16, 16) },
            { Particles.CannonBlast, new Size(16, 16) },
            { Particles.BombBlast, new Size(16, 16) }
        });
        private readonly static ReadOnlyDictionary<Particles, Func<Texture2DRegion[], ParticleEffect>> _particleCreateActions = new(new Dictionary<Particles, Func<Texture2DRegion[], ParticleEffect>>() 
        {
            {
                Particles.BallTrail,
                delegate(Texture2DRegion[] textureRegions)
                {
                    var particleEffect = new ParticleEffect()
                    {
                        Emitters = new List<ParticleEmitter>
                        {
                            new ParticleEmitter(
                                textureRegion: textureRegions[0],
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
                            },
                            new ParticleEmitter(
                                textureRegion: textureRegions[0],
                                capacity: 500,
                                lifeSpan: TimeSpan.FromSeconds(0.10f),
                                profile: Profile.Circle(2, Profile.CircleRadiation.In))
                            {
                                Parameters = new ParticleReleaseParameters
                                {
                                    Quantity = 3,
                                    Speed = new Range<float>(1, 1),
                                    Scale = new Range<float>(1.0f, 2.0f),
                                    Color = new Color(207, 87, 60).ToHsl()
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
            },
            {
                Particles.BrickBreak,
                delegate(Texture2DRegion[] textureRegions)
                {
                    var sprayProfile = Profile.Spray(new Vector2(0, -1), 500);
                    var particleReleaseParameters = new ParticleReleaseParameters
                    {
                        Quantity = 3,
                        Speed = new Range<float>(100, 100),
                    };
                    var opacityFastModifier = new OpacityFastFadeModifier();
                    var particleEffect = new ParticleEffect()
                    {
                        Emitters = 
                        {
                            new ParticleEmitter(
                                textureRegion: textureRegions[0],
                                capacity: 3,
                                lifeSpan: TimeSpan.FromSeconds(0.5),
                                profile: sprayProfile)
                            {
                                Parameters = particleReleaseParameters,
                                Modifiers = { opacityFastModifier, },
                                AutoTrigger = false
                            },
                            new ParticleEmitter(
                                textureRegion: textureRegions[4],
                                capacity: 3,
                                lifeSpan: TimeSpan.FromSeconds(0.5),
                                profile: sprayProfile)
                            {
                                Parameters = particleReleaseParameters,
                                Modifiers = { opacityFastModifier, },
                                AutoTrigger = false
                            },
                            new ParticleEmitter(
                                textureRegion: textureRegions[9],
                                capacity: 3,
                                lifeSpan: TimeSpan.FromSeconds(0.5),
                                profile: sprayProfile)
                            {
                                Parameters = particleReleaseParameters,
                                Modifiers = { opacityFastModifier, },
                                AutoTrigger = false
                            },
                        }
                    };
                    return particleEffect;
                }
            },
            {            
                Particles.CannonBlast,
                delegate(Texture2DRegion[] textureRegions)
                {
                    var particleEffect = new ParticleEffect()
                    {
                        Emitters =
                        {
                            new ParticleEmitter(
                                textureRegion: textureRegions[0],
                                capacity: 100,
                                lifeSpan: TimeSpan.FromSeconds((float)1 / 2),
                                profile: Profile.Circle(4, Profile.CircleRadiation.Out))
                            {
                                Parameters =
                                {
                                    Quantity = 8,
                                    Speed = 16,
                                    Rotation = 0,
                                    Scale = new Range<float>(0.75f, 1)
                                },
                                Modifiers =
                                {
                                    new OpacityFastFadeModifier(),
                                },
                                AutoTrigger = false,
                            },
                        }
                    };
                    return particleEffect;
                }
            },
            {
                Particles.BombBlast,
                delegate(Texture2DRegion[] textureRegions)
                {
                    var particleEffect = new ParticleEffect()
                    {
                        Emitters =
                        {
                            new ParticleEmitter(
                                textureRegion: textureRegions[0],
                                capacity: 100,
                                lifeSpan: TimeSpan.FromSeconds((float)1 / 2),
                                profile: Profile.Circle(128, Profile.CircleRadiation.Out))
                            {
                                Parameters =
                                {
                                    Quantity = 16,
                                    Speed = 128,
                                    Rotation = 0,
                                    Scale = new Range<float>(0.75f, 1.25f)
                                },
                                Modifiers =
                                {
                                    new OpacityFastFadeModifier(),
                                    new CircleContainerModifier()
                                    {
                                        Radius = 128,
                                        Inside = true,
                                        RestitutionCoefficient = 0.2f
                                    }
                                },
                                AutoTrigger = true,
                            },
                        }
                    };
                    return particleEffect;
                }
            }
        });
        private readonly Dictionary<Particles, ParticleEffect> _particleEffects = [];
        private readonly Bag<Particles> _particles = [];
    }
}
