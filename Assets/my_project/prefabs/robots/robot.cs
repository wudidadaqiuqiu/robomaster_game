using UniRx;
using UnityEngine;

namespace Robots
{
    

using StructDef.TeamInfo;
public class Robot : MonoBehaviour
{
    public Subject<object> RobotSubject { get; private set; }

    private void Awake()
    {
        // 初始化 Subject
        RobotSubject = new Subject<object>();

        // 将 Subject 传递给子对象或子组件
        foreach (var component in GetComponentsInChildren<InterfaceDef.IRobotComponent>())
        {
            component.SetSubject(RobotSubject);
        }
    }

    private void Start()
    {
        // RobotSubject.OnNext(new team_info());
    }

    private void OnDestroy()
    {
        // 销毁时释放 Subject，防止内存泄漏
        RobotSubject.OnCompleted();
        RobotSubject.Dispose();
    }
}

}