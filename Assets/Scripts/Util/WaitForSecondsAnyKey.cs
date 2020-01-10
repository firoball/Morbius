using UnityEngine;
namespace Morbius.Scripts.Util
{
    public class WaitForSecondsAnyKey : CustomYieldInstruction
    {
        private float m_delay;
        public override bool keepWaiting
        {
            get
            {
                m_delay -= Time.deltaTime;
                return !(Input.anyKeyDown || m_delay <= 0.0f);
            }
        }

        public WaitForSecondsAnyKey(float seconds)
        {
            m_delay = seconds;
        }
    }
}
