export function RegisterServiceUpdates(dotnetHelper)
{
    window.document.addEventListener('touchstart', (event) => {
        // Adding this empty event listener allows for dragging
        // finger accross touch screen on safari.
    })
    window.document.addEventListener('touchend', (event) => {
        // Needed this to prevent the magnifying glass from appearing on safari and chrome.
    })
    window.document.addEventListener('click', (event) => {
        // Needed this to prevent the magnifying glass from appearing on safari and chrome.
    })
}