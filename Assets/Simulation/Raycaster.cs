using System;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;

namespace UnityEngine.XR.Mock.Example
{
    /// <summary>
    /// Performs a raycast against AR Trackables and invokes an event when a hit is detected.
    /// </summary>
    public class Raycaster : MonoBehaviour
    {
        [SerializeField]
        ARSessionOrigin m_ARSessionOrigin;

        /// <summary>
        /// The <see cref="ARSessionOrigin"/> to raycast against.
        /// </summary>
        public ARSessionOrigin sessionOrigin
        {
            get { return m_ARSessionOrigin; }
            set { m_ARSessionOrigin = value; }
        }

        [Header("Trackable Types")]

        [SerializeField, Tooltip("Raycast against each point in the point cloud.")]
        bool m_PointCloud;

        /// <summary>
        /// If true, raycasts against each point in the point cloud.
        /// </summary>
        public bool pointCloud
        {
            get { return m_PointCloud; }
            set { m_PointCloud = value; }
        }

        [SerializeField, Tooltip("Raycast against each plane's exact polygon, using its boundary points.")]
        bool m_PlanePolygon = true;

        /// <summary>
        /// If true, raycasts against each plane's exact polygon, using its boundary points.
        /// </summary>
        public bool planePolygon
        {
            get { return m_PlanePolygon; }
            set { m_PlanePolygon = value; }
        }

        [SerializeField, Tooltip("Raycast against each plane's bounding box.")]
        bool m_PlaneBounds;

        /// <summary>
        /// If true, raycasts against each plane's bounding box.
        /// </summary>
        public bool planeBounds
        {
            get { return m_PlaneBounds; }
            set { m_PlaneBounds = value; }
        }

        [SerializeField, Tooltip("Raycast against each plane as if it were infinitely large.")]
        bool m_PlaneInfinte;

        /// <summary>
        /// If true, raycasts against each plane as if it were infinitely large.
        /// </summary>
        public bool planeInfinite
        {
            get { return m_PlaneInfinte; }
            set { m_PlaneInfinte = value; }
        }

        /// <summary>
        /// Invoked when a raycast hits one of the requested trackables.
        /// If multiple trackables are hit, the closest one is provided.
        /// </summary>
        public event Action<ARRaycastHit> rayHit;

        /// <summary>
        /// A mask of <c>TrackableType</c>s to raycast against.
        /// </summary>
        public TrackableType trackableTypeMask
        {
            get
            {
                var mask = TrackableType.None;
                if (m_PointCloud)
                    mask |= TrackableType.FeaturePoint;
                if (m_PlaneInfinte)
                    mask |= TrackableType.PlaneWithinInfinity;
                if (m_PlaneBounds)
                    mask |= TrackableType.PlaneWithinBounds;
                if (m_PlanePolygon)
                    mask |= TrackableType.PlaneWithinPolygon;

                return mask;
            }
        }

        void Update()
        {
            if (m_ARSessionOrigin != null &&
                rayHit != null &&
                Input.GetMouseButton(0) &&
                m_ARSessionOrigin.Raycast(Input.mousePosition, s_Hits, trackableTypeMask))
            {
                rayHit(s_Hits[0]);
            }
        }

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    }
}
