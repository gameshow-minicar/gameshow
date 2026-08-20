using UnityEngine;
using System.IO;
using System.Globalization;

public class Gagescore : MonoBehaviour
{
    // data.txtの場所
    public string filePath;

    // 移動量を累積した値
    public float totalMovement = 0.0f;

    // 前回読み込んだ値
    private float previousValue;

    // 初回読み込みかどうか
    private bool firstRead = true;

    void Start()
    {
        // UnityプロジェクトのAssetsフォルダにあるdata.txt
        filePath = Path.Combine(Application.dataPath, "data.txt");

        Debug.Log("読み込むファイル: " + filePath);
    }

    void Update()
    {
        // ファイルが存在しない場合
        if (!File.Exists(filePath))
        {
            return;
        }

        try
        {
            // data.txtを読み込む
            string text = File.ReadAllText(filePath).Trim();

            // 空の場合
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            // 数値に変換
            float currentValue;

            if (float.TryParse(
                text,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out currentValue))
            {
                // 初回は基準値として保存
                if (firstRead)
                {
                    previousValue = currentValue;
                    firstRead = false;

                    Debug.Log("初期値: " + currentValue);
                }
                else
                {
                    // 前回との差を計算
                    float movement = Mathf.Abs(currentValue - previousValue);

                    // 移動量を累積
                    totalMovement += movement;

                    // 今回の値を次回の比較用に保存
                    previousValue = currentValue;

                    Debug.Log(
                        "現在値: " + currentValue +
                        " / 移動量: " + movement +
                        " / 累積移動量: " + totalMovement
                    );
                }
            }
        }
        catch (IOException)
        {
            // C++がファイルを書き換えている最中なら無視
        }
    }
}