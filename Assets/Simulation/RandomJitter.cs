namespace UnityEngine.XR.Mock.Example
{
    public class RandomJitter : MonoBehaviour
    {
        [SerializeField]
        float m_Probability = .1f;

        [SerializeField]
        float m_MaxDisplacementPerJitter = .1f;

        void Update()
        {
            if (Random.value < m_Probability)
                transform.localPosition += Random.insideUnitSphere * m_MaxDisplacementPerJitter;
        }
    }
}
