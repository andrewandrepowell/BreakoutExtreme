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
        private readonly static ReadOnlyDictionary<Particles, Action<Particler, ParticleEffect>> _particleParticleEffects = new(new Dictionary<Particles, Action<Particler, ParticleEffect>>() 
        {
            {
                Particles.BallTrail,
                delegate(Particler particler, ParticleEffect particleEffect)
                {
                    
                }
            }
        });
    }
}
