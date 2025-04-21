using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class AnalogClock : MonoBehaviour
{
  public Transform hourHand;   // 시침
    public Transform minuteHand; // 분침
    public Transform secondHand; // 초침

    void Update()
    {
        DateTime currentTime = DateTime.Now;

        float seconds = currentTime.Second + currentTime.Millisecond / 1000f;
        float minutes = currentTime.Minute + seconds / 60f;
        float hours = currentTime.Hour % 12 + minutes / 60f;

        // 각도 계산 (Z축 회전)
        float secondAngle = -seconds * 6f;
        float minuteAngle = -minutes * 6f;
        float hourAngle = -hours * 30f;

        // 바늘 회전 적용
        secondHand.localRotation = Quaternion.Euler(0, 0, secondAngle);
        minuteHand.localRotation = Quaternion.Euler(0, 0, minuteAngle);
        hourHand.localRotation = Quaternion.Euler(0, 0, hourAngle);
    }
}