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
        // Получаем ссылку на PlayerLoader для отслеживания появления игрока
        playerLoader = FindObjectOfType<PlayerLoader>();

        // Автопоиск игрока
        if (target == null)
        {
            FindPlayer();
        }

        // Мгновенно устанавливаем камеру на игрока при старте
        SnapToPlayer();

        // Если игрок еще не найден, начинаем периодический поиск
        if (target == null)
        {
            InvokeRepeating("FindPlayerDelayed", 0.5f, 1f);
        }
    }

    private void LateUpdate()
    {
        if (!isFollowing || target == null)
        {
            // Если цель потеряна, пытаемся найти снова
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
                // Полностью мгновенное следование
                transform.position = target.position + offset;
            }
            else
            {
                // Почти мгновенное следование с минимальной плавностью
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

        // Камера всегда смотрит на игрока
        transform.LookAt(target);
    }

    // Мгновенное перемещение камеры к игроку
    public void SnapToPlayer()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(target);
            currentVelocity = Vector3.zero;
            Debug.Log($"Камера мгновенно переключена на: {target.name}");
        }
        else
        {
            Debug.LogWarning("Нет цели для мгновенного переключения камеры!");
        }
    }

    // Мгновенное перемещение к любой позиции
    public void SnapToPosition(Vector3 position)
    {
        transform.position = position;
        currentVelocity = Vector3.zero;
    }

    // Включить/выключить слежение
    public void SetFollowing(bool following)
    {
        isFollowing = following;
        if (following && target != null)
        {
            SnapToPlayer();
        }
    }

    // Сменить цель с мгновенным переключением
    public void SwitchTarget(Transform newTarget)
    {
        target = newTarget;
        SnapToPlayer();
    }

    // Автопоиск игрока с улучшенной логикой
    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
            Debug.Log($"Камера нашла игрока: {player.name}");

            // Останавливаем периодический поиск если нашли игрока
            CancelInvoke("FindPlayerDelayed");

            // Мгновенно переключаемся на нового игрока
            SnapToPlayer();
        }
    }

    // Отложенный поиск игрока
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

    // Публичный метод для принудительного поиска игрока
    public void ForceFindPlayer()
    {
        FindPlayer();
    }

    // Метод для установки смещения камеры
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
        if (target != null)
        {
            SnapToPlayer();
        }
    }

    // Метод вызываемый из PlayerLoader когда персонаж создан
    public void OnPlayerSpawned(Transform playerTransform)
    {
        target = playerTransform;
        SnapToPlayer();
        Debug.Log($"Камера переключена на нового персонажа: {playerTransform.name}");
    }

    // Визуализация в редакторе (только для отладки)
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