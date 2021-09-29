using UnityEngine;

public class DifficultyButton : MonoBehaviour
{
    public void SetDifficulty(int difficulty)
    {
        Sequence.Instance.Difficulty = difficulty;
    }

    public void LoadSequenceScene()
    {
        GameManager.Instance.LoadSceneAsync(1);
    }
}