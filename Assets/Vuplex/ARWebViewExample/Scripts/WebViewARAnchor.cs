using UnityEngine;
using UnityEngine.XR.ARFoundation.Samples;
using Vuplex.WebView;

class WebViewARAnchor : MonoBehaviour {

    async void Start() {

        // First, disable the AnchorCreator in the scene so that only one WebViewPrefab is created.
        // This is done so that it doesn't create other webviews in response to interacting with the webview.
        _setAnchorCreatorEnabled(false);

        // Instantiate a 0.6 x 0.3 WebViewPrefab.
        // https://developer.vuplex.com/webview/WebViewPrefab#Instantiate
        var webViewPrefab = WebViewPrefab.Instantiate(0.6f, 0.3f);
        webViewPrefab.transform.SetParent(transform, false);
        webViewPrefab.transform.localPosition = new Vector3(0, 0.7f, 0);
        webViewPrefab.transform.localEulerAngles = new Vector3(0, 180, 0);

        // Add an on-screen keyboard under the webview.
        // https://developer.vuplex.com/webview/Keyboard
        var keyboard = Keyboard.Instantiate();
        keyboard.transform.parent = webViewPrefab.transform;
        keyboard.transform.localPosition = new Vector3(0, -0.31f, 0);
        keyboard.transform.localEulerAngles = Vector3.zero;

        // Wait for the prefab to initialize because its WebView property is null until then.
        // https://developer.vuplex.com/webview/WebViewPrefab#WaitUntilInitialized
        await webViewPrefab.WaitUntilInitialized();

        // After the prefab has initialized, you can use the IWebView APIs via its WebView property.
        // https://developer.vuplex.com/webview/IWebView
        webViewPrefab.WebView.LoadUrl("https://www.google.com");
    }

    // Re-enable the anchor creator so that a new webview can be created.
    void OnDestroy() => _setAnchorCreatorEnabled(true);

    void _setAnchorCreatorEnabled(bool enabled) {

        var anchorCreator = FindObjectOfType<AnchorCreator>();
        if (anchorCreator != null) {
            anchorCreator.enabled = enabled;
        }
    }
}
