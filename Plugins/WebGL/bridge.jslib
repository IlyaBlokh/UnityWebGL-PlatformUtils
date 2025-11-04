mergeInto(LibraryManager.library, {
    IsMobile: function() {
        var userAgent = navigator.userAgent || navigator.vendor || window.opera;
        return /android|iphone|ipad|ipod|mobile/i.test(userAgent);
    },

    RequestLandscape: function() {
        if (screen.orientation && screen.orientation.lock) {
            screen.orientation.lock("landscape").catch(err => console.warn("Orientation lock failed:", err));
        }
    },

    RequiresFullscreen: function() {
        var canvas = document.getElementById("unity-canvas");
        if (canvas.requestFullscreen) {
            canvas.requestFullscreen().catch(err => console.warn("Fullscreen failed:", err));
        } else if (canvas.webkitRequestFullscreen) {
            canvas.webkitRequestFullscreen();
        }
    }
});
