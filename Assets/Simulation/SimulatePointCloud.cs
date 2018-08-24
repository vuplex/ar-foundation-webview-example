using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.XR.Mock.Example
{
    public class SimulatePointCloud : MonoBehaviour
    {
        [SerializeField]
        float m_Radius = 5f;

        [SerializeField]
        float m_Density = 100;

        [SerializeField]
        Camera m_ARCamera;

        [SerializeField]
        float m_MovementProbability = .1f;

        [SerializeField]
        float m_MovementSpeed = 1f;

        [SerializeField]
        float m_SleepProbability = .01f;

        [SerializeField]
        float m_MinSleepTime = .5f;

        [SerializeField]
        float m_MaxSleepTime = 2f;

        Vector3[] m_Positions;

        float[] m_SleepTime;

        Vector3[] GeneratePoints()
        {
            var positions = new List<Vector3>();

            for (int i = 0; i < m_Positions.Length; ++i)
            {
                if (Random.value < m_SleepProbability)
                    m_SleepTime[i] = Random.Range(m_MinSleepTime, m_MaxSleepTime);

                if (m_SleepTime[i] > 0)
                {
                    m_SleepTime[i] -= Time.deltaTime;
                    continue;
                }

                var point = m_Positions[i];
                var viewportPoint = m_ARCamera.WorldToViewportPoint(point);
                if (viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                    viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
                    viewportPoint.z >= m_ARCamera.nearClipPlane && viewportPoint.z <= m_ARCamera.farClipPlane)
                {
                    positions.Add(point);
                }
            }

            return positions.ToArray();
        }

        IEnumerator Start()
        {
            int count = (int)(m_Density * 4f * Mathf.PI * m_Radius * m_Radius * m_Radius / 3f);
            m_Positions = new Vector3[count];
            m_SleepTime = new float[count];

            for (int i = 0; i < m_Positions.Length; ++i)
            {
                m_Positions[i] = Random.insideUnitSphere * m_Radius + transform.position;
            }

            while (enabled)
            {
                for (int i = 0; i < m_Positions.Length; ++i)
                {
                    if (Random.value < m_MovementProbability)
                        m_Positions[i] += Random.insideUnitSphere * Time.deltaTime * m_MovementSpeed;
                }

                DepthApi.SetDepthData(GeneratePoints(), null);
                yield return null;
            }
        }
    }
}
