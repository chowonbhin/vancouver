using System.Collections;
using UnityEngine;
using TMPro;

public class JudgmentUI : MonoBehaviour
{
    [SerializeField] private GameObject judgmentPanel;
    [SerializeField] private TextMeshProUGUI judgmentText;
    [SerializeField] private float displayDuration = 0.75f;
    
    [Header("색상 설정")]
    [SerializeField] private Color perfectColor = Color.green;
    [SerializeField] private Color goodColor = Color.yellow;
    [SerializeField] private Color badColor = Color.red;
    
    [Header("점수 표시")]
    [SerializeField] private TextMeshProUGUI scoreText;
    
    // 시작 시 판넬 비활성화
    private void Start()
    {
        if (judgmentPanel != null)
        {
            judgmentPanel.SetActive(false);
        }
    }
    
    // 판정 결과 표시
    public void DisplayJudgment(JudgmentResult result)
    {
        Debug.Log($"DisplayJudgment 호출됨: {result}");
    
        if (judgmentPanel == null)
        {
            Debug.LogError("judgmentPanel이 null입니다!");
            return;
        }
        
        if (judgmentText == null)
        {
            Debug.LogError("judgmentText가 null입니다!");
            return;
        }
        
        // 텍스트 및 색상 설정
        switch (result)
        {
            case JudgmentResult.Perfect:
                judgmentText.text = "PERFECT!";
                judgmentText.color = perfectColor;
                Debug.Log("PERFECT! 텍스트 설정됨");
                break;
                
            case JudgmentResult.Good:
                judgmentText.text = "GOOD";
                judgmentText.color = goodColor;
                Debug.Log("GOOD 텍스트 설정됨");
                break;
                
            case JudgmentResult.Bad:
                judgmentText.text = "MISS";
                judgmentText.color = badColor;
                Debug.Log("MISS 텍스트 설정됨");
                break;
                
            default:
                return;
        }
        
        // 캔버스 강제 업데이트
        if (judgmentPanel.transform.parent != null)
        {
            Canvas canvas = judgmentPanel.transform.parent.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.enabled = false;
                canvas.enabled = true;
            }
        }
        
        // 패널 활성화 확인
        judgmentPanel.SetActive(true);
        Debug.Log($"판정 패널 활성화: {judgmentPanel.activeSelf}");
        
        // 표시 후 자동 숨김
        StopAllCoroutines();
        StartCoroutine(ShowJudgmentRoutine());
    }
    
    // 판정 표시 코루틴
    private IEnumerator ShowJudgmentRoutine()
    {
        judgmentPanel.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        judgmentPanel.SetActive(false);
    }
    
    // 점수 텍스트 업데이트
    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }
}