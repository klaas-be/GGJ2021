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

    private Vector3 lastFrameMovDir = Vector3.zero;
    private bool isCharMoving = false;


    [Header("Debug")]
    public float testDistance = 0;
    public float testDot = 0;
    public Vector3 testDir = Vector3.zero;

    //TODO in LateUpdate verschieben wegen Attached Status oder Script Reihenfolge ändern

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = movementScriptRef.GetLocalMoveDir();
        moveDir.y = 0;
        moveDir = moveDir.normalized;
        Quaternion moveRot = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.forward, moveDir, Vector3.up), 0);

        testDir = moveDir;

        //Aus dem Stand losgehen - Linke Fuss immer zuerst - Wenn noch nicht bewegt und moveDir ist != 0
        if (isCharMoving == false && moveDir != Vector3.zero)
        {
            testDistance = Vector3.Distance(FeetRef.position, leftFootTarget.position);
            testDot = Vector3.Dot(moveDir, (leftFootTarget.position - FeetRef.position).normalized);
            //Wenn die Ref einen halben Schritt in Richtung MoveDir bewegt ist
            if (Vector3.Distance(FeetRef.position, leftFootTarget.position) > stepTriggerSize / 2
             && Vector3.Dot(moveDir, (leftFootTarget.position - FeetRef.position).normalized) < 1)
            {
                //Linken Fuss zum losgehen setzen
                Vector3 newPos = moveDir * stepSize;
                newPos += FeetRef.position - FeetRef.right * feetOffsetLeftRight;
                Debug.Log("Linken Fuss zum ersten Mal setzen. newPos: " + newPos);
                StartCoroutine(SetLeftFootTarget(newPos));
                isCharMoving = true;
            }
        }

        //Stehenbleiben
        if (lastFrameMovDir != Vector3.zero && moveDir == Vector3.zero)
        {
            Debug.Log("Stehen bleiben.");
            StopAllCoroutines();
            isLeftFootMoving = false; isRightFootMoving = false; isCharMoving = false;
            StartCoroutine(SetLeftFootTarget(FeetRef.position - FeetRef.right * feetOffsetLeftRight));
            StartCoroutine(SetRightFootTarget(FeetRef.position + FeetRef.right * feetOffsetLeftRight));
        }

        /*/OLD CHECK
        if (isLeftFoot)
        {
            CheckLeftFoot(moveDir);
        }
        else
        {
            CheckRightFoot(moveDir);
        }*/

        lastFrameMovDir = moveDir;
    }

    //Linken Fuss setzen, wenn attached
    private void CheckLeftFoot(Vector3 moveDir)
    {
        //Linken Fuss setzen, wenn beide da sind
        if (leftFootThrowable.IsAttached && rightFootThrowable.IsAttached)
        {
            //Setzen wenn Distance max Step überschreitet und entgegengesetzte Richtung zum Movement
            if (Vector3.Distance(FeetRef.position, leftFootTarget.position) > stepTriggerSize
                && Vector3.Dot(moveDir, (leftFootTarget.position - FeetRef.position).normalized) < 1)
            {
                Vector3 newPos = moveDir * stepSize;
                //newPos += FeetRef.position + new Vector3(-FeetOffsetLeftRight, 0, 0);
                //StartCoroutine(SetFootTarget(leftFootTarget, newPos, false, timeForAStep));
            }
        }
        //Setzen wenn der rechte Fuss nicht da ist
        else if (!rightFootThrowable.IsAttached)
        {


        }
        else if (!leftFootThrowable.IsAttached)
        {
            CheckRightFoot(moveDir);
        }
    }

    private void CheckRightFoot(Vector3 moveDir)
    {
        //Rechten Fuss setzen, wenn beide da sind
        if (leftFootThrowable.IsAttached && rightFootThrowable.IsAttached)
        {
            //Setzen wenn Distance max Step überschreitet und entgegengesetzte Richtung zum Movement
            if (Vector3.Distance(FeetRef.position, rightFootTarget.position) > stepTriggerSize
                && Vector3.Dot(moveDir, (rightFootTarget.position - FeetRef.position).normalized) < 1)
            {
                Vector3 newPos = moveDir * stepSize;
                newPos.y = 0;
                //newPos += FeetRef.position + new Vector3(+FeetOffsetLeftRight, 0, 0);
                //StartCoroutine(SetFootTarget(rightFootTarget, newPos, true, timeForAStep));
            }
        }
        //Setzen wenn der rechte Fuss nicht da ist
        else if (!rightFootThrowable.IsAttached)
        {


        }
        else if (!leftFootThrowable.IsAttached)
        {
            CheckRightFoot(moveDir);
        }
    }

    IEnumerator SetLeftFootTarget(Vector3 newPos)
    {
        //TODO: Checken ob der andere Fuss sich noch bewegt - zur not nach 5sek bewegen
        float timeOutMove = 0f;
        while (isRightFootMoving && timeOutMove < 5f)
        {
            yield return null;
            timeOutMove += Time.deltaTime;
        }

        isLeftFootMoving = true;

        Vector3 origPos = leftFootTarget.position;
        float elapse_time = 0;
        while (elapse_time < timeForAStep)
        {
            elapse_time += Time.deltaTime;
            leftFootTarget.position = Vector3.Lerp(origPos, newPos, elapse_time / timeForAStep);
            yield return null;
        }

        leftFootTarget.position = newPos;

        isLeftFootMoving = false;
    }
    IEnumerator SetRightFootTarget(Vector3 newPos)
    {
        //TODO: Checken ob der andere Fuss sich noch bewegt - zur not nach 5sek bewegen
        float timeOutMove = 0f;
        while (isLeftFootMoving && timeOutMove < 5f)
        {
            yield return null;
            timeOutMove += Time.deltaTime;
        }

        isRightFootMoving = true;

        Vector3 origPos = rightFootTarget.position;
        float elapse_time = 0;
        while (elapse_time < timeForAStep)
        {
            elapse_time += Time.deltaTime;
            rightFootTarget.position = Vector3.Lerp(origPos, newPos, elapse_time / timeForAStep);
            yield return null;
        }

        rightFootTarget.position = newPos;

        isRightFootMoving = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(FeetRef.position, FeetRef.position + testDir);
    }
}
