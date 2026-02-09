using UnityEngine;

[CreateAssetMenu(fileName = "NewMonsterSkill", menuName = "Data/Skill")]
public class MonsterSkillData : ScriptableObject
{
    [SerializeField] private string _skillName;                     // 스킬 이름
    public string SkillName => _skillName;

    [SerializeField] private bool[] _targetRanks = new bool[4];     // 타겟 위치
    public bool[] TargetRanks => _targetRanks;      

    [SerializeField] private bool _isTargetChained;                 // 타겟 연결됨 여부
    public bool IsTargetChained => _isTargetChained;

    [SerializeField] private bool[] _usableRanks = new bool[4];     // 사용 가능 위치
    public bool[] UsableRanks => _usableRanks;

    [SerializeField] private bool _isUsableChained;                 // 아군 타겟 연결됨 여부
    public bool IsUsableChained => _isUsableChained;

    [SerializeField] private float _accuracy;                       // 정확도
    public float Accuracy => _accuracy;

    [SerializeField] private float _crit;                           // 치명타 확률
    public float Crit => _crit;

    [SerializeField] private int _minDmg;                           // 최소 대미지
    public int MinDmg => _minDmg;

    [SerializeField] private int _maxDmg;                           // 최대 대미지
    public int MaxDmg => _maxDmg;

    public string note;                 // 특이 사항


    public void Initialize(string name, bool[] targetRanks, bool isTargetChained,
                           bool[] usableRanks, bool isUsableChained, float accuracy,
                           float crit, int minDmg, int maxDmg, string note)
    {
        // 스킬 기본 능력치
        _skillName = name;
        _targetRanks = targetRanks;
        _isTargetChained = isTargetChained;
        _usableRanks = usableRanks;
        _isUsableChained = isUsableChained;
        _accuracy = accuracy;
        _crit = crit;
        _minDmg = minDmg;
        _maxDmg = maxDmg;

        // 특이 사항
        this.note = note;
    }
}
