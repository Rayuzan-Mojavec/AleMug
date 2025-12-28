using System.Collections.Generic;
using UnityEngine;

public class LaserBeam
{
    GameObject laserObj;
    LineRenderer laser;
    List<Vector3> laserIndices = new List<Vector3>();


    const float AIR_IOR = 1.0f; // ε0​μ0​
    const float MIRROR_IOR = 2.0f; // 4ε0​μ0​
    const float GLASS_IOR = 9.0f; // 81ε0​μ0​

    int maxBounces = 100;

    public LaserBeam(Vector3 pos, Vector3 dir, Material material)
    {
        laserObj = new GameObject("Plane Wave");
        laser = laserObj.AddComponent<LineRenderer>();

        laser.startWidth = 0.05f;
        laser.endWidth = 0.05f;
        laser.material = material;
        laser.positionCount = 0;

        CastRay(pos, dir.normalized, 0);
    }

    void CastRay(Vector3 origin, Vector3 direction, int depth)
    {
        if (depth > maxBounces) return;

        laserIndices.Add(origin);

        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 30f))
        {
            HandleHit(hit, direction, depth);
        }
        else
        {
            laserIndices.Add(ray.GetPoint(30f));
            UpdateLaser();
        }
    }

    void HandleHit(RaycastHit hit, Vector3 direction, int depth)
    {
        Vector3 hitPoint = hit.point + direction * 0.001f;
        Vector3 normal = hit.normal;

        if (hit.collider.CompareTag("Mirror"))
        {
            Vector3 reflectDir = Vector3.Reflect(direction, normal).normalized;
            Vector3 refractDir = Refract(direction, normal, AIR_IOR / MIRROR_IOR);

            CastRay(hitPoint, reflectDir, depth + 1);
            CastRay(hitPoint, refractDir.normalized, depth + 1);

            return; // do NOT change currentIOR
        }
        else if (hit.collider.CompareTag("Glass"))
        {
            Vector3 reflectDir = Vector3.Reflect(direction, normal).normalized;
            Vector3 refractDir = Refract(direction, normal, AIR_IOR / GLASS_IOR);

            CastRay(hitPoint, reflectDir, depth + 1);
            CastRay(hitPoint, refractDir.normalized, depth + 1);

            return; // do NOT change currentIOR
        }
        else
        {
            laserIndices.Add(hit.point);
            UpdateLaser();
        }
    }

    Vector3 Refract(Vector3 I, Vector3 N, float eta)
    {
        float cosi = Mathf.Clamp(Vector3.Dot(I, N), -1f, 1f);
        Vector3 n = N;

        if (cosi < 0f)
            cosi = -cosi;
        else
            n = -N;

        float k = 1f - eta * eta * (1f - cosi * cosi);

        if (k <= 0f)
            return Vector3.zero;

        return eta * I + (eta * cosi - Mathf.Sqrt(k)) * n;
    }

    void UpdateLaser()
    {
        laser.positionCount = laserIndices.Count;
        for (int i = 0; i < laserIndices.Count; i++)
        {
            laser.SetPosition(i, laserIndices[i]);
        }
    }

    public void Rebuild(Vector3 origin, Vector3 direction)
    {
        laserIndices.Clear();
        laser.positionCount = 0;

        CastRay(origin, direction.normalized, 0);
    }

    public void Destroy()
    {
        if (laserObj != null)
            GameObject.Destroy(laserObj);
    }

}
