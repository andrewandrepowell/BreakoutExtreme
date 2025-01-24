using Microsoft.JSInterop;
using System.Threading.Tasks;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;


namespace BreakoutExtreme
{
    public class BrowserService
    {
        // https://blazor.tips/blazor-how-to-ready-window-dimensions/
        // https://www.thinktecture.com/en/blazor/understanding-and-controlling-the-blazor-webassembly-startup-process/
        // https://www.javatpoint.com/javascript-events
        // https://stackoverflow.com/questions/71598291/what-event-handler-fires-on-resize-of-a-client-side-blazor-page
        // https://www.w3schools.com/jsref/obj_touchevent.asp
        // https://www.w3schools.com/jsref/dom_obj_event.asp
        // https://learn.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/call-dotnet-from-javascript?view=aspnetcore-8.0
#if DEBUG
        private static bool _initialized = false;
#endif
        private const int _touchLimit = 64;
        private IJSObjectReference _jsModule;
        private IJSRuntime _jsRuntime;
        public class Touch
        {
            public float X { get; set; }
            public float Y { get; set; }
        }
        public static Deque<Vector2> Touches = new Deque<Vector2>();
        public BrowserService(IJSRuntime jsRuntime)
        {
            Debug.Assert(_jsRuntime == null);
            _jsRuntime = jsRuntime;
        }
        public async Task ConfigureBrowserServer()
        {
            Debug.Assert(_jsModule == null);
            _jsModule = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/utility.js");
            await _jsModule.InvokeVoidAsync("RegisterServiceUpdates", DotNetObjectReference.Create(this));
        }
        [JSInvokable]
        public void ServiceTouchStartUpdate(Touch[] touches)
        {
            foreach (ref var touch in touches.AsSpan())
            {
                if (Touches.Count == _touchLimit)
                    Touches.RemoveFromFront();
                Touches.AddToBack(new Vector2(touch.X, touch.Y));
            }
        }
    }
}
