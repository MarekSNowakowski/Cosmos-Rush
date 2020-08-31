using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CloudOnce;

public class CloudOnceServices : MonoBehaviour
{
    public static CloudOnceServices instance;

    private void Awake()
    {
        TestSingleton();
    }

    private void TestSingleton()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SubmitScoreToLeaderboard(long score)
    {
        Leaderboards.highScore.SubmitScore(score);
    }

    public void SubmitMaxComboToLeaderboard(long maxCombo)
    {
        Leaderboards.maxCombo.SubmitScore(maxCombo);
    }

    public void SubmitMoneyToLeaderboard(long money)
    {
        Leaderboards.moneyEarned.SubmitScore(money);
    }

    public void SubmitDistanceToLeaderboard(long distance)
    {
        Leaderboards.distance.SubmitScore(distance);
    }

    public void SubmitBallsToLeaderboard(long ballsDestroyed)
    {
        Leaderboards.ballsDistroyed.SubmitScore(ballsDestroyed);
    }

    public void SubmitSpeedToLeaderboard(long maxSpeed)
    {
        Leaderboards.maxSpeed.SubmitScore(maxSpeed);
    }

}
