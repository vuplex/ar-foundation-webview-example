using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;
using System.IO;

public class SaveCameraImage : MonoBehaviour
{
    [SerializeField]
    string m_Filename;

    [SerializeField]
    ARCameraBackground m_ARCameraBackground;

    [SerializeField]
    RenderTexture m_RenderTexture;

    [SerializeField]
    ImageFormat m_ImageFormat;

    Texture2D m_CachedCameraTexture;

    public enum ImageFormat
    {
        EXR,
        JPEG,
        PNG
    }

    public void SaveCameraTexture()
    {
        if (m_RenderTexture == null || m_ARCameraBackground == null || string.IsNullOrEmpty(m_Filename))
            return;

        EnsureTextureCreated();

        BlitToRenderTexture(m_RenderTexture, m_ARCameraBackground);

        if (SystemInfo.supportsAsyncGPUReadback)
        {
            Debug.Log("Using async GPU readback.");
            var request = AsyncGPUReadback.Request(m_RenderTexture);
            StartCoroutine(HandleGPUReadback(request));
        }
        else
        {
            Debug.Log("Async GPU readback not supported. Using ReadPixels (slow).");

            // Copy the RenderTexture from GPU to CPU
            var activeRenderTexture = RenderTexture.active;
            RenderTexture.active = m_RenderTexture;
            m_CachedCameraTexture.ReadPixels(new Rect(0, 0, m_RenderTexture.width, m_RenderTexture.height), 0, 0);
            m_CachedCameraTexture.Apply();
            RenderTexture.active = activeRenderTexture;
            WriteTextureToFile();
        }
    }

    public static void BlitToRenderTexture(RenderTexture renderTexture, ARCameraBackground cameraBackground)
    {
        if (renderTexture == null || cameraBackground == null || cameraBackground.material == null)
            return;

        // Copy the camera background to a RenderTexture
        Graphics.Blit(null, renderTexture, cameraBackground.material);
    }

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
        var path = Path.Combine(Application.persistentDataPath, m_Filename);
        WriteTextureToFile(m_CachedCameraTexture, path, m_ImageFormat);
        Debug.LogFormat("Saved texture to \"{0}\"", path);
    }

    void EnsureTextureCreated()
    {
        if (m_CachedCameraTexture == null)
            m_CachedCameraTexture = new Texture2D(m_RenderTexture.width, m_RenderTexture.height, TextureFormat.RGB24, true);
    }
}
