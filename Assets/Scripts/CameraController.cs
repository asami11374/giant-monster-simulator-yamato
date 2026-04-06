using UnityEngine;

/// <summary>
/// プレイヤー後方追従 + 右側ドラッグでY軸回転。
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;

    [Header("Follow")]
    [SerializeField] private float distance = 16f;
    [SerializeField] private float height = 7f;
    [SerializeField] private float followSmooth = 7f;

    [Header("Look")]
    [SerializeField] private float pitch = 20f;
    [SerializeField] private float yaw = 0f;
    [SerializeField] private float yawSpeed = 0.15f;

    private int rightSideFingerId = -1;
    private float halfWidth;

    private void Start()
    {
        halfWidth = Screen.width * 0.5f;
        if (target != null)
        {
            yaw = target.eulerAngles.y;
        }
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        HandleRightSideDrag();

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPos = target.position - (rotation * Vector3.forward * distance) + Vector3.up * height;

        transform.position = Vector3.Lerp(transform.position, desiredPos, followSmooth * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 2f);
    }

    private void HandleRightSideDrag()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButton(0) && Input.mousePosition.x > halfWidth)
        {
            yaw += Input.GetAxis("Mouse X") * 220f * yawSpeed * Time.deltaTime;
        }
#else
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began && touch.position.x > halfWidth && rightSideFingerId < 0)
            {
                rightSideFingerId = touch.fingerId;
            }

            if (touch.fingerId == rightSideFingerId)
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    yaw += touch.deltaPosition.x * yawSpeed;
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    rightSideFingerId = -1;
                }
            }
        }
#endif
    }
}
