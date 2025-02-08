﻿using BreakoutExtreme.Systems;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ECS;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class Runner
    {
        private const int _poolSize = 32;
        readonly private World _world;
        readonly private CollisionComponent _collisionComponent;
        readonly private Deque<ScorePopup> _scorePopupPool = new();
        readonly private Deque<Glower> _glowerPool = new();
        readonly private Deque<PulseGlower> _pulseGlowerPool = new();
        readonly private Deque<Laser> _laserPool = new();
        readonly private Deque<Cannon> _cannonPool = new();
        readonly private Deque<Bomb> _bombPool = new();
        readonly private Deque<Ball> _ballPool = new();
        private bool _initialized = false;
        public void RemoveEntity(Entity entity)
        {
            Debug.Assert(_initialized);

            if (entity.Has<ScorePopup>())
                _scorePopupPool.AddToBack(entity.Get<ScorePopup>());
            if (entity.Has<Glower>())
                _glowerPool.AddToBack(entity.Get<Glower>());
            if (entity.Has<PulseGlower>())
                _pulseGlowerPool.AddToBack(entity.Get<PulseGlower>());
            if (entity.Has<Laser>())
                _laserPool.AddToBack(entity.Get<Laser>());
            if (entity.Has<Cannon>())
                _cannonPool.AddToBack(entity.Get<Cannon>());
            if (entity.Has<Bomb>())
                _bombPool.AddToBack(entity.Get<Bomb>());
            if (entity.Has<Ball>())
                _ballPool.AddToBack(entity.Get<Ball>());

            if (entity.Has<Collider>())
                _collisionComponent.Remove(entity.Get<Collider>());
            if (entity.Has<Particler>())
            {
                var particler = entity.Get<Particler>();
                if (particler.Disposable)
                    particler.Dispose();
            }
            entity.Destroy();
        }
        public void Initialize()
        {
            Debug.Assert(!_initialized);

            GumUI.Initialize();

            // Initialize all the pools.
            {
                for (var i = 0; i < _poolSize; i++)
                {
                    var scorePopup = new ScorePopup();
                    _scorePopupPool.AddToBack(scorePopup);

                    var glower = new Glower();
                    _glowerPool.AddToBack(glower);

                    var pulseGlower = new PulseGlower();
                    _pulseGlowerPool.AddToBack(pulseGlower);

                    var laser = new Laser();
                    _laserPool.AddToBack(laser);

                    var cannon = new Cannon();
                    _cannonPool.AddToBack(cannon);

                    var bomb = new Bomb();
                    _bombPool.AddToBack(bomb);

                    var ball = new Ball();
                    _ballPool.AddToBack(ball);
                }
            }

            _initialized = true;
        }
        public Runner()
        {
            _collisionComponent = new CollisionComponent(Globals.PlayAreaBounds);

            {
                var worldBuilder = new WorldBuilder();
                worldBuilder.AddSystem(new GameWindowSystem());

                worldBuilder.AddSystem(new RemoveSystem<Brick>());
                worldBuilder.AddSystem(new RemoveSystem<Ball>());
                worldBuilder.AddSystem(new RemoveSystem<Bomb>());
                worldBuilder.AddSystem(new RemoveSystem<ScorePopup>());
                worldBuilder.AddSystem(new RemoveSystem<Laser>());
                worldBuilder.AddSystem(new RemoveSystem<Cannon>());

                worldBuilder.AddSystem(new UpdateSystem<Paddle>());
                worldBuilder.AddSystem(new UpdateSystem<Ball>());
                worldBuilder.AddSystem(new UpdateSystem<Brick>());
                worldBuilder.AddSystem(new UpdateSystem<Laser>());
                worldBuilder.AddSystem(new UpdateSystem<Bomb>());
                worldBuilder.AddSystem(new UpdateSystem<Cannon>());
                worldBuilder.AddSystem(new UpdateSystem<ScorePopup>());
                worldBuilder.AddSystem(new UpdateSystem<PulseGlower>());

                worldBuilder.AddSystem(new ColliderSystem(_collisionComponent));
                worldBuilder.AddSystem(new PositionSystem());
                worldBuilder.AddSystem(new RenderSystem());
                _world = worldBuilder.Build();
            }
        }
        public void Update()
        {
            Debug.Assert(_initialized);
            _world.Update(Globals.GameTime);
        }
        public void Draw()
        {
            Debug.Assert(_initialized);
            _world.Draw(Globals.GameTime);
        }
    }
}
