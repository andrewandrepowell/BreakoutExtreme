namespace BreakoutExtreme.Components
{
    public partial class PlayArea
    {
        public enum Levels
        {
            Test,
            Test2,
            Test3,
            Beginner0,
            Beginner1,
            Beginner2,
            Beginner3,
            Beginner4,
            Loop0,
            Loop1,
        }
        public enum Components
        {
            None,
            Ball,
            Paddle,
            PowerMultiBall,
            PowerProtection,
            PowerNewBall,
            PowerEnlarge,
            PowerEmpowered,
            BrickSmall,
            BrickMedium,
            BrickLarge,
            Cannon
        }
        public enum States
        {
            Unloaded,
            Loaded,
            PlayerTakingAim,
            GameRunning,
            SpawnNewBall,
            Clearing,
            GameEnding
        }
    }
}
