public enum EffectType 
{
    // 기본 능력치 긍정적 버프(강화) 또는 디버프(약화)
    Accuracy,
    Dodge,
    Protection,
    Damage,
    DamageMod,      // 특정 스킬의 데미지 보정치를 줌    
    Critical,
    Speed,
    Stress,  // 음수일 시 스트레스 힐, 양수일 시 스트레스 데미지
    StressMod,     // 받는 스트레스 증가 또는 감소

    // 효과 부여 기술 성공률
    StunChance,
    BlightChance,
    MoveChance,
    BleedChance,
    DebuffChance,
    DisableTrapChance,
    
    // 수치만큼 내성 강화 또는 약화
    StunResist,
    BlightResist,
    DiseaseResist,
    DeathBlowResist,
    MoveResist,
    BleedResist,
    DebuffResist,
    DisableTrap,

    // 효과
    DeathDoor,      // 죽음의 문턱 상태
    Recovery,       // 죽음의 문턱 회생
    Affliction,     // 붕괴
    Virtue,         // 영웅의 기상
    Stun,
    Bleed,
    Blight,
    Move,
    SelfMove,       // 스스로 움직이는 효과, 이동 저항값을 무시함
    Disease,        // 질병
    Debuff,         // 디버프 속성 자체가 디버프인 경우
    Mark,           // 표식
    Guard,
    Riposte,        // 반격
    Restoration,    // 복구, 적용 턴 수가 1 이상인 경우 도트 힐
    Horror,
    Stealth,
    Transform,       // "변신" 상태 = 폼 변환
    Pierce,          // 보호 무시

    // 특수 효과
    HumanForm,      // 괴인의 "인간"상태
    BeastForm       // 괴인의 "짐승"상태
}

[System.Serializable]   // 직렬화
public class BattleEffect
{
    public EffectType Type;
    public int Value;               // 버프/디버프 위력
    public int Duration;            // 턴 수
    public string TargetSkillName;  // "대단원" 같은 스킬 이름 저장

    public BattleEffect(EffectType type, int value, int duration)
    {
        Type = type;
        Value = value;
        Duration = duration;
    }

    public BattleEffect(EffectType type, int value, int duration, string targetSkillName)
    {
        Type = type;
        Value = value;
        Duration = duration;
        TargetSkillName = targetSkillName;
    }
}
