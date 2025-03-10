using MonoGame.Extended.Collections;
using System;
using System.Diagnostics;

namespace BreakoutExtreme
{
    public class Loader(Action<string> messageAction = null, Action loadedAction = null)
    {
        private bool _loaded = false;
        private Bag<ActionNode> _nodes = [];
        private States _state = States.Waiting;
        private string _loadMessage = "";
        private int _currentNode = 0;
        private Action<string> _messageAction = messageAction;
        private Action _loadedAction = loadedAction;
        private record ActionNode(Action Operation, string Message);
        public enum States { Waiting, Loading, Loaded }
        public bool Loaded => _state == States.Loaded;
        public void Add(Action action, string message)
        {
            Debug.Assert(_state == States.Waiting);
            _nodes.Add(new(action, message));
        }
        public void Start()
        {
            Debug.Assert(_state == States.Waiting);
            _state = States.Loading;
        }
        public void Update()
        {
            if (_state == States.Loading)
            {

                var node = _nodes[_currentNode++];
                _loadMessage = $"({_currentNode} / {_nodes.Count}) {node.Message}...";
                _messageAction?.Invoke(_loadMessage);
                node.Operation();
                if (_currentNode == _nodes.Count - 1)
                {
                    _loadedAction?.Invoke();
                    _state = States.Loaded;
                }
            }
        }
    }
}
