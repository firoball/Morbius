
namespace Morbius.Scripts.Messages
{
    public interface IDialogResultMessage : IMessageSystemHandler
    {
        void OnDecision(int index);
    }
}
