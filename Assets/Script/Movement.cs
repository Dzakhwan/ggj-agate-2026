using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;
    private float vertical, horizontal;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, vertical, 0.0f);
        rb.linearVelocity = movement * speed;

    }
}
