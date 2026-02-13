using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    private class TurnOrderInfo
    {
        public BattleCharacter Character;
        public int FinalSpeed;
        
        public TurnOrderInfo(BattleCharacter character)
        {
            Character = character;
            FinalSpeed = character.Speed + Random.Range(1, DICE + 1);
        }
    }

    [Header("턴을 표시할 텍스트")]
    public TMP_Text turnNumber;

    // 현제 게임 상에 존재하는 캐릭터(엔티티)들의 리스트
    [SerializeField] private List<BattleCharacter> entityList = new List<BattleCharacter>();

    [SerializeField] private List<BattleCharacter> orderList = new List<BattleCharacter>();

    private int turn = 0;   // 턴 수를 기록
    private const int DICE = 8;     // 속도 주사위는 "1d8"

    /// <summary>
    /// 부를때마다 턴 수 증가, 턴을 나타내는 텍스트에 증가된 턴을 반영
    /// </summary>
    public void SetTurnText()
    {
        turnNumber.text = (++turn).ToString();
    }

    /// <summary>
    /// 처음 전투 시작 전 현제 아군/적의 리스트를 가져옴
    /// </summary>
    public void InitEntityList()
    {
        // 우선 리스트 초기화
        entityList.Clear();

        // 아군 찾기
        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");
        foreach (GameObject go in heroes)
        {
            BattleCharacter character = go.GetComponent<BattleCharacter>();
            if (character != null)
            {
                entityList.Add(character);
            }
        }

        // 적군 찾기
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject go in enemies)
        {
            BattleCharacter character = go.GetComponent<BattleCharacter>();
            if (character != null)
            {
                entityList.Add(character);
            }
        }

        Debug.Log($"전투 개시 | 총 {entityList.Count}명 감지");
    }

    /// <summary>
    /// 현제 entityList에 있는 캐릭터들의 순서를 속도 + 1d8 계산을 통해 내림차순으로 나열
    /// </summary>
    public void CalculateOrder()
    {
        List<TurnOrderInfo> calc = new List<TurnOrderInfo>();

        foreach(var entity in entityList)
            calc.Add(new TurnOrderInfo(entity));

        calc.Sort((a, b) =>
        {
            if (a.FinalSpeed != b.FinalSpeed)
                return b.FinalSpeed.CompareTo(a.FinalSpeed);

            // 속도가 같을 경우 동전 던지기 실행
            return (Random.value > 0.5f) ? -1 : 1;
        });

        orderList.Clear();      // 기존 순서 초기화
        Debug.Log("--이번 라운드 턴 순서--");
        foreach(var c in calc)
        {
            orderList.Add(c.Character);
            Debug.Log($"[{c.Character.Name}] 속도합계: {c.FinalSpeed}");
        }
    }

    private void Start()
    {
        InitEntityList();

        CalculateOrder();
    }
}
