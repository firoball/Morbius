using UnityEngine;

namespace Morbius.Scripts.Messages
{
    public class MessageSystemStarter : MonoBehaviour
    {
        void Awake()
        {
            MessageSystem.Clear();
        }
    }
}
