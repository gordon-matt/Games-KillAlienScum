public class GameManager
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get { return instance ?? (instance = new GameManager()); }
    }

    // Private constructor to disallow creating instances from outside this class
    private GameManager()
    {
    }

    public string PlayerName { get; set; } = "Player 1";

    public int Points { get; private set; }

    public void AddPoints(int pointsToAdd)
    {
        Points += pointsToAdd;
    }

    public void Reset()
    {
        Points = 0;
    }

    public void ResetPoints(int points)
    {
        Points = points;
    }
}