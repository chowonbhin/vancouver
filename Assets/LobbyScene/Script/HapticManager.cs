using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticManager : MonoBehaviour
{
    [Range(0f, 1f)]
    public float defaultIntensity = 0.5f;
    public float defaultDuration = 0.1f;

    // 싱글톤 패턴 구현 (선택적)
    public static HapticManager Instance { get; private set; }

    private void Awake()
    {
        // 싱글톤 설정 (다른 씬으로 전환해도 유지하려면 DontDestroyOnLoad도 추가)
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // 필요하다면 주석 해제
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Unity 이벤트 시스템에 연결할 퍼블릭 메서드
    public void SendHapticFeedback()
    {
        SendHapticFeedbackToControllers(defaultIntensity, defaultDuration);
    }

    // 강한 햅틱 피드백
    public void SendStrongHapticFeedback()
    {
        SendHapticFeedbackToControllers(0.8f, 0.2f);
    }

    // 약한 햅틱 피드백
    public void SendLightHapticFeedback()
    {
        SendHapticFeedbackToControllers(0.3f, 0.1f);
    }

    // 내부 구현 메서드
    private void SendHapticFeedbackToControllers(float intensity, float duration)
    {
        // 모든 ActionBasedController 찾기
        ActionBasedController[] controllers = FindObjectsOfType<ActionBasedController>();

        foreach (var controller in controllers)
        {
            // 각 컨트롤러에 햅틱 피드백 전송
            controller.SendHapticImpulse(intensity, duration);
        }

        // 로그 남기기 (디버깅용, 필요하지 않으면 제거)
        Debug.Log($"햅틱 피드백 전송: 강도 {intensity}, 지속 시간 {duration}초");
    }
}