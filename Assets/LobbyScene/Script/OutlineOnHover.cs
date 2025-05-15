using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OutlineOnHover : MonoBehaviour
{
    public Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public void OnHoverEntered(HoverEnterEventArgs args)
    {
        outline.enabled = true;
    }

    public void OnHoverExited(HoverExitEventArgs args)
    {
        outline.enabled = false;
    }
}
