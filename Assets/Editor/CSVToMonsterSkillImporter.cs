using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
public class CSVToMonsterSkillImporter : MonoBehaviour
{
    [MenuItem("Tools/Import All Monster Skills")]
    public static void ImportSkills()
    {
        string path = "Assets/CSV/[DB]몬스터_전체_스킬_목록.csv";
        if (!File.Exists(path))
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다: " + path);
            return;
        }

        string csvText = File.ReadAllText(path);

        string[] lines = Regex.Split(csvText, @"\r\n|\r|\n(?=(?:[^""]*""[^""]*"")*[^""]*$)");

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))    continue;

            string[] data = Regex.Split(lines[i], ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
            if (data.Length < 9) continue;

            string sName = data[0].Trim('"');
            Debug.Log("이름 읽기 완료: " + sName);

            // 2. 위치 데이터 및 "연결됨" 여부 파싱전체_스킬_목록.csv
            bool[] target = ParseRanks(data[2]);
            bool targetChained = data[2].Contains("연결됨"); // "연결됨" 태그 추적
            Debug.Log("기술 타겟 위치 읽기 완료");

            bool[] usable = ParseRanks(data[3]);
            bool usableChained = data[3].Contains("연결됨"); // "연결됨" 태그 추적
            Debug.Log("기술 사용 가능 위치 읽기 완료");

            // 3. 수치 파싱전체_스킬_목록.csv
            int acc = ParseInt(data[4]);
            Debug.Log("명중률 읽기 완료");

            float crit = ParseFloat(data[5]);
            Debug.Log("치명타율 읽기 완료");

            int minDmg = ParseInt(data[6]);
            Debug.Log("최소 대미지 읽기 완료");

            int maxDmg = ParseInt(data[7]);
            Debug.Log("최대 대미지 읽기 완료");

            string note = data[8].Trim('"');
            Debug.Log("특이 사항 읽기 완료");

            // 5. ScriptableObject 생성 및 새로운 Initialize 호출
            MonsterSkillData skill = ScriptableObject.CreateInstance<MonsterSkillData>();
            skill.Initialize(sName, target, targetChained, usable, usableChained, acc, crit, minDmg, maxDmg, note);

            if (!Directory.Exists("Assets/Data/Skills/Monster")) Directory.CreateDirectory("Assets/Data/Skills/Monster");
            AssetDatabase.CreateAsset(skill, $"Assets/Data/Skills/Monster/{sName}.asset");
        }
    }

    private static bool[] ParseRanks(string raw)
    {
        bool[] ranks = new bool[4];
        if (string.IsNullOrEmpty(raw)) return ranks;
        for (int j = 1; j <= 4; j++) if (raw.Contains(j + "열")) ranks[j - 1] = true;
        if (raw.Contains("전체")) { for (int j = 0; j < 4; j++) ranks[j] = true; }
        return ranks;
    }

    private static int ParseInt(string val)
    {
        if (string.IsNullOrWhiteSpace(val) || val.Contains("NaN") || val.Trim() == "-1")
            return 0;

        // 모든 종류의 공백과 %, 따옴표 제거
        string cleanVal = Regex.Replace(val, @"[\s%\""]", "");

        if (int.TryParse(cleanVal, out int result))
        {
            return result;
        }

        // 만약 소수점이 섞여 들어온 경우
        if (float.TryParse(cleanVal, out float fResult))
        {
            return Mathf.RoundToInt(fResult);
        }

        return 0;
    }

    private static float ParseFloat(string val)
    {
        if (string.IsNullOrEmpty(val) || val.Contains("NaN") || val == "-1") return 0f;
        return float.Parse(val.Replace("%", "").Replace(" ", ""));
    }
}
