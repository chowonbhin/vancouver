using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class GlovePunchXR : MonoBehaviour
{
    public XRNode controllerNode = XRNode.RightHand;
    public float minPunchSpeed = 1.0f;

    public int hitCount = 0;
    public TextMeshProUGUI hitText;
    public AudioClip punchSound;
    public GameObject spawnManager;    // Объект с компонентом Spawn.cs

    private AudioSource audioSource;
    private Vector3 currentVelocity;
    private InputDevice device;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        device = InputDevices.GetDeviceAtXRNode(controllerNode);

        if (hitText != null)
            hitText.text = "Hits: 0";
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
        if (other.CompareTag("PunchBag") && currentVelocity.magnitude > minPunchSpeed)
        {
            hitCount++;

            if (audioSource != null && punchSound != null)
                audioSource.PlayOneShot(punchSound);

            if (hitText != null)
                hitText.text = "Hits: " + hitCount;

            Rigidbody rb = other.attachedRigidbody;
            if (rb != null)
            {
                Vector3 forceDirection = currentVelocity.normalized;
                float forceStrength = currentVelocity.magnitude * 5f;
                rb.AddForce(forceDirection * forceStrength, ForceMode.Impulse);
            }

            
            ParticleSystem ps = other.GetComponentInChildren<ParticleSystem>();
            if (ps != null)
            {
                ps.transform.parent = null;
                ps.Play();
                Destroy(ps.gameObject, ps.main.duration + 0.5f);
            }

            // 2. VFX Graph
            UnityEngine.VFX.VisualEffect vfx = other.GetComponentInChildren<UnityEngine.VFX.VisualEffect>();
            if (vfx != null)
            {
                vfx.transform.parent = null;
                vfx.Play();
                Destroy(vfx.gameObject, 2f); 
            }

            Destroy(other.gameObject, 2.5f);

            if (spawnManager != null)
            {
                spawnManager.GetComponent<Spawn>().SpawnBall();
            }
        }
    }
}
