using System.Collections;
using UnityEngine;

public class fireballTrap : MonoBehaviour
{
    public GameObject fireballPrefab;
    public float spawnInterval;
    public GameObject fireballInstance;
    public float duration;
    public float delay;

    public Vector3 fireballRotation = Vector3.zero;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        InvokeRepeating("SpawnFireball", delay, spawnInterval);
    }

    // Update is called once per frame
    void SpawnFireball()
    {

        fireballInstance = Instantiate(fireballPrefab, transform.position, Quaternion.Euler(fireballRotation));


        StartCoroutine(DestroyFireball(fireballInstance, duration));
    }

    IEnumerator DestroyFireball(GameObject fireball, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(fireball);
    }

}
