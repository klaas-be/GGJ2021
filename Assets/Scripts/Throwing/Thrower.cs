using System.Collections;
using Selection;
using Throwing;
using UnityEngine;

public class Thrower : MonoBehaviour
{

    public Anchor throwOrigin; 
    public Selectable Target;
    public float firingAngle = 45.0f;
    public float gravityScale = 1f;
    public KeyCode ThrowKey; 

    public Throwable Projectile;


    private Coroutine throwingCoroutine; 
 
    void Throw()
    {
        SetTarget();
        if (Target == null) return; 
        Projectile.Detach();
        Projectile.MoveIkTargetToTarget(Target.transform.position);
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
        if (Input.GetKeyDown(ThrowKey))
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


    private void SetTarget()
    {
        if (!(SelectionManager.Selected is null))
            Target = SelectionManager.Selected;
        else
            Target = null; 
    }

    
    IEnumerator SimulateProjectileCor()
    {
        // Move projectile to the position of throwing object + add some offset if needed.
        Projectile.transform.position = throwOrigin.position;
       
        // Calculate distance to target
        float target_Distance = Vector3.Distance(throwOrigin.position, Target.transform.position);
 
        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / ((Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / (Physics.gravity.magnitude * gravityScale)));

        projectile_Velocity *= 2; 
        //calculateFiringAngleIfSet

        
        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = (target_Distance / Vx) ;
   
        // Rotate projectile to face the target.
       // Projectile.transform.rotation = Quaternion.LookRotation(Projectile.transform.position- throwOrigin.position);


        float elapse_time = 0;
 
        while (elapse_time < flightDuration)
        {
            Projectile.transform.Translate(0, (Vy - (Physics.gravity.magnitude * gravityScale  * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
           Projectile.transform.LookAt(Target.transform);
            elapse_time += Time.deltaTime;
 
            yield return null;
        }
    }
}
