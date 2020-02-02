using UnityEngine;
using UnityEngine.UI;
using Morbius.Scripts.Util;

public class DebugWebGL : MonoBehaviour
{
    [SerializeField]
    private Text m_output;

    // Use this for initialization
    void Start()
    {
        if (m_output)
            m_output.text = URLReader.ReadURL()+"\n"+ Application.streamingAssetsPath;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
