using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class CSVToMonsterImporter : MonoBehaviour
{
    [MenuItem("Tools/Import Monsters and Link Skills")]
    public static void ImportMonster()
    {
        string path = "Assets/CSV/[DB]몬스터_능력치_데이터.csv";
        if (!File.Exists(path))
        {
            Debug.LogError("몬스터 CSV 파일을 찾을 수 없습니다.");
            return;
        }

        string[] lines = File.ReadAllLines(path);

        for(int i = 1;  i < lines.Length; i++)
        {
            string[] data = Regex.Split(lines[i], ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
            if (data.Length < 11) continue;

            MonsterData monster = ScriptableObject.CreateInstance<MonsterData>();

            // 스탯 파싱 몬스터 데이터...csv]
            string mName = data[0].Trim('"');
            int hp = int.Parse(data[1]);
            float dodge = float.Parse(data[2]);
            float prot = float.Parse(data[3]);
            int spd = int.Parse(data[4]);

            string rawTypes = data[5].Trim('"');        // 다중 타입 구분
            List<MonsterType> typeList = new List<MonsterType>();

            string[] splitTypes = rawTypes.Split(",");
            foreach (string t in splitTypes)
            {
                string cleanType = t.Trim();
                if (!string.IsNullOrEmpty(cleanType))
                    typeList.Add(ParseMonsterType(cleanType));
            }

            // 저항력 파싱 (7~11번 열)
            float[] resists = new float[6];
            for (int j = 0; j < 5; j++)
            {
                resists[j] = float.Parse(data[6 + j]);
            }

            string[] skillNames = data[11].Trim('"').Split(",");
            List<MonsterSkillData> linkedSkills = new List<MonsterSkillData>();

            foreach (string sName in skillNames)
            {
                string cleanName = sName.Trim();
                Debug.Log($"[검색 시도] 몬스터: {sName}, 스킬명: '{cleanName}'");

                // "Assets/Data/Skills/Monster" 폴더에서 이름이 일치하는 스킬 에셋 찾기
                string[] guids = AssetDatabase.FindAssets($"{cleanName} t:MonsterSkillData", new[] { "Assets/Data/Skills/Monster" });

                if(guids.Length > 0)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                    linkedSkills.Add(AssetDatabase.LoadAssetAtPath<MonsterSkillData>(assetPath));
                    Debug.Log($"[검색 성공] '{cleanName}' 에셋을 찾아 연결했습니다.");
                }
                else
                {
                    Debug.LogWarning($"{mName}의 스킬 '{cleanName}' 에셋을 찾을 수 없습니다.");
                }

            }

            // 초기화 및 저장
            monster.Initialize(mName, hp, dodge, prot, spd, typeList, resists, linkedSkills.ToArray());

            EditorUtility.SetDirty(monster);   // 변경 사항을 유니티에 알림

            if (!Directory.Exists("Assets/Data/Entities/Monster")) Directory.CreateDirectory("Assets/Data/Entities/Monster");
            AssetDatabase.CreateAsset(monster, $"Assets/Data/Entities/Monster/{mName}.asset");
        }
    }

    private static MonsterType ParseMonsterType(string rawType)
    {
        if (rawType.Contains("인간")) return MonsterType.Human;
        if (rawType.Contains("짐승")) return MonsterType.Beast;
        if (rawType.Contains("이물")) return MonsterType.Eldrich;
        if (rawType.Contains("불경")) return MonsterType.Unholy;
        if (rawType.Contains("석재")) return MonsterType.Stone;
        if (rawType.Contains("목공품")) return MonsterType.Woodwork;

        return MonsterType.Human;       // 전부 아닐경우 인간형 타입 (기본값) 반환
    }
}
