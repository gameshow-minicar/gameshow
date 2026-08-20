using UnityEngine;

public class ChickenRaceMap : MonoBehaviour
{
    [Header("追従対象")]
    public Transform player; // プレイヤーのTransform

    [Header("UI要素 (RectTransform)")]
    public RectTransform startLineUI; // スタート線のUI
    public RectTransform goalLineUI;  // ゴール線のUI
    public RectTransform blackDotUI;   // 黒点（プレイヤーマーカー）のUI

    [Header("マップUIの設定")]
    public float mapWidth = 400f; // マップUIの横幅（ピクセル単位）

    // StageGeneratorから受け取って記憶する変数
    private float worldStartPos;
    private float worldGoalPos;
    private bool isInitialized = false;

    /// <summary>
    /// StageGeneratorからステージ生成完了時に呼び出され、マップの基準値を初期化します
    /// </summary>
    public void InitializeMap(float startPos, float goalPos)
    {
        worldStartPos = startPos;
        worldGoalPos = goalPos;

        // 【UI位置の初期設定】
        // スタートラインをマップUIの左端に配置
        startLineUI.anchoredPosition = new Vector2(-mapWidth / 2f, 0f);

        // ゴールラインをマップUIの右端に配置
        goalLineUI.anchoredPosition = new Vector2(mapWidth / 2f, 0f);

        isInitialized = true;
        
        // 最初のフレームの黒点位置を計算
        UpdateDotPosition();
    }

    void Update()
    {
        // StageGeneratorから初期化される前、またはプレイヤーがいない時は処理しない
        if (!isInitialized || player == null) return;

        UpdateDotPosition();
    }

    void UpdateDotPosition()
    {
        // プレイヤーの現在のX座標を取得
        float currentPos = player.position.x; 

        // スタートからゴール（崖の手前）までの進捗率（0.0 〜 1.0）を計算
        // ※Clampを解除しているため、ゴールを過ぎると 1.1, 1.2 と増え続けます
        float progress = Mathf.InverseLerp(worldStartPos, worldGoalPos, currentPos);

        // 進捗率（progress）をもとに、UI上のX座標に変換
        float dotUIX = Mathf.Lerp(-mapWidth / 2f, mapWidth / 2f, progress);

        // 黒点のUI位置を更新（Y軸は0で固定）
        blackDotUI.anchoredPosition = new Vector2(dotUIX, 0f);
    }
}
