using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �������� ��������
    public float Speed = 5f;
    public float JumpForce = 300f;

    private bool isGrounded = false;
    private bool canJump = true;

    void Update()
    {
        // �������� �����-������
        float horizontal = Input.GetAxis("Horizontal");
        transform.Translate(horizontal * Speed * Time.deltaTime, 0, 0);

        // ������ ������ ���� �� ����� � ����� �������
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * JumpForce);
            canJump = false; // ��������� ��������� ������
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // ���������, ��� ����������� � ������
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canJump = true; // ��������� ������ �����
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // ����� ���������� �� �����
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}