using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class InputManager : MonoBehaviour
{
    // 싱글톤 패턴 구현
    public static InputManager Instance { get; private set; }

    // SceneTransitionManager 참조
    public SceneTransitionManager sceneTransitionManager;
    
    // 전환할 씬 인덱스 (Inspector에서 설정 가능)
    public int yButtonSceneIndex = 1; // Y 버튼으로 전환할 씬
    public int bButtonSceneIndex = 2; // B 버튼으로 전환할 씬

    // 입력 장치 참조
    private InputDevice rightController;
    private InputDevice leftController;

    // 버튼 상태 확인용 변수
    private bool wasYPressed = false;
    private bool wasBPressed = false;

    private void Awake()
    {
        // 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // SceneTransitionManager가 설정되어 있지 않으면 찾기
        if (sceneTransitionManager == null)
        {
            sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
            
            if (sceneTransitionManager == null)
            {
                Debug.LogError("SceneTransitionManager를 찾을 수 없습니다!");
            }
        }
    }

    private void Update()
    {
        // 컨트롤러를 찾지 못했다면 다시 찾기 시도
        if (!rightController.isValid || !leftController.isValid)
        {
            FindControllers();
        }

        // 버튼 입력 확인
        CheckButtonInput();
    }

    private void FindControllers()
    {
        // 오른쪽 컨트롤러(B 버튼) 찾기
        var rightHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);
        if (rightHandDevices.Count > 0)
        {
            rightController = rightHandDevices[0];
        }

        // 왼쪽 컨트롤러(Y 버튼) 찾기
        var leftHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
        if (leftHandDevices.Count > 0)
        {
            leftController = leftHandDevices[0];
        }
    }

    private void CheckButtonInput()
    {
        if (leftController.isValid)
        {
            bool isYPressed = false;
            if (leftController.TryGetFeatureValue(CommonUsages.primaryButton, out isYPressed))
            {
                // Y 버튼 눌림 상태가 바뀌었을 때만 실행 (첫 누름 감지)
                if (isYPressed && !wasYPressed)
                {
                    // Y 버튼이 눌렸을 때 씬 전환
                    OnYButtonPressed();
                }
                wasYPressed = isYPressed;
            }
        }

        if (rightController.isValid)
        {
            bool isBPressed = false;
            if (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out isBPressed))
            {
                // B 버튼 눌림 상태가 바뀌었을 때만 실행 (첫 누름 감지)
                if (isBPressed && !wasBPressed)
                {
                    // B 버튼이 눌렸을 때 씬 전환
                    OnBButtonPressed();
                }
                wasBPressed = isBPressed;
            }
        }
    }

    // Y 버튼 누를 때 실행할 함수
    private void OnYButtonPressed()
    {
        Debug.Log("Y 버튼으로 씬 전환: " + yButtonSceneIndex);
        
        if (sceneTransitionManager != null)
        {
            sceneTransitionManager.GoToScene(yButtonSceneIndex);
        }
        else
        {
            // SceneTransitionManager가 없으면 다시 찾기 시도
            sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
            
            if (sceneTransitionManager != null)
            {
                sceneTransitionManager.GoToScene(yButtonSceneIndex);
            }
            else
            {
                Debug.LogError("SceneTransitionManager를 찾을 수 없습니다!");
            }
        }
    }

    // B 버튼 누를 때 실행할 함수
    private void OnBButtonPressed()
    {
        Debug.Log("B 버튼으로 씬 전환: " + bButtonSceneIndex);
        
        if (sceneTransitionManager != null)
        {
            sceneTransitionManager.GoToScene(bButtonSceneIndex);
        }
        else
        {
            // SceneTransitionManager가 없으면 다시 찾기 시도
            sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
            
            if (sceneTransitionManager != null)
            {
                sceneTransitionManager.GoToScene(bButtonSceneIndex);
            }
            else
            {
                Debug.LogError("SceneTransitionManager를 찾을 수 없습니다!");
            }
        }
    }
}