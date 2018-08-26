using Morbius.Scripts.Level;
using UnityEngine;


namespace Morbius.Scripts.Debugging
{
    public class DebugScenePortal : MonoBehaviour
    {
        [SerializeField]
        private ScenePortal m_scenePortal;

        private void OnGUI()
        {
            if (m_scenePortal)
            {
                if (GUI.Button(new Rect(25, 480, 100, 30), "Load Scene"))
                {
                    m_scenePortal.Load();
                }
            }


        }

    }
}
