using System.IO;
using System.Globalization;
using UnityEngine;

public class ChangeManager : MonoBehaviour
{
    public static ChangeManager Instance;
    public static double change;

    public double min = 1.0;
    public double max = 25.0;

    // 1つ上の階層にあるdata.txt
    private string filePath;

    void Awake()
    {
        filePath = Path.Combine(Application.dataPath, "data.txt");
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        Debug.Log("ChangeManager起動");
        Debug.Log("読み込み先: " + Path.GetFullPath(filePath));
        Debug.Log("ファイル存在: " + File.Exists(filePath));
    }

    void Update()
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        string text = File.ReadAllText(filePath).Trim();

        Debug.Log("data.txt: [" + text + "]");

        if (double.TryParse(
            text,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out double value))
        {
            change = value;

            if (change <= min)
            {
                change = 0;
            }
            else if (change >= max)
            {
                change = 0;
            }

            Debug.Log("change = " + change);
        }
        else
        {
            Debug.LogWarning("数値として読み込めません: [" + text + "]");
        }
    }
}