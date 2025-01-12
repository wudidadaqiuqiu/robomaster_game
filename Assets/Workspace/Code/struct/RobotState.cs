namespace robot
{
    #region 数据结构
    public enum RobotVisionMode
    {
        first_person = 0,
        third_person = 1,
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

    public struct Info
    {
        int max_HP;
        float HP;

        int max_heat;
        float heat;

        int grade;
        int index;
    };
    #endregion

    public class RobotState
    {
        public RobotVisionMode vision_mode;
        public RobotShootMode shoot_mode;
        public RobotGroup group;
        public RobotType type;
        public Info info;

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
