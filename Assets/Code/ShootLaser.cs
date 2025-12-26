using UnityEngine;

public class ShootLaser : MonoBehaviour
{
    public Material material;
    LaserBeam beam;

    void Update()
    {
        Destroy(GameObject.Find("Plane Wave"));
        beam = new LaserBeam(gameObject.transform.position, gameObject.transform.right, material);
    }
}
