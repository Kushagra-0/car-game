using UnityEngine;

public class AIWheelContoller : MonoBehaviour
{
    [SerializeField] private Transform frontLeftWheel;
    [SerializeField] private Transform frontRightWheel;
    [SerializeField] private float maxSteerAngle = 30f;

    public void UpdateSteering(float steerInput)
    {
        float steerAngle = steerInput * maxSteerAngle;

        if (frontLeftWheel != null)
            frontLeftWheel.localEulerAngles = new Vector3(
                frontLeftWheel.localEulerAngles.x, steerAngle, frontLeftWheel.localEulerAngles.z);

        if (frontRightWheel != null)
            frontRightWheel.localEulerAngles = new Vector3(
                frontRightWheel.localEulerAngles.x, steerAngle, frontRightWheel.localEulerAngles.z);
    }
}
