namespace StructDef.Game {

public enum RobotVisionMode {
    first_person = 0,
    third_person = 1,
}

public enum RobotShootMode {
    None,
    Normal,
}
public struct RobotState {
    public RobotVisionMode vision_mode;
    public RobotShootMode shoot_mode;
}
}