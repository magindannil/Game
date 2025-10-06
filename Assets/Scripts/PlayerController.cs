using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 5f;

    [Header("Ground Detection")]
    public float groundCheckDistance = 1.1f;
    public LayerMask groundLayer = 1;

    private Rigidbody rb;
    private bool isGrounded;
    private float horizontalInput;
    private bool canJump = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // Получаем ввод только по горизонтали
        horizontalInput = Input.GetAxis("Horizontal");

        // Проверяем землю под ногами
        bool previouslyGrounded = isGrounded;
        CheckGrounded();

        // Сбрасываем прыжок только когда приземлились после падения
        if (isGrounded && !previouslyGrounded)
        {
            canJump = true;
        }

        // Обрабатываем прыжок - только если на земле И можно прыгать
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        // Применяем движение в FixedUpdate для плавности
        Move();

        // Гарантируем что игрок не вращается
        LockRotation();
    }

    private void Move()
    {
        // Движение только по оси X (влево-вправо)
        Vector3 velocity = new Vector3(horizontalInput * speed, rb.linearVelocity.y, 0f);
        rb.linearVelocity = velocity;
    }

    private void Jump()
    {
        // Сбрасываем вертикальную скорость перед прыжком
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // Применяем силу прыжка
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

        // Запрещаем прыгать до следующего приземления
        canJump = false;

        Debug.Log("Прыжок! Осталось прыжков: 0");
    }

    private void CheckGrounded()
    {
        // Пускаем луч вниз для обнаружения земли
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer);
    }

    private void LockRotation()
    {
        // Жестко фиксируем вращение
        transform.rotation = Quaternion.identity;
    }

    // Метод для принудительного сброса прыжков (например, при респавне)
    public void ResetJumps()
    {
        canJump = true;
    }

    // Визуализация в редакторе
    private void OnDrawGizmosSelected()
    {
        // Показываем луч проверки земли
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);

        // Показываем статус прыжка
        Gizmos.color = canJump ? Color.blue : Color.yellow;
        Gizmos.DrawWireCube(transform.position + Vector3.up * 1.5f, Vector3.one * 0.3f);
    }
}