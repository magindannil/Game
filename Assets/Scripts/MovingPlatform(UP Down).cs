using UnityEngine;

public class MovingPlatform_Up_Down : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 3f;    // Дистанция движения
    public float moveSpeed = 2f;       // Скорость движения
    public float startDelay = 0f;      // Задержка перед началом движения

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingUp = true;
    private float delayTimer;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.up * moveDistance;
        delayTimer = startDelay;
    }

    void Update()
    {
        // Обработка задержки
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
            return;
        }

        // Движение платформы
        if (movingUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Проверка достижения верхней точки
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);

            // Проверка достижения нижней точки
            if (Vector3.Distance(transform.position, startPosition) < 0.01f)
            {
                movingUp = true;
            }
        }
    }
}