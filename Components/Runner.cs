using BreakoutExtreme.Systems;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ECS;
using MonoGame.Extended;

namespace BreakoutExtreme.Components
{
    public class Runner
    {
        readonly private World _world;
        readonly private CollisionComponent _collisionComponent;
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
        public Ball CreateBall()
        {
            var entity = _world.CreateEntity();
            var ball = new Ball(entity);
            var animater = ball.GetAnimater();
            var collider = ball.GetCollider();
            entity.Attach(ball);
            entity.Attach(animater);
            entity.Attach(collider);
            _collisionComponent.Insert(collider);
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
        public void RemoveEntity(int entityId) => RemoveEntity(_world.GetEntity(entityId));
        public void RemoveEntity(Entity entity)
        {
            if (entity.Has<Collider>())
                _collisionComponent.Remove(entity.Get<Collider>());
            entity.Destroy();
        }
        public Runner()
        {
            _collisionComponent = new CollisionComponent(Globals.PlayAreaBounds);

            var worldBuilder = new WorldBuilder();
            var gameWindowSystem = new GameWindowSystem();
            var colliderSystem = new ColliderSystem(_collisionComponent);
            var positionSystem = new PositionSystem();
            var gumRenderSystem = new GumRenderSystem();
            var renderSystem = new RenderSystem();
            worldBuilder.AddSystem(gameWindowSystem);
            worldBuilder.AddSystem(colliderSystem);
            worldBuilder.AddSystem(positionSystem);
            worldBuilder.AddSystem(gumRenderSystem);
            worldBuilder.AddSystem(renderSystem);
            _world = worldBuilder.Build();
        }
        public void Update()
        {
            _world.Update(Globals.GameTime);
        }
        public void Draw()
        {
            _world.Draw(Globals.GameTime);
        }
    }
}
