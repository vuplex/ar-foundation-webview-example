namespace UnityEngine.XR.Mock.Example
{
    public class Oscillate : MonoBehaviour
    {
        [SerializeField]
        float m_Speed = 1f;

        [SerializeField]
        float m_Distance = .25f;

        void Update()
        {
            transform.localPosition = Vector3.up * (Mathf.Sin(Time.realtimeSinceStartup * m_Speed) * m_Distance);
        }
    }
}
