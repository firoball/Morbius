
namespace Morbius.Scripts.Messages
{
    public interface IChapterResultMessage : IMessageSystemHandler
    {
        void OnChapterDone();
    }
}
