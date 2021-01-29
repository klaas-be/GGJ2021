using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{

    public Transform throwOrigin; 
    public Transform Target;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;

    public Transform Projectile;
    
 
    void Throw()
    {          
        StartCoroutine(SimulateProjectileCor());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Throw();
        }
        ;
    }


    IEnumerator SimulateProjectileCor()
    {
        // Move projectile to the position of throwing object + add some offset if needed.
        Projectile.position = throwOrigin.position;
       
        // Calculate distance to target
        float target_Distance = Vector3.Distance(Projectile.position, Target.position);
 
        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
 
        
        //calculateFiringAngleIfSet

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
 
        // Calculate flight time.
        float flightDuration = target_Distance / Vx;
   
        // Rotate projectile to face the target.
        Projectile.rotation = Quaternion.LookRotation(Target.position - Projectile.position);
       
        float elapse_time = 0;
 
        while (elapse_time < flightDuration)
        {
            Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
           
            elapse_time += Time.deltaTime;
 
            yield return null;
        }
    }
}
