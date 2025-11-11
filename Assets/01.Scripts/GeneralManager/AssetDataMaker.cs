using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
using System.Linq;

public class AssetDataMaker : MonoBehaviour
{
    [MenuItem("CustomFunc/Card Generator/Generate CardSpecData")]
    public static void GenerateAttackCards()
    {
        string path = "Assets/GoogleSheetsCSV/Card.csv"; // CSV 경로
        if (!File.Exists(path))
        {
            Debug.LogError("CSV 파일이 존재하지 않습니다.");
            return;
        }

        string[] lines = File.ReadAllLines(path);
        for (int i = 1; i < lines.Length; i++) // 헤더 스킵
        {
            string[] parts = lines[i].Split(',');
            // foreach (string word in parts)
            //     Debug.Log(word);
            string id = parts[0].Trim('"');
            string name = parts[1].Trim('"');

            if (name == "")
            {
                continue;
            }

            string cost = parts[2].Trim('"');
            string type = parts[3].Trim('"');
            string instruction = parts[4].Trim('"');
            string[] targeting = parts[5].Trim('"').Split('/');
            string rarity = parts[6].Trim('"');
            string discardPolicy = parts[7].Trim('"');
            string[] effect = parts[9].Trim('"').Split('/');
            string[] effectAmountStr = parts[10].Trim('"').Split('/');
            string[] effectHoldingTimeStr = parts[11].Trim('"').Split('/');

            int[] effectAmount = new int[effectAmountStr.Length];
            for (int j = 0; j < effectAmount.Length; j++)
            {
                string word = effectAmountStr[j].Trim();
                if (int.TryParse(word, out int value))
                {
                    effectAmount[j] = value;
                }
                else
                {
                    Debug.LogWarning($"[ID {id}] EffectAmount 변환 실패: '{word}'");
                    effectAmount[j] = -1;
                }
            }

            int[] effectHoldingTime = new int[effectHoldingTimeStr.Length];
            for (int j = 0; j < effectHoldingTime.Length; j++)
            {
                string word = effectHoldingTimeStr[j].Trim();
                if (int.TryParse(word, out int value))
                {
                    effectHoldingTime[j] = value;
                }
                else
                {
                    Debug.LogWarning($"[ID {id}] EffectAmount 변환 실패: '{word}'");
                    effectHoldingTime[j] = -1;
                }
            }

            // int[] effectHoldingTime = new int[effectHoldingTimeStr.Length];
            // for (int j = 0; j < effectHoldingTime.Length; j++)
            //     effectHoldingTime[j] = int.Parse(effectHoldingTimeStr[j]);

            // if (type == "attack") continue;
            // else if (type == "defence") continue;
            // else if (type == "effect") continue;

            // CardSpec.asset 생성
            var cardSpec = ScriptableObject.CreateInstance<CardSpec>();
            cardSpec.id = int.Parse(id);
            cardSpec.cardName = name;
            cardSpec.cost = int.Parse(cost);
            cardSpec.type = type;
            cardSpec.instruction = instruction;
            cardSpec.targeting = targeting;
            cardSpec.rarity = rarity;
            cardSpec.discardPolicy = discardPolicy;
            cardSpec.effect = effect;

            cardSpec.effectAmount = effectAmount;
            cardSpec.effectHoldingTime = effectHoldingTime;

#if UNITY_EDITOR
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/PrivateAssets/CardSprite/{id}.png");
            cardSpec.cardImage = sprite;

            sprite = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/02.Sprites/card_category_{type}.png");
            cardSpec.cardCategoryBar = sprite;
#endif


            string assetPath = $"Assets/CardAssets/Card_{id}.asset";
            AssetDatabase.CreateAsset(cardSpec, assetPath);
            Debug.Log($"CardSpec 생성: {assetPath}");
            // todo : db 자동화 만들면 편할 것 같음
        }
        
        // 모든 CardSpec 로드
        var allSpecs = AssetDatabase.FindAssets("t:CardSpec")
            .Select(guid => AssetDatabase.LoadAssetAtPath<CardSpec>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToArray();

        // 씬의 CardDatabase 찾기
        CardDatabase cardDB = Object.FindAnyObjectByType<CardDatabase>();

        if (cardDB == null)
        {
            Debug.LogWarning("씬에 CardDatabase 오브젝트가 없습니다. 자동 연결 실패.");
        }
        else
        {
            // Undo 등록 (에디터 작업 취소 가능)
            Undo.RecordObject(cardDB, "Auto Assign CardSpecs");

            // 배열 갱신
            var so = new SerializedObject(cardDB);
            var specsProp = so.FindProperty("cardSpecs");
            specsProp.ClearArray();
            specsProp.arraySize = allSpecs.Length;
            for (int i = 0; i < allSpecs.Length; i++)
            {
                specsProp.GetArrayElementAtIndex(i).objectReferenceValue = allSpecs[i];
            }
            so.ApplyModifiedProperties();

            EditorUtility.SetDirty(cardDB);
            Debug.Log($"CardDatabase에 {allSpecs.Length}개의 CardSpec 자동 연결 완료.");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("카드 생성 완료");
    }
}




            // // DealDamageEffect.asset 생성 또는 로드
            // string effectName = $"DealDamage_{damage}";
            // string effectPath = $"Assets/Effects/{effectName}.asset";
            // DealDamageEffect effect = AssetDatabase.LoadAssetAtPath<DealDamageEffect>(effectPath);
            // if (effect == null)
            // {
            //     effect = ScriptableObject.CreateInstance<DealDamageEffect>();
            //     effect.DamageAmount = damage;
            //     AssetDatabase.CreateAsset(effect, effectPath);
            //     Debug.Log($"새 Effect 생성: {effectPath}");
            // }
            
            // // 데미지 수치 추출
            // var match = Regex.Match(instruction, @"<b>(\d+)\s*데미지</b>");
            // int damage = match.Success ? int.Parse(match.Groups[1].Value) : 0;
            // if (damage == 0)
            // {
            //     Debug.LogWarning($"[{id}] 데미지 추출 실패");
            //     continue;
            // }
