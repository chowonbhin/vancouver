using UnityEngine;

public class SlowGravity : MonoBehaviour
{
    public float gravityForce = 0.5f; 
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.useGravity = false;

    }

    void FixedUpdate()
    {
        if (rb != null)
            rb.AddForce(Vector3.down * gravityForce, ForceMode.Acceleration);
    }
}
