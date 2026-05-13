namespace ConnectFour;

public class ConnectFourGame : IConnectFour
{
    private const int Rows = 6;
    private const int Cols = 7;

    private readonly Player[,] _board = new Player[Rows, Cols];

    public Player PlayerOnTurn { get; private set; }
    public bool IsGameOver { get; private set; }
    public Player Winner { get; private set; }

    public ConnectFourGame(Player playerOnTurn = Player.Yellow)
    {
        PlayerOnTurn = playerOnTurn;
    }

    public Player GetPlayerAt(int row, int col)
    {
        if (row < 0 || row >= Rows || col < 0 || col >= Cols)
            throw new ArgumentOutOfRangeException();
        return _board[row, col];
    }

    public void Reset(Player playerOnTurn)
    {
        for (int r = 0; r < Rows; r++)
            for (int c = 0; c < Cols; c++)
                _board[r, c] = Player.None;

        PlayerOnTurn = playerOnTurn;
        IsGameOver = false;
        Winner = Player.None;
    }

    public void Drop(int col)
    {
        if (IsGameOver)
            throw new InvalidOperationException("Game is already over.");
        if (col < 0 || col >= Cols)
            throw new ArgumentOutOfRangeException(nameof(col));

        int targetRow = -1;
        for (int r = Rows - 1; r >= 0; r--)
        {
            if (_board[r, col] == Player.None)
            {
                targetRow = r;
                break;
            }
        }

        if (targetRow == -1)
            throw new InvalidOperationException("Column is full.");

        _board[targetRow, col] = PlayerOnTurn;

        if (CheckWin(targetRow, col))
        {
            Winner = PlayerOnTurn;
            IsGameOver = true;
        }
        else if (IsBoardFull())
        {
            IsGameOver = true;
        }
        else
        {
            PlayerOnTurn = PlayerOnTurn == Player.Yellow ? Player.Red : Player.Yellow;
        }
    }

    private bool IsBoardFull()
    {
        for (int c = 0; c < Cols; c++)
            if (_board[0, c] == Player.None)
                return false;
        return true;
    }

    private bool CheckWin(int row, int col)
    {
        Player p = _board[row, col];
        return CountLine(row, col, 0, 1, p) >= 4   // horizontal
            || CountLine(row, col, 1, 0, p) >= 4   // vertikal
            || CountLine(row, col, 1, 1, p) >= 4   // diagonal ↘
            || CountLine(row, col, 1, -1, p) >= 4; // diagonal ↙
    }

    private int CountLine(int row, int col, int dr, int dc, Player p)
    {
        return 1
            + CountDirection(row, col, dr, dc, p)
            + CountDirection(row, col, -dr, -dc, p);
    }

    private int CountDirection(int row, int col, int dr, int dc, Player p)
    {
        int count = 0;
        int r = row + dr, c = col + dc;
        while (r >= 0 && r < Rows && c >= 0 && c < Cols && _board[r, c] == p)
        {
            count++;
            r += dr;
            c += dc;
        }
        return count;
    }
    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("+---+---+---+---+---+---+---+");
        for (int r = 0; r < Rows; r++)
        {
            sb.Append("|");
            for (int c = 0; c < Cols; c++)
            {
                string token = _board[r, c] switch
                {
                    Player.Yellow => " Y ",
                    Player.Red => " R ",
                    _ => "   "
                };
                sb.Append(token + "|");
            }
            sb.AppendLine();
            sb.AppendLine("+---+---+---+---+---+---+---+");
        }
        sb.AppendLine("  1   2   3   4   5   6   7  ");
        if (IsGameOver)
            sb.AppendLine(Winner == Player.None
                ? "DRAW!"
                : $"Winner: {Winner.ToString().ToUpper()}");
        else
            sb.AppendLine($"Player on turn: {PlayerOnTurn}");
        return sb.ToString();
    }
}