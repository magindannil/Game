using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    [Header("������� ������� (� �����������)")]
    public GameObject[] gameCharacterPrefabs;

    [Header("����� ���������")]
    public Transform spawnPoint;

    [Header("������")]
    public Camera playerCamera;

    private GameObject currentPlayer;

    void Start()
    {
        SpawnPlayerWithController();
    }

    void SpawnPlayerWithController()
    {
        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);
        Debug.Log($"������� �������� ���������: {selectedCharacter}");

        // ��������
        if (gameCharacterPrefabs == null || gameCharacterPrefabs.Length == 0)
        {
            Debug.LogError("��� ������� ��������!");
            return;
        }

        if (selectedCharacter < 0 || selectedCharacter >= gameCharacterPrefabs.Length)
        {
            Debug.LogWarning($"�������� ������: {selectedCharacter}. ���������� 0");
            selectedCharacter = 0;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("��� ����� ���������!");
            return;
        }

        // ������� �������� ���������
        currentPlayer = Instantiate(
            gameCharacterPrefabs[selectedCharacter],
            spawnPoint.position,
            spawnPoint.rotation
        );

        currentPlayer.name = "Player";

        // ����������� ���������� � ������
        SetupPlayerController(currentPlayer);
        SetupCamera(currentPlayer);

        Debug.Log($"������� �������� ������: {selectedCharacter}");
    }

    void SetupPlayerController(GameObject player)
    {
        // ��������� ���������� ���������� ���� �� ���
        if (player.GetComponent<PlayerController>() == null)
        {
            player.AddComponent<PlayerController>();
            Debug.Log("�������� PlayerController");
        }

        // ��������� ������ ���� ���
        if (player.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = player.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            Debug.Log("�������� Rigidbody");
        }

        // ��������� ��������� ���� ���
        if (player.GetComponent<Collider>() == null)
        {
            player.AddComponent<CapsuleCollider>();
            Debug.Log("�������� Collider");
        }

        // ������������� ���
        player.tag = "Player";
    }

    void SetupCamera(GameObject player)
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                Debug.LogError("������ �� �������!");
                return;
            }
        }

        // ��������� ��� ������� ������ ���������� ������
        CameraFollow cameraFollow = playerCamera.GetComponent<CameraFollow>();
        if (cameraFollow == null)
        {
            cameraFollow = playerCamera.gameObject.AddComponent<CameraFollow>();
        }

        // ����������� ������ �� ������
        cameraFollow.target = player.transform;
        cameraFollow.SnapToPlayer();

        Debug.Log("������ ��������� �� ������");
    }
}