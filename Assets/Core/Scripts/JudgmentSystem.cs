using System;
using UnityEngine;

// 판정 결과 열거형
public enum JudgmentResult
{
    None,     // 초기 상태
    Bad,      // 0점
    Good,     // 1점
    Perfect   // 2점
}

// 판정 시스템 - 싱글톤 패턴
public class JudgmentSystem : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static JudgmentSystem Instance { get; private set; }
    
    // 판정 범위 설정
    [Header("판정 설정")]
    [SerializeField] private float perfectWindow = 0.1f;  // ±100ms
    [SerializeField] private float goodWindow = 0.2f;     // ±300ms
    
    // UI 참조
    [Header("UI 참조")]
    [SerializeField] private JudgmentUI judgmentUI;
    
    // 이벤트
    public event Action<GameObject, JudgmentResult, float> OnJudgment;

    // 게임 시작 시간
    private float gameStartTime = 0f;
    
    // 현재 점수
    private int currentScore = 0;
    
    // 디버그 설정
    [Header("디버그")]
    [SerializeField] private bool showDebugLogs = true;
    
    
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

    // 게임 시작 시간 설정
    public void SetGameStartTime(float startTime)
    {
        gameStartTime = startTime;
    }
    
    // 오브젝트 상호작용 보고 - 기존 상호작용 코드에서 호출
    public void ReportInteraction(GameObject interactedObject)
    {
        Debug.Log($"ReportInteraction 호출됨: {interactedObject.name}");
    
        // RhythmData 컴포넌트 확인
        RhythmData rhythmData = interactedObject.GetComponent<RhythmData>();
        if (rhythmData == null)
        {
            Debug.LogError($"오브젝트 {interactedObject.name}에 RhythmData가 없습니다!");
            return;
        }
        
        // 상호작용 시간 계산
        float interactionTime = Time.time - gameStartTime;
        
        // 타이밍 차이 계산
        float timingDifference = Mathf.Abs(interactionTime - rhythmData.targetTime);
        
        Debug.Log($"상호작용 시간: {interactionTime:F3}, 타겟 시간: {rhythmData.targetTime:F3}, 차이: {timingDifference:F3}");
        
        // 판정 결과 결정
        JudgmentResult result = JudgeTimingDifference(timingDifference);
        
        Debug.Log($"판정 결과: {result}");
        
        // 점수 계산
        UpdateScore(result);
        
        // 판정 결과 UI 표시
        if (judgmentUI != null)
        {
            Debug.Log($"DisplayJudgment 호출: {result}");
            judgmentUI.DisplayJudgment(result);
        }
        else
        {
            Debug.LogError("judgmentUI가 null입니다!");
        }
        
        // 이벤트 발생
        OnJudgment?.Invoke(interactedObject, result, timingDifference);
        
        // 디버그 로그
        if (showDebugLogs)
        {
            Debug.Log($"판정: {result} (차이: {timingDifference:F3}초, 타겟: {rhythmData.targetTime:F3}, 실제: {interactionTime:F3})");
        }
    }
    
    // 타이밍 차이에 따른 판정
    private JudgmentResult JudgeTimingDifference(float timingDifference)
    {
        if (timingDifference <= perfectWindow)
        {
            return JudgmentResult.Perfect;
        }
        else if (timingDifference <= goodWindow)
        {
            return JudgmentResult.Good;
        }
        else
        {
            return JudgmentResult.Bad;
        }
    }

    // UI 참조를 업데이트하는 메서드
    public void SetJudgmentUI(JudgmentUI ui)
    {
        if (ui != null)
        {
            judgmentUI = ui;
            
            // 현재 점수 표시 업데이트
            judgmentUI.UpdateScore(currentScore);
            
            Debug.Log("JudgmentUI가 성공적으로 설정되었습니다.");
        }
    }

    public void ClearJudgmentUI(JudgmentUI ui)
    {
        // 동일한 UI 인스턴스인 경우에만 제거
        if (judgmentUI == ui)
        {
            judgmentUI = null;
            Debug.Log("JudgmentUI 참조가 제거되었습니다.");
        }
    }
    
    // 점수 업데이트
    public void UpdateScore(int score , string debugLog = null)
    {
        if (showDebugLogs)
        {
            Debug.Log($"{debugLog}, CurrentScore : {currentScore} + Score : {score} ");
        }

        currentScore += score;
        // UI 업데이트
        if (judgmentUI != null)
        {
            judgmentUI.UpdateScore(currentScore);
        }
    }

    private void UpdateScore(JudgmentResult result)
    {
        switch (result)
        {
            case JudgmentResult.Perfect:
                currentScore += 2;
                break;

            case JudgmentResult.Good:
                currentScore += 1;
                break;

                // Bad는 점수 없음
        }

        // UI 업데이트
        if (judgmentUI != null)
        {
            judgmentUI.UpdateScore(currentScore);
        }
    }


    // 점수 리셋
    public void ResetScore()
    {
        currentScore = 0;
        
        if (judgmentUI != null)
        {
            judgmentUI.UpdateScore(currentScore);
        }
    }
    
    // 판정 범위 설정
    public void SetJudgmentWindows(float perfectWindow, float goodWindow)
    {
        this.perfectWindow = perfectWindow;
        this.goodWindow = goodWindow;
    }

    
}