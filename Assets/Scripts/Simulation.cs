using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    private CameraController camController;

    public Vector3 endCamPos;
    public float transitionTime = 3f;
    private float transitionTimer = 0;
    private bool staticCamPos = false;

    void Awake()
    {
        camController = GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (staticCamPos)
        {
            transitionTimer += Time.deltaTime / transitionTime;

            camController.cameraPosition = new Vector3(
                Mathf.SmoothStep(transform.position.x, endCamPos.x, transitionTimer),
                Mathf.SmoothStep(transform.position.y, endCamPos.y, transitionTimer),
                camController.zPos);

            transform.position = camController.cameraPosition;
        }
    }

    #region Animation Event functions
    private void AnimStarted()
    {
        staticCamPos = true;
        camController.isFollowing = false;
    }

    private void AnimFinished()
    {

    }
    #endregion
}
