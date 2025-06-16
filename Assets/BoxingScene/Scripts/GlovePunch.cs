using UnityEngine;
using UnityEngine.XR;
using TMPro;
using static UnityEngine.UI.Image;
using UnityEngine.AI;

public class GlovePunchXR : MonoBehaviour
{
    public XRNode controllerNode = XRNode.RightHand;
    public float minPunchSpeed = 1.0f;
    public Mesh[] newMesh;
    public AudioClip punchSound;
    public GameObject fractured;
    private AudioSource audioSource;
    private Vector3 currentVelocity;
    private InputDevice device;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        device = InputDevices.GetDeviceAtXRNode(controllerNode);

    
    }

    void Update()
    {
        if (!device.isValid)
            device = InputDevices.GetDeviceAtXRNode(controllerNode);

        if (device.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity))
            currentVelocity = velocity;
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("PunchBag") || other.CompareTag("Bomb"))  && currentVelocity.magnitude > minPunchSpeed)// 
        {
            if (audioSource != null && punchSound != null)
                audioSource.PlayOneShot(punchSound);
            effects(other);

            Rigidbody rb = other.attachedRigidbody;
            if (rb != null)
            {

                Vector3 forceDirection = currentVelocity.normalized;
                float forceStrength = currentVelocity.magnitude * 1.1f;
                rb.AddForce(forceDirection * forceStrength, ForceMode.Impulse);
            }

            if(other.gameObject.GetComponent<RhythmData>() != null)
            {
                if (InteractionNotifier.Instance != null)
                {
                    InteractionNotifier.Instance.NotifyInteraction(other.gameObject);
                }
            }

            other.gameObject.tag = "processed";

            Destroy(other.gameObject, 1.5f);

        }

        else if (other.CompareTag("Barrel") && currentVelocity.magnitude > minPunchSpeed)
        {
            if (audioSource != null && punchSound != null)
                audioSource.PlayOneShot(punchSound);
            effects(other);


            Transform bombSpawnTransform = other.transform.Find("BombSpawn");


            if (bombSpawnTransform != null)
            {
                Spawn spawner = bombSpawnTransform.GetComponent<Spawn>();
                if (spawner != null)
                {
                    spawner.SpawnBall();
                }
                else
                {
                    Debug.LogWarning("Spawn not found in BombSpawn.");
                }
            }
            else
            {
                Debug.LogWarning(" BombSpawn not found in Barrel.");
            }

            if(other.gameObject.GetComponent<RhythmData>() != null)
            {
                if (InteractionNotifier.Instance != null)
                {
                    InteractionNotifier.Instance.NotifyInteraction(other.gameObject);
                }
            }

            other.gameObject.tag = "processed";

            SpawnFracturedObject(other.gameObject);


        } else if (other.CompareTag("bonusTarget" ) && currentVelocity.magnitude > minPunchSpeed) { 


            Debug.Log("Hit bonus");
            if (audioSource != null && punchSound != null)
                audioSource.PlayOneShot(punchSound);

            effects(other);

            Transform root = FindParentWithTag(other.transform, "PunchBag"); 
            if (root != null)
            {
                Debug.Log("found Parent");
                effects(root.GetComponent<Collider>()); 
            }

            Rigidbody rb = root.GetComponent<Collider>().attachedRigidbody;
            if (rb != null)
            {

                Vector3 forceDirection = currentVelocity.normalized;
                float forceStrength = currentVelocity.magnitude * 1.1f;
                rb.AddForce(forceDirection * forceStrength, ForceMode.Impulse);
            }

            if(other.gameObject.GetComponent<RhythmData>() != null)
            {
                if (InteractionNotifier.Instance != null)
                {
                    InteractionNotifier.Instance.NotifyInteraction(other.gameObject);
                }
            }

            other.gameObject.tag = "processed";

            Destroy(root.gameObject, 1.5f);
        }
    }

    public void SpawnFracturedObject(GameObject original)
    {
        Vector3 spawnPosition = Vector3.zero;
        Quaternion spawnRotation = Quaternion.identity;

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

    public void effects(Collider objc) {

        

        ParticleSystem ps = objc.GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            ParticleSystemRenderer renderer = ps.GetComponent<ParticleSystemRenderer>();
            if (renderer != null && newMesh != null && newMesh.Length > 0)
            {
                renderer.renderMode = ParticleSystemRenderMode.Mesh;
                renderer.mesh = newMesh[Random.Range(0, newMesh.Length)];// here u can set number [0: "1", 1:"2", 2:"3"] newMesh[0] => "1"
            }
            ps.transform.parent = null;
            ps.Play();
            Destroy(ps.gameObject, ps.main.duration + 0.5f);
        }


        // 2. VFX Graph
        UnityEngine.VFX.VisualEffect vfx = objc.GetComponentInChildren<UnityEngine.VFX.VisualEffect>();
        if (vfx != null)
        {
            vfx.transform.parent = null;
            vfx.Play();
            Destroy(vfx.gameObject, 2f);
        }
    }

    private Transform FindParentWithTag(Transform child, string tag)
    {
        Transform current = child;

        while (current != null)
        {
            if (current.CompareTag(tag))
            {
                return current;
            }
            current = current.parent;
        }

        return null;
    }

}
