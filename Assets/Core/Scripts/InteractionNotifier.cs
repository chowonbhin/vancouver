using UnityEngine;

// 기존 상호작용 코드에서 호출할 간단한 통지 클래스
public class InteractionNotifier : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static InteractionNotifier Instance { get; private set; }
    
    private void Awake()
    {
        // 싱글톤 패턴 적용
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
    
    // 상호작용 보고 - 기존 상호작용 코드에서 호출
    public void NotifyInteraction(GameObject interactedObject)
    {
        // 판정 시스템에 보고
        Debug.Log($"NotifyInteraction 호출됨: {interactedObject.name}");
    
        // 판정 시스템에 보고
        if (JudgmentSystem.Instance != null)
        {
            Debug.Log("JudgmentSystem.ReportInteraction 호출 시도");
            JudgmentSystem.Instance.ReportInteraction(interactedObject);
        }
        else
        {
            Debug.LogError("JudgmentSystem.Instance가 null입니다!");
        }
    }
}