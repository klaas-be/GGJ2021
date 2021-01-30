using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyHeightPosition : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] float wobbleDist = 0.5f;
    IKTargetController IKTargetController;

    private Vector3 origCenter;

    private void Start()
    {
        origCenter = characterController.center;
        IKTargetController = GameObject.FindGameObjectWithTag("IKTargets").GetComponent<IKTargetController>();
    }

    // Update is called once per frame
    void Update()
    {
        float distA = Vector3.Distance(IKTargetController.GetIKTarget(IKTargetController.IKTarget.LFoot).position,
                                        IKTargetController.GetIKTarget(IKTargetController.IKTarget.FeetRef).position) - IKTargetController.GetFeetOffset();
        float distB = Vector3.Distance(IKTargetController.GetIKTarget(IKTargetController.IKTarget.RFoot).position,
                                        IKTargetController.GetIKTarget(IKTargetController.IKTarget.FeetRef).position) - IKTargetController.GetFeetOffset();

        float closest = (distA > distB) ? distB : distA;

        characterController.center = Vector3.Lerp(origCenter, origCenter - Vector3.down * wobbleDist, closest*0.75f);
    }
}
