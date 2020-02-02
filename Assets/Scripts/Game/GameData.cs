
namespace Morbius.Scripts.Game
{
    public class GameData
    {
        private bool m_goodEnding;

        public bool GoodEnding { get => m_goodEnding; set => m_goodEnding = value; }

        public void Initialize()
        {
            m_goodEnding = false;
        }
    }

}
