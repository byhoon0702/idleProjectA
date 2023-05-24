using UnityEngine;

public static class PathHelper
{
	public const string projectileBehaviorPath = "ProjectileBehaviors";
	public const string skillEffectObjectSOPath = "SkillEffectSO";
	public const string skillAbilitySOPath = "SkillAbilitySO";
	public const string applyDamageBehaviorPath = "DamageAffectSO";

	public const string iconPath = "Icon";
	public const string projectilePath = "Projectile";
	public const string hitEffectPath = "HitEffect";
	public const string spawnEffectPath = "SpawnEffect";
	public const string particleEffectPath = "ParticleEffect";

	public const string hyperCasualFXPath = "Hyper Casual FX";

	public const string jsonDataPath = "Data/Json";
	public const string csvDataPath = "Data/Csv";


	public static string AssetFolder(this string s)
	{
		return $"AssetFolder/{s}";
	}
	public static string Resources(this string s)
	{
		return $"Resources/{s}";
	}

	public static string Icon(this string s)
	{
		return $"Icon/{s}";
	}

	public static string Assets(this string s)
	{
		return $"Assets/{s}";
	}

	public static string DataPath(this string s)
	{
		return $"{Application.dataPath}/{s}";
	}

}
