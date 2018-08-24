using UnityEngine.XR.ARFoundation;

namespace UnityEngine.XR.Mock.Example
{
    [RequireComponent(typeof(Raycaster))]
    public class PlaceObjectAtRaycastHit : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        GameObject m_PlacedPrefab;

        /// <summary>
        /// The prefab to instantiate on touch.
        /// </summary>
        public GameObject placedPrefab
        {
            get { return m_PlacedPrefab; }
            set { m_PlacedPrefab = value; }
        }

        /// <summary>
        /// The object instantiated as a result of a successful raycast intersection with a plane.
        /// </summary>
        public GameObject spawnedObject { get; private set; }

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

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
            }
            else
            {
                spawnedObject.transform.position = hitPose.position;
                spawnedObject.transform.rotation = hitPose.rotation;
            }
        }
    }
}
