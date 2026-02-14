using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpineHardcoreLoader : MonoBehaviour
{
    public TextAsset jsonFile;
    public Sprite[] sprites;
    public float globalScale = 0.01f;

    [Header("Options")]
    [Tooltip("체크하면 메쉬/스킨드 메쉬 데이터를 무시하고 강제로 스프라이트 이미지로 배치합니다. (스탠딩 잡을 때 추천)")]
    public bool forceSpriteMode = true;

    private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();

    [ContextMenu("Assemble Fixed Character")]
    public void Assemble()
    {
        if (jsonFile == null) { Debug.LogError("JSON 파일이 없습니다."); return; }

        // 1. 스프라이트 정리 (_0 제거)
        spriteDict.Clear();
        foreach (var s in sprites)
        {
            if (s == null) continue;
            string cleanName = s.name.EndsWith("_0") ? s.name.Substring(0, s.name.Length - 2) : s.name;
            if (!spriteDict.ContainsKey(cleanName)) spriteDict.Add(cleanName, s);
        }

        // 2. 기존 오브젝트 청소
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        foreach (GameObject child in children) DestroyImmediate(child);

        // 3. 뼈대 생성
        JObject rootJson = JObject.Parse(jsonFile.text);
        Dictionary<string, Transform> boneDict = new Dictionary<string, Transform>();

        foreach (var b in rootJson["bones"])
        {
            string boneName = b["name"].ToString();
            GameObject go = new GameObject(boneName);

            string parentName = b["parent"]?.ToString();
            if (!string.IsNullOrEmpty(parentName) && boneDict.ContainsKey(parentName))
                go.transform.SetParent(boneDict[parentName]);
            else
                go.transform.SetParent(this.transform);

            float x = (float)(b["x"] ?? 0) * globalScale;
            float y = (float)(b["y"] ?? 0) * globalScale;
            float rot = (float)(b["rotation"] ?? 0);

            go.transform.localPosition = new Vector3(x, y, 0);
            go.transform.localRotation = Quaternion.Euler(0, 0, rot);
            boneDict[boneName] = go.transform;
        }

        // 4. 파츠(Slot) 생성
        var slots = rootJson["slots"] as JArray;
        var defaultSkin = rootJson["skins"]?["default"];

        if (slots != null && defaultSkin != null)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                string slotName = slot["name"].ToString();
                string boneName = slot["bone"].ToString();
                string attachmentName = slot["attachment"]?.ToString();

                if (string.IsNullOrEmpty(attachmentName)) continue;
                if (!boneDict.ContainsKey(boneName)) continue;

                var attData = defaultSkin[slotName]?[attachmentName];
                if (attData == null) continue;

                GameObject go = new GameObject(attachmentName);
                go.transform.SetParent(boneDict[boneName]);

                string type = attData["type"]?.ToString();

                // [중요 수정] forceSpriteMode가 켜져있거나, skinnedmesh인 경우 무조건 스프라이트로 처리
                if (forceSpriteMode || type == "skinnedmesh" || type == "mesh")
                {
                    // 검(Sword)이나 튜닉(Tunic)처럼 메쉬 정보가 복잡한 경우, 
                    // 그냥 이미지를 뼈대 위치(0,0)에 붙이는게 스탠딩 포즈 잡기에 훨씬 유리합니다.
                    CreateSpriteObject(go, attachmentName, attData, i, isFallback: true);
                }
                else
                {
                    // 일반적인 파츠들
                    CreateSpriteObject(go, attachmentName, attData, i, isFallback: false);
                }
            }
        }
        Debug.Log("캐릭터 조립 완료 (수정 버전)");
    }

    private void CreateSpriteObject(GameObject go, string attName, JToken attData, int sortingOrder, bool isFallback)
    {
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        if (spriteDict.TryGetValue(attName, out Sprite s)) sr.sprite = s;
        else
        {
            // 만약 이미지를 못 찾으면(예: tunic 등), 이름 매칭을 한 번 더 시도
            if (spriteDict.TryGetValue(go.name, out Sprite s2)) sr.sprite = s2;
        }

        // isFallback: 메쉬인데 강제로 스프라이트로 그리는 경우 (좌표 정보가 없을 수 있음)
        float x = (float)(attData["x"] ?? 0) * globalScale;
        float y = (float)(attData["y"] ?? 0) * globalScale;
        float rot = (float)(attData["rotation"] ?? 0);
        float sx = (float)(attData["scaleX"] ?? 1);
        float sy = (float)(attData["scaleY"] ?? 1);

        // 튜닉이나 검 같은 메쉬는 x, y 오프셋이 JSON에 없을 수 있습니다.
        // 그럴 땐 뼈대의 원점(0,0)에 붙이는 게 가장 안전합니다.
        go.transform.localPosition = new Vector3(x, y, 0);
        go.transform.localRotation = Quaternion.Euler(0, 0, rot);
        go.transform.localScale = new Vector3(sx, sy, 1);

        sr.sortingOrder = sortingOrder;
    }

    // [에디터 기능] 자동 할당
#if UNITY_EDITOR
    [ContextMenu("Auto Assign Sprites From Folder")]
    public void AutoAssign()
    {
        if (jsonFile == null) return;
        string path = AssetDatabase.GetAssetPath(jsonFile);
        string folderPath = System.IO.Path.GetDirectoryName(path);
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { folderPath });
        sprites = new Sprite[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            sprites[i] = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guids[i]));
        }
        Debug.Log($"총 {sprites.Length}개의 스프라이트를 찾아서 할당했습니다.");
    }
#endif
}
