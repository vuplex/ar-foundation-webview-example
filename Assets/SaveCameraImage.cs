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
    ImageFormat m_ImageType;

    Texture2D m_CachedCameraTexture;

    enum ImageFormat
    {
        EXR,
        JPEG,
        PNG
    }

    public void SaveScreenshot()
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
            WriteToFile();
        }
    }

    public static void BlitToRenderTexture(RenderTexture renderTexture, ARCameraBackground cameraBackground)
    {
        if (renderTexture == null || cameraBackground == null || cameraBackground.material == null)
            return;

        // Copy the camera background to a RenderTexture
        Graphics.Blit(null, renderTexture, cameraBackground.material);
    }

    IEnumerator HandleGPUReadback(AsyncGPUReadbackRequest request)
    {
        if (!request.done)
            yield return new WaitWhile(() => !request.done);

        if (request.hasError)
            yield break;

        var colors = request.GetData<Color>();
        m_CachedCameraTexture.SetPixels(colors.ToArray());
        WriteToFile();
    }

    void WriteToFile()
    {
        if (m_CachedCameraTexture == null)
            return;

        byte[] bytes;
        switch (m_ImageType)
        {
            case ImageFormat.EXR:
                bytes = m_CachedCameraTexture.EncodeToEXR();
                break;

            case ImageFormat.JPEG:
                bytes = m_CachedCameraTexture.EncodeToJPG();
                break;

            case ImageFormat.PNG:
                bytes = m_CachedCameraTexture.EncodeToPNG();
                break;

            default:
                return;
        }

        var path = Path.Combine(Application.persistentDataPath, m_Filename);
        File.WriteAllBytes(path, bytes);
        Debug.LogFormat("Saved screenshot to \"{0}\"", path);
    }

    void EnsureTextureCreated()
    {
        if (m_CachedCameraTexture == null)
        {
            m_CachedCameraTexture = new Texture2D(m_RenderTexture.width, m_RenderTexture.height, TextureFormat.RGB24, true);
            m_CachedCameraTexture.wrapMode = TextureWrapMode.Repeat;
        }
    }
}
