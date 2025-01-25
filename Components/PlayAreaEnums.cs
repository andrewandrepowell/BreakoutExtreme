﻿namespace BreakoutExtreme.Components
{
    public partial class PlayArea
    {
        public enum Levels
        {
            Test
        }
        public enum Components
        {
            None,
            Ball,
            Paddle,
            ThickBrick
        }
        public enum States
        {
            Unloaded,
            Loaded,
            PlayerTakingAim,
            GameRunning
        }
    }
}
