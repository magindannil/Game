using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float Speed = 2f;
    public float Distance = 3f;

    private Vector3 startPosition;

    void Start()
    {
        // —охран€ем начальную позицию из редактора
        startPosition = transform.position;
    }

    void Update()
    {
        // ƒвижение относительно сохраненной позиции
        float movement = Mathf.Sin(Time.time * Speed) * Distance;
        transform.position = startPosition + new Vector3(movement, 0, 0);
    }
}