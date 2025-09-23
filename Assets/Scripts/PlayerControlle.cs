using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Скорость движения
    public float Speed = 5f;
    public float JumpForce = 300f;

    private bool isGrounded = false;
    private bool canJump = true;

    void Update()
    {
        // Движение влево-вправо
        float horizontal = Input.GetAxis("Horizontal");
        transform.Translate(horizontal * Speed * Time.deltaTime, 0, 0);

        // Прыжок только если на земле и можно прыгать
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * JumpForce);
            canJump = false; // Запрещаем следующий прыжок
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Проверяем, что столкнулись с землей
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canJump = true; // Разрешаем прыжок снова
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Когда отрываемся от земли
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}