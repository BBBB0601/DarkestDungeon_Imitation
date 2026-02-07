using UnityEngine;

public enum SkillType
{
    Melee,
    Ranged,
    Buff,
    Heal
}

[CreateAssetMenu(fileName = "NewSkill", menuName = "Data/Skill")]
public class SkillData : ScriptableObject
{
    [SerializeField] private string skillName;
    public string Name => skillName;

    [SerializeField] private SkillType type;
    public SkillType Type => type;

    [Header("스킬 사용 위치 | 대상 위치")]
    public bool[] usableRanks = new bool[4];
    public bool isUsableChained;
    public bool[] targetRanks = new bool[4];
    public bool isTargetChained;

    [Header("스킬 능력치")]
    public int accuracy;    // 명중률
    public int damageMod;   // dmg 보정
    public float critMod;   // crit 보정

    [Tooltip("몬스터의 경우 스킬 대미지 범위를 따라감")]
    public bool useFixedDamage;
    public int minDamage;
    public int maxDamage;

    public string effect;   // 특이사항
}