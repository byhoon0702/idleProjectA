public enum AttackType
{
	MELEE = 1,
	RANGED,
	SKILL,
	NOATTACK,
}

public enum AttackRange
{
	MELEE,
	RANGED,
	SUMMON,
}


public enum ClassType
{
	WARRIOR = 1,
	ARCHER,
	WIZARD,
	REWARDGEM,
	WALL,
}

[System.Flags]
public enum StateType
{
	NONE = 0,
	IDLE = 1 << 1,
	MOVE = 1 << 2,
	ATTACK = 1 << 3,
	HIT = 1 << 4,
	SKILL = 1 << 5,
	DEATH = 1 << 6,
	NEUTRALIZE = 1 << 7,
	DASH = 1 << 8,
	KNOCKBACK = 1 << 9,
	HYPER_FINISH = 1 << 15,
}

public enum ControlSide
{
	PLAYER = 1,
	ENEMY,
	NO_CONTROL,
}

public enum Targeting
{
	OPPONENT = 1,
	WALL,
}


public enum RewardCategory
{
	Ability,
	Equip,
	Costume,
	Pet,
	Currency,
	Skill,
	EXP,
	RewardBox,
	Relic,
	Persistent,
}
