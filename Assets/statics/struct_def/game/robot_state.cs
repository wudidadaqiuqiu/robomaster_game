namespace StructDef.Game {

public enum RobotVisionMode {
    first_person = 0,
    third_person = 1,
}
public struct RobotState {
    public RobotVisionMode vision_mode;
    
}
}