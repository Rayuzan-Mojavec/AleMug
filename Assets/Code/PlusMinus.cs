using UnityEngine;
using TMPro;

public class PlusMinus : MonoBehaviour
{
    public TMP_InputField inputField;
    public Transform target;
    public Vector3 rotationAxis = Vector3.up;

    public float step = 1f;
    public float minAngle = -45f;
    public float maxAngle = 45f;

    float currentAngle = 0f;

    void Start()
    {
        UpdateUI();
        ApplyRotation();
    }

    public void Increase()
    {
        currentAngle += step;
        ClampAndApply();
    }

    public void Decrease()
    {
        currentAngle -= step;
        ClampAndApply();
    }

    void ClampAndApply()
    {
        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
        UpdateUI();
        ApplyRotation();
    }

    void UpdateUI()
    {
        inputField.text = currentAngle.ToString("0");
    }

    void ApplyRotation()
    {
        target.localRotation = Quaternion.AngleAxis(currentAngle, rotationAxis);
    }
}
