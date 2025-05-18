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
    public void Launch(float beatTime)
    {
        Debug.Log($"ObjectLauncher.Launch({beatTime}) 호출됨");
        
        if (launcher == null)
        {
            Debug.LogError("발사기 컴포넌트가 null입니다.");
            return;
        }
        
        // 리플렉션으로 발사 메서드 호출
        var method = launcher.GetType().GetMethod(launchMethodName);
        if (method != null)
        {
            Debug.Log($"메서드 '{launchMethodName}' 찾음, 호출 시도");
            
            try
            {
                var parameters = method.GetParameters();
                if (parameters.Length == 0)
                {
                    method.Invoke(launcher, null);
                    Debug.Log($"'{launchMethodName}' 메서드를 파라미터 없이 호출 성공");
                }
                else if (parameters.Length == 1 && parameters[0].ParameterType == typeof(float))
                {
                    method.Invoke(launcher, new object[] { objectTravelTime });
                    Debug.Log($"'{launchMethodName}' 메서드를 파라미터 {objectTravelTime}으로 호출 성공");
                }
                else
                {
                    Debug.LogError($"'{launchMethodName}' 메서드의 파라미터가 지원되지 않는 형식입니다.");
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
    }
}