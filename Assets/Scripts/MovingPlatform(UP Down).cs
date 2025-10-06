using UnityEngine;

public class MovingPlatform_Up_Down : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 3f;    // ��������� ��������
    public float moveSpeed = 2f;       // �������� ��������
    public float startDelay = 0f;      // �������� ����� ������� ��������

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
        // ��������� ��������
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
            return;
        }

        // �������� ���������
        if (movingUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // �������� ���������� ������� �����
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);

            // �������� ���������� ������ �����
            if (Vector3.Distance(transform.position, startPosition) < 0.01f)
            {
                movingUp = true;
            }
        }
    }
}