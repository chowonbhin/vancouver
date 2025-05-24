using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject countdownPanel;
    [SerializeField] private TextMeshProUGUI countdownText;
    
    private void Awake()
    {
        // RhythmGameManager와 함께 유지됨
        transform.SetParent(RhythmGameManager.Instance.transform);
    }
    
    public void ShowCountdown(float duration)
    {
        if (countdownPanel == null || countdownText == null)
        {
            Debug.LogWarning("카운트다운 UI가 설정되지 않았습니다.");
            return;
        }
        
        // 카운트다운 패널 활성화
        countdownPanel.SetActive(true);
        
        // 카운트다운 시작
        StartCoroutine(CountdownRoutine(duration));
    }
    
    private IEnumerator CountdownRoutine(float duration)
    {
        // 초 단위로 카운트다운 표시
        for (int i = Mathf.CeilToInt(duration); i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        
        // 카운트다운 종료 메시지
        countdownText.text = "시작!";
        yield return new WaitForSeconds(0.5f);
        
        // 카운트다운 UI 비활성화
        countdownPanel.SetActive(false);
    }
}