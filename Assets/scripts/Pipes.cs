
using UnityEngine;

public class Pipes : MonoBehaviour
{
    public float speed = 5f;

    private float leftEdge;

    private void Start()
    {
        leftEdge = -3f;

    }

    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
        if(transform.localPosition.x < leftEdge)
        {
            Destroy(gameObject);
        }
    }
}
