using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawn : MonoBehaviour
{

    public GameObject[] ballPrefabs;
    public GameObject barrelPrefab;
    public Transform spawnPoint;
    public Transform pointB;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        pointB = Camera.main.transform;
        if (spawnPoint != null && pointB != null)
        {
            distance = Vector3.Distance(spawnPoint.position, pointB.position);
            //Debug.Log("distance: " + distance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void SpawnBall(float time = 5f, bool barrel = false)
    {

        //float defTime = distance / 0.5f;

        //Debug.Log("time: " + defTime);
        GameObject prefabToSpawn = barrelPrefab;
        if (!barrel) {
            int index = Random.Range(0, ballPrefabs.Length);
            prefabToSpawn = ballPrefabs[index];
        }
        

        GameObject newBall = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        var slowGravity = newBall.AddComponent<SlowGravity>();
        slowGravity.gravityForce = distance/time; 
        Destroy(newBall.gameObject, 15f);

        //test score board
       
    }

}
