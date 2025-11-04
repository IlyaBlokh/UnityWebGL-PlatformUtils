(function () {
  window.PlatformChecker = {
    isMobile: function () {
      return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i
        .test(navigator.userAgent);
    },

    requestLandscape: function () {
      if (screen.orientation && screen.orientation.lock) {
        screen.orientation.lock("landscape").catch(function (err) {
          console.warn("Orientation lock failed:", err);
        });
      } else {
        console.warn("Screen Orientation API not supported");
      }
    },

    requiresFullscreen: function () {
      const canvas = document.getElementById("unity-canvas") || document.querySelector("canvas") || document.documentElement;
      const request = canvas.requestFullscreen || canvas.webkitRequestFullscreen || canvas.mozRequestFullScreen || canvas.msRequestFullscreen;
      if (request) {
        try {
          request.call(canvas);
        } catch (err) {
          console.warn("Fullscreen request failed:", err);
        }
      } else {
        console.warn("Fullscreen API not supported");
      }
    }
  };

  // Expose minimal functions for Unity to call via [DllImport("__Internal")]
  window.IsMobile = function () {
    return PlatformChecker.isMobile();
  };

  window.RequestLandscape = function () {
    PlatformChecker.requestLandscape();
  };

  window.RequiresFullscreen = function () {
    PlatformChecker.requiresFullscreen();
  };
})();
