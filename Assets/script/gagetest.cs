using UnityEngine;
// ⬇ 新しい入力システムを使うための宣言を追加
using UnityEngine.InputSystem; 

public class GageTestInput : MonoBehaviour
{
    [SerializeField] private VerticalGageController gageController;

    private float currentEnergy = 0f; 
    private float maxEnergy = 100f;   

    void Update()
    {
        // ⬇ 新しい入力システムの書き方に変更（キーボードの現在の状態を取得）
        var keyboard = Keyboard.current;
        if (keyboard == null) return; // キーボードが接続されていない場合は何もしない

        // 「上矢印キー（↑）」が押されているか
        if (keyboard.upArrowKey.isPressed)
        {
            currentEnergy += Time.deltaTime * 30f; 
        }

        // 「下矢印キー（↓）」が押されているか
        if (keyboard.downArrowKey.isPressed)
        {
            currentEnergy -= Time.deltaTime * 30f; 
        }

        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);

        if (gageController != null)
        {
            gageController.SetGageValue(currentEnergy);
        }
    }
}
