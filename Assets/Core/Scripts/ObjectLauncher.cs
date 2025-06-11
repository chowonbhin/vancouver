using UnityEngine;

// 모든 오브젝트 발사 클래스가 구현해야 하는 인터페이스
public interface IObjectLauncher
{
    void LaunchObject(float targetTime);
}

// 이 컴포넌트를 각 씬의 발사기 오브젝트에 추가
[RequireComponent(typeof(MonoBehaviour))]
public class ObjectLauncher : MonoBehaviour
{
    private MonoBehaviour launcher;  // 실제 발사 기능이 있는 컴포넌트
    [SerializeField] private string launchMethodName = "ThrowBall";  // 호출할 메서드 이름
    [SerializeField] private float objectTravelTime = 0.5f;  // 오브젝트 이동 시간
    
    private void Awake()
    {
        // 컴포넌트 캐싱
        launcher = GetComponent<MonoBehaviour>();
    }
    
    // 리듬 매니저가 호출하는 메서드
    public GameObject Launch(float beatTime)
    {
        Debug.Log($"ObjectLauncher.Launch({beatTime}) 호출됨");
        
        if (launcher == null)
        {
            Debug.LogError("발사기 컴포넌트가 null입니다.");
            return null;
        }
        
        GameObject launchedObject = null;
        
        // 리플렉션으로 발사 메서드 호출
        var method = launcher.GetType().GetMethod(launchMethodName);
        if (method != null)
        {
            Debug.Log($"메서드 '{launchMethodName}' 찾음, 호출 시도");
            
            try
            {
                var parameters = method.GetParameters();
                object result = null;
                
                if (parameters.Length == 0)
                {
                    result = method.Invoke(launcher, null);
                    Debug.Log($"'{launchMethodName}' 메서드를 파라미터 없이 호출 성공");
                }
                else if (parameters.Length == 1 && parameters[0].ParameterType == typeof(float))
                {
                    result = method.Invoke(launcher, new object[] { objectTravelTime });
                    Debug.Log($"'{launchMethodName}' 메서드를 파라미터 {objectTravelTime}으로 호출 성공");
                }
                else if (parameters.Length == 2)
                {
                    result = method.Invoke(launcher, new object[] { objectTravelTime, beatTime });
                    Debug.Log($"'{launchMethodName}' 메서드를 파라미터 {objectTravelTime} {beatTime} 으로 호출 성공");
                }
                
                // 반환값이 GameObject인 경우 반환
                if (result is GameObject)
                {
                    launchedObject = result as GameObject;
                }
                else
                {
                    // 발사 메서드가 GameObject를 반환하지 않는 경우
                    // 발사 추적 기능 사용
                    launchedObject = TrackLastLaunchedObject();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"메서드 호출 중 오류 발생: {e.Message}\n{e.StackTrace}");
            }
        }
        else
        {
            Debug.LogError($"'{launchMethodName}' 메서드를 찾을 수 없습니다. 올바른 메서드 이름인지 확인하세요.");
        }
        
        // 발사된 오브젝트에 RhythmData 추가
        if (launchedObject != null)
        {
            RhythmData rhythmData = launchedObject.AddComponent<RhythmData>();
            rhythmData.Initialize(beatTime);
            Debug.Log($"리듬 데이터 추가: 오브젝트 '{launchedObject.name}', 타겟시간 {beatTime}");
        }
        
        return launchedObject;
    }

    // 최근 발사된 오브젝트 추적
    private GameObject TrackLastLaunchedObject()
    {
        // 적절한 태그로 검색 (씬에 맞게 조정 필요)
        string[] possibleTags = { "BaseBall", "PingpongBall", "PunchBag", "Bomb" };
        
        foreach (string tag in possibleTags)
        {
            if (string.IsNullOrEmpty(tag)) continue;
            
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
            if (objectsWithTag.Length > 0)
            {
                // 가장 최근에 생성된 오브젝트 반환 (간단한 가정)
                return objectsWithTag[objectsWithTag.Length - 1];
            }
        }
        
        return null;
    }

    
}