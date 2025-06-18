using UnityEngine;

// 발사된 오브젝트에 부착하는 타이밍 데이터
public class RhythmData : MonoBehaviour
{
    // 시간 데이터
    [System.NonSerialized] public float spawnTime;    // 생성 시간
    [System.NonSerialized] public float targetTime;   // 타겟 타격 시간
    [System.NonSerialized] public string beatId;      // 비트 식별자
    
    // 오브젝트 초기화
    public void Initialize(float targetBeatTime)
    {
        spawnTime = Time.time;
        targetTime = targetBeatTime;
        beatId = targetBeatTime.ToString("F3");
    }

    public void Edit(float spawnTime, float targetBeatTime)
    {
        this.spawnTime = spawnTime;
        this.targetTime = targetBeatTime;
        this.beatId = targetBeatTime.ToString("F3");
    }
}