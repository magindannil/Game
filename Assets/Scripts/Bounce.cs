using UnityEngine;

public class Bouncer : MonoBehaviour
{
    public float bounceForce = 500f;

    private void OnCollisionEnter(Collision collision)
    {
        // ѕровер€ем, что столкнулс€ именно игрок
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * bounceForce);
            }
        }
    }
}