using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpHeight = 5.0f;
    private float gravityValue = -20f;
    private Vector3 motion;

    private CharacterController characterController;

    private void Start()
    {
        characterController = this.GetComponent<CharacterController>();    
    }
    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 camRot = mainCamera.transform.eulerAngles;
        camRot.x = 0;
        camRot.z = 0;
        //transform.rotation = Quaternion.Euler(camRot);

        motion = new Vector3(horizontal, 0, vertical) * (speed * Time.deltaTime);
        //motion = Quaternion.Euler(camRot) * motion;
        motion.y = gravityValue * Time.deltaTime;
        characterController.Move(motion);
    }
}
