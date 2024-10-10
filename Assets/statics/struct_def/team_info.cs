
namespace StructDef.TeamInfo {

public enum camp_info {
    camp_none = 0,
    camp_red = 1,
    camp_blue = 2,
    camp_num = 3,
}

public struct team_info {
    public camp_info camp;
    public ushort id;
}

}