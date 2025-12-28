using UnityEngine;
using UnityEngine.InputSystem;

public class ShootLaser : MonoBehaviour
{
    public Material material;

    LaserBeam beam;
    GameObject beamObject;

    void Update()
    {
        // HOLD SPACE = laser ON
        if (Keyboard.current.spaceKey.isPressed)
        {
            if (beam == null)
            {
                beam = new LaserBeam(
                    transform.position,
                    transform.right,
                    material
                );
            }
            else
            {
                // 🔄 update direction in real-time
                beam.Rebuild(
                    transform.position,
                    transform.right
                );
            }
        }
        else
        {
            // release = laser OFF
            if (beam != null)
            {
                beam.Destroy();
                beam = null;
            }
        }
    }
}
