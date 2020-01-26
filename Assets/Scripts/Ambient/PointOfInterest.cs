using UnityEngine;

namespace Morbius.Scripts.Ambient
{
    public class PointOfInterest : MonoBehaviour
    {

        private void Start()
        {
            PointOfInterestWatcher.Register(transform);
        }

        private void OnDestroy()
        {
            PointOfInterestWatcher.UnRegister(transform);
        }

        private void OnDisable()
        {
            PointOfInterestWatcher.UnRegister(transform);
        }

        private void OnEnable()
        {
            PointOfInterestWatcher.Register(transform);
        }

    }
}
