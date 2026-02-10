public enum EffectType 
{
    // 기본 능력치 긍정적 버프(강화) 또는 디버프(약화)
    Accuracy,
    Dodge,
    Protection,
    Damage,
    Critical,
    Speed,
    Stress,     // 받는 스트레스 증가 또는 감소

    // 효과 부여 기술 성공률
    StunChance,
    BlightChance,
    MoveChance,
    BleedChance,
    DebuffChance,
    DisableTrapChance,
    
    // 내성
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
    Disease,        // 질병
    Debuff,         // 디버프 속성 자체가 디버프인 경우
    Mark,           // 표식
    Guard,
    Riposte,        // 반격
    Restoration,    // 복구 = 도트 힐
    Horror,
    Stealth,
    Transform       // "변신" 상태 = 폼 변환
}

[System.Serializable]   // 직렬화
public class BattleEffect
{
    public EffectType Type;
    public int Value;       // 버프/디버프 위력
    public int Duration;    // 턴 수

    public BattleEffect(EffectType type, int value, int duration)
    {
        Type = type;
        Value = value;
        Duration = duration;
    }
}
