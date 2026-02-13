using UnityEngine;
using System.Collections.Generic;

public class BattleCharacter : MonoBehaviour, IBattleEntity
{
    [Header("Character Base")]
    public HeroData heroData;
    public MonsterData monsterData;
    public bool isHero;

    [Header("UI & Visual")]
    private CharacterUI _characterUI;

    private List<BattleEffect> _activeEffects = new List<BattleEffect>();

    // 현제 체력 추적
    private int _currHp;

    // 현제 스트레스 추적
    private int _currStress;

    // 기본 명중 보정치 = 5%
    private const float HIDDEN_ACC = 0.05f;

    // 인터페이스 IBattleEntity 필드
    public string Name => isHero ? heroData.name : monsterData.name;
    public List<MonsterType> EntityTypes => isHero ?
                                    new List<MonsterType> { MonsterType.Human } :
                                    monsterData.MonsterTypes;
    public int CurrHp => _currHp;
    public int MaxHp => isHero ? heroData.MaxHp : monsterData.MaxHp;
    public float Dodge => isHero ? heroData.Dodge : monsterData.Dodge;
    public float Protection => isHero ? heroData.Protection : monsterData.Protection;
    public float Critical => isHero ? heroData.Critical : 0;    // 몬스터는 스킬에서 치명타 확률을 읽어옴
    public int CurrStress => _currStress;
    public int Speed => isHero ? heroData.Speed : monsterData.Speed;
    public float StunResist => isHero ? heroData.StunResist : monsterData.StunResist;
    public float BlightResist => isHero ? heroData.BlightResist : monsterData.BlightResist;
    public float DiseaseResist => isHero ? heroData.DiseaseResist : 0;  // 몬스터는 질병 X
    public float DeathblowResist => isHero ? heroData.DeathblowResist : 0;  // 몬스터는 죽음의 문턱 굴리지 않음
    public float MoveResist => isHero ? heroData.MoveResist : monsterData.MoveResist;
    public float BleedResist => isHero ? heroData.BleedResist : monsterData.BleedResist;
    public float DebuffResist => isHero ? heroData.DebuffResist : monsterData.DebuffResist;
    public float TrapDisable => isHero ? heroData.TrapDisable : 0;  // 몬스터는 트랩 해제 X

    public void TakeDamage(int dmg)
    {
        float reducedDmg = dmg * (1f - Protection);
        _currHp -= Mathf.RoundToInt(reducedDmg);

        if(_currHp <= 0 )
        {
            _currHp = Mathf.Clamp(_currHp, 0, MaxHp);
            // 사망 판정
            if (IsDead())
            {
                Debug.Log($"{Name}이 쓰러졌습니다.");
                Destroy(gameObject);
            }
        }

        _characterUI.UpdateHealth(_currHp);
    }

    public void HeartAttack()
    {
        if (!isHero) return;

        if(_currHp <= 0)
        {
            // 사망처리
            Debug.Log($"{Name}이 쓰러졌습니다.");
            Destroy(gameObject);
        }
        else
        {
            _currHp = 0;
        }
    }

    public bool IsDead()
    {
        // 영웅일 경우
        if (isHero)
        {
            // 죽음의 문턱 확률 시행
            int dice = Random.Range(1, 101);
            if (dice <= heroData.DeathblowResist)
                return false;
            else
                return true;
        }

        // 몬스터일 경우
        if(_currHp <= 0)
            return true;
        else 
            return false;
    }

    /// <summary>
    /// target에게 보정치가 합산된 dmg 계산 후 TakeDamage(dmg)를 호출
    /// </summary>
    /// <param name="target">공격할 대상</param>
    public void Attack(IBattleEntity target, HeroSkillData skill)
    {
        // 명중 버프 보정치 합산
        float accMod = GetTotalEffModifier(EffectType.Accuracy);
        float targetDodgeMod = target.GetTotalEffModifier(EffectType.Dodge);    // 타겟의 Dodge 보정치

        // 기본 능력치에서 min ~ max dmg 불러옴
        int baseDmg = Random.Range(heroData.MinDamage, heroData.MaxDamage);

        // 보정치 계산
        float dmgEffMod = GetTotalEffModifier(EffectType.Damage);     // 버프로 받은 데미지 보정치

        // 스킬에 특정 종족에 따른 추가 대미지 보정치가 있는 경우, dmgEffMod에 추가
        foreach (var cond in skill.conditionalMods)
        {
            if (target.HasType(cond.targetType))
            {
                dmgEffMod += cond.damageMod;
            }
        }

        // 최종 데미지 보정치
        float finalModDmg = baseDmg * (1f + dmgEffMod);

        // 명중률 계산

        // 타겟(몬스터)의 회피율
        float finalTargetDodge = target.Dodge + targetDodgeMod;

        int finalAcc = Mathf.RoundToInt(100 * (skill.Accuracy + heroData.Correction + accMod + HIDDEN_ACC - finalTargetDodge));
        finalAcc = Mathf.Clamp(finalAcc, 5, 100);

        // 명중 성공시 target의 피격 함수 부르기, 실패시 회피 판정
        int dice = Random.Range(1, 101);

        if (dice <= finalAcc)    // 명중 성공
        {
            target.TakeDamage(Mathf.RoundToInt(finalModDmg));


            //// 스킬에 붙은 상태 이상 부여, 상태 이상은 우선 보류
            //foreach(var eff in skill.effects)
            //{
            //    float effResist = GetTargetResist(target, eff.type);

            //    float effDice = eff.chance - effResist;

            //    if(effDice > 0 && Random.Range(0f, 1f) <= effDice)      // 상태 이상 저항 실패
            //        target.AddEffect(new BattleEffect(eff.type, eff.value, eff.duration));
            //    // TODO: 상태 이상 저항 성공시 로직 필요

            //}

        }
        else                    // 명중 실패
        {
            // 명중 실패 로직
            // "회피" 텍스트 띄우기
            Debug.Log($"{target.Name}이 {skill.name}을 회피했습니다.");
        }
    }

