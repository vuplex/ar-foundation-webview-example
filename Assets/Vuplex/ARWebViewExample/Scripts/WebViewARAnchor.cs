using UnityEngine;
using UnityEngine.XR.ARFoundation.Samples;
using Vuplex.WebView;

class WebViewARAnchor : MonoBehaviour {

    async void Start() {

        // First, disable the AnchorCreator in the scene so that only one WebViewPrefab is created.
        // This is done so that it doesn't create other webviews in response to interacting with the webview.
        _setAnchorCreatorEnabled(false);

        // Create a 0.6 x 0.3 WebViewPrefab.
        var webViewPrefab = WebViewPrefab.Instantiate(0.6f, 0.3f);
        webViewPrefab.transform.SetParent(transform, false);
        webViewPrefab.transform.localPosition = new Vector3(0, 0.7f, 0);
        webViewPrefab.transform.localEulerAngles = new Vector3(0, 180, 0);

        // Add the keyboard under the WebViewPrefab.
        var keyboard = Keyboard.Instantiate();
        keyboard.transform.parent = webViewPrefab.transform;
        keyboard.transform.localPosition = new Vector3(0, -0.31f, 0);
        keyboard.transform.localEulerAngles = new Vector3(0, 0, 0);
        // Hook up the keyboard so that characters are routed to the main webview.
        keyboard.InputReceived += (sender, e) => webViewPrefab.WebView.HandleKeyboardInput(e.Value);

        // Wait for the WebViewPrefab to initialize, because the WebViewPrefab.WebView property
        // is null until the prefab has initialized.
        await webViewPrefab.WaitUntilInitialized();
        webViewPrefab.WebView.LoadUrl("https://www.google.com");
    }

    void OnDestroy() {

        // Re-enable the anchor creator so that a new webview can be created.
       _setAnchorCreatorEnabled(true);
    }

    void _setAnchorCreatorEnabled(bool enabled) {

        var anchorCreator = FindObjectOfType<AnchorCreator>();
        if (anchorCreator != null) {
            anchorCreator.enabled = enabled;
        }
    }
}
