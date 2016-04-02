using UnityEngine;
using System.Collections;

public class CamControl : MonoBehaviour
{
    public static CamControl Instance { get; private set; }

    public string buttonFree = "Free Cam";
    public float lockedSize;

    public Teacher teacher;

    private float freeSize;
    private Vector3 freePos;
    private Vector3 lockedPos;
    private float locking;
    private Camera cam;

    void Awake()
    {
        Instance = this;
    }

	void Start ()
    {
        freePos = transform.position;
        cam = GetComponent<Camera>();
        freeSize = cam.orthographicSize;
	}
	
	void Update ()
    {
        if (Input.GetButtonDown(buttonFree))
            teacher = null;

	    if (teacher)
        {
            locking = Mathf.Min(1, locking + Time.deltaTime);
            lockedPos = teacher.transform.position;
            lockedPos.y = freePos.y;
        }
        else
        {
            locking = Mathf.Max(0, locking - Time.deltaTime);
        }

        cam.orthographicSize = Mathf.Lerp(freeSize, lockedSize, locking);
        transform.position = Vector3.Lerp(freePos, lockedPos, locking);
	}
}
