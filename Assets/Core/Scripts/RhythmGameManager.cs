using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RhythmGameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static RhythmGameManager Instance { get; private set; }
    
    // 오디오 컴포넌트
    [SerializeField] private AudioSource musicSource;
    
    // 게임 설정
    [SerializeField] private float countdownDuration = 5f;
    
    // 씬별 음악 파일 설정
    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName;
        public string musicFileName;
    }
    
    [SerializeField] private List<SceneMusic> sceneMusicList = new List<SceneMusic>();
    
    // UI 참조
    [SerializeField] private UIManager uiManager;
    
    // 현재 씬 이름
    private string currentSceneName;
    
    private void Awake()
{
    Debug.Log("RhythmGameManager Awake 호출됨");
    
    // 싱글톤 패턴 구현
    if (Instance == null)
    {
        Debug.Log("RhythmGameManager 싱글톤 인스턴스 생성");
        Instance = this;
        
        // UI 컴포넌트가 자식으로 있다면 분리하여 DontDestroyOnLoad 적용
        if (uiManager != null && uiManager.transform.IsChildOf(transform))
        {
            // UI 캔버스를 임시로 분리
            Transform uiTransform = uiManager.transform;
            uiTransform.SetParent(null);
            
            // 현재 게임오브젝트 유지
            DontDestroyOnLoad(gameObject);
            
            // UI 캔버스를 다시 자식으로 설정
            uiTransform.SetParent(transform);
        }
        else
        {
            // UI가 자식이 아니면 바로 DontDestroyOnLoad 적용
            DontDestroyOnLoad(gameObject);
        }
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        // 필요한 컴포넌트 확인
        if (musicSource == null)
        {
            Debug.Log("AudioSource 컴포넌트 추가");
            musicSource = gameObject.AddComponent<AudioSource>();
        }
    }
    else
    {
        Debug.Log("RhythmGameManager 중복 인스턴스 제거");
        Destroy(gameObject);
        return;
    }
}
    
    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneName = scene.name;
        
        // 로비씬이 아닌 게임씬에서만 처리
        if (currentSceneName != "LobbyScene")
        {
            Debug.Log($"게임씬 '{currentSceneName}'이 로드되었습니다. 카운트다운을 시작합니다.");
            StartCoroutine(PrepareGameSession());
        }
        else
        {
            Debug.Log("로비씬이 로드되었습니다.");
        }
    }
    
    private IEnumerator PrepareGameSession()
    {
        // UI가 있으면 카운트다운 표시
        if (uiManager != null)
        {
            uiManager.ShowCountdown(countdownDuration);
        }
        
        // 카운트다운
        yield return new WaitForSeconds(countdownDuration);
        
        // 리듬 매니저를 통해 게임 시작
        if (RhythmManager.Instance != null)
        {
            // 리듬 매니저가 자동으로 게임을 시작함
            // (SceneSetup에서 씬이 로드될 때 자동 등록)
        }
        else
        {
            // 리듬 매니저가 없으면 기존 방식으로 음악 재생
            StartMusic();
        }
    }
    
    private void StartMusic()
    {
        // 현재 씬에 맞는 음악 파일 찾기
        string musicFileName = GetMusicFileForCurrentScene();
        
        if (string.IsNullOrEmpty(musicFileName))
        {
            Debug.LogWarning($"씬 '{currentSceneName}'에 대한 음악 파일이 설정되지 않았습니다.");
            return;
        }
        
        // 음악 파일 로드
        AudioClip musicClip = Resources.Load<AudioClip>($"Music/{musicFileName}");
        
        if (musicClip != null)
        {
            Debug.Log($"음악 '{musicFileName}'을 재생합니다.");
            musicSource.clip = musicClip;
            musicSource.Play();
        }
        else
        {
            Debug.LogError($"음악 파일을 찾을 수 없습니다: {musicFileName}");
        }
    }
    
    private string GetMusicFileForCurrentScene()
    {
        // 현재 씬에 해당하는 음악 파일명 찾기
        foreach (var sceneMusic in sceneMusicList)
        {
            if (sceneMusic.sceneName == currentSceneName)
            {
                return sceneMusic.musicFileName;
            }
        }
        
        // 기본 음악 파일명
        return "default_music";
    }
}