using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    [Header("ИГРОВЫЕ префабы (с управлением)")]
    public GameObject[] gameCharacterPrefabs;

    [Header("Точка появления")]
    public Transform spawnPoint;

    [Header("Камера")]
    public Camera playerCamera;

    private GameObject currentPlayer;

    void Start()
    {
        SpawnPlayerWithController();
    }

    void SpawnPlayerWithController()
    {
        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);
        Debug.Log($"Создаем игрового персонажа: {selectedCharacter}");

        // Проверки
        if (gameCharacterPrefabs == null || gameCharacterPrefabs.Length == 0)
        {
            Debug.LogError("Нет игровых префабов!");
            return;
        }

        if (selectedCharacter < 0 || selectedCharacter >= gameCharacterPrefabs.Length)
        {
            Debug.LogWarning($"Неверный индекс: {selectedCharacter}. Используем 0");
            selectedCharacter = 0;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("Нет точки появления!");
            return;
        }

        // СОЗДАЕМ ИГРОВОГО ПЕРСОНАЖА
        currentPlayer = Instantiate(
            gameCharacterPrefabs[selectedCharacter],
            spawnPoint.position,
            spawnPoint.rotation
        );

        currentPlayer.name = "Player";

        // НАСТРАИВАЕМ УПРАВЛЕНИЕ И КАМЕРУ
        SetupPlayerController(currentPlayer);
        SetupCamera(currentPlayer);

        Debug.Log($"Игровой персонаж создан: {selectedCharacter}");
    }

    void SetupPlayerController(GameObject player)
    {
        // Добавляем компоненты управления если их нет
        if (player.GetComponent<PlayerController>() == null)
        {
            player.AddComponent<PlayerController>();
            Debug.Log("Добавлен PlayerController");
        }

        // Добавляем физику если нет
        if (player.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = player.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            Debug.Log("Добавлен Rigidbody");
        }

        // Добавляем коллайдер если нет
        if (player.GetComponent<Collider>() == null)
        {
            player.AddComponent<CapsuleCollider>();
            Debug.Log("Добавлен Collider");
        }

        // Устанавливаем тег
        player.tag = "Player";
    }

    void SetupCamera(GameObject player)
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                Debug.LogError("Камера не найдена!");
                return;
            }
        }

        // Добавляем или находим скрипт следования камеры
        CameraFollow cameraFollow = playerCamera.GetComponent<CameraFollow>();
        if (cameraFollow == null)
        {
            cameraFollow = playerCamera.gameObject.AddComponent<CameraFollow>();
        }

        // Настраиваем камеру на игрока
        cameraFollow.target = player.transform;
        cameraFollow.SnapToPlayer();

        Debug.Log("Камера настроена на игрока");
    }
}