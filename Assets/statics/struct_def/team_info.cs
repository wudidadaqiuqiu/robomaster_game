
namespace StructDef.TeamInfo {

public enum CampInfo {
    camp_none = 0,
    camp_red = 1,
    camp_blue = 2,
    camp_num = 3,
}

public struct TeamInfo {
    public CampInfo camp;
    public ushort id;
}

}