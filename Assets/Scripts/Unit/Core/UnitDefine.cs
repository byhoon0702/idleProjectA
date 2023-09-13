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
	Ability = 0,
	Equip = 1,
	Costume = 2,
	Pet = 3,
	Currency = 4,
	Skill = 5,
	EXP = 6,
	RewardBox = 7,
	Relic = 8,
	Persistent = 9,
	Event_Currency = 10,
}
