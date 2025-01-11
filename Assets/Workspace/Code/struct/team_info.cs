
namespace StructDef.TeamInfo {

public enum CampInfo {
    camp_red = 0,
    camp_blue = 1,
    camp_none = 2,
    camp_num = 3,
}

public struct TeamInfo {
    public CampInfo camp;
    public ushort id;
}

public struct IdentityId {
    public int id;
    public IdentityId(int id) {
        this.id = id;
    }

    public override string ToString() {
        return id.ToString();
    }
}

}