using UnityEngine;
using System.Collections;

public class ShootLaser : MonoBehaviour
{
    public Transform firePoint;
    public Material material;
    public float duration = 3f;

    Collider trackedCollider;
    float hitTimer = 0f;
    public float holdTime = 0.5f;
    bool triggered = false;

    LaserBeam beam;
    bool laserOn;
    Coroutine timer;

    public void FireLaser()
    {
        StopLaser();

        laserOn = true;

        beam = new LaserBeam(
            firePoint.position,
            firePoint.right,
            material
        );

        timer = StartCoroutine(LaserTimer());
    }

    IEnumerator LaserTimer()
    {
        yield return new WaitForSeconds(duration);
        StopLaser();
    }

    void StopLaser()
    {
        trackedCollider = null;
        hitTimer = 0f;
        triggered = false;
        laserOn = false;

        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }

        beam?.Destroy();
        beam = null;
    }

    void Update()
{
        if (!laserOn || beam == null) return;

        beam.Rebuild(
            firePoint.position,
            firePoint.right
        );

        Collider hit = beam.CurrentHit;

        if (hit != null && hit.CompareTag("Target"))
        {
            if (hit == trackedCollider)
            {
                hitTimer += Time.deltaTime;

                if (!triggered && hitTimer >= holdTime)
                {
                    triggered = true;
                    Debug.Log("Target held for 2 seconds!");
                    // 👉 DO SOMETHING HERE
                }
            }
            else
            {   
            // New collider hit
                trackedCollider = hit;
                hitTimer = 0f;
                triggered = false;
            }
        }
        else
        {
        // Interrupted
            trackedCollider = null;
            hitTimer = 0f;
            triggered = false;
        }
    }
}
