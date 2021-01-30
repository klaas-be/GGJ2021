using System.Collections;
using System.Collections.Generic;
using Throwing;
using UnityEngine;

public class IKTargetController : MonoBehaviour
{
    [Header("Throwables")]
    [SerializeField] Throwable leftHandThrowable;
    [SerializeField] Throwable rightHandThrowable;
    [SerializeField] Throwable leftFootThrowable;
    [SerializeField] Throwable rightFootThrowable;
    [Header("Targets")]
    [SerializeField] Transform leftHandTarget;
    [SerializeField] Transform rightHandTarget;
    [SerializeField] Transform leftFootTarget;
    [SerializeField] Transform rightFootTarget;
    private bool isLeftHandMoving = false, isRightHandMoving = false, isLeftFootMoving = false, isRightFootMoving = false;

    [Header("Refs")]
    [SerializeField] PlayerMovement movementScriptRef;
    [SerializeField] Transform HandsRef;
    [SerializeField] Transform FeetRef;

    [Header("Settings")]
    public float stepTriggerSize = 1.2f;
    public float stepSize = 1f;
    public float feetOffsetLeftRight = 0.3f;
    public float timeForAStep = 0.5f;

    private Vector3 moveDir  = Vector3.zero;
    private Vector3 lastFrameMovDir = Vector3.zero;
    private Vector3 lastmoveDir = Vector3.zero;
    private bool isLeftFootTurn = true;

    [Header("Debug")]
    public Vector3 testnewPosLeft = Vector3.zero;
    public Vector3 testnewPosRight = Vector3.zero;
    public Vector3 testLeftDotDir = Vector3.zero;
    public Vector3 testRightDotDir = Vector3.zero;

    private void Awake()
    {
        movementScriptRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        moveDir = movementScriptRef.GetLocalMoveDir();
        moveDir.y = 0;
        moveDir = moveDir.normalized;
        if (moveDir != Vector3.zero)
        {
            lastmoveDir = moveDir;
        }

        testLeftDotDir = (leftFootTarget.position - FeetRef.position).normalized;
        testRightDotDir = (rightFootTarget.position - FeetRef.position).normalized;

        //Laufen
        if (moveDir != Vector3.zero)
        {
            //Wenn beide Fusse noch da sind
            if (leftFootThrowable.IsAttached
            && rightFootThrowable.IsAttached)
            {
                //linken Fuss bewegen
                if (isLeftFootTurn)
                {
                    if (Vector3.Distance(FeetRef.position, rightFootTarget.position) > stepTriggerSize
                     && Vector3.Dot(lastmoveDir, rightFootTarget.position - FeetRef.position) < 0
                     && Vector3.Dot(lastmoveDir, leftFootTarget.position - FeetRef.position) < 0)
                    {
                        //linken Fuss setzen
                        Vector3 newPos = FeetRef.position + lastmoveDir * stepSize - FeetRef.right * feetOffsetLeftRight;
                        testnewPosLeft = newPos;
                        if (!isLeftFootMoving)
                            StartCoroutine(SetLeftFootTarget(newPos));
                    }
                }
                //rechten Fuss bewegen
                else if(!isLeftFootTurn)
                {
                    if (Vector3.Distance(FeetRef.position, leftFootTarget.position) > stepTriggerSize
                     && Vector3.Dot(lastmoveDir, rightFootTarget.position - FeetRef.position) < 0
                     && Vector3.Dot(lastmoveDir, leftFootTarget.position - FeetRef.position) < 0)
                    {
                        //rechten Fuss setzen
                        Vector3 newPos = FeetRef.position + lastmoveDir * stepSize + FeetRef.right * feetOffsetLeftRight;
                        testnewPosRight = newPos;
                        if (!isRightFootMoving)
                            StartCoroutine(SetRightFootTarget(newPos));
                    }
                }
            }
            //Linker Fuss fehlt
            else if (!leftFootThrowable.IsAttached && rightFootThrowable.IsAttached)
            {
                //rechten Fuss bewegen
                if (Vector3.Distance(FeetRef.position, rightFootTarget.position) > stepSize*0.75f
                     && Vector3.Dot(lastmoveDir, rightFootTarget.position - FeetRef.position) < 0)
                {
                    //rechten Fuss setzen
                    Vector3 newPos = FeetRef.position + lastmoveDir * stepSize;
                    testnewPosLeft = newPos;
                    if(!isRightFootMoving)
                        StartCoroutine(SetRightFootTarget(newPos));
                }
            }
            //Rechter Fuss fehlt
            else if (leftFootThrowable.IsAttached && !rightFootThrowable.IsAttached)
            {
                //linken Fuss bewegen
                if (Vector3.Distance(FeetRef.position, leftFootTarget.position) > stepSize*0.75f
                     && Vector3.Dot(lastmoveDir, leftFootTarget.position - FeetRef.position) < 0)
                {
                    //linken Fuss setzen
                    Vector3 newPos = FeetRef.position + lastmoveDir * stepSize;
                    testnewPosLeft = newPos;
                    if(!isLeftFootMoving)
                        StartCoroutine(SetLeftFootTarget(newPos));
                }
            }
        }

        //Stehenbleiben
        if (lastFrameMovDir != Vector3.zero && moveDir == Vector3.zero)
        {
            StopAllCoroutines();
            isLeftFootMoving = false; isRightFootMoving = false;

            //Wenn beide Fusse noch da sind
            if (leftFootThrowable.IsAttached
            && rightFootThrowable.IsAttached)
            {
                isLeftFootTurn = true;
                StartCoroutine(SetLeftFootTarget(FeetRef.position - FeetRef.right * feetOffsetLeftRight));
                StartCoroutine(SetRightFootTarget(FeetRef.position + FeetRef.right * feetOffsetLeftRight));
            }
            //Linker Fuss fehlt
            else if (!leftFootThrowable.IsAttached && rightFootThrowable.IsAttached)
            {
                StartCoroutine(SetRightFootTarget(FeetRef.position));
            }
            //Rechter Fuss fehlt
            else if (leftFootThrowable.IsAttached && !rightFootThrowable.IsAttached)
            {
                StartCoroutine(SetLeftFootTarget(FeetRef.position));
            }
        }

        lastFrameMovDir = moveDir;
    }

