using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;
using System.IO;

/// <summary>
/// Saves an AR Camera Background image to a file.
/// Demonstrates blitting the texture to a <c>RenderTexture</c>
/// and then copying from the GPU to a CPU <c>Texture2D</c>.
/// Finally, the image can then be saved to an image on disk.
/// </summary>
public class SaveCameraImage : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The filename to save the camera background to.")]
    string m_Filename;

    /// <summary>
    /// The filename to save the camera background to.
    /// </summary>
    public string filename
    {
        get { return m_Filename; }
        set { m_Filename = value; }
    }

    [SerializeField]
    [Tooltip("The camera background which controls the camera image.")]
    ARCameraBackground m_ARCameraBackground;

    /// <summary>
    /// Get the <c>ARCameraBackground</c> which controls the camera image.
    /// </summary>
    public ARCameraBackground arCameraBackground
    {
        get { return m_ARCameraBackground; }
        private set { m_ARCameraBackground = value; }
    }

    [SerializeField]
    [Tooltip("The RenderTexture to blit the camera image to.")]
    RenderTexture m_RenderTexture;

    /// <summary>
    /// Get or set the <c>RenderTexture</c> to blit to.
    /// </summary>
    public RenderTexture renderTexture
    {
        get { return m_RenderTexture; }
        set { m_RenderTexture = value; }
    }

    [SerializeField]
    [Tooltip("The image format to use when saving to disk.")]
    ImageFormat m_ImageFormat;

    /// <summary>
    /// Defines an image format to use when saving to disk.
    /// </summary>
    public enum ImageFormat
    {
        EXR,
        JPEG,
        PNG
    }

    /// <summary>
    /// Get or set the image format used when saving to disk.
    /// </summary>
    public ImageFormat imageFormat
    {
        get { return m_ImageFormat; }
        set { m_ImageFormat = value; }
    }

    Texture2D m_CachedCameraTexture;

    /// <summary>
    /// Copies the <c>ARCameraBackground</c> to a texture and
    /// saves it to <see cref="filename"/>.
    /// </summary>
    public void SaveCameraTexture()
    {
        if (m_RenderTexture == null || m_ARCameraBackground == null)
            return;

        EnsureTextureCreated();

        // Copy the RenderTexture from GPU to CPU
        if (SystemInfo.supportsAsyncGPUReadback)
        {
            Debug.Log("Using async GPU readback.");

            // On platforms that support it, we can copy from GPU to CPU
            // using an asynchronous request, which doesn't stall the main thread.
            var request = AsyncGPUReadback.Request(m_RenderTexture);
            StartCoroutine(HandleGPUReadback(request));
        }
        else
        {
            Debug.Log("Async GPU readback not supported. Using ReadPixels (slow).");

            // On other platforms, we have to copy from GPU to CPU on the main thread.
            var activeRenderTexture = RenderTexture.active;
            RenderTexture.active = m_RenderTexture;
            m_CachedCameraTexture.ReadPixels(new Rect(0, 0, m_RenderTexture.width, m_RenderTexture.height), 0, 0);
            m_CachedCameraTexture.Apply();
            RenderTexture.active = activeRenderTexture;
            WriteTextureToFile();
        }
    }

    /// <summary>
    /// Blits the <c>ARCameraBackground</c> to a <c>RenderTexture</c>. Fast.
    /// </summary>
    /// <param name="renderTexture">The target <c>RenderTexture</c>.</param>
    /// <param name="cameraBackground">The <c>ARCameraBackground</c> which controls the camera image.</param>
    public static void BlitToRenderTexture(RenderTexture renderTexture, ARCameraBackground cameraBackground)
    {
        if (renderTexture == null)
            throw new ArgumentNullException("renderTexture");

        if (cameraBackground == null)
            throw new ArgumentNullException("cameraBackground");

        // Copy the camera background to a RenderTexture
        Graphics.Blit(null, renderTexture, cameraBackground.material);
    }

    /// <summary>
    /// Writes a <c>Texture2D</c> to a file.
    /// </summary>
    /// <param name="texture">The <c>Textur2D</c> to save.</param>
    /// <param name="path">The path to write the image to.</param>
    /// <param name="imageFormat">The image format to use.</param>
    public static void WriteTextureToFile(Texture2D texture, string path, ImageFormat imageFormat)
    {
        if (texture == null)
            throw new System.ArgumentNullException("texture");

        if (string.IsNullOrEmpty(path))
            throw new System.InvalidOperationException("No path specified");

        byte[] bytes;
        switch (imageFormat)
        {
            case ImageFormat.EXR:
                bytes = texture.EncodeToEXR();
                break;

            case ImageFormat.JPEG:
                bytes = texture.EncodeToJPG();
                break;

            case ImageFormat.PNG:
                bytes = texture.EncodeToPNG();
                break;

            default:
                return;
        }

        File.WriteAllBytes(path, bytes);
    }

    void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        if (renderTexture != null && arCameraBackground != null)
            BlitToRenderTexture(renderTexture, arCameraBackground);
    }

    void OnEnable()
    {
        ARSubsystemManager.cameraFrameReceived += OnCameraFrameReceived;
    }

    void OnDisable()
    {
        ARSubsystemManager.cameraFrameReceived -= OnCameraFrameReceived;
    }

    IEnumerator HandleGPUReadback(AsyncGPUReadbackRequest request)
    {
        if (!request.done)
            yield return new WaitWhile(() => !request.done);

        if (request.hasError)
            yield break;

        var colors = request.GetData<Color>();
        m_CachedCameraTexture.SetPixels(colors.ToArray());
        WriteTextureToFile();
    }

    void WriteTextureToFile()
    {
        if (string.IsNullOrEmpty(filename))
            return;

        var path = Path.Combine(Application.persistentDataPath, filename);
        WriteTextureToFile(m_CachedCameraTexture, path, imageFormat);
        Debug.LogFormat("Saved texture to \"{0}\"", path);
    }

    void EnsureTextureCreated()
    {
        if (m_CachedCameraTexture == null)
            m_CachedCameraTexture = new Texture2D(m_RenderTexture.width, m_RenderTexture.height, TextureFormat.RGB24, true);
    }
}
