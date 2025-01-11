using UniRx;

namespace InterfaceDef {
    public interface IRobotComponent
    {
        void SetSubject(Subject<object> subject);
    }
}