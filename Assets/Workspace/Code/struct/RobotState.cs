using Unity.Netcode;

namespace Robot
{
    #region 数据结构
    public enum RobotVisionMode
    {
        first_person = 0,
        third_person = 1,
        second_person = 114514,
    };

    public enum RobotShootMode : byte
    {
        None,
        Normal,
    };

    public enum RobotGroup
    {
        Blue = 0,
        Red = 1,
    };

    // 暂时只做有血量的，其他无血量的可以单独编写逻辑
    public enum RobotType
    {
        Hero = 1,
        Engineer = 2,
        Infantry1 = 3,
        Infantry2 = 4,
        Infantry3 = 5,
        Sentry = 6,
    };


    [System.Serializable]
    public struct RobotInfoDynamic : INetworkSerializable
    {
        public float HP;
        public float heat;
        public float power;

        public int grade;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref HP);
            serializer.SerializeValue(ref heat);
            serializer.SerializeValue(ref power);
            serializer.SerializeValue(ref grade);
        }
    };

    public struct RobotInfoFixed
    {
        int max_HP;
        int max_heat;
        int max_power;

        int bullet_num;
    }
    #endregion

    public class RobotState
    {
        public RobotVisionMode vision_mode;
        public RobotShootMode shoot_mode;
        public RobotGroup group;
        public RobotType type;

        public RobotInfoDynamic info_dynamic;
        public RobotInfoFixed info_fixed;

        // 暂时固定，后面会和功率联合
        public float velocity = 5.0f;

        public RobotState(RobotType _type, RobotGroup _group)
        {
            type = _type;
            group = _group;
            init_info();
        }

        public void init_info()
        {
            switch (type)
            {
                case RobotType.Hero:
                    break;
                case RobotType.Engineer:
                    break;
                case RobotType.Infantry1:
                case RobotType.Infantry2:
                case RobotType.Infantry3:
                    break;
                case RobotType.Sentry:
                    break;
            }
        }
        public void upgrade()
        {

        }
    };
};
