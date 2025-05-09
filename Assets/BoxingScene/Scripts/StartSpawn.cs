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

    // Start is called before the first frame update
    void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(controllerNode);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && spawnManager != null)
        {
           
            spawnManager.GetComponent<Spawn>().SpawnBall();
        }
        if (!device.isValid)
            device = InputDevices.GetDeviceAtXRNode(controllerNode);
        
        if (device.IsPressed(InputHelpers.Button.PrimaryButton, out bool isPressed) && isPressed)
        {
            if (spawnManager != null)
            {
                Debug.Log("A button pressed on Oculus controller");
                spawnManager.GetComponent<Spawn>().SpawnBall();
            }
        }
    }
}
