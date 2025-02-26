using MonoGame.Extended;
using System;
using BreakoutExtreme.Utility;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public class GameWindow : IUpdate
    {
        private readonly PlayArea _playArea;
        private readonly Panel _scorePanel;
        private readonly Panel _highScorePanel;
        private readonly RemainingBallsPanel _remainingBallsPanel;
        private readonly Button _menuButton;
        private readonly Dimmer _dimmer;
        private readonly Menus _menus;
        private const float _menuLockPeriod = 0.25f;
        private float _menuLockTime;
        private int _score = 0;
        private int _highScore = 0;
        private int _levelsCleared = 0;
        private void UpdateScorePanel()
        {
            _scorePanel.Text = $"{_score}";
        }
        private void UpdateHighScorePanel()
        {
            _highScorePanel.Text = $"{_highScore}";
        }
        private void Reset()
        {
            Debug.Assert(!_playArea.Loaded);
            if (Score > HighScore)
                HighScore = Score;
            Score = 0;
            _remainingBallsPanel.FlashNewBall = false;
            RemainingBalls = 3;
            LevelsCleared = 0;
            _playArea.GameStart();
        }
        private void OpenMenu()
        {
            MenuLock();
            Globals.Pause();
            _dimmer.Start();
            _menus.Start();
            _menus.Goto("main");
        }
        private void CloseMenu()
        {
            MenuLock();
            Globals.Resume();
            _dimmer.Stop();
            _menus.Stop();
            _menus.Goto();
        }
        public void MenuLock()
        {
            _menuLockTime = _menuLockPeriod;
        }
        public bool MenuLocked => _menuLockTime > 0;
        public int Score
        {
            get => _score;
            set
            {
                Debug.Assert(value >= 0);
                if (_score == value)
                    return;
                _score = value;
                UpdateScorePanel();
            }
        }
        public int HighScore
        {
            get => _highScore;
            set
            {
                Debug.Assert(value >= 0);
                Debug.Assert(value >= _highScore);
                _highScore = value;
                UpdateHighScorePanel();
            }
        }
        public int LevelsCleared
        {
            get => _levelsCleared;
            set
            {
                Debug.Assert(value >= 0);
                _levelsCleared = value;
            }
        }
        public int RemainingBalls
        {
            get => _remainingBallsPanel.RemainingBalls;
            set => _remainingBallsPanel.RemainingBalls = value;
        }
        public static int MaximumBalls => RemainingBallsPanel.MaximumBalls;
        public void DropBall()
        {
            _remainingBallsPanel.FlashNewBall = false;
            RemainingBalls--;
        }
        public void NewBall()
        {
            _remainingBallsPanel.FlashNewBall = true;
            RemainingBalls++;
        }
        public GameWindow()
        {
            // Finally, instantiate the play area.
            _playArea = Globals.Runner.CreatePlayArea(this);

            // Create background area for UI.
            {
                var topPatch = Globals.Runner.CreateNinePatcher();
                topPatch.Texture = NinePatcher.Textures.GameWindowFilled;
                topPatch.Bounds = new RectangleF(0, 0, Globals.GameWindowBounds.Width, Globals.PlayAreaBounds.Y);
            }

            // Prepare score panel and label.
            {
                var scoreLabel = Globals.Runner.CreateLabel(Globals.ScoreLabelBlockBounds.Size.ToSize() * Globals.GameBlockSize);
                var gumDrawer = scoreLabel.GetGumDrawer();
                gumDrawer.Position = (gumDrawer.Size / 2).ToVector2() + Globals.ScoreLabelBlockBounds.Location.ToVector2() * Globals.GameBlockSize;
                scoreLabel.Text = "Score:";
            }
            {
                _scorePanel = Globals.Runner.CreatePanel();
                var gumDrawer = _scorePanel.GetGumDrawer();
                _scorePanel.Size = Globals.ScorePanelBlockBounds.Size.ToSize() * Globals.GameBlockSize;
                gumDrawer.Position = (gumDrawer.Size / 2).ToVector2() + Globals.ScorePanelBlockBounds.Location.ToVector2() * Globals.GameBlockSize;
                _scorePanel.Text = "0";
            }

            // Prepare high score panel and label.
            {
                var label = Globals.Runner.CreateLabel(Globals.HighScoreLabelBlockBounds.Size.ToSize() * Globals.GameBlockSize);
                var gumDrawer = label.GetGumDrawer();
                gumDrawer.Position = (gumDrawer.Size / 2).ToVector2() + Globals.HighScoreLabelBlockBounds.Location.ToVector2() * Globals.GameBlockSize;
                label.Text = "High Score:";
            }
            {
                _highScorePanel = Globals.Runner.CreatePanel();
                var gumDrawer = _highScorePanel.GetGumDrawer();
                _highScorePanel.Size = Globals.HighScorePanelBlockBounds.Size.ToSize() * Globals.GameBlockSize;
                gumDrawer.Position = (gumDrawer.Size / 2).ToVector2() + Globals.HighScorePanelBlockBounds.Location.ToVector2() * Globals.GameBlockSize;
                _highScorePanel.Text = "0";
            }

            // Prepare balls remaining label
            {
                var label = Globals.Runner.CreateLabel(Globals.BallsRemainingLabelBlockBounds.Size.ToSize() * Globals.GameBlockSize);
                var gumDrawer = label.GetGumDrawer();
                gumDrawer.Position = (gumDrawer.Size / 2).ToVector2() + Globals.BallsRemainingLabelBlockBounds.Location.ToVector2() * Globals.GameBlockSize;
                label.Text = "Balls Remaining:";
            }
            {
                _remainingBallsPanel = Globals.Runner.CreateRemainingBallsPanel(Globals.BallsRemainingPanelBlockBounds.Center.ToVector2() * Globals.GameBlockSize);
            }

            // Create the menu button.
            {
                _menuButton = Globals.Runner.CreateButton(
                    parent: this, 
                    action: (object parent) =>
                    {
                        if (_menus.Busy)
                            return;
                        OpenMenu(); 
                    }, 
                    bounds: Globals.MenuButtonBlockBounds.ToBounds(), 
                    text: "Menu");
            }


            // Create dimmer.
            {
                _dimmer = Globals.Runner.CreateDimmer();
            }

            {
                _menus = Globals.Runner.CreateMenus();
                var mainWindow = new Menus.Window() 
                { 
                    ID  = "main", 
                    Text = "Welcome to Break Out Extreme! Check the help to learn how to play!"
                };
                var helpWindow = new Menus.Window()
                {
                    ID = "help",
                    Text =
                    "How to Play:\n" +
                    "\n" +
                    "Resume / Start the game by tapping the screen outside of the menu!\n" +
                    "\n" +
                    "Drag the Paddle to bounce the Ball away from the Spikes!\n" +
                    "\n" +
                    "Tap the Paddle to fire a Laser to destroy Bombs!\n" +
                    "\n" +
                    "Clear Levels by breaking all Bricks with the Ball!\n"
                };
                var optionsWindow = new Menus.Window()
                {
                    ID = "options",
                    Text = "Options - TBA"
                };
                var creditsWindow = new Menus.Window()
                {
                    ID = "credits",
                    Text = 
                    "Andrew Powell - Game Design, Programming, and Art\n" +
                    "Tools:\n" +
                    "MonoGame / KNI - Game Engine\n" +
                    "MonoGame Extended - ECS, Collisions, Particle Effects, and More\n" +
                    "Gum - UI and Menus"
                };
                var helpButton = new Menus.Button()
                {
                    Text = "Help",
                    Action = (object o) => _menus.Goto("help")
                };
                var optionsButton = new Menus.Button()
                {
                    Text = "Options",
                    Action = (object o) => _menus.Goto("options")
                };
                var creditsButton = new Menus.Button()
                {
                    Text = "Credits",
                    Action = (object o) => _menus.Goto("credits")
                };
                var restartButton = new Menus.Button()
                {
                    Text = "Restart",
                    Action = (object o) =>
                    {
                        CloseMenu();
                        _playArea.Unload();
                        Reset();
                    }
                };
                var helpBackButton = new Menus.Button()
                {
                    Text = "Back",
                    Action = (object o) => _menus.Goto(_menus.PrevID)
                };
                var optionsBackButton = new Menus.Button()
                {
                    Text = "Back",
                    Action = (object o) => _menus.Goto(_menus.PrevID)
                };
                var creditsBackButton = new Menus.Button()
                {
                    Text = "Back",
                    Action = (object o) => _menus.Goto(_menus.PrevID)
                };
                mainWindow.Add(helpButton);
                mainWindow.Add(optionsButton);
                mainWindow.Add(creditsButton);
                mainWindow.Add(restartButton);
                helpWindow.Add(helpBackButton);
                optionsWindow.Add(optionsBackButton);
                creditsWindow.Add(creditsBackButton);
                _menus.Add(mainWindow);
                _menus.Add(helpWindow);
                _menus.Add(optionsWindow);
                _menus.Add(creditsWindow);
            }

            Reset(); // Start the game.
            OpenMenu(); // Have the menu opened at game start.
        }
        public void Update()
        {
            // For now, loop through the levels.
            if (!_playArea.Loaded && RemainingBalls > 0)
            {
                var levels = 6;
                if (LevelsCleared % levels == 0)
                    _playArea.Load(PlayArea.Levels.Beginner0);
                else if (LevelsCleared % levels == 1)
                    _playArea.Load(PlayArea.Levels.Beginner1);
                else if (LevelsCleared % levels == 2)
                    _playArea.Load(PlayArea.Levels.Beginner2);
                else if (LevelsCleared % levels == 3)
                    _playArea.Load(PlayArea.Levels.Beginner3);
                else if (LevelsCleared % levels == 4)
                    _playArea.Load(PlayArea.Levels.Beginner4);
                else if (LevelsCleared % levels == 5)
                    _playArea.Load(PlayArea.Levels.Loop0);
            }

            // If game is over, reset the state of the game.
            if (!_playArea.Loaded && RemainingBalls == 0)
                Reset();

            // Clicking anywhere closes the menu.
            if (Globals.Paused && 
                Globals.ControlState.CursorSelectState == Controller.SelectStates.Pressed && 
                !MenuLocked && !_menus.IsCursorInWindow() && !_menus.Busy)
                CloseMenu();

            // A timer is put on opening/closing the menu to prevent unintended actions when closing/opening the main menu.
            if (_menuLockTime > 0)
                _menuLockTime -= Globals.GameTime.GetElapsedSeconds();
        }
    }
}
