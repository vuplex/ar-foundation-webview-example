using UnityEngine.Experimental.XR;

namespace UnityEngine.XR.Mock.Example
{
    public class SimulateSession : MonoBehaviour
    {
        [SerializeField]
        TrackingState m_TrackingState = TrackingState.Tracking;

        void Update()
        {
            SessionApi.trackingState = m_TrackingState;
        }
    }
}
