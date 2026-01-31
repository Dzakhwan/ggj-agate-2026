using System.Collections;
using UnityEngine;

public class fireballTrap : MonoBehaviour
{
    public GameObject fireballPrefab;
    public float spawnInterval = 2f;
    public GameObject fireballInstance;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        InvokeRepeating("SpawnFireball", 0f, spawnInterval);
    }

    // Update is called once per frame
    void SpawnFireball()
    {

        fireballInstance = Instantiate(fireballPrefab, transform.position, Quaternion.identity);


        StartCoroutine(DestroyFireball(fireballInstance, 1f));
    }

    IEnumerator DestroyFireball(GameObject fireball, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(fireball);
    }

}
