using UnityEngine;
using UnityEngine.Video;
using System.IO;

[RequireComponent(typeof(VideoPlayer))]
public class VideoStreamer : MonoBehaviour
{
    [SerializeField]
    private string m_videoAsset;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer && !string.IsNullOrWhiteSpace(m_videoAsset))
        {
            VideoPlayer player = GetComponent<VideoPlayer>();
            player.source = VideoSource.Url;
            string url = Application.streamingAssetsPath;
            player.url = Path.Combine(url, m_videoAsset);
        }

    }

}
