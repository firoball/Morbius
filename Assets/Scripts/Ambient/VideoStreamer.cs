using UnityEngine;
using UnityEngine.Video;
using System.IO;

[RequireComponent(typeof(VideoPlayer))]
public class VideoStreamer : MonoBehaviour
{
    [SerializeField]
    private string m_videoAsset;
    [SerializeField]
    private AudioSource m_audio;
    [SerializeField]
    private AudioClip m_clip;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer && !string.IsNullOrWhiteSpace(m_videoAsset))
        {
            VideoPlayer player = GetComponent<VideoPlayer>();
            player.source = VideoSource.Url;
            string url = Application.streamingAssetsPath;
            player.url = Path.Combine(url, m_videoAsset);
            player.audioOutputMode = VideoAudioOutputMode.None;
            if (m_clip && m_audio)
            {
                m_audio.clip = m_clip;
                m_audio.loop = true;
                m_audio.Play();
            }
        }

    }

}
