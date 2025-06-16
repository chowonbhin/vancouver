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

    private float currentBeatTime = 0f;

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

    public GameObject SpawnBall(float travelTime = 5f, float beatTime = 0f, bool barrel = false)
    {
        Debug.Log($"CreateBall 호출됨: time={travelTime}, beatTime={beatTime}, barrel={barrel}");
        //float defTime = distance / 0.5f;

        currentBeatTime = beatTime;

        //Debug.Log("time: " + defTime);
        GameObject prefabToSpawn = barrelPrefab;
        if (!barrel) {
            int index = Random.Range(0, ballPrefabs.Length);
            prefabToSpawn = ballPrefabs[index];
        }
        

        GameObject newBall = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        RhythmData rhythmData = newBall.AddComponent<RhythmData>();
        rhythmData.Initialize(currentBeatTime, barrel);
        Debug.Log($"리듬 데이터 추가: 오브젝트 '{newBall.name}', 타겟시간 {currentBeatTime}");
        var slowGravity = newBall.AddComponent<SlowGravity>();
        slowGravity.gravityForce = 2f*distance/(travelTime*travelTime); 

        Destroy(newBall.gameObject, 15f);

        //adding bonus target
        int events = Random.Range(0, 3);
        if(events == 1 ){

            Transform[] children = newBall.GetComponentsInChildren<Transform>(true);

            var targets = new System.Collections.Generic.List<GameObject>();

            foreach (Transform child in children)
            {
                if ( child.CompareTag("bonusTarget")) //  or child.name  == "bonusTarget"
                {
                    targets.Add(child.gameObject);
                }
            }
            // deactivation
            foreach (var t in targets)
            {
                t.SetActive(false);
            }

            if (targets.Count > 0)
            {
                int index = Random.Range(0, targets.Count);
                targets[index].SetActive(true);
            }
        }

        return newBall;
    }

}
