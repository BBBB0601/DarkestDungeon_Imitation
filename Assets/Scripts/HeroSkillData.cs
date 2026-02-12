using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Melee,
    Ranged,
    Buff,
    Heal
}

[CreateAssetMenu(fileName = "NewHeroSkill", menuName = "Data/Skill")]
public class HeroSkillData : ScriptableObject
{
    [SerializeField] private string _skillName;                  // 스킬 이름
    public string Name => _skillName;

    [SerializeField] private SkillType _type;                    // 스킬 유형
    public SkillType Type => _type;

    [System.Serializable]
    public struct SkillEffectData
    {
        public EffectType type; // 출혈, 중독, 스턴, 버프
        public float chance;    // 상태 부여 확률
        public int value;       // 위력 또는 수치
        public int duration;    // 지속 시간
    }
    
    // 상태 이상 리스트
    public List<SkillEffectData> effects = new List<SkillEffectData>();


    [System.Serializable]
    public struct ConditionalModifier
    {
        public MonsterType targetType; // 대상이 이 종족일 때
        public float damageMod;        // 데미지 보정치 (+0.25f 등)
    }

    // HeroSkillData에 추가
    public List<ConditionalModifier> conditionalMods = new List<ConditionalModifier>();

    [Header("스킬 사용 위치 | 대상 위치")]
    
    [SerializeField] private bool[] _usableRanks = new bool[4]; // 아군 스킬 사용 가능 위치
    public bool[] UsableRanks => _usableRanks;

    [SerializeField] private bool _isUsableChained;             // 아군 타겟 "연결됨"인지 확인
    public bool IsUsableChained => _isUsableChained;

    [SerializeField] private bool[] _targetRanks = new bool[4]; // 타겟 스킬 가능 범위
    public bool[] TargetRanks => _targetRanks;

    [SerializeField] private bool _isTargetChained;             // 적군 타겟 "연결됨"인지 확인
    public bool IsTargetChained => _isTargetChained;

    [Header("스킬 능력치")]
    
    [SerializeField] private float _accuracy;    // 명중률
    public float Accuracy => _accuracy;

    [SerializeField] private float _damageMod;   // dmg 보정
    public float DamageMod => _damageMod;
    
    [SerializeField] private float _critMod;   // crit 보정
    public float CritMod => _critMod;

    public string note;   // 특이사항

    public void Initialize(string skillName, SkillType skillType, bool[] usable,
                           bool isUsableChained, bool[] target, bool isTargetChained,
                           float acc, float dmg, float crit, string note)
    {
        // 영웅 | 몬스터 공통 적용
        _skillName = skillName;
        _type = skillType;
        _usableRanks = usable;
        _isUsableChained = isUsableChained;
        _targetRanks = target;
        _isTargetChained = isTargetChained;
        _accuracy = acc;
        _damageMod = dmg;
        _critMod = crit;

        // 특이 사항
        this.note = note;
    }
}
