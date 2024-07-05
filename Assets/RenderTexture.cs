using UnityEngine;
using System.IO;

public class SaveRenderTextureAsPNG : MonoBehaviour
{
    public RenderTexture renderTexture;

    void Start()
    {
        SaveRenderTextureToPNG(renderTexture, "renderTexture.png");
    }

    void SaveRenderTextureToPNG(RenderTexture rt, string filePath)
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);

        RenderTexture.active = currentRT;

        Debug.Log("Saved RenderTexture to " + filePath);
    }
}
