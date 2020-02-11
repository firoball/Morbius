using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Morbius.Scripts.Game;
using Morbius.Scripts.Messages;

public class DebugScreenCapture : MonoBehaviour
{

//read only
    [SerializeField]
    private Texture2D m_original;
    [SerializeField]
    private Texture2D m_resized;
    [SerializeField]
    private string m_base64;

//apply in inspector
    [SerializeField]
    private Image m_reference;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Save(0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Save(1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Save(2);
        }
    }

    private void Save(int slot)
    {
        m_original = ScreenCapture.CaptureScreenshotAsTexture();

        //this ispart of Quit to MainMenu activity
        //execute in OnPostRender
        float newHeight = m_reference.rectTransform.rect.height * 1.2f; //add some factor so UI is cut off
        float factor = newHeight / m_original.height;
        float newWidth = m_original.width * factor;
        m_resized = Resize(m_original, Convert.ToInt32(newWidth), Convert.ToInt32(newHeight));

        GameStatus.Data.SetScreenshot(m_resized, slot);
        GameStatus.Data.Scene = SceneManager.GetActiveScene().name;
        MessageSystem.Execute<ISaveStatusMessage>((x, y) => x.OnSave());

        GameStatus.SaveGame(slot);
    }

    public void Capture()
    {
        //Debug.Log(m_reference.rectTransform.rect.width + " " + m_reference.rectTransform.rect.height + " " + m_reference.sprite.texture.width + " " + m_reference.sprite.texture.height);
        m_original = ScreenCapture.CaptureScreenshotAsTexture();

        //this ispart of Quit to MainMenu activity
        //execute in OnPostRender
        float newHeight = m_reference.rectTransform.rect.height * 1.2f; //add some factor so UI is cut off
        float factor = newHeight / m_original.height;
        float newWidth = m_original.width * factor;
        //Debug.Log(newWidth + " " + newHeight);
        m_resized = Resize(m_original, Convert.ToInt32(newWidth), Convert.ToInt32(newHeight));

        byte[] bytes = m_resized.EncodeToPNG();
        m_base64 = Convert.ToBase64String(bytes);




        //this would go to LoadGameUI
        byte[] sourceData = Convert.FromBase64String(m_base64);
        Texture2D source = new Texture2D(2,2);
        source.LoadImage(sourceData);
        source.Apply();
        m_reference.sprite = BuildSprite(source, m_reference);
    }

    private Texture2D Resize(Texture2D texture2D, int width, int height)
    {
        RenderTexture rt = new RenderTexture(width, height, 24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D, rt);
        Texture2D result = new Texture2D(width, height);
        result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        result.Apply();
        return result;
    }

    //this would go to LoadGameUI
    private Sprite BuildSprite(Texture2D texture, Image target)
    {
        Rect baseRect = target.rectTransform.rect;
        float x = (texture.width - baseRect.width) * 0.5f;
        float y = (texture.height - baseRect.height) * 0.5f;
        float w = Mathf.Min(baseRect.width, texture.width);
        float h = Mathf.Min(baseRect.height, texture.height);
        Rect newRect = new Rect(x, y, w, h);
        return Sprite.Create(texture, newRect, target.sprite.pivot);
    }
}
