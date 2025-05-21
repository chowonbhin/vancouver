using UnityEngine;
using UnityEngine.SceneManagement;

// 각 씬마다 하나씩 배치하는 컴포넌트
public class SceneSetup : MonoBehaviour
{
    [Header("리듬 게임 설정")]
    [SerializeField] private TextAsset beatMapJson;
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private float leadTime = 0.5f;  // 비트 전 오브젝트 발사 시간
    
    [Header("오브젝트 발사기")]
    [SerializeField] private ObjectLauncher objectLauncher;

    [Header("UI 참조")]
    [SerializeField] private JudgmentUI judgmentUI;
    
    private void Start()
    {
        Debug.Log($"SceneSetup.Start() 호출됨: {gameObject.scene.name} 씬");
        
        // RhythmManager 확인
        if (RhythmManager.Instance != null)
        {
            Debug.Log("RhythmManager 인스턴스 찾음");
            
            // 비트맵 로깅
            if (beatMapJson != null)
                Debug.Log($"비트맵: {beatMapJson.name}");
            else
                Debug.LogError("비트맵이 설정되지 않았습니다.");
            
            // 음악 로깅
            if (musicClip != null)
                Debug.Log($"음악: {musicClip.name}");
            else
                Debug.LogError("음악이 설정되지 않았습니다.");
            
            // 발사기 로깅
            if (objectLauncher != null)
                Debug.Log($"발사기 찾음: {objectLauncher.gameObject.name}");
            else
                Debug.LogError("발사기가 설정되지 않았습니다.");
            
            // 리듬 매니저에 등록
            RhythmManager.Instance.RegisterScene(beatMapJson, musicClip, leadTime, objectLauncher);
            Debug.Log("RhythmManager에 씬 등록 완료");
            
            // 직접 게임 시작 (디버그용)
            RhythmManager.Instance.StartGame();
        }
        else
        {
            Debug.LogError("RhythmManager 인스턴스를 찾을 수 없습니다.");
        }

        // JudgmentSystem에 현재 씬의 UI 연결
        if (JudgmentSystem.Instance != null && judgmentUI != null)
        {
            JudgmentSystem.Instance.SetJudgmentUI(judgmentUI);
            Debug.Log($"현재 씬 '{gameObject.scene.name}'의 JudgmentUI를 JudgmentSystem에 연결했습니다.");
        }

        if (JudgmentSystem.Instance != null && judgmentUI != null)
        {
            JudgmentSystem.Instance.SetJudgmentUI(judgmentUI);
            Debug.Log($"현재 씬 '{gameObject.scene.name}'의 JudgmentUI를 JudgmentSystem에 연결했습니다.");
            
            // VR 모드인지 확인하고 Canvas 설정
            bool isVRMode = UnityEngine.XR.XRSettings.enabled;
            if (isVRMode)
            {
                judgmentUI.PositionCanvasForVR();
            }
        }
    }

    
    
    private void OnDestroy()
    {
        // 씬이 언로드될 때 리듬 매니저에서 해제
        if (RhythmManager.Instance != null)
        {
            RhythmManager.Instance.UnregisterScene();
        }
    }
}