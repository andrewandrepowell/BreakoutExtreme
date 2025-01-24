export function RegisterServiceUpdates(dotnetHelper)
{
    window.document.addEventListener('touchstart', (event) => {
        var array = new Array(event.touches.length)
        for (var i = 0; i < event.touches.length; i++) {
            let touch = event.touches[i];
            array[i] = {
                X: touch.pageX,
                Y: touch.pageY
            }
        }
        dotnetHelper.invokeMethodAsync('ServiceTouchStartUpdate', array)
    })
}