using BreakoutExtreme.Systems;
using MonoGame.Extended.Collections;
using MonoGame.Extended.ECS;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace BreakoutExtreme.Utility
{
    public class GameObjectManager<T> where T : GameObject, new()
    {
        private readonly World _world;
        private readonly Deque<T> _pool;
        private readonly Action<GameObject> _returnToPool;
        public GameObjectManager(World world, int poolSize)
        {
            _world = world;
            _pool = [];
            for (var i = 0; i < poolSize; i++)
                _pool.AddToBack(new());
            _returnToPool = (GameObject gameObject) => _pool.AddToBack((T)gameObject);
        }
        public T Create()
        {
            var success = _pool.RemoveFromFront(out var gameObject);
            Debug.Assert(success);
            gameObject.Reset(_world, _returnToPool);
            return gameObject;
        }
    }
    public abstract class ComponentObject
    {
        private bool _initialized;
        private readonly GameObject _parent;
        public ComponentObject(GameObject parent)
        {
            _initialized = false;
            _parent = parent;
            parent.Add(this);
        }
        public virtual void Reset()
        {
            Debug.Assert(!_initialized);
            _initialized = true;
        }
        public virtual void Remove()
        {
            Debug.Assert(_initialized);
            _initialized = false;
        }
    }
    public abstract class GameObject 
    {
        private Entity _entity;
        private bool _initialized;
        private Action<GameObject> _returnToPool;
        private readonly Bag<ComponentObject> _components;
        private readonly Bag<GameObject> _children;
        public GameObject()
        {
            _initialized = false;
            _components = [];
            _children = [];
        }
        public void Add(ComponentObject component)
        {
            Debug.Assert(!_initialized);
            Debug.Assert(!_components.Contains(component));
            _components.Add(component);
        }
        public void Add(GameObject child)
        {
            Debug.Assert(_initialized);
            Debug.Assert(!_children.Contains(child));
            _children.Add(child);
        }
        public void Reset(World world, Action<GameObject> returnToPool)
        {
            Debug.Assert(!_initialized);
            _returnToPool = returnToPool;
            _entity = world.CreateEntity();
            for (var i = 0; i < _components.Count; i++)
            {
                var component = _components[i];
                component.Reset();
                _entity.Attach(component);
            }
            _initialized = true;
        }
        public virtual bool RemoveLater => false;
        public virtual void Remove()
        {
            Debug.Assert(_initialized);
            for (var i = 0; i < _components.Count; i++)
                _components[i].Remove();
            for (var i = 0; i < _children.Count; i++)
                _children[i].Remove();
            _entity.Destroy();
            _returnToPool(this);
            _initialized = false;
        }
        public virtual void Update()
        {

        }
    }
}
