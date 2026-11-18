mergeInto(LibraryManager.library, {
  WebBridge_SendScore: function (v) {
    if (typeof window.onUnityScore === "function") {
      window.onUnityScore(v);
    } else {
      console.log("[Unity->JS] score =", v);
    }
  },

  WebBridge_SendStatus: function (statusPtr) {
    var status = UTF8ToString(statusPtr);

    if (typeof window.onUnityStatus === "function") {
      window.onUnityStatus(status);
    } else {
      console.log("[Unity->JS] status =", status);
    }
  }
});
