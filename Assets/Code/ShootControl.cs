using UnityEngine;
using TMPro;

public class ShootControl : MonoBehaviour
{
    public TMP_InputField inputField;
    public Transform target;
    public Vector3 rotationAxis = Vector3.up;

    public float minAngle = -45f;
    public float maxAngle = 45f;

    void Start()
    {
        inputField.onValueChanged.AddListener(OnValueChanged);
    }

    void OnValueChanged(string value)
    {
        if (!float.TryParse(value, out float angle))
            return;

        // 🔒 CLAMP HERE
        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        // 🔄 APPLY ROTATION
        target.localRotation = Quaternion.AngleAxis(angle, rotationAxis);
    }
}
