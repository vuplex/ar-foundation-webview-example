namespace UnityEngine.XR.Mock.Example
{
    public class Scaler : MonoBehaviour
    {
        [SerializeField]
        float m_Scale = 1f;

        void Update()
        {
            transform.localScale = Vector3.one * m_Scale;
        }
    }
}
