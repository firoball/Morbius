
namespace Morbius.Scripts.Messages
{
    public interface IUnlockTriggerMessage : IMessageSystemHandler
    {
        void OnUnlock();
        void OnLock();
    }
}
