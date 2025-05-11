using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class StartSpawn : MonoBehaviour
{
    public GameObject spawnManager;
    public XRNode controllerNode = XRNode.RightHand;
    private InputDevice device;

    private bool buttonPressedLastFrame = false; 

    void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(controllerNode);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && spawnManager != null)
        {
            spawnManager.GetComponent<Spawn>().SpawnBall();
        }

        if (!device.isValid)
            device = InputDevices.GetDeviceAtXRNode(controllerNode);

        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed))
        {
            if (isPressed && !buttonPressedLastFrame)
            {
                buttonPressedLastFrame = true;

                if (spawnManager != null)
                {
                    Debug.Log("A button pressed on Oculus controller");
                    spawnManager.GetComponent<Spawn>().SpawnBall();
                }
            }
            else if (!isPressed)
            {
                buttonPressedLastFrame = false;
            }
        }
    }
}
