#if UNITY_EDITOR
using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetDataMaker : MonoBehaviour
{
    // 스프레드시트 ID (링크의 d/와 /edit 사이)
    private const string SpreadsheetID = "17Isxtr92qhGeb0l4AvbYWyr4WaXjfpKRYEkf5sm21R8";

    // CSV로 받을 시트 이름
    private static readonly string[] SheetNames = {
        "Card"
    };
    private const string OutputFolder = "Assets/GoogleSheetsCSV";

    [MenuItem("CustomFunc/GoogleSheetParsing")]
    public static void DownloadGoogleSheetToCSV()
    {
        try
        {
            // find directory, if not exist => mkdir
            if (!Directory.Exists(OutputFolder))
            {
                Directory.CreateDirectory(OutputFolder);
            }
            for (int i = 0; i < SheetNames.Length; i++)
            {
                string sheet = SheetNames[i];
                string url = BuildURL(sheet);

                EditorUtility.DisplayProgressBar
                (
                    "GoogleSheet To CSV",
                    $"Downloading : {sheet}",
                    (float)i / Mathf.Max(1, SheetNames.Length)
                );

                string csvData = GetCsvData(url);
                string path = Path.Combine(OutputFolder, $"{sheet}.csv");

                File.WriteAllText(path, csvData, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
                Debug.Log($"Save File To = {path}");
            }
        }
        catch (Exception ecode)
        {
            Debug.LogError("googleSheet parsing false\n" + ecode);
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }

        AssetDatabase.Refresh();
    }

    // url 조립 함수
    private static string BuildURL(string sheetName)
    {
        string encoded = Uri.EscapeDataString(sheetName);
        return $"https://docs.google.com/spreadsheets/d/{SpreadsheetID}/gviz/tq?tqx=out:csv&sheet={encoded}";
    }

    private static string GetCsvData(string url)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            var op = req.SendWebRequest();

            while (!op.isDone)
            {
                EditorUtility.DisplayProgressBar
                (
                    "Downloading...",
                    url,
                    req.downloadProgress
                );
                System.Threading.Thread.Sleep(10);
            }
            bool hasError = (req.result != UnityWebRequest.Result.Success);

            if (hasError) throw new Exception($"HTTP = {(int)req.responseCode} = {req.error}\n{url}");

            return req.downloadHandler.text;
        }
    }
}
#endif