using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static RhythmManager Instance { get; private set; }
    
    [Header("오디오 설정")]
    [SerializeField] private AudioSource musicSource;
    
    [Header("디버그")]
    [SerializeField] private bool showDebugLogs = true;
    
    // 현재 씬 설정
    private BeatData currentBeatData;
    private float currentLeadTime = 0.5f;
    private ObjectLauncher currentLauncher;
    
    // 게임 상태
    private int nextBeatIndex = 0;
    private float gameStartTime;
    private bool isPlaying = false;
    
    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // 오디오 소스 확인
            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // 씬 등록 메서드 (SceneSetup에서 호출)
    public void RegisterScene(TextAsset beatMapJson, AudioClip musicClip, float leadTime, ObjectLauncher launcher)
    {
        Debug.Log("RhythmManager: RegisterScene 호출됨");
        
        // 비트맵 로드
        if (beatMapJson != null)
        {
            Debug.Log($"비트맵 '{beatMapJson.name}' 파싱 시작");
            currentBeatData = BeatData.FromMidiJson(beatMapJson);
            currentLeadTime = leadTime;
            
            Debug.Log($"비트맵 로드 완료. 비트 수: {(currentBeatData?.beatTimes?.Count ?? 0)}");
        }
        else
        {
            currentBeatData = null;
            Debug.LogError("비트맵이 설정되지 않았습니다.");
        }
        
        // 음악 설정
        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip;
            Debug.Log($"음악 '{musicClip.name}' 설정 완료");
        }
        else
        {
            Debug.LogError("음악 소스 또는 클립이 설정되지 않았습니다.");
        }
        
        // 발사기 설정
        currentLauncher = launcher;
        
        if (launcher == null)
        {
            Debug.LogError("오브젝트 발사기가 설정되지 않았습니다.");
        }
        else
        {
            Debug.Log("오브젝트 발사기 설정 완료");
        }
    }
    // 씬 해제 메서드
    public void UnregisterScene()
    {
        // 게임 진행 중이면 중지
        if (isPlaying)
        {
            StopGame();
        }
        
        // 설정 초기화
        currentBeatData = null;
        currentLauncher = null;
        
        if (musicSource != null)
        {
            musicSource.clip = null;
        }
    }
    
    // 카운트다운 후 게임 시작
    public void StartGameWithCountdown(float countdownDuration = 5f)
    {
        StartCoroutine(CountdownAndStart(countdownDuration));
    }
    
    private IEnumerator CountdownAndStart(float duration)
    {
        if (showDebugLogs)
            Debug.Log($"{duration}초 카운트다운을 시작합니다.");
        
        // UI 매니저가 있다면 여기서 카운트다운 표시
        
        yield return new WaitForSeconds(duration);
        
        StartGame();
    }
    
    // 게임 즉시 시작
    public void StartGame()
    {
        Debug.Log("RhythmManager: StartGame 호출됨");
        
        if (currentBeatData == null || currentBeatData.beatTimes == null || currentBeatData.beatTimes.Count == 0)
        {
            Debug.LogError("비트맵이 없거나 비트 데이터가 비어 있어 게임을 시작할 수 없습니다.");
            return;
        }
        
        if (currentLauncher == null)
        {
            Debug.LogError("오브젝트 발사기가 없어 게임을 시작할 수 없습니다.");
            return;
        }
        
        // 게임 상태 초기화
        gameStartTime = Time.time;
        nextBeatIndex = 0;
        isPlaying = true;
        
        // 음악 재생
        if (musicSource != null && musicSource.clip != null)
        {
            musicSource.Play();
            Debug.Log("음악 재생 시작");
        }
        else
        {
            Debug.LogError("음악이 설정되지 않아 재생할 수 없습니다.");
        }
        
        // 비트 처리 코루틴 시작
        StartCoroutine(ProcessBeats());
        Debug.Log("리듬 게임을 시작합니다.");
    }
    
    // 비트 처리 코루틴
    private IEnumerator ProcessBeats()
    {
        Debug.Log("비트 처리 코루틴 시작");
        
        yield return new WaitForSeconds(0.5f); // 약간의 지연
        
        Debug.Log($"처리할 총 비트 수: {currentBeatData.beatTimes.Count}, 리드 타임: {currentLeadTime}");
        
        int processedBeats = 0;
        
        while (isPlaying && nextBeatIndex < currentBeatData.beatTimes.Count)
        {
            // 현재 음악 경과 시간
            float currentMusicTime = Time.time - gameStartTime;
            
            // 발사 타이밍 계산: 비트 시간 - 리드 타임 ≤ 현재 시간
            while (nextBeatIndex < currentBeatData.beatTimes.Count && 
                currentBeatData.beatTimes[nextBeatIndex] - currentLeadTime <= currentMusicTime)
            {
                float beatTime = currentBeatData.beatTimes[nextBeatIndex];
                
                Debug.Log($"비트 {nextBeatIndex} 처리 중: 비트 시간 {beatTime:F2}초, 현재 음악 시간 {currentMusicTime:F2}초");
                
                // 발사기를 통해 오브젝트 발사
                if (currentLauncher != null)
                {
                    Debug.Log($"비트 {nextBeatIndex}: 오브젝트 발사 시도");
                    currentLauncher.Launch(beatTime);
                    processedBeats++;
                }
                else
                {
                    Debug.LogError("발사기가 null이 되었습니다.");
                }
                
                nextBeatIndex++;
            }
            
            // 음악이 끝났는지 확인
            if (musicSource != null && !musicSource.isPlaying && isPlaying)
            {
                Debug.Log("음악이 끝났습니다.");
                StopGame();
            }
            
            yield return null;
        }
        
        Debug.Log($"비트 처리 완료: {processedBeats}개의 비트 처리됨");
        
        // 모든 비트 처리 완료
        if (isPlaying)
        {
            Debug.Log("모든 비트를 처리했습니다.");
            StopGame();
        }
    }
    
    // 게임 중지
    public void StopGame()
    {
        isPlaying = false;
        
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
        
        if (showDebugLogs)
            Debug.Log("리듬 게임을 중지합니다.");
    }
}