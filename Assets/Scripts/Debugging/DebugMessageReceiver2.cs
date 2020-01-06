using Morbius.Scripts.Movement;
using UnityEngine;


namespace Morbius.Scripts.Debugging
{
    public class DebugMessageReceiver2 : MonoBehaviour, IPlayerClickEventTarget
    {
        public void OnPlayerClick()
        {
            Debug.Log("DebugMessageReceiver2 : MonoBehaviour, IPlayerClickEventTarget - OnPlayerClick");
        }
    }
}
