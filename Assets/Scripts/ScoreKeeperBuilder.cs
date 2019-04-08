using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ScoreKeeperBuilder
{
    /// <summary>
    /// Builds an instance of <see cref="ScoreKeeper"/>. Saves a new score keeper as an asset.
    /// </summary>
    /// <returns>The saved score keeper asset containing the player scores</returns>
    public static ScoreKeeper Build()
    {
        // Create an instance of a language collection as an asset, save, and return.
        ScoreKeeper scoreKeeperAsset = ScriptableObject.CreateInstance<ScoreKeeper>();
        scoreKeeperAsset.playerScores = new List<PlayerScore>();
        AssetDatabase.CreateAsset(scoreKeeperAsset, "Assets/Resources/ScoreKeeper.asset");
        AssetDatabase.SaveAssets();
        return scoreKeeperAsset;
    }
}
