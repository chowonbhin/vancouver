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
        fail
    }


    public State state;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        CalculateAnchoredPosition();
    }

    void CalculateAnchoredPosition()
    {
        // ĵ���� ���� �¿� �ʺ� ���
        Canvas canvas = GetComponentInParent<Canvas>();
        float canvasWidth = canvas.GetComponent<RectTransform>().rect.width;

        // ���� y ��ġ�� �������� ����
        float yPos = rectTransform.anchoredPosition.y;

        // ���� �̹��� width ���� ���� ���
        float noteWidth = GetComponent<Image>().rectTransform.rect.width;

        // �¿� ȭ�� �ۿ��� ����/���� ��ġ ����
        startPos = new Vector2(-canvasWidth / 2f - noteWidth / 2f, yPos);
        endPos = new Vector2(canvasWidth / 2f + noteWidth / 2f, yPos);

        SwitchState(State.normal);
        rectTransform.anchoredPosition = startPos;
    }


    public void Initialize(Note note, float moveDuration, int noteIndex)
    {
        this.note = note;
        this.moveDuration = moveDuration;
        this.noteIndex = noteIndex;
        
        // ��Ʈ ���̿� ���� Width ���� 
        float totalMoveDistance = Vector2.Distance(startPos, endPos);
        float speed = totalMoveDistance / moveDuration;
        float imageWidth = speed * (note.end- note.start);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imageWidth);
        CalculateAnchoredPosition();

        SwitchState(State.normal);
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
            case State.normal:
            default:
                color = Color.white;
                break;
        }
        GetComponent<Image>().color = color;
    }

    public bool UpdatePos(float currentTime)
    {
        float appearTime = note.start - moveDuration / 2f;
        float disappearTime = note.end + moveDuration / 2f;
        float t = Mathf.InverseLerp(appearTime, disappearTime, currentTime);
        t = Mathf.Clamp01(t);  // �ٿ���� üũ
        GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPos, endPos, t);
        return t == 1;
    }
}