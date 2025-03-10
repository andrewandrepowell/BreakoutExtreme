using MonoGame.Extended.Collections;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

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
        private record ActionNode(Func<CancellationToken, Task> Operation, string Message);
        public enum States { Waiting, Loading, Loaded }
        public bool Loaded => _state == States.Loaded;
        public void Add(Action action, string message)
        {
            Debug.Assert(_state == States.Waiting);
            var task = new Task(() => 
            { 
                action(); 
                _currentNode++; 
            });
            var operation = delegate (CancellationToken token)
            {
                task.RunSynchronously();
                return task;
            };
            _nodes.Add(new(operation, message));
        }
        public void Start()
        {
            Debug.Assert(_state == States.Waiting);
            _state = States.Loading;
        }
        public void Update()
        {
            if (_state == States.Loading && QueuedHostedService.TaskQueue.Count == 0)
            {
                if (_currentNode == _nodes.Count)
                {
                    _loadMessage = "Playing...";
                    _messageAction?.Invoke(_loadMessage);
                    _loadedAction?.Invoke();
                    _state = States.Loaded;
                }
                else
                {
                    var node = _nodes[_currentNode];
                    _loadMessage = $"({_currentNode + 1} / {_nodes.Count}) {node.Message}...";
                    _messageAction?.Invoke(_loadMessage);
                    QueuedHostedService.TaskQueue.QueueBackgroundWorkItem(node.Operation);
                }
            }
        }
    }
}
