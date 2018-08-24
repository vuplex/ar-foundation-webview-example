using UnityEngine.XR.ARFoundation;

namespace UnityEngine.XR.Mock.Example
{
    [RequireComponent(typeof(Raycaster))]
    public class MakeObjectAppearAtRaycastHit : MonoBehaviour
    {
        [SerializeField]
        Transform m_Content;

        [SerializeField]
        ARSessionOrigin m_SessionOrigin;

        void OnEnable()
        {
            GetComponent<Raycaster>().rayHit += OnRayHit;
        }

        void OnDisable()
        {
            GetComponent<Raycaster>().rayHit -= OnRayHit;
        }

        void OnRayHit(ARRaycastHit hit)
        {
            Pose hitPose = hit.pose;
            m_SessionOrigin.MakeContentAppearAt(m_Content, hitPose.position);
        }
    }
}
