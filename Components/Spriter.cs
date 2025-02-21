using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BreakoutExtreme.Utility;
using System;

namespace BreakoutExtreme.Components
{
    public class Spriter
    {
        private readonly Texture2D _texture;
        private readonly Rectangle[] _regions;
        private readonly Dictionary<string, Node> _nodes = [];
        private Node _node = null;
        private string _name = null;
        private float _time;
        private int _frame, _index;
        private bool _initialized = false;
        private bool _running = false;
        private Vector2 _origin;
        private class Node(int[] indices, float period, bool repeat)
        {
            public readonly int[] Indices = indices;
            public readonly float Period = period;
            public readonly bool Repeat = repeat;
        }
        public Spriter(string assetName, Size regionSize)
        {
            _texture = Globals.ContentManager.Load<Texture2D>(assetName);
            Debug.Assert(_texture.Width % regionSize.Width == 0);
            Debug.Assert(_texture.Height % regionSize.Height == 0);
            var xRegions = _texture.Width / regionSize.Width;
            var yRegions = _texture.Height / regionSize.Height;
            var totalRegions = xRegions * yRegions;
            _regions = new Rectangle[totalRegions];
            for (var y = 0; y < yRegions; y++)
            {
                for (var x = 0; x < xRegions; x++)
                {
                    _regions[x + y * xRegions] = new Rectangle(
                        x * regionSize.Width, 
                        y * regionSize.Height, 
                        regionSize.Width, 
                        regionSize.Height);
                }
            }

            _origin = (regionSize / 2).ToVector2();
        }
        public void Add(string name, int[] indices, float period = 0, bool repeat = false)
        {
            Debug.Assert(!_nodes.ContainsKey(name));
            Debug.Assert(indices.All(x => x >= 0 && x < _regions.Length));
            Debug.Assert(indices.Length > 0);
            Debug.Assert(period >= 0);
            _nodes.Add(name, new Node(indices, period, repeat));
        }
        public void Play(string name)
        {
            Debug.Assert(_nodes.ContainsKey(name));
            _name = name;
            _node = _nodes[name];
            _time = _node.Period;
            _frame = 0;
            _index = _node.Indices[_frame];
            _initialized = true;
            _running = true;
        }
        public void Stop()
        {
            _running = false;
        }
        public bool Running => _running;
        public bool Initialized => _initialized;
        public Texture2D Texture => _texture;
        public Rectangle Region
        {
            get
            {
                Debug.Assert(_initialized);
                return _regions[_index];
            }
        }
        public int Index
        {
            get
            {
                Debug.Assert(_initialized);
                return _index;
            }
        }
        public int Frame
        {
            get
            {
                Debug.Assert(_initialized);
                return _frame;
            }
        }
        public Vector2 Origin => _origin;
        public Color Color = Color.White;
        public Vector2 Position;
        public float Scale = 1;
        public float Rotation = 0;
        public SpriteEffects SpriteEffect = SpriteEffects.None;
        public void Update()
        {
            if (_running && _node.Period > 0)
            {
                while (_time <= 0)
                {
                    if (_frame == _node.Indices.Length - 1)
                    {
                        if (_node.Repeat)
                        {
                            _frame = 0;
                            _index = _node.Indices[_frame];
                        }
                        else
                        {
                            _running = false;
                        }
                    }
                    else
                    {
                        _frame++;
                        _index = _node.Indices[_frame];
                    }
                    _time += _node.Period;
                }

                _time -= Globals.GameTime.GetElapsedSeconds();
            }
        }
        public void Draw()
        {
            if (Color.A == 0)
                return;
            Globals.SpriteBatch.Draw(
                texture: _texture,
                position: Position,
                sourceRectangle: Region,
                color: Color,
                rotation: Rotation,
                origin: _origin,
                scale: Scale,
                effects: SpriteEffect,
                layerDepth: 0);
        }
    }
}
