using UnityEngine;

public class OscillatingDoor : MonoBehaviour
{
    private HingeJoint _hingeJoint;
    private JointMotor motor;
    private float timer;
    public float oscillationSpeed = 2.0f; // Speed of oscillation
    public float motorForce = 50.0f;      // Force applied by the motor
    public float maxAngle = 45.0f;        // Maximum angle for oscillation

    void Start()
    {
        _hingeJoint = GetComponent<HingeJoint>();
        motor = _hingeJoint.motor;
        motor.force = motorForce;
        _hingeJoint.useMotor = true;
    }

    void Update()
    {
        timer += Time.deltaTime * oscillationSpeed;
        motor.targetVelocity = Mathf.Sin(timer) * maxAngle;
        _hingeJoint.motor = motor;
    }
}
