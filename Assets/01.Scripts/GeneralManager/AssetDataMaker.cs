using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public class AssetDataMaker : MonoBehaviour
{
    [MenuItem("CustomFunc/Card Generator/Generate Attack Cards")]
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
            string cost = parts[2].Trim('"');
            string type = parts[3].Trim('"');
            string instruction = parts[4].Trim('"');
            string[] targeting = parts[5].Trim('"').Split('/');
            string rarity = parts[6].Trim('"');
            string discardPolicy = parts[7].Trim('"');

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


            string assetPath = $"Assets/CardAssets/Card_{id}.asset";
            AssetDatabase.CreateAsset(cardSpec, assetPath);
            Debug.Log($"CardSpec 생성: {assetPath}");
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
