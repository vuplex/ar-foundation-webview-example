using UnityEngine.XR.ARFoundation;

namespace UnityEngine.XR.Mock.Example
{
    [RequireComponent(typeof(Raycaster))]
    public class PlaneAttacher : MonoBehaviour
    {
        [SerializeField]
        ARSessionOrigin m_ARSessionOrigin;

        [SerializeField]
        float m_DistanceFromPlane = .1f;

        ARReferencePoint m_Attachment;

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
            if (!Input.GetMouseButtonDown(0))
                return;

            var referencePointManager = m_ARSessionOrigin.GetComponent<ARReferencePointManager>();
            var planeManager = m_ARSessionOrigin.GetComponent<ARPlaneManager>();

            if (referencePointManager == null || planeManager == null)
                return;

            var plane = planeManager.TryGetPlane(hit.trackableId);
            if (plane == null)
                return;

            if (m_Attachment != null)
            {
                referencePointManager.TryRemoveReferencePoint(m_Attachment);
                m_Attachment = null;
            }

            var planeNormal = plane.transform.up;
            var pose = new Pose(hit.pose.position + planeNormal * m_DistanceFromPlane, hit.pose.rotation);
            m_Attachment = referencePointManager.TryAttachReferencePoint(plane, pose);
        }
    }
}
