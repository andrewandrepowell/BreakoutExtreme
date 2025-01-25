using System.Collections.Generic;
using System.Collections.ObjectModel;
using MonoGame.Extended.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Diagnostics;


namespace BreakoutExtreme.Components
{
    public class NinePatcher
    {
        private static readonly int[] _textureRegionIndices = [0, 1, 2, 3, 4, 5, 6, 7, 8];
        private static readonly ReadOnlyDictionary<Textures, string> _textureAssetNames = new(new Dictionary<Textures, string>() 
        {
            { Textures.PlayArea, "animations/tiny_wonder_ui/play_area_0" },
            { Textures.PlayAreaBottomRemoved, "animations/tiny_wonder_ui/play_area_1" },
            { Textures.PlayAreaFilled, "animations/tiny_wonder_ui/play_area_2" }
        });
        private Dictionary<Textures, NinePatch> _textureNinePatches = new();
        private Textures _texture = Textures.PlayArea;
        private Rectangle _drawRectangle;
        private RectangleF _bounds;
        private void UpdateTextureNinePatches()
        {
            var assetName = _textureAssetNames[Texture];
            var texture = Globals.ContentManager.Load<Texture2D>(assetName);
            Debug.Assert(texture.Width % 3 == 0);
            Debug.Assert(texture.Height % 3 == 0);
            var textureRegion = new Size(texture.Width / 3, texture.Height / 3);
            var textureAtlas = Texture2DAtlas.Create(assetName, texture, textureRegion.Width, textureRegion.Height);
            var textureRegions = textureAtlas.GetRegions(_textureRegionIndices);
            var ninePatch = new NinePatch(textureRegions);
            _textureNinePatches.Add(Texture, ninePatch);
        }
        private void UpdateDrawRectangle()
        {
            _drawRectangle.X = (int)Math.Floor(Bounds.X);
            _drawRectangle.Y = (int)Math.Floor(Bounds.Y);
            _drawRectangle.Width = (int)Math.Floor(Bounds.Width);
            _drawRectangle.Height = (int)Math.Floor(Bounds.Height);
        }
        public enum Textures
        {
            PlayArea,
            PlayAreaBottomRemoved,
            PlayAreaFilled
        }
        public RectangleF Bounds
        {
            get => _bounds;
            set
            {
                if (_bounds == value) 
                    return;
                _bounds = value;
                UpdateDrawRectangle();
            }
        }
        public Textures Texture 
        {
            get => _texture;
            set
            {
                if (_texture == value) 
                    return;
                _texture = value;
                if (!_textureNinePatches.ContainsKey(_texture))
                {
                    UpdateTextureNinePatches();
                }
            }
        }
        public Color Color = Color.White;
        public NinePatcher()
        {
            UpdateTextureNinePatches();
        }
        public void Draw()
        {
            Globals.SpriteBatch.Draw(
                ninePatchRegion: _textureNinePatches[Texture],
                destinationRectangle: _drawRectangle,
                color: Color);
        }
    }
}
