namespace Bowling;

public class Game
{

    private readonly int[] _rolls = new int[21];
    private int _currentRoll;
    private int _score;
    public void Roll(int pins)
    {
        _rolls[_currentRoll++] = pins;
    }

    public void Roll(params int[] rolls)
    {
        foreach (var pins in rolls)
        {
            Roll(pins);
        }
    }

    public int GetScore()
    {
        var score = 0;
        var rollIndex = 0;

        for (var frame = 0; frame < 10; frame++)
        {
            if (IsStrike(rollIndex)) // Strike
            {
                score += StrikeBonus(rollIndex);
                rollIndex++; // Ein Strike beendet den Frame (1 Wurf)
            }
            else if (IsSpare(rollIndex)) // Spare
            {
                score += SpareBonus(rollIndex);
                rollIndex += 2; // Ein Spare braucht 2 Würfe
            }
            else // Normaler Frame
            {
                score += SumOfPinsInFrame(rollIndex);
                rollIndex += 2; // Ein normaler Frame braucht 2 Würfe
            }
        }
        return score;
    }

    private bool IsSpare(int rollIndex)
    {
        // Ein Spare liegt vor, wenn die ersten zwei Würfe des Frames 10 ergeben
        return _rolls[rollIndex] + _rolls[rollIndex + 1] == 10;
    }

    private bool IsStrike(int rollIndex)
    {
        return _rolls[rollIndex] == 10;
    }

    private int StrikeBonus(int rollIndex)
    {
        // 10 Punkte für den Strike + die nächsten zwei Würfe
        return 10 + _rolls[rollIndex + 1] + _rolls[rollIndex + 2];
    }

    private int SpareBonus(int rollIndex)
    {
        // 10 Punkte für den Spare + den nächsten einen Wurf
        return 10 + _rolls[rollIndex + 2];
    }

    private int SumOfPinsInFrame(int rollIndex)
    {
        return _rolls[rollIndex] + _rolls[rollIndex + 1];
    }
}
