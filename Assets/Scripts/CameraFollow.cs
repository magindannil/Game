using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Offset")]
    public Vector3 offset = new Vector3(0, 3, -10);

    [Header("Settings")]
    public bool followInRealTime = true;
    public float minimalSmoothness = 0.1f;

    private Vector3 currentVelocity;
    private bool isFollowing = true;
    private PlayerLoader playerLoader;

    private void Start()
    {
        // �������� ������ �� PlayerLoader ��� ������������ ��������� ������
        playerLoader = FindObjectOfType<PlayerLoader>();

        // ��������� ������
        if (target == null)
        {
            FindPlayer();
        }

        // ��������� ������������� ������ �� ������ ��� ������
        SnapToPlayer();

        // ���� ����� ��� �� ������, �������� ������������� �����
        if (target == null)
        {
            InvokeRepeating("FindPlayerDelayed", 0.5f, 1f);
        }
    }

    private void LateUpdate()
    {
        if (!isFollowing || target == null)
        {
            // ���� ���� ��������, �������� ����� �����
            if (target == null && Time.frameCount % 30 == 0)
            {
                FindPlayer();
            }
            return;
        }

        if (followInRealTime)
        {
            if (minimalSmoothness <= 0.01f)
            {
                // ��������� ���������� ����������
                transform.position = target.position + offset;
            }
            else
            {
                // ����� ���������� ���������� � ����������� ����������
                Vector3 targetPosition = target.position + offset;
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    targetPosition,
                    ref currentVelocity,
                    minimalSmoothness,
                    Mathf.Infinity,
                    Time.deltaTime
                );
            }
        }

        // ������ ������ ������� �� ������
        transform.LookAt(target);
    }

    // ���������� ����������� ������ � ������
    public void SnapToPlayer()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(target);
            currentVelocity = Vector3.zero;
            Debug.Log($"������ ��������� ����������� ��: {target.name}");
        }
        else
        {
            Debug.LogWarning("��� ���� ��� ����������� ������������ ������!");
        }
    }

    // ���������� ����������� � ����� �������
    public void SnapToPosition(Vector3 position)
    {
        transform.position = position;
        currentVelocity = Vector3.zero;
    }

    // ��������/��������� ��������
    public void SetFollowing(bool following)
    {
        isFollowing = following;
        if (following && target != null)
        {
            SnapToPlayer();
        }
    }

    // ������� ���� � ���������� �������������
    public void SwitchTarget(Transform newTarget)
    {
        target = newTarget;
        SnapToPlayer();
    }

    // ��������� ������ � ���������� �������
    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
            Debug.Log($"������ ����� ������: {player.name}");

            // ������������� ������������� ����� ���� ����� ������
            CancelInvoke("FindPlayerDelayed");

            // ��������� ������������� �� ������ ������
            SnapToPlayer();
        }
    }

    // ���������� ����� ������
    private void FindPlayerDelayed()
    {
        if (target == null)
        {
            FindPlayer();
        }
        else
        {
            CancelInvoke("FindPlayerDelayed");
        }
    }

    // ��������� ����� ��� ��������������� ������ ������
    public void ForceFindPlayer()
    {
        FindPlayer();
    }

    // ����� ��� ��������� �������� ������
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
        if (target != null)
        {
            SnapToPlayer();
        }
    }

    // ����� ���������� �� PlayerLoader ����� �������� ������
    public void OnPlayerSpawned(Transform playerTransform)
    {
        target = playerTransform;
        SnapToPlayer();
        Debug.Log($"������ ����������� �� ������ ���������: {playerTransform.name}");
    }

    // ������������ � ��������� (������ ��� �������)
    private void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.position);
            Gizmos.DrawWireSphere(target.position + offset, 0.5f);
        }
    }
}