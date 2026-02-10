using System.Collections.Generic;

public interface IBattleEntity
{
    string Name { get; }
    List<MonsterType> EntityTypes { get; }
    int MaxHp { get; }
    int CurrHp { get; }
    float Dodge { get; }
    float Protection { get; }
    float Critical { get; }
    int CurrStress { get; }
    int Speed { get; }
    float StunResist { get; }
    float BlightResist { get; }
    float DiseaseResist { get; }
    float DeathblowResist { get; }
    float MoveResist { get; }
    float BleedResist { get; }
    float DebuffResist { get; }
    float TrapDisable { get; }

    const int MAX_STRESS = 200;

    /// <summary> 
    /// dmg를 받아 자신이 받을 대미지 계산 후 체력에서 차감
    /// </summary>
    /// <param name="dmg">dmg는 보정치까지 전부 합산된 dmg여야 함</param>
    void TakeDamage(int dmg);

    /// <summary>
    /// "영웅일 때" 스트레스가 MAX_STRESS와 동등해질 경우, 심장마비 이벤트 발동
    /// </summary>
    void HeartAttack();

    /// <summary>
    /// 영웅일 때 체력이 없으면 죽음의 문턱 확률을 굴리고, 적일 경우 체력이 없으면 사망처리 
    /// </summary>
    /// <returns>사망 판정에 따른 사망 유무 반환</returns>
    bool IsDead();

    /// <summary>
    /// 매개변수로 받은 effect 상태 이상을 부여함
    /// </summary>
    /// <param name="effect">부여할 상태 이상</param>
    void AddEffect(BattleEffect effect);

    /// <summary>
    /// 특정 능력치 type의 총 보정치 또는 위력을 반환
    /// </summary>
    /// <param name="type">위력 또는 보정치를 알아낼 효과(Effect)</param>
    /// <returns>총 보정치 또는 위력 반환</returns>
    int GetTotalEffModifier(EffectType type);

    /// <summary>
    /// 특정 type 종족인지 확인
    /// </summary>
    /// <param name="type">확인하고 싶은 종족 type</param>
    /// <returns>이 엔티티가 type과 같으면 true, 아니면 false 반환</returns>
    bool HasType(MonsterType type);
}
