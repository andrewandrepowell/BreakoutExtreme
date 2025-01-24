using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.Diagnostics;


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
        private IJSObjectReference _jsModule;
        private IJSRuntime _jsRuntime;
        public BrowserService(IJSRuntime jsRuntime)
        {
            Debug.Assert(_jsRuntime == null);
            _jsRuntime = jsRuntime;
        }
        public async Task ConfigureBrowserServer()
        {
#if DEBUG
            Debug.Assert(!_initialized);
            _initialized = true;
#endif
            Debug.Assert(_jsModule == null);
            _jsModule = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/utility.js");
            await _jsModule.InvokeVoidAsync("RegisterServiceUpdates", DotNetObjectReference.Create(this));
        }
    }
}
