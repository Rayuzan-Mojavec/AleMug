using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class ShootLaser : MonoBehaviour
{
    public Transform firePoint;
    public Material material;
    public float duration = 3f;

    public float target_holdTime = 2f;
    public float enemy_holdTime = 0.5f;

    LaserBeam beam;
    bool laserOn;
    Coroutine timer;

    // ✅ Per-collider timers
    Dictionary<Collider, float> hitTimers = new Dictionary<Collider, float>();
    HashSet<Collider> triggered = new HashSet<Collider>();

    // =============================
    // BUTTON
    // =============================
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
        laserOn = false;

        hitTimers.Clear();
        triggered.Clear();

        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }

        beam?.Destroy();
        beam = null;
    }

    // =============================
    // UPDATE
    // =============================
    void Update()
    {
        if (!laserOn || beam == null) return;

        beam.Rebuild(firePoint.position, firePoint.right);

        HashSet<Collider> currentHits = new HashSet<Collider>();

        foreach (var c in beam.HitColliders)
        {
            if (!c.CompareTag("Target") && !c.CompareTag("Enemy"))
                continue;

            currentHits.Add(c);

            if (!hitTimers.ContainsKey(c))
                hitTimers[c] = 0f;

            hitTimers[c] += Time.deltaTime;

            float requiredTime =
                c.CompareTag("Target") ? target_holdTime : enemy_holdTime;

            if (!triggered.Contains(c) && hitTimers[c] >= requiredTime)
            {
                triggered.Add(c);

                if (c.CompareTag("Target"))
                    SceneManager.LoadScene(2);
                else
                    SceneManager.LoadScene(3);

                // 👉 DO SOMETHING HERE
            }
        }

        // Remove interrupted hits
        List<Collider> toRemove = new List<Collider>();

        foreach (var kv in hitTimers)
        {
            if (!currentHits.Contains(kv.Key))
                toRemove.Add(kv.Key);
        }

        foreach (var c in toRemove)
        {
            hitTimers.Remove(c);
            triggered.Remove(c);
        }
    }
}
