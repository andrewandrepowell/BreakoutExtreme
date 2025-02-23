using MonoGame.Extended;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using BreakoutExtreme.Utility;

namespace BreakoutExtreme.Components
{
    public partial class Runner
    {
        public Power CreatePower(Powers powerEnum, PlayArea parent)
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            _powerPool.RemoveFromFront(out var power);
            power.Reset(entity, powerEnum, parent);
            var animater = power.GetAnimater();
            var collider = power.GetCollider();
            entity.Attach(power);
            entity.Attach(animater);
            entity.Attach(collider);
            _collisionComponent.Insert(collider);
            return power;
        }
        public PlayArea CreatePlayArea(GameWindow parent)
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            var playArea = new PlayArea(parent);
            entity.Attach(playArea);
            return playArea;
        }
        public Menus CreateMenus()
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            var menus = new Menus();
            var gumDrawer = menus.GetGumDrawer();
            entity.Attach(menus);
            entity.Attach(gumDrawer);
            return menus;
        }
        public Dimmer CreateDimmer()
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            var dimmer = new Dimmer();
            var gumDrawer = dimmer.GetGumDrawer();
            entity.Attach(dimmer);
            entity.Attach(gumDrawer);
            return dimmer;
        }
        public Splasher CreateSplasher(Splasher.Splashes splash)
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            var splasher = new Splasher();
            splasher.Reset(entity, splash);
            var animater = splasher.GetAnimater();
            entity.Attach(splasher);
            entity.Attach(animater);
            return splasher;
        }
        public Bomb CreateBomb(Bomb.Bombs bombEnum, Vector2 position)
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            _bombPool.RemoveFromFront(out var bomb);
            bomb.Reset(entity, bombEnum, position);
            var animater = bomb.GetAnimater();
            var collider = bomb.GetCollider();
            var particler = bomb.GetParticler();
            entity.Attach(bomb);
            entity.Attach(animater);
            entity.Attach(collider);
            entity.Attach(particler);
            _collisionComponent.Insert(collider);
            return bomb;
        }
        public Cannon CreateCannon(Cannon.Cannons cannonEnum, Vector2 position)
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            _cannonPool.RemoveFromFront(out var cannon);
            cannon.Reset(entity, cannonEnum, position);
            var animater = cannon.GetAnimater();
            var collider = cannon.GetCollider();
            var particler = cannon.GetParticler();
            entity.Attach(cannon);
            entity.Attach(animater);
            entity.Attach(collider);
            entity.Attach(particler);
            _collisionComponent.Insert(collider);
            return cannon;
        }
        public Laser CreateLaser(PlayArea parent, bool empowered)
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            _laserPool.RemoveFromFront(out var laser);
            laser.Reset(entity, parent, empowered);
            var animater = laser.GetAnimater();
            var collider = laser.GetCollider();
            entity.Attach(laser);
            entity.Attach(animater);
            entity.Attach(collider);
            _collisionComponent.Insert(collider);
            return laser;
        }
        public PulseGlower CreatePulseGlower(
            Animater parent, Color color,
            float minVisibility, float maxVisibility,
            float pulsePeriod)
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            _pulseGlowerPool.RemoveFromFront(out var pulseGlower);
            pulseGlower.Reset(entity, parent, color, minVisibility, maxVisibility, pulsePeriod);
            var texturer = pulseGlower.GetTexturer();
            entity.Attach(pulseGlower);
            entity.Attach(texturer);
            return pulseGlower;
        }
        public Glower CreateGlower(
            Animater parent, Color color, 
            float minVisibility, float maxVisibility,
            float pulsePeriod, bool pulseRepeating,
            float appearVanishPeriod)
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            _glowerPool.RemoveFromFront(out var glower);
            glower.Reset(entity, parent, color, minVisibility, maxVisibility, pulsePeriod, pulseRepeating, appearVanishPeriod);
            var texturer = glower.GetTexturer();
            entity.Attach(glower);
            entity.Attach(texturer);
            return glower;
        }
        public Button CreateButton(object parent, Action<object> action, RectangleF bounds, string text)
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            var button = new Button(parent, action, bounds, text);
            var gumDrawer = button.GetGumDrawer();
            entity.Attach(button);
            entity.Attach(gumDrawer);
            return button;
        }
        public ScorePopup CreateScorePopup()
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            _scorePopupPool.RemoveFromFront(out var scorePopup);
            scorePopup.Reset(entity);
            var gumDrawer = scorePopup.GetGumDrawer();
            entity.Attach(scorePopup);
            entity.Attach(gumDrawer);
            return scorePopup;
        }
        public Spike CreateSpike(Vector2 position, Spike.Edges edge)
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            var spike = new Spike(position, edge);
            var animater = spike.GetAnimater();
            entity.Attach(spike);
            entity.Attach(animater);
            return spike;
        }
        public RemainingBallsPanel CreateRemainingBallsPanel(Vector2 position)
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            var remainingBallsPanel = new RemainingBallsPanel(position);
            entity.Attach(remainingBallsPanel);
            return remainingBallsPanel;
        }
        public DisplayBall CreateDisplayBall(float floatStartTime = 0)
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            var displayBall = new DisplayBall(floatStartTime);
            var animater = displayBall.GetAnimater();
            entity.Attach(displayBall);
            entity.Attach(animater);
            return displayBall;
        }
        public DeathWall CreateDeathWall(RectangleF bounds)
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            var deathWall = new DeathWall(bounds);
            var collider = deathWall.GetCollider();
            entity.Attach(deathWall);
            entity.Attach(collider);
            _collisionComponent.Insert(collider);
            return deathWall;
        }
        public Shadower CreateShadower(Animater parent, Vector2 position)
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            _shadowerPool.RemoveFromFront(out var shadower);
            shadower.Reset(entity, parent, position);
            var texturer = shadower.GetTexturer();
            entity.Attach(shadower);
            entity.Attach(texturer);
            return shadower;
        }
        public Shadow CreateShadow(Animater parent) 
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            _shadowPool.RemoveFromFront(out var shadow);
            shadow.Reset(entity, parent);
            var texturer = shadow.GetTexturer();
            entity.Attach(shadow);
            entity.Attach(texturer);
            return shadow;
        }
        public Label CreateLabel(Size size)
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            var label = new Label(size);
            var gumDrawer = label.GetGumDrawer();
            entity.Attach(label);
            entity.Attach(gumDrawer);
            return label;
        }
        public Panel CreatePanel()
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            var panel = new Panel();
            var gumDrawer = panel.GetGumDrawer();
            entity.Attach(panel);
            entity.Attach(gumDrawer);
            return panel;
        }
        public GameWindow CreateGameWindow()
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            var gameWindow = new GameWindow();
            entity.Attach(gameWindow);
            return gameWindow;
        }
        public Brick CreateBrick(Brick.Bricks brickEnum, Vector2 position, Powers? power = null)
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            _brickPool.RemoveFromFront(out var brick);
            brick.Reset(entity, brickEnum, position, power);
            var animater = brick.GetAnimater();
            var collider = brick.GetCollider();
            var particler = brick.GetParticler();
            entity.Attach(brick);
            entity.Attach(animater);
            entity.Attach(collider);
            entity.Attach(particler);
            _collisionComponent.Insert(collider);
            return brick;
        }
        public Ball CreateBall(PlayArea parent)
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            _ballPool.RemoveFromFront(out var ball);
            ball.Reset(entity, parent);
            var animater = ball.GetAnimater();
            var collider = ball.GetCollider();
            var particler = ball.GetParticler();
            entity.Attach(ball);
            entity.Attach(animater);
            entity.Attach(collider);
            entity.Attach(particler);
            _collisionComponent.Insert(collider);
            return ball;
        }
        public Paddle CreatePaddle()
        {
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            _paddlePool.RemoveFromBack(out var paddle);
            paddle.Reset(entity);
            var animator = paddle.GetAnimater();
            var collider = paddle.GetCollider();
            var particler = paddle.GetParticler();
            entity.Attach(paddle);
            entity.Attach(animator);
            entity.Attach(collider);
            entity.Attach(particler);
            _collisionComponent.Insert(collider);
            return paddle;
        }
        public Wall CreateWall(RectangleF bounds)
        {
            Debug.Assert(_initialized);
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
            Debug.Assert(_initialized);
            var entity = _world.CreateEntity();
            var ninePatcher = new NinePatcher();
            entity.Attach(ninePatcher);
            return ninePatcher;
        }
    }
}
