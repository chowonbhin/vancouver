using BaseBallScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherEvent : MonoBehaviour
{
    public Canvas PitcherLeft;
    public Canvas PitcherRight;
    public Pitcher pitcher;

    RectTransform left;
    RectTransform right;

    private void Awake()
    {
        left = PitcherLeft.GetComponent<RectTransform>();
        right = PitcherRight.GetComponent<RectTransform>();
    }

    public void OnEventImg(Ball.SwingEvent e)
    {
        OffEventImg();
        if (e == Ball.SwingEvent.Left)
        {
            PitcherRight.gameObject.SetActive(true);
        }
        else if (e == Ball.SwingEvent.Right)
        {
            PitcherLeft.gameObject.SetActive(true);
        }
    }

    public void OffEventImg()
    {

        PitcherLeft.gameObject.SetActive(false);
        PitcherRight.gameObject.SetActive(false);
    }

    public void StartBadBallCoroutine(Ball ball)
    {
        ball.BadBallCoroutine = StartCoroutine(BadBallCoroutine(ball));
    }
    IEnumerator BadBallCoroutine(Ball ball)
    {
        Vector3 startPos = ball.transform.position;
        Vector3 targetPos = (ball.PitcherE == Ball.SwingEvent.Right) ? PitcherLeft.transform.position + Vector3.up * 0.9f* left.sizeDelta.y /2  : PitcherRight.transform.position + Vector3.up * 0.9f * right.sizeDelta.y / 2;
        float duration = 1f; 
        float elapsed = 0f;
        Vector3 gravity = Physics.gravity;
        Vector3 initialVelocity = (targetPos - startPos - 0.5f * gravity * duration * duration) / duration;
        Vector3 pos = startPos;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Min(elapsed, duration);
            pos = startPos + initialVelocity * t + 0.5f * gravity * t * t;
            ball.transform.position = pos;
            yield return null;
        }
        ball.transform.position = targetPos;
        OffEventImg();
        pitcher.ReturnBall(ball);
    }
}
