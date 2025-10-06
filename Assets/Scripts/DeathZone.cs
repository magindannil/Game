using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Игрок вошел в зону смерти");

            PlayerDeathHandler playerDeath = other.GetComponent<PlayerDeathHandler>();
            if (playerDeath != null)
            {
                playerDeath.Die();
            }
            else
            {
                Debug.LogError("PlayerDeathHandler не найден на игроке!");
            }
        }
    }
}