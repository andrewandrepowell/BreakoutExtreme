using BreakoutExtreme.Components;
using BreakoutExtreme.Shaders;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace BreakoutExtreme.Features
{
    public class Cracks : Feature
    {
        private static readonly ReadOnlyDictionary<Degrees, Animater.Animations> _degreeAnimations = new(new Dictionary<Degrees, Animater.Animations>() 
        {
            { Degrees.Small, Animater.Animations.CrackSmall },
            { Degrees.Medium, Animater.Animations.CrackMedium },
            { Degrees.Large, Animater.Animations.CrackLarge },
        });
        private static readonly Degrees[] _degrees = Enum.GetValues<Degrees>();
        private readonly Animater _parent;
        private readonly Dictionary<Degrees, Node> _degreeNodes;
        private class Node(Texture2D texture, Rectangle region)
        {
            public readonly Texture2D Texture = texture;
            public readonly Rectangle Region = region;
        }
        public enum Degrees
        {
            None,
            Small,
            Medium,
            Large
        }
        public Degrees Degree = Degrees.None;
        public Cracks(Animater parent)
        {
            _parent = parent;
            _degreeNodes = new();
            {
                var animater = new Animater();
                foreach (ref var degree in _degrees.AsSpan())
                {
                    if (degree == Degrees.None)
                        continue;
                    animater.Play(_degreeAnimations[degree]);
                    _degreeNodes.Add(degree, new Node(animater.Texture, animater.Region));
                }
            }
        }
        public override Scripts? Script => (Degree == Degrees.None) ? null : Scripts.Pattern;
        public override void UpdateShaderNode(PatternNode node)
        {
            Debug.Assert(Degree != Degrees.None);
            var parentTexture = _parent.Texture;
            var parentRegion = _parent.Region;
            var patternNode = _degreeNodes[Degree];
            node.PatternTexture.SetValue(patternNode.Texture);
            node.PatternTextureDimensions.SetValue(patternNode.Texture.Bounds.Size.ToVector2());
            node.PatternRegionPosition.SetValue(patternNode.Region.Location.ToVector2());
            node.PatternRegionDimensions.SetValue(patternNode.Region.Size.ToVector2());
            node.SpriteTextureDimensions.SetValue(parentTexture.Bounds.Size.ToVector2());
            node.SpriteRegionDimensions.SetValue(parentRegion.Size.ToVector2());
        }
    }
}
