using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class InputManager : MonoBehaviour
{
    // 싱글톤 패턴 구현
    public static InputManager Instance { get; private set; }

    // B 버튼으로 전환할 씬 인덱스 (Inspector에서 설정 가능)
    public int bButtonSceneIndex = 2;

    // 입력 장치 참조 (오른쪽 컨트롤러만)
    private InputDevice rightController;

    // 버튼 상태 확인용 변수
    private bool wasBPressed = false;

    private void Awake()
    {
        // 싱글톤 설정 및 씬 전환 시 파괴되지 않도록 유지
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

    private void Update()
    {
        // 오른쪽 컨트롤러가 유효하지 않으면 다시 찾기
        if (!rightController.isValid)
            FindRightController();

        // B 버튼 입력 체크
        CheckButtonInput();
    }

    private void FindRightController()
    {
        var rightHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);
        if (rightHandDevices.Count > 0)
            rightController = rightHandDevices[0];
    }

    private void CheckButtonInput()
    {
        if (!rightController.isValid)
            return;

        bool isBPressed = false;
        // secondaryButton을 사용하면 B 버튼 입력을 읽어옵니다.
        if (rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out isBPressed))
        {
            if (isBPressed && !wasBPressed)
                OnBButtonPressed();
            wasBPressed = isBPressed;
        }
    }

    private void OnBButtonPressed()
    {
        Debug.Log("B 버튼으로 씬 전환: " + bButtonSceneIndex);
        // 동기 로드
        SceneManager.LoadScene(bButtonSceneIndex);
        // 비동기 로딩을 원하시면 아래 코루틴 호출을 사용하세요.
        // StartCoroutine(LoadSceneAsync(bButtonSceneIndex));
    }

    private IEnumerator LoadSceneAsync(int index)
    {
        yield return SceneManager.LoadSceneAsync(index);
    }
}
