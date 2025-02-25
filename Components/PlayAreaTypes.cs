namespace BreakoutExtreme.Components
{
    public partial class PlayArea
    {
        public enum Levels
        {
            Test,
            Test2,
            Test3
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
