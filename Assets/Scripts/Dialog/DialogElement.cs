using UnityEngine;

namespace Morbius.Scripts.Dialog
{
    public abstract class DialogElement : MonoBehaviour
    {
        public virtual DialogElement Next
        {
            get
            {
                return null;
            }

            set
            {
                //do nothing
            }
        }

        public virtual bool IsChoice()
        {
            return false;
        }
    }
}
