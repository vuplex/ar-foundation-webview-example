# AR Foundation WebView Example

This Unity project demonstrates how to interact with web content in augmented reality using AR Foundation and [Vuplex 3D WebView](https://developer.vuplex.com/webview/overview). It's a fork of the [Unity AR Foundation Samples](https://github.com/Unity-Technologies/arfoundation-samples) repo that adds a scene named [ARWebViewDemoScene](./Assets/Vuplex/ARWebViewExample/Scenes).

<p align="center">
  <img alt="demo" src="./demo.gif" width="480">
</p>

## Instructions for building

1. Download Unity 2020.3 or 2021.1

2. Open Unity and load the project at the root of the *ar-foundation-webview-example* repository.

3. Import [Vuplex 3D WebView](https://developer.vuplex.com/webview/overview).

4. Select Android or iOS as the platform.

5. Build [ARWebViewDemoScene](./Assets/Vuplex/ArWebViewExample/Scenes) and run it on your device.

## Steps taken to create this project

1. Forked the [Unity AR Foundation Samples repo (4.1 branch)](https://github.com/Unity-Technologies/arfoundation-samples/tree/4.1).

2. Imported [Vuplex 3D WebView](https://developer.vuplex.com/webview/overview) ([.gitignore](https://github.com/vuplex/ar-foundation-webview-example/blob/83323472959cf110c7196e5c05a323cd07828967/.gitignore#L45)).

3. Made a copy of the [Anchors](./Assets/Scenes/Anchors.unity) scene named [ARWebViewDemoScene](./Assets/Vuplex/ARWebViewExample/Scenes).

4. Made the following modifications to the new ARWebViewDemoScene scene ([e31b637](https://github.com/vuplex/ar-foundation-webview-example/commit/e31b63736a6f6042d6c5bce2e6ba134870c0d889)):
    - Changed the "Prefab" setting of the scene's [AnchorCreator](./Assets/Scripts/AnchorCreator.cs) to a new prefab with a [WebViewARAnchor.cs](./Assets/Vuplex/ARWebViewExample/Scripts/WebViewARAnchor.cs) script that creates a [WebViewPrefab](https://developer.vuplex.com/webview/WebViewPrefab) and [Keyboard](https://developer.vuplex.com/webview/Keyboard).
    - Added a Physics Raycaster to the AR Camera to enable touch interaction with the WebViewPrefab and Keyboard.
    - Replaced the scene's StandaloneInputModule with InputSystemUIInputModule because the project uses the new Input System.
