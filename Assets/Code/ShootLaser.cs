using UnityEngine;
using System.Collections;

public class ShootLaser : MonoBehaviour
{
    public Transform firePoint;      // green dot
    public Material material;
    public float duration = 3f;      // seconds

    LaserBeam beam;
    bool laserOn = false;
    Coroutine timer;

    // Called by Button
    public void FireLaser()
    {
        // Restart laser if already on
        if (laserOn)
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
        laserOn = false;

        if (timer != null)
            StopCoroutine(timer);

        if (beam != null)
        {
            beam.Destroy();
            beam = null;
        }
    }

    void Update()
    {
        if (!laserOn || beam == null) return;

        // 🔥 real-time optics update
        beam.Rebuild(
            firePoint.position,
            firePoint.right
        );
    }
}
