using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject ballPrefab;     
    public Transform spawnPoint;

    public void SpawnBall()
    {
        GameObject newBall = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
        newBall.AddComponent<SlowGravity>();
        Destroy(newBall.gameObject, 5f);
    }

}
