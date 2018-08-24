using System.Collections;
using UnityEngine.Experimental.XR;

namespace UnityEngine.XR.Mock.Example
{
    public class SimulatePlanes : MonoBehaviour
    {
        [SerializeField]
        bool m_Rotate;

        [SerializeField]
        float m_TrackingLostProbability = 0.01f;

        IEnumerator Start()
        {
            var boundaryPoints = new Vector2[4];
            boundaryPoints[0] = new Vector2(-.5f, -.5f);
            boundaryPoints[1] = new Vector2(-.5f, +.5f);
            boundaryPoints[2] = new Vector2(+.5f, +.5f);
            boundaryPoints[3] = new Vector2(+.5f, -.5f);

            var planeId = PlaneApi.Add(pose, boundaryPoints);

            float angle = 0f;
            while (enabled)
            {
                if (m_Rotate)
                {
                    transform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
                    angle += Time.deltaTime * 10f;
                }

                PlaneApi.Update(planeId, pose, boundaryPoints);

                if (Random.value < m_TrackingLostProbability)
                {
                    PlaneApi.SetTrackingState(planeId, TrackingState.Unavailable);
                    yield return new WaitForSeconds(1f);
                    PlaneApi.SetTrackingState(planeId, TrackingState.Tracking);
                }

                yield return null;
            }
        }

        Pose pose { get { return new Pose(transform.localPosition, transform.localRotation); } }
    }
}
