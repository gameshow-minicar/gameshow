using System.Collections.Generic;

public static class RankingManager
{
    // 1位～5位のスコア
    public static List<int> scores = new List<int>();


    public static void AddScore(int score)
    {
        scores.Add(score);

        // 残り距離が小さい順に並べる
        scores.Sort();

        // 5位より下を削除
        if (scores.Count > 5)
        {
            scores.RemoveAt(5);
        }
    }
}
