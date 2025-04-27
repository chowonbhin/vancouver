using UnityEngine;
using UnityEngine.UI;

public class NoteUI : MonoBehaviour
{
    RectTransform rectTransform;
    private Vector2 startPos;
    private Vector2 endPos;
    float moveDuration = 1;
    public Note note;
    public int noteIndex;

    public enum State
    {
        normal,
        success,
        fail,
        judging // 추가된 상태
    }

    public State state;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        CalculateAnchoredPosition();
        SwitchState(State.normal);  // 초기 상태를 normal로 설정하여 흰색으로 시작
    }

    void CalculateAnchoredPosition()
    {
        // 시작 및 종료 위치 계산
        Canvas canvas = GetComponentInParent<Canvas>();
        float canvasWidth = canvas.GetComponent<RectTransform>().rect.width;

        // Y 위치 유지
        float yPos = rectTransform.anchoredPosition.y;

        // 이미지 크기 계산
        float noteWidth = GetComponent<Image>().rectTransform.rect.width;

        // 시작/끝 위치 계산
        startPos = new Vector2(-canvasWidth / 2f - noteWidth / 2f, yPos);
        endPos = new Vector2(canvasWidth / 2f + noteWidth / 2f, yPos);

        rectTransform.anchoredPosition = startPos;
    }

    public void Initialize(Note note, float moveDuration, int noteIndex)
    {
        this.note = note;
        this.moveDuration = moveDuration;
        this.noteIndex = noteIndex;

        CalculateAnchoredPosition();

        SwitchState(State.normal);  // 정상 상태로 시작
    }

    public void SwitchState(State state)
    {
        this.state = state;
        Color color = Color.white;
        switch (state)
        {
            case State.success:
                color = Color.green;
                break;
            case State.fail:
                color = Color.red;
                break;
            case State.judging: // 판정 구간에 들어왔을 때 파란색
                color = Color.white;
                break;
            case State.normal:
            default:
                color = Color.black;
                break;
        }
        GetComponent<Image>().color = color;
    }

    public bool UpdatePos(double currentTime)
    {
        float appearTime = note.start - moveDuration / 2f;
        float disappearTime = note.start + moveDuration / 2f;
        float t = Mathf.InverseLerp(appearTime, disappearTime, (float)currentTime);
        t = Mathf.Clamp01(t);  // 범위 제한
        GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPos, endPos, t);
        return t == 1;
    }
}
