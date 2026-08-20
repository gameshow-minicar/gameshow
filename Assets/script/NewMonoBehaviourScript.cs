using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VerticalGageController : MonoBehaviour
{
    [SerializeField] private List<Image> gage; // 下のブロックから順番に登録する
    [SerializeField] private float maxValue = 100f; // ゲージの最大値

    // 外部からこの関数を呼んで、ゲージの現在の値を更新する
    public void SetGageValue(float currentValue)
    {
        // 値が範囲外（0未満、最大値超え）にならないように固定
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);

        // 1ブロックあたりが担当する「値の量」
        float valuePerBlock = maxValue / gage.Count;

        for (int i = 0; i < gage.Count; i++)
        {
            // このブロックが満タンになるための最小値と最大値
            float minThreshold = i * valuePerBlock;
            float maxThreshold = (i + 1) * valuePerBlock;

            if (currentValue >= maxThreshold)
            {
                // 現在値がしきい値を超えていれば、このブロックは満タン
                gage[i].fillAmount = 1f;
            }
            else if (currentValue <= minThreshold)
            {
                // まだ届いていなければ、このブロックは完全に「0（空）」
                gage[i].fillAmount = 0f;
            }
            else
            {
                // ちょうどこのブロックの間を現在値が通過中（中間の滑らかな変化）
                float remainingValue = currentValue - minThreshold;
                gage[i].fillAmount = remainingValue / valuePerBlock;
            }
        }
    }
}

