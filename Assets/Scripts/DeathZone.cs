using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("����� ����� � ���� ������");

            PlayerDeathHandler playerDeath = other.GetComponent<PlayerDeathHandler>();
            if (playerDeath != null)
            {
                playerDeath.Die();
            }
            else
            {
                Debug.LogError("PlayerDeathHandler �� ������ �� ������!");
            }
        }
    }
}