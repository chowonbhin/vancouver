using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrelDestroy : MonoBehaviour
{
    public GameObject original;
    public GameObject fractured;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            SpawnFracturedObject();
        }      
    }

    public void SpawnFracturedObject()
    {
        Vector3 spawnPosition = Vector3.zero;
        Quaternion spawnRotation = Quaternion.identity;
        Vector3 spawnScale = Vector3.one;

        if (original != null)
        {
            spawnPosition = original.transform.position;
            spawnRotation = original.transform.rotation;

            Destroy(original);
        }
        else
        {
            Debug.LogWarning("Original barrel is not assigned!");
        }

        if (fractured != null)
        {
            GameObject fracturedObj = Instantiate(fractured, spawnPosition, spawnRotation);
            fracturedObj.GetComponent<ExplodeBarrel>().Explode();
        }
        else
        {
            Debug.LogWarning("Fractured barrel prefab is not assigned!");
        }
    }
}