    private void LeanCharBodyToMovement()
    {

    }

    IEnumerator SetLeftFootTarget(Vector3 newPos, float timeToStep = -1f)
    {
        if(timeToStep == -1)
        {
            timeToStep = timeForAStep;
        }

        float timeOutMove = 0f;
        while (isRightFootMoving && timeOutMove < 5f)
        {
            yield return null;
            timeOutMove += Time.deltaTime;
        }

        isLeftFootMoving = true;

        Vector3 origPos = leftFootTarget.position;
        float elapse_time = 0;
        while (elapse_time < timeToStep)
        {
            elapse_time += Time.deltaTime;
            leftFootTarget.position = Vector3.Lerp(origPos, newPos, elapse_time / timeToStep);
            yield return null;
        }

        leftFootTarget.position = newPos;

        isLeftFootMoving = false;
        isLeftFootTurn = false;
    }

    IEnumerator SetRightFootTarget(Vector3 newPos, float timeToStep = -1f)
    {
        if (timeToStep == -1)
        {
            timeToStep = timeForAStep;
        }

        float timeOutMove = 0f;
        while (isLeftFootMoving && timeOutMove < 5f)
        {
            yield return null;
            timeOutMove += Time.deltaTime;
        }

        isRightFootMoving = true;

        Vector3 origPos = rightFootTarget.position;
        float elapse_time = 0;
        while (elapse_time < timeToStep)
        {
            elapse_time += Time.deltaTime;
            rightFootTarget.position = Vector3.Lerp(origPos, newPos, elapse_time / timeToStep);
            yield return null;
        }

        rightFootTarget.position = newPos;

        isRightFootMoving = false;
        isLeftFootTurn = true;
    }

    public Transform GetIKTarget(IKTarget targetName)
    {
        switch (targetName)
        {
            case IKTarget.LHand:
                return leftHandTarget;
            case IKTarget.RHand:
                return rightHandTarget;
            case IKTarget.LFoot:
                return leftFootTarget;
            case IKTarget.RFoot:
                return rightFootTarget;
            default:
                return null;
        }
    }

    public enum IKTarget
    {
        LHand,
        RHand,
        LFoot,
        RFoot
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(FeetRef.position, FeetRef.position + moveDir);
        Gizmos.DrawLine(FeetRef.position, FeetRef.position + testLeftDotDir);
        Gizmos.DrawLine(FeetRef.position, FeetRef.position + testRightDotDir);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(FeetRef.position, FeetRef.position + lastmoveDir);
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(testnewPosLeft, 0.3f);
        Gizmos.DrawSphere(testnewPosRight, 0.3f);
    }
}
