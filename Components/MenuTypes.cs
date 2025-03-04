using BreakoutExtreme.Utility;
using MonoGameGum.GueDeriving;

namespace BreakoutExtreme.Components
{
    public partial class Menus
    {
        public interface IInteractable : IUpdate
        {
            public bool Running { get; set; }
            public ContainerRuntime GetContainerRuntime();
        }
    }
}
