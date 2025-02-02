using MonoGameGum.GueDeriving;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BreakoutExtreme.Components
{
    public class Button
    {
        private static readonly ReadOnlyDictionary<States, string> _stateNineSliceAssetNames = new(new Dictionary<States, string>()
        {
            { States.Pressed, "animations/button_0" },
            { States.Released, "animations/button_1" }
        });
        private readonly Dictionary<States, Texture2D> _stateNineSlaceTextures = new();
        private readonly ContainerRuntime _containerRuntime;
        private readonly TextRuntime _textRuntime;
        private readonly NineSliceRuntime _nineSliceRuntime;
        private readonly GumDrawer _gumDrawer;
        private readonly string _text;
        private States _state = States.Released;
        public enum States { Released, Pressed }
        public States State => _state;
        public bool Running;
        public void Start()
        {

        }
        public void Stop()
        {

        }
        public GumDrawer GetGumDrawer() => _gumDrawer;
        public Button()
        {



            _gumDrawer = new(_containerRuntime);
        }
    }
}
