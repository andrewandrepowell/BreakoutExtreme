using BreakoutExtreme.Components;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collections;

namespace BreakoutExtreme.Utility
{
    public enum Directions { Left, Right, Up, Down }
    public enum RoundingModes { Floor, Round, Ceil }
    public enum RunningStates { Waiting, Starting, Running, Stopping }
    public enum Layers { Shadow, Ground, Foreground, Dimmer, Menus }
    public enum Powers { MultiBall, Protection, NewBall }
    public interface IUpdate
    {
        public void Update();
    }
    public interface IRemoveEntity
    {
        public void RemoveEntity();
    }
    public interface IDestroyed
    {
        public bool Destroyed {  get; }
    }
    public interface ITexture
    {
        public Texture2D Texture { get; }
    }
    public interface IAttacher
    {
        public Attacher<IMovable> GetAttacher();
    }
    public class GameBag<T> where T : IDestroyed
    {
        private readonly Bag<T> _values = [];
        private readonly Bag<T> _destroyedValues = [];
        public int Count => _values.Count;
        public T this[int index]
        {
            get => _values[index];
            set => _values[index] = value;
        }
        public void Add(T value) => _values.Add(value);
        public bool Remove(T value) => _values.Remove(value);
        public void Clear() => _values.Clear();
        public void Destroy()
        {
            _destroyedValues.Clear();
            for (var i = 0; i < _values.Count; i++)
            {
                var value = _values[i];
                if (value.Destroyed)
                    _destroyedValues.Add(value);
            }
            for (var i = 0; i < _destroyedValues.Count; i++)
            {
                var value = _destroyedValues[i];
                _values.Remove(value);
            }
        }
    }
}