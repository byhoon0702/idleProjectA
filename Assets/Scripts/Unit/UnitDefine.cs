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

public enum StateType
{
	NONE,
	IDLE = 1,
	MOVE,
	ATTACK,
	HIT,
	SKILL,
	DEATH,
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
