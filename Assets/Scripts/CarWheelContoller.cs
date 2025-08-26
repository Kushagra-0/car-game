using UnityEngine;

public class CarWheelContoller : MonoBehaviour
{
    [SerializeField] private Transform frontLeftWheel;
    [SerializeField] private Transform frontRightWheel;
    [SerializeField] private Transform rearLeftWheel;
    [SerializeField] private Transform rearRightWheel;

    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private float steerSpeed = 5f;
    [SerializeField] private float wheelRadius = 0.3f;

    private CarInputHandler carInputHandler;
    private Rigidbody carRigidbody;
    private float currentSteerAngle = 0f;
    void Start()
    {
        carInputHandler = GetComponent<CarInputHandler>();
        carRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector2 moveInput = carInputHandler.GetMoveInput();

        AnimateSteering(moveInput.x);
    }

    private void AnimateSteering(float steerInput)
    {
        float targetSteerAngle = steerInput * maxSteerAngle;
        currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetSteerAngle, steerSpeed * Time.deltaTime);

        if (frontLeftWheel != null)
        {
            Vector3 leftRotation = frontLeftWheel.localEulerAngles;
            leftRotation.y = currentSteerAngle;
            frontLeftWheel.localEulerAngles = leftRotation;
        }

        if (frontRightWheel != null)
        {
            Vector3 rightRotation = frontRightWheel.localEulerAngles;
            rightRotation.y = currentSteerAngle;
            frontRightWheel.localEulerAngles = rightRotation;
        }
    }
}
