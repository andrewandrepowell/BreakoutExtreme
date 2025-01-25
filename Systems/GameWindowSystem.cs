using BreakoutExtreme.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using System;


namespace BreakoutExtreme.Systems
{
    public class GameWindowSystem : EntityProcessingSystem
    {
        private ComponentMapper<Components.GameWindow> _gameWindowMapper;
        public GameWindowSystem() : base(Aspect.One(typeof(Components.GameWindow)))
        {
        }
        public override void Initialize(IComponentMapperService mapperService)
        {
            _gameWindowMapper = mapperService.GetMapper<Components.GameWindow>();
        }
        public override void Process(GameTime gameTime, int entityId)
        {
            Console.WriteLine("reach");
            _gameWindowMapper.Get(entityId).Update();
        }
    }
}
