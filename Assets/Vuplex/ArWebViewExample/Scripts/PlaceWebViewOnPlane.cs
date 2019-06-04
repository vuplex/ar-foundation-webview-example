using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using Vuplex.WebView;

[RequireComponent(typeof(ARSessionOrigin))]
public class PlaceMultipleObjectsOnPlane : MonoBehaviour
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

    /// <summary>
    /// Invoked whenever an object is placed in on a plane.
    /// </summary>
    public static event Action onPlacedObject;

    ARSessionOrigin m_SessionOrigin;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Awake()
    {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
    }

    void Update()
    {
        if (spawnedObject != null) {
            return;
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (m_SessionOrigin.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
                {

                    Pose hitPose = s_Hits[0].pose;

                    // spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);

                    // Create a 0.6 x 0.3 instance of the prefab.
                    var webViewPrefab = WebViewPrefab.Instantiate(0.6f, 0.3f);
                    spawnedObject = webViewPrefab.gameObject;
                    webViewPrefab.transform.position = new Vector3(
                        hitPose.position.x,
                        hitPose.position.y + 0.5f,
                        hitPose.position.z
                    );
                    webViewPrefab.transform.rotation = hitPose.rotation;
                    webViewPrefab.transform.Rotate(0, 180, 0);
                    webViewPrefab.Initialized += (sender, e) => {
                        webViewPrefab.WebView.LoadUrl("https://www.google.com");
                    };

                    // Add the keyboard under the main webview.
                    var keyboard = Keyboard.Instantiate();
                    keyboard.transform.parent = webViewPrefab.transform;
                    keyboard.transform.localPosition = new Vector3(0, -0.31f, 0);
                    keyboard.transform.localEulerAngles = new Vector3(0, 0, 0);
                    // Hook up the keyboard so that characters are routed to the main webview.
                    keyboard.InputReceived += (sender, e) => webViewPrefab.WebView.HandleKeyboardInput(e.Value);

                    if (onPlacedObject != null)
                    {
                        onPlacedObject();
                    }
                }
            }
        }
    }

}
