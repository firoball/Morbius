using UnityEngine;
using Morbius.Scripts.Dialog;

namespace Morbius.Scripts.Level
{
    [RequireComponent(typeof(DialogPlayer))]
    public class DialogTrigger : BaseTrigger
    {
        private DialogPlayer m_dialogPlayer;

        private void Awake()
        {
            m_dialogPlayer = GetComponent<DialogPlayer>();
        }

        protected override void Entered()
        {
            m_dialogPlayer.Play();
        }

        protected override void Clicked()
        {
            m_dialogPlayer.Play();
        }

        protected override void AutoPlay()
        {
            m_dialogPlayer.Play();
        }

    }
}
