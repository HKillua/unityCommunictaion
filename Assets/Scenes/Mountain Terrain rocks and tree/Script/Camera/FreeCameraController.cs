using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    public float lookSpeed = 2.0f;
    public float moveSpeed = 10.0f;
    public float sprintSpeed = 20.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private bool isMouseWithinWindow = true;
    private bool isRightMouseButtonHeld = false;

    void Update()
    {
        // �������Ҽ�״̬
        if (Input.GetMouseButtonDown(1))
        {
            isRightMouseButtonHeld = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRightMouseButtonHeld = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // �������Ƿ�����Ϸ������
        if (Input.mousePosition.x >= 0 && Input.mousePosition.x <= Screen.width &&
            Input.mousePosition.y >= 0 && Input.mousePosition.y <= Screen.height)
        {
            isMouseWithinWindow = true;
        }
        else
        {
            isMouseWithinWindow = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (isMouseWithinWindow && isRightMouseButtonHeld)
        {
            // �������鿴
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -90f, 90f);

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }

        // ��������ƶ�
        float moveSpeedToUse = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        float moveForward = Input.GetAxis("Vertical") * moveSpeedToUse * Time.deltaTime;
        float moveRight = Input.GetAxis("Horizontal") * moveSpeedToUse * Time.deltaTime;
        float moveUp = 0f;

        if (Input.GetKey(KeyCode.E))
        {
            moveUp = moveSpeedToUse * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            moveUp = -moveSpeedToUse * Time.deltaTime;
        }

        transform.Translate(new Vector3(moveRight, moveUp, moveForward));
    }
}
