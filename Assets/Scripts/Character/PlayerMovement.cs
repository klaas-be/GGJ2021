using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpHeight = 5.0f;
    private Camera mainCamera;
    private float gravityValue = -20f;
    private Vector3 motion = Vector3.zero;
    private Vector3 camRot;

    [Header("CrawlSettings")]
    [SerializeField] Transform CrawlCollCenterRef;
    [SerializeField] float crawlHeight = 0.5f;
    private CharacterController characterController;
    private float origCollHeight;
    private Vector3 origCollCenter;
    private bool hasNoLimbs = false;

    private void Start()
    {
        mainCamera = Camera.main;
        
        characterController = this.GetComponent<CharacterController>();
        origCollHeight = characterController.height;
        origCollCenter = characterController.center;

        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        camRot = mainCamera.transform.eulerAngles;
        camRot.x = 0;
        camRot.z = 0;
        transform.rotation = Quaternion.Euler(camRot);

        motion = Quaternion.Euler(camRot) * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        motion = motion * (speed * Time.deltaTime);
        motion.y = gravityValue * Time.deltaTime;
        if (!hasNoLimbs)
            characterController.Move(motion);
        else
            characterController.Move(new Vector3(0, gravityValue * Time.deltaTime, 0));
    }

    public void SetCrawlmode(bool to)
    {
        if (to)
        {
            characterController.height = crawlHeight;
            characterController.center = CrawlCollCenterRef.position - this.transform.position;
        }
        else
        {
            StartCoroutine(ResetCrawlmode(2f));
        }
    }

    public void SetNoLimbsMode(bool to)
    {
        hasNoLimbs = to;
    }

    IEnumerator ResetCrawlmode(float timeToReset)
    {
        Debug.Log("ResetCrawlMode");
        float elapsed_time = 0f;
        while (elapsed_time < timeToReset)
        {
            elapsed_time += Time.deltaTime;
            characterController.center = Vector3.Lerp(CrawlCollCenterRef.position - this.transform.position, origCollCenter, elapsed_time / timeToReset);
            characterController.height = Mathf.Lerp(crawlHeight, origCollHeight, elapsed_time / timeToReset);
            yield return null;
        }
        characterController.height = origCollHeight;
        characterController.center = origCollCenter;
    }

    public Vector3 GetLocalMoveDir()
    {
        return motion;
    }

    public Vector3 GetMotionVector()
    {
        return Quaternion.Euler(-camRot) * motion;
    }

    public Vector3 GetBodyForward()
    {
        return this.transform.forward;
    }
}
