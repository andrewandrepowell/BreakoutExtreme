﻿using MonoGame.Extended;
using Microsoft.Xna.Framework;
using System;

namespace BreakoutExtreme.Components
{
    public partial class Runner
    { 
        public Spike CreateSpike(Vector2 position)
        {
            var entity = _world.CreateEntity();
            var spike = new Spike(position);
            var animater = spike.GetAnimater();
            entity.Attach(spike);
            entity.Attach(animater);
            return spike;
        }
        public RemainingBallsPanel CreateRemainingBallsPanel(Vector2 position)
        {
            var entity = _world.CreateEntity();
            var remainingBallsPanel = new RemainingBallsPanel(position);
            entity.Attach(remainingBallsPanel);
            return remainingBallsPanel;
        }
        public DisplayBall CreateDisplayBall(float floatStartTime = 0)
        {
            var entity = _world.CreateEntity();
            var displayBall = new DisplayBall(floatStartTime);
            var animater = displayBall.GetAnimater();
            entity.Attach(displayBall);
            entity.Attach(animater);
            return displayBall;
        }
        public DeathWall CreateDeathWall(RectangleF bounds)
        {
            var entity = _world.CreateEntity();
            var deathWall = new DeathWall(bounds);
            var collider = deathWall.GetCollider();
            entity.Attach(deathWall);
            entity.Attach(collider);
            _collisionComponent.Insert(collider);
            return deathWall;
        }
        public Shadow CreateShadow(Animater parent) 
        {
            var entity = _world.CreateEntity();
            var shadow = new Shadow(entity, parent);
            var animater = shadow.GetAnimater();
            entity.Attach(animater);
            return shadow;
        }
        public Label CreateLabel(Size size)
        {
            var entity = _world.CreateEntity();
            var label = new Label(size);
            var gumDrawer = label.GetGumDrawer();
            entity.Attach(label);
            entity.Attach(gumDrawer);
            return label;
        }
        public Panel CreatePanel()
        {
            var entity = _world.CreateEntity();
            var panel = new Panel();
            var gumDrawer = panel.GetGumDrawer();
            entity.Attach(panel);
            entity.Attach(gumDrawer);
            return panel;
        }
        public GameWindow CreateGameWindow()
        {
            var entity = _world.CreateEntity();
            var gameWindow = new GameWindow();
            entity.Attach(gameWindow);
            return gameWindow;
        }
        public Brick CreateBrick(Brick.Bricks brick)
        {
            var entity = _world.CreateEntity();
            var brickObj = new Brick(entity, brick);
            var animater = brickObj.GetAnimater();
            var collider = brickObj.GetCollider();
            entity.Attach(brickObj);
            entity.Attach(animater);
            entity.Attach(collider);
            _collisionComponent.Insert(collider);
            return brickObj;
        }
        public Ball CreateBall(PlayArea parent)
        {
            var entity = _world.CreateEntity();
            var ball = new Ball(entity, parent);
            var animater = ball.GetAnimater();
            var collider = ball.GetCollider();
            var particler = ball.GetParticler();
            entity.Attach(ball);
            entity.Attach(animater);
            entity.Attach(collider);
            _collisionComponent.Insert(collider);
            entity.Attach(particler);
            return ball;
        }
        public Paddle CreatePaddle()
        {
            var entity = _world.CreateEntity();
            var paddle = new Paddle(entity);
            var animator = paddle.GetAnimater();
            var collider = paddle.GetCollider();
            entity.Attach(paddle);
            entity.Attach(animator);
            entity.Attach(collider);
            _collisionComponent.Insert(collider);
            return paddle;
        }
        public Wall CreateWall(RectangleF bounds)
        {
            var entity = _world.CreateEntity();
            var wall = new Wall(bounds);
            var collider = wall.GetCollider();
            entity.Attach(wall);
            entity.Attach(collider);
            _collisionComponent.Insert(collider);
            return wall;
        }
        public NinePatcher CreateNinePatcher()
        {
            var entity = _world.CreateEntity();
            var ninePatcher = new NinePatcher();
            entity.Attach(ninePatcher);
            return ninePatcher;
        }
    }
}
