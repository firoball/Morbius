using UnityEngine;

namespace Morbius.Scripts.Messages
{
    public interface ISitMessage : IMessageSystemHandler
    {
        void OnSit(Transform target);
        void OnSit();
        void OnStand();
    }
}