    /// <summary>
    /// target에게 보정치가 합산된 dmg 계산 후 TakeDamage(dmg)를 호출
    /// </summary>
    /// <param name="target">공격할 대상</param>
    public void Attack(IBattleEntity target, MonsterSkillData skill)
    {
        // 명중 버프 보정치 합산
        float accMod = GetTotalEffModifier(EffectType.Accuracy);
        float targetDodgeMod = target.GetTotalEffModifier(EffectType.Dodge);    // 타겟의 Dodge 보정치

        // 사용할 스킬에서 min ~ max dmg 불러옴
        int baseDmg = Random.Range(skill.MinDmg, skill.MaxDmg + 1);

        // 보정치 계산
        float dmgEffMod = GetTotalEffModifier(EffectType.Damage);     // 버프로 받은 데미지 보정치
        float modDmg = baseDmg * (1 + dmgEffMod);

        // 명중률 계산
        float finalTargetDodge = target.Dodge + targetDodgeMod;
        int finalAcc = Mathf.RoundToInt(100 * (skill.Accuracy + accMod + HIDDEN_ACC - finalTargetDodge));
        finalAcc = Mathf.Clamp(finalAcc, 5, 100);

        // 명중 성공시 target의 피격 함수 부르기, 실패시 회피 판정
        int dice = Random.Range(1, 101);

        if (dice <= finalAcc)    // 명중 성공
        {
            target.TakeDamage(Mathf.RoundToInt(modDmg));


            //// 스킬에 붙은 상태 이상 부여, 상태 이상은 일단 보류
            //foreach (var eff in skill.effects)
            //{
            //    float effResist = GetTargetResist(target, eff.type);

            //    float effDice = eff.chance - effResist;

            //    if (effDice > 0 && Random.Range(0f, 1f) <= effDice)      // 상태 이상 저항 실패
            //        target.AddEffect(new BattleEffect(eff.type, eff.value, eff.duration));
            //    // TODO: 상태 이상 저항 성공시 로직 필요

            //}
        }
        else                    // 명중 실패
        {
            // 명중 실패 로직
            // "회피" 텍스트 띄우기
            Debug.Log($"{target.Name}이 {skill.name}을 회피했습니다.");
        }
    }

    /// <summary>
    /// 영웅의 skill 데이터의 상태 이상을 target에게 부여
    /// </summary>
    /// <param name="skill">상태 이상을 부여할 스킬</param>
    /// <param name="target">상태 이상을 부여 받을 타겟</param>
    public void ApplyEffect(HeroSkillData skill, IBattleEntity target)
    {
        foreach(var eff in skill.effects)
        {
            // 확률 주사위 굴림 (최종 확률 = 스킬 확률 + 확률 보정치(버프 등) - 대상 저항력)
            float finalChance = eff.chance - GetTargetResist(target, eff.type);

            if(Random.Range(0f, 100f) <= finalChance)
            {
                target.AddEffect(new BattleEffect(eff.type, eff.value, eff.duration));
            }
        }
    }

    /// <summary>
    /// target과 상태 이상 eff를 받고 상태 이상 eff의 저항력을 반환함
    /// </summary>
    /// <param name="target">상태 이상 저항력을 받아올 대상</param>
    /// <param name="eff">상태 이상의 유형</param>
    /// <returns>상태 이상의 저항력 반환</returns>
    public float GetTargetResist(IBattleEntity target, EffectType eff)
    {
        if (eff == EffectType.Stun)         return target.StunResist;
        else if (eff == EffectType.Blight)  return target.BlightResist;
        else if (eff == EffectType.Disease) return target.DiseaseResist;
        else if (eff == EffectType.Move)    return target.MoveResist;
        else if (eff == EffectType.Bleed)   return target.BleedResist;
        else if (eff == EffectType.Debuff)  return target.DebuffResist;
        else return 0;
    }

    public float GetTotalEffModifier(EffectType type)
    {
        float total = 0;
        foreach(var eff in _activeEffects)
        {
            if (eff.Type == type)
                total += eff.Value;
        }
        return total;
    }

    public void AddEffect(BattleEffect effect) => _activeEffects.Add(effect);

    public bool HasType(MonsterType type) => EntityTypes.Contains(type);

    /// <summary>
    /// 턴 시작시 모든 효과의 지속시간 1턴 감소시켜야 함
    /// </summary>
    public void OnTurnStart()
    {
        // 지속시간 1 감소, 0이 되면 제거
        for(int i = _activeEffects.Count - 1; i >= 0; i--)
        {
            _activeEffects[i].Duration--;
            if(_activeEffects[i].Duration <= 0)
            {
                _activeEffects.RemoveAt(i);
            }
        }
    }

    void Start()
    { 
        _currStress = 0;

        Initialize();
    }

    private void Initialize()
    {
        // 위치 조정
        _characterUI = GetComponentInChildren<CharacterUI>();

        if(_characterUI == null)
        {
            Debug.LogError($"{gameObject.name} 하위에 CharacterUI가 없습니다.");
            return;
        }
        
        // 데이터 주입
        if(isHero && heroData != null)
        {
            _characterUI.SetHealthBar(_currHp, heroData.MaxHp);
            _characterUI.SetStressBar(_currStress);
        }
        else if(!isHero && monsterData != null)
        {
            _characterUI.SetHealthBar(_currHp, monsterData.MaxHp);
        }
    }

}
