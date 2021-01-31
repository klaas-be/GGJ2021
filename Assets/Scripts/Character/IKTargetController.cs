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
    private bool isLeftFootMoving = false, isRightFootMoving = false;

    [Header("Refs")]
    [SerializeField] PlayerMovement movementScriptRef;
    [SerializeField] Transform HandsRef;
    [SerializeField] Transform FeetRef;

    [Header("Settings")]
    public float angleAfterTurningStandingStillUpdateFeetAndArms = 60f;
    public float stepTriggerSize = 1.2f;
    public float stepLength = 1f;
    public float handMoveAmount = 1.5f;
    public float feetOffsetLeftRight = 0.3f;
    public float handOffsetLeftRight = 0.5f;
    public float timeForAStep = 0.5f;

    private Vector3 moveDir = Vector3.zero;
    private Vector3 lastFrameMovDir = Vector3.zero;
    private Vector3 lastmoveDir = Vector3.zero;
    private Vector3 lastFeetRotationWhileStanding = Vector3.forward;
    private bool isLeftFootTurn = true;
    private bool isCrawling = false;


    [Header("Debug")]
    public Vector3 testnewPosLeft = Vector3.zero;
    public Vector3 testnewPosRight = Vector3.zero;
    public Vector3 testLeftDotDir = Vector3.zero;
    public Vector3 testRightDotDir = Vector3.zero;

    private void Start()
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
                        Vector3 newPos = FeetRef.position + lastmoveDir * stepLength - FeetRef.right * feetOffsetLeftRight;
                        if (!isLeftFootMoving)
                            StartCoroutine(SetLeftFootTarget(newPos));

                        //Rechte Hand Setzen
                        newPos = HandsRef.position + lastmoveDir * handMoveAmount + HandsRef.right * handOffsetLeftRight;
                        if (rightHandThrowable.IsAttached)
                        {
                            StartCoroutine(SetRightHandTarget(newPos));
                        }
                    }
                }
                //rechten Fuss und linke Hand bewegen
                else if (!isLeftFootTurn)
                {
                    if (Vector3.Distance(FeetRef.position, leftFootTarget.position) > stepTriggerSize
                     && Vector3.Dot(lastmoveDir, rightFootTarget.position - FeetRef.position) < 0
                     && Vector3.Dot(lastmoveDir, leftFootTarget.position - FeetRef.position) < 0)
                    {
                        //rechten Fuss setzen
                        Vector3 newPos = FeetRef.position + lastmoveDir * stepLength + FeetRef.right * feetOffsetLeftRight;
                        if (!isRightFootMoving)
                            StartCoroutine(SetRightFootTarget(newPos));

                        //Linke Hand Setzen
                        newPos = HandsRef.position + lastmoveDir * handMoveAmount - HandsRef.right * handOffsetLeftRight;
                        if (leftHandThrowable.IsAttached)
                        {
                            StartCoroutine(SetLeftHandTarget(newPos));
                        }
                    }
                }
            }
            //Linker Fuss fehlt
            else if (!leftFootThrowable.IsAttached && rightFootThrowable.IsAttached)
            {
                //rechten Fuss bewegen
                if (Vector3.Distance(FeetRef.position, rightFootTarget.position) > stepLength * 0.75f
                     && Vector3.Dot(lastmoveDir, rightFootTarget.position - FeetRef.position) < 0)
                {
                    //rechten Fuss setzen
                    Vector3 newPos = FeetRef.position + lastmoveDir * stepLength;
                    if (!isRightFootMoving)
                        StartCoroutine(SetRightFootTarget(newPos));

                    //Linke Hand Setzen
                    newPos = HandsRef.position + lastmoveDir * handMoveAmount - HandsRef.right * handOffsetLeftRight;
                    if (leftHandThrowable.IsAttached)
                    {
                        StartCoroutine(SetLeftHandTarget(newPos));
                    }
                    //Rechte Hand Setzen
                    newPos = HandsRef.position + lastmoveDir * handMoveAmount + HandsRef.right * handOffsetLeftRight;
                    if (rightHandThrowable.IsAttached)
                    {
                        StartCoroutine(SetRightHandTarget(newPos));
                    }
                }
            }
            //Rechter Fuss fehlt
            else if (leftFootThrowable.IsAttached && !rightFootThrowable.IsAttached)
            {
                //linken Fuss bewegen
                if (Vector3.Distance(FeetRef.position, leftFootTarget.position) > stepLength * 0.75f
                     && Vector3.Dot(lastmoveDir, leftFootTarget.position - FeetRef.position) < 0)
                {
                    //linken Fuss setzen
                    Vector3 newPos = FeetRef.position + lastmoveDir * stepLength;
                    if (!isLeftFootMoving)
                        StartCoroutine(SetLeftFootTarget(newPos));

                    //Linke Hand Setzen
                    newPos = HandsRef.position + lastmoveDir * handMoveAmount - HandsRef.right * handOffsetLeftRight;
                    if (leftHandThrowable.IsAttached)
                    {
                        StartCoroutine(SetLeftHandTarget(newPos));
                    }
                    //Rechte Hand Setzen
                    newPos = HandsRef.position + lastmoveDir * handMoveAmount + HandsRef.right * handOffsetLeftRight;
                    if (rightHandThrowable.IsAttached)
                    {
                        StartCoroutine(SetRightHandTarget(newPos));
                    }
                }
            }
            //Wenn beide Füsse weg sind
            else
            {
                RaycastHit hit;
                //Linke Hand Setzen 
                if (Vector3.Distance(HandsRef.position, leftHandTarget.position) > stepTriggerSize
                     && Vector3.Dot(lastmoveDir, leftHandTarget.position - HandsRef.position) < 0)
                {
                    Vector3 newPos = HandsRef.position + lastmoveDir * handMoveAmount - HandsRef.right * handOffsetLeftRight;
                    if (Physics.Raycast(newPos + Vector3.up * 1f, Vector3.down, out hit, 2f))
                    {
                        newPos = hit.point;
                    }
                    if (leftHandThrowable.IsAttached)
                    {
                        StartCoroutine(SetLeftHandTarget(newPos));
                    }
                }

                //Rechte Hand Setzen
                if (Vector3.Distance(HandsRef.position, rightHandTarget.position) > stepTriggerSize
                     && Vector3.Dot(lastmoveDir, rightHandTarget.position - HandsRef.position) < 0)
                {
                    Vector3 newPos = HandsRef.position + lastmoveDir * handMoveAmount + HandsRef.right * handOffsetLeftRight;
                    if (Physics.Raycast(newPos + Vector3.up * 1f, Vector3.down, out hit, 2f))
                    {
                        newPos = hit.point;
                    }
                    if (rightHandThrowable.IsAttached)
                    {
                        StartCoroutine(SetRightHandTarget(newPos));
                    }
                }

            }
        }

        //Stehenbleiben
        if (lastFrameMovDir != Vector3.zero && moveDir == Vector3.zero)
        {
            StandStillSetIKs();
        }
        else
        {
            CheckIfFeetAndHandsShouldRotate();
        }

        lastFrameMovDir = moveDir;
    }

    public void StandStillSetIKs(bool ignoreAttached = false)
    {
        StopAllCoroutines();
        isLeftFootMoving = false; isRightFootMoving = false;

        //Linke Hand Setzen
        if (leftHandThrowable.IsAttached || ignoreAttached)
        {
            StartCoroutine(SetLeftHandTarget(HandsRef.position - HandsRef.right * handOffsetLeftRight));
        }
        //Rechte Hand Setzen
        if (rightHandThrowable.IsAttached || ignoreAttached)
        {
            StartCoroutine(SetRightHandTarget(HandsRef.position + HandsRef.right * handOffsetLeftRight));
        }

        //Wenn beide Fusse noch da sind
        if ((leftFootThrowable.IsAttached
        && rightFootThrowable.IsAttached) || ignoreAttached)
        {
            isLeftFootTurn = true;
            StartCoroutine(SetLeftFootTarget(FeetRef.position - FeetRef.right * feetOffsetLeftRight));
            StartCoroutine(SetRightFootTarget(FeetRef.position + FeetRef.right * feetOffsetLeftRight));
        }
        //Linker Fuss fehlt
        else if ((!leftFootThrowable.IsAttached && rightFootThrowable.IsAttached) || ignoreAttached)
        {
            StartCoroutine(SetRightFootTarget(FeetRef.position));
        }
        //Rechter Fuss fehlt
        else if ((leftFootThrowable.IsAttached && !rightFootThrowable.IsAttached) || ignoreAttached)
        {
            StartCoroutine(SetLeftFootTarget(FeetRef.position));
        }
    }

    IEnumerator SetLeftHandTarget(Vector3 newPos, float timeToStep = -1f)
    {
        if (timeToStep == -1)
        {
            timeToStep = timeForAStep * 2;
        }

        Vector3 origPos = leftHandTarget.position;
        float elapse_time = 0;
        while (elapse_time < timeToStep)
        {
            elapse_time += Time.deltaTime;
            leftHandTarget.position = Vector3.Lerp(origPos, newPos, elapse_time / timeToStep);
            yield return null;
        }

        leftHandTarget.position = newPos;
        leftHandTarget.LookAt(leftHandTarget.position + newPos);
    }

    IEnumerator SetRightHandTarget(Vector3 newPos, float timeToStep = -1f)
    {
        if (timeToStep == -1)
        {
            timeToStep = timeForAStep * 2;
        }

        Vector3 origPos = rightHandTarget.position;
        float elapse_time = 0;
        while (elapse_time < timeToStep)
        {
            elapse_time += Time.deltaTime;
            rightHandTarget.position = Vector3.Lerp(origPos, newPos, elapse_time / timeToStep);
            yield return null;
        }

        rightHandTarget.position = newPos;
        rightHandTarget.LookAt(rightHandTarget.position + newPos);
    }
    IEnumerator SetLeftFootTarget(Vector3 newPos, float timeToStep = -1f)
    {
        RaycastHit hit;
        if (Physics.Raycast(newPos + Vector3.up * 2f, Vector3.down, out hit, 3f))
        {
            newPos = hit.point;
        }

        if (timeToStep == -1)
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
        leftFootTarget.rotation = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.forward, movementScriptRef.GetBodyForward(), Vector3.up), 0);

        isLeftFootMoving = false;
        isLeftFootTurn = false;
    }

    IEnumerator SetRightFootTarget(Vector3 newPos, float timeToStep = -1f)
    {
        RaycastHit hit;
        if (Physics.Raycast(newPos + Vector3.up * 2f, Vector3.down, out hit, 3f))
        {
            newPos = hit.point;
        }

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
        rightFootTarget.rotation = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.forward, movementScriptRef.GetBodyForward(), Vector3.up), 0);

        isRightFootMoving = false;
        isLeftFootTurn = true;
    }

    //Wird in UnityEvents benutzt
    public void CheckIfLegsAttached()
    {
        bool check = !leftFootThrowable.IsAttached && !rightFootThrowable.IsAttached;
        bool noLimbsCheck = !leftHandThrowable.IsAttached && !rightHandThrowable.IsAttached && !leftFootThrowable.IsAttached && !rightFootThrowable.IsAttached;

        if (check != isCrawling)
        {
            Debug.Log("crawlmode change");
            isCrawling = check;
            movementScriptRef.SetCrawlmode(isCrawling);
        }

        movementScriptRef.SetNoLimbsMode(noLimbsCheck);
    }

    public void CheckIfFeetAndHandsShouldRotate()
    {
        if (lastFeetRotationWhileStanding.y - movementScriptRef.GetCurrentCamRot().y > angleAfterTurningStandingStillUpdateFeetAndArms
            || lastFeetRotationWhileStanding.y - movementScriptRef.GetCurrentCamRot().y < -angleAfterTurningStandingStillUpdateFeetAndArms)
        {
            lastFeetRotationWhileStanding = movementScriptRef.GetCurrentCamRot();
            StandStillSetIKs();
        }
    }

    public float GetFeetOffset()
    {
        return feetOffsetLeftRight;
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
            case IKTarget.FeetRef:
                return FeetRef;
            default:
                return null;
        }
    }

    public enum IKTarget
    {
        LHand,
        RHand,
        LFoot,
        RFoot,
        FeetRef
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
        Gizmos.DrawSphere(testnewPosLeft, 0.1f);
        Gizmos.DrawSphere(testnewPosRight, 0.1f);
    }
}
