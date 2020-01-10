
namespace Morbius.Scripts.Messages
{
    public interface IInputBlockerMessage : IMessageSystemHandler
    {
        void OnBlock();
        void OnUnblock();
    }
}
