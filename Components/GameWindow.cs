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
        private int _levelsCleared = 0;
        private void UpdateScorePanel()
        {
            _scorePanel.Text = $"{_score}";
        }
        private void OpenMenu()
        {
            MenuLock();
            Globals.Pause();
            _dimmer.Start();
            _menus.Start(); 
        }
        private void CloseMenu()
        {
            MenuLock();
            Globals.Resume();
            _dimmer.Stop();
            _menus.Stop();
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
                _menus.Add(new() { Text = "We are testing something interesting" });
            }
        }
        public void Update()
        {
            // temporary
            if (!_playArea.Loaded && _levelsCleared < 3)
            {
                if (_levelsCleared == 0)
                    _playArea.Load(PlayArea.Levels.Test2);
                if (_levelsCleared == 1)
                    _playArea.Load(PlayArea.Levels.Test3);
                if (_levelsCleared == 2)
                    _playArea.Load(PlayArea.Levels.Test);
            }

            // Clicking anywhere closes the menu.
            if (Globals.Paused && Globals.ControlState.CursorSelectState == Controller.SelectStates.Pressed && !MenuLocked)
                CloseMenu();

            if (_menuLockTime > 0)
                _menuLockTime -= Globals.GameTime.GetElapsedSeconds();
        }
    }
}
