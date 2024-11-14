public enum PlayerSystem
{
    Move,
    Dash,
    Attack,
    Collect,
    Throw,
    Health
}
/// <summary>
/// Name of animation clips from "Unit" Controller in Attack Blend Tree
/// </summary>
public enum HitVariation
{
    None = -1,
    AttackMeleeRightSmash = 0,
    AttackMeleeRight = 1,
    AttackMeleeLeft = 2,
    AttackKickRight = 3,
    AttackKickLeft = 4,
}

public enum AttackType
{
    Melee,
    Around,
    Range
}