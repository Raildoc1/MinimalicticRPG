using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class CharacterMover : MonoBehaviour
{

    public float speed = 5f;
    public float stopTime = 0.1f;
    public float rotationSpeed = 10f;
    public float maxAngleError = 0.2f;

    public float inputMagnitude => Mathf.Max( Mathf.Abs(Input.GetAxis("Vertical")), Mathf.Abs(Input.GetAxis("Horizontal")));
    public float rawInputMagnitude => Mathf.Max( Mathf.Abs(Input.GetAxisRaw("Vertical")), Mathf.Abs(Input.GetAxisRaw("Horizontal")));
    public bool isRunning => Input.GetKey(KeyCode.LeftShift);

    public Vector3 direction { get; private set; }

    private const string animatorSpeedParamName = "Speed";
    private int animatorSpeedParamID;
    private Animator animator;
    private new Transform camera;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        animator = GetComponent<Animator>();
        camera = Camera.main.transform;

        animatorSpeedParamID = Animator.StringToHash(animatorSpeedParamName);
    }

    private void Update()
    {
        var cameraForward = new Vector3(camera.forward.x, 0f, camera.forward.z).normalized;
        var cameraRight = new Vector3(camera.right.x, 0f, camera.right.z).normalized;

        direction = cameraForward * Input.GetAxisRaw("Vertical") + cameraRight * Input.GetAxisRaw("Horizontal");

        direction = direction.normalized;

        if (Mathf.Approximately(direction.magnitude, 0f))
        {
            direction = Vector3.zero;
        }
        else
        {
            var angle = Vector3.Angle(transform.forward, direction);
            if (angle > maxAngleError)
            {
                UpdateRotation();

            }
            else
            {
                transform.forward = direction;
            }
        }

        animator.SetFloat(animatorSpeedParamID, Mathf.Clamp01(rawInputMagnitude) * (isRunning ? 1.5f : 1f), stopTime, Time.deltaTime);

    }

    private void UpdateRotation()
    {

        if (!Mathf.Approximately(animator.deltaRotation.eulerAngles.magnitude, 0f))
        {
            return;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime * rotationSpeed);
    }

}
