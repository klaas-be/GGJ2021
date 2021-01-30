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
    private Vector3 motion;
    private Vector3 camRot;

    private CharacterController characterController;

    private void Start()
    {
        mainCamera = Camera.main;
        
        characterController = this.GetComponent<CharacterController>();    
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
        characterController.Move(motion);
    }

    public Vector3 GetLocalMoveDir()
    {
        return motion;
    }

    public Vector3 GetMotionVector()
    {
        return Quaternion.Euler(-camRot) * motion;
    }
}
