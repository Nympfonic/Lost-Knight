using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Vector3 cameraPosition = Vector3.zero;
	private GameObject target;

    public BoxCollider2D bounds;

    private Camera cam;
    private float minX, minY, maxX, maxY;

    public float zPos = -2.5f;

    public bool isFollowing;

    private bool staticCamPos = false;
    public float transitionTime = 3f;
    private float transitionTimer = 0;
    public Vector3 bossCameraPos;

    private Vector3 camInitialPosition;
    public float shakeMagnitude = 0.05f, shakeTime = 0.5f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        cam = GetComponent<Camera>();

        isFollowing = true;
    }

    void Update()
    {
        minX = bounds.transform.position.x - bounds.size.x / 2 + cam.orthographicSize * Screen.width / Screen.height;
        maxX = bounds.transform.position.x + bounds.size.x / 2 - cam.orthographicSize * Screen.width / Screen.height;

        minY = bounds.transform.position.y - bounds.size.y / 2 + cam.orthographicSize;
        maxY = bounds.transform.position.y + bounds.size.y / 2 - cam.orthographicSize;

        
    }

    void FixedUpdate()
    {
        if (isFollowing)
        {
            cameraPosition = new Vector3(
                Mathf.Clamp(Mathf.SmoothStep(transform.position.x, target.transform.position.x, 0.3f), minX, maxX),
                Mathf.Clamp(Mathf.SmoothStep(transform.position.y, target.transform.position.y, 0.3f), minY, maxY),
                zPos);
        }

        else if (staticCamPos)
        {
            transitionTimer += Time.deltaTime / transitionTime;

            cameraPosition = new Vector3(
                Mathf.SmoothStep(transform.position.x, bossCameraPos.x, transitionTimer),
                Mathf.SmoothStep(transform.position.y, bossCameraPos.y, transitionTimer),
                zPos);

            transform.position = cameraPosition;
        }
    }
	
	void LateUpdate()
    {
        if (isFollowing)
        {
            transform.position = cameraPosition;
        }
	}

    private void SetBossFightCamera()
    {
        staticCamPos = true;
        isFollowing = false;
    }

    private void DisableBossFightCamera()
    {
        staticCamPos = false;
        isFollowing = true;
    }

    #region Camera Shake functions
    public void CameraShake()
    {
        camInitialPosition = cameraPosition;
        InvokeRepeating("StartCameraShake", 0f, 0.005f);
        Invoke("StopCameraShake", shakeTime);
    }

    private void StartCameraShake()
    {
        float camShakeOffsetX = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        float camShakeOffsetY = Random.value * shakeMagnitude * 2 - shakeMagnitude;

        Vector3 camIntermediatePos = cameraPosition;
        camIntermediatePos.x += camShakeOffsetX;
        camIntermediatePos.y += camShakeOffsetY;
        cam.transform.position = camIntermediatePos;
    }

    private void StopCameraShake()
    {
        CancelInvoke("StartCameraShake");
        cameraPosition = camInitialPosition;
    }
    #endregion
}