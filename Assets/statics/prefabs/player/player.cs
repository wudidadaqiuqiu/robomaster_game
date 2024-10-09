using UniRx;
using UnityEngine;

namespace PlayerNameSpace
{
    

using StructDef.TeamInfo;
public class Player : MonoBehaviour
{
    public Subject<object> PlayerSubject { get; private set; }

    private void Awake()
    {
        // 初始化 Subject
        PlayerSubject = new Subject<object>();

        // 将 Subject 传递给子对象或子组件
        foreach (var component in GetComponentsInChildren<InterfaceDef.IPlayerComponent>())
        {
            component.SetSubject(PlayerSubject);
        }
    }

    private void Start()
    {
        PlayerSubject.OnNext(new team_info());
    }

    private void OnDestroy()
    {
        // 销毁时释放 Subject，防止内存泄漏
        PlayerSubject.OnCompleted();
        PlayerSubject.Dispose();
    }
}

}