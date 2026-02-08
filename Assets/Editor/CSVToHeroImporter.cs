using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class CSVToHeroImporter
{
    [MenuItem("Tools/Import Heroes and Link Skills")]
    public static void ImportHeroes()
    {
        string path = "Assets/CSV/[DB]영웅_능력치_데이터.csv";
        if (!File.Exists(path))
        {
            Debug.LogError("영웅 CSV 파일을 찾을 수 없습니다.");
            return;
        }

        string[] lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] data = Regex.Split(lines[i], ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
            if (data.Length < 17) continue;

            EntityData hero = ScriptableObject.CreateInstance<EntityData>();

            // 스탯 파싱 영웅 데이터...csv]
            string hName = data[0].Trim('"');
            int hp = int.Parse(data[1]);
            int dodge = int.Parse(data[2]);
            float prot = float.Parse(data[3].Replace("%", "")) / 100f;
            int spd = int.Parse(data[4]);
            int correct = int.Parse(data[5]);
            float crit = float.Parse(data[6].Replace("%", "")) / 100f;
            int minAtk = int.Parse(data[7]);
            int maxAtk = int.Parse(data[8]);

            // 스킬 연결 (9번 열: 사용 기술) 영웅 데이터...csv]
            string[] skillNames = data[9].Trim('"').Split(',');
            List<SkillData> linkedSkills = new List<SkillData>();

            foreach (string sName in skillNames)
            {
                string cleanName = sName.Trim();
                Debug.Log($"[검색 시도] 영웅: {hName}, 스킬명: '{cleanName}'");

                // "Assets/Data/Skills" 폴더에서 이름이 일치하는 스킬 에셋 찾기
                string[] guids = AssetDatabase.FindAssets($"{cleanName} t:SkillData", new[] { "Assets/Data/Skills" });

                if (guids.Length > 0)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                    linkedSkills.Add(AssetDatabase.LoadAssetAtPath<SkillData>(assetPath));
                    Debug.Log($"[검색 성공] '{cleanName}' 에셋을 찾아 연결했습니다.");
                }
                else
                {
                    Debug.LogWarning($"{hName}의 스킬 '{cleanName}' 에셋을 찾을 수 없습니다.");
                }
            }

            // 저항력 파싱 (10~17번 열)
            float[] resists = new float[8];
            for (int j = 0; j < 8; j++)
            {
                resists[j] = float.Parse(data[10 + j].Replace("%", "")) / 100f;
            }

            // 초기화 및 저장
            hero.Initialize(hName, hp, dodge, prot, spd, correct, crit, minAtk, maxAtk, resists, linkedSkills.ToArray());

            EditorUtility.SetDirty(hero);   // 변경 사항을 유니티에 알림

            if (!Directory.Exists("Assets/Data/Entities/Hero")) Directory.CreateDirectory("Assets/Data/Entities/Player");
            AssetDatabase.CreateAsset(hero, $"Assets/Data/Entities/Hero/{hName}.asset");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("모든 영웅 데이터 및 스킬 연결 완료");
    }
}