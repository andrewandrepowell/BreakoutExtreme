using BreakoutExtreme.Utility;
using MonoGame.Extended.ECS;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public class Shadower : IRemoveEntity
    {
        private readonly Texturer _texturer;
        private bool _initialized = false;
        private Entity _entity;
        private Animater _parent;
        public Texturer GetTexturer() => _texturer;
        public void Reset(Entity entity, Animater parent, Vector2 position)
        {
            Debug.Assert(!_initialized);
            _entity = entity;
            _parent = parent;
            _texturer.Parent = parent;
            _texturer.Position = position;
            _texturer.ShaderFeatures.Clear();
            parent.GetAttacher().Attach(_texturer);
            _initialized = true;
        }
        public void RemoveEntity()
        {
            Debug.Assert(_initialized);
            _parent.GetAttacher().Detach(_texturer);
            _initialized = false;
        }
        public Shadower()
        {
            _texturer = new(null);
            _initialized = false;
        }
    }
}
