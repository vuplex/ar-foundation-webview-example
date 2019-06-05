# AR Foundation WebView Example

This Unity project demonstrates how to interact with web content in augmented reality using AR Foundation and [Vuplex 3D WebView](https://developer.vuplex.com/webview/overview). It's a fork of the Unity AR Foundation Samples repo that adds a scene named [ArWebViewDemoScene](./Assets/Vuplex/ArWebViewExample/Scenes).

<p align="center">
  <img alt="demo" src="./demo.gif" width="480">
</p>

## Instructions for building

1. Download Unity 2019.1 or later

2. Open Unity and load the project at the root of the *ar-foundation-webview-example* repository

3. Import [Vuplex 3D WebView](https://developer.vuplex.com/webview/overview)

4. Select Android or iOS as the platform

5. Build [ArWebViewDemoScene](./Assets/Vuplex/ArWebViewExample/Scenes) and run it on your device

## Steps taken to create this project

1. Forked the Unity AR Foundation Samples repo

2. Imported [Vuplex 3D WebView](https://developer.vuplex.com/webview/overview) ([.gitignore](https://github.com/vuplex/ar-foundation-webview-example/blob/83323472959cf110c7196e5c05a323cd07828967/.gitignore#L45))

3. Made a copy of [SampleUXScene](https://github.com/vuplex/ar-foundation-webview-example/blob/83323472959cf110c7196e5c05a323cd07828967/Assets/Scenes/UX/SampleUXScene.unity) named [ArWebViewDemoScene](./Assets/Vuplex/ArWebViewExample/Scenes) ([3efff2e](https://github.com/vuplex/ar-foundation-webview-example/commit/3efff2e999c04501955d5ac5627f07cf2d1d3aac))

4. Made copies of [PlaceMultipleObjectsOnPlane.cs](https://github.com/vuplex/ar-foundation-webview-example/blob/83323472959cf110c7196e5c05a323cd07828967/Assets/Scripts/PlaceMultipleObjectsOnPlane.cs) and [UIManager.cs](https://github.com/vuplex/ar-foundation-webview-example/blob/83323472959cf110c7196e5c05a323cd07828967/Assets/Scripts/UX/UIManager.cs) named [PlaceWebViewOnPlane.cs](./Assets/Vuplex/ArWebViewExample/Scripts/PlaceWebViewOnPlane.cs) and [WebViewDemoUIManager.cs](./Assets/Vuplex/ArWebViewExample/Scripts/WebViewDemoUIManager.cs) ([517f20e](https://github.com/vuplex/ar-foundation-webview-example/commit/517f20e9a5c9c190185a2e0a6143c5496adf2b20))

5. Updated those scripts to place a [WebViewPrefab](https://developer.vuplex.com/webview/WebViewPrefab) and [Keyboard](https://developer.vuplex.com/webview/Keyboard) in the scene when the screen is tapped ([8332347](https://github.com/vuplex/ar-foundation-webview-example/commit/83323472959cf110c7196e5c05a323cd07828967))

6. Made the following changes in the scene to allow the webview and keyboard to be controlled by touching them ([8332347](https://github.com/vuplex/ar-foundation-webview-example/commit/83323472959cf110c7196e5c05a323cd07828967))
    - Added a Physics Raycaster to the AR Camera
    - Updated MoveDeviceAnimation and TapToPlaceAnimation to disable their images' "Raycast Target" option
