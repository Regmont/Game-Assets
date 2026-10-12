using UnityEngine;

public class MineExplode : MonoBehaviour
{
    public GameObject objectToSpawn;
    public GameObject LightSpawn;
    float explosionHeight = 15.4f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
            playerStats.Damage();

            Vector3 pos = transform.position;
            pos.y += explosionHeight;

            
            Instantiate(objectToSpawn, pos, Quaternion.identity);
            Instantiate(LightSpawn, pos, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}