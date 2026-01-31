using Unity.VisualScripting;
using UnityEngine;

public class fireball : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 direction = Vector3.up;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            Debug.Log("Fireball Hit Platform");
            Destroy(gameObject);
        }
    }


}
