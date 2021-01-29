using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Throwing;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Thrower : MonoBehaviour
{

    public Transform throwOrigin; 
    public Transform Target;
    public float firingAngle = 45.0f;
    public float gravityScale = 1f;

    public Throwable Projectile;


    private Coroutine throwingCoroutine; 
 
    void Throw()
    {
        Projectile.Detach();
        if(throwingCoroutine != null)
            StopCoroutine(throwingCoroutine);
        throwingCoroutine = StartCoroutine(SimulateProjectileCor());
    }

    private void Unparent(Transform projectile)
    {
        projectile.parent = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Throw();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
        ;
    }

    private void Reset()
    {
        if(throwingCoroutine != null)
            StopCoroutine(throwingCoroutine);
        
        StartCoroutine(Projectile.Reattach());
    }


    IEnumerator SimulateProjectileCor()
    {
        // Move projectile to the position of throwing object + add some offset if needed.
        Projectile.transform.position = throwOrigin.position;
       
        // Calculate distance to target
        float target_Distance = Vector3.Distance(Projectile.transform.position, Target.position);
 
        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / Physics.gravity.magnitude);
 
        
        //calculateFiringAngleIfSet

        
        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
 
        // Calculate flight time.
        float flightDuration = target_Distance / Vx;
   
        // Rotate projectile to face the target.
        Projectile.transform.rotation = Quaternion.LookRotation(Target.position - Projectile.transform.position);
       
        float elapse_time = 0;
 
        while (elapse_time < flightDuration)
        {
            Projectile.transform.Translate(0, (Vy - (Physics.gravity.magnitude  * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
           
            elapse_time += Time.deltaTime;
 
            yield return null;
        }
    }
}
