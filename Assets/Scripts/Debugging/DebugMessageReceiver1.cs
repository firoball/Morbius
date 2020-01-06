using Morbius.Scripts.Messages;
using UnityEngine;


namespace Morbius.Scripts.Debugging
{
    public class DebugMessageReceiver1 : MonoBehaviour, ICursorUIMessage
    {
        public void OnUIEnter()
        {
            Debug.Log("DebugMessageReceiver1 : MonoBehaviour, ICursorUIEventTarget - OnUIEnter");
        }

        public void OnUIExit()
        {
            Debug.Log("DebugMessageReceiver1 : MonoBehaviour, ICursorUIEventTarget - OnUIExit");
        }

    }
}
