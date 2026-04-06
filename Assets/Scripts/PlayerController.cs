using UnityEngine;

/// <summary>
/// CharacterControllerベースの怪獣移動。
/// カメラ向きを基準に移動し、重量感のある回転を行う。
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Joystick joystick;
    [SerializeField] private Transform cameraPivot;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3.2f;
    [SerializeField] private float rotationSmooth = 3.5f;
    [SerializeField] private float gravity = -18f;

    private CharacterController controller;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (joystick == null || cameraPivot == null)
        {
            return;
        }

        Vector2 input = joystick.Input;

        Vector3 camForward = cameraPivot.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cameraPivot.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 move = (camForward * input.y + camRight * input.x);
        move = Vector3.ClampMagnitude(move, 1f);

        if (move.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmooth * Time.deltaTime);
        }

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
        Vector3 velocity = move * moveSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }
}
