using FluentAssertions;
using Xunit;

namespace ConnectFour.Tests;

public class ConnectFourGameTests
{
    // ── Anfangszustand ─────────────────────────────────────────────────────────

    [Fact]
    public void NewGame_YellowStartsByDefault()
    {
        var game = new ConnectFourGame();
        game.PlayerOnTurn.Should().Be(Player.Yellow);
    }

    // ── Drop Grundverhalten ────────────────────────────────────────────────────

    [Fact]
    public void Drop_TokenFallsToBottomRow()
    {
        var game = new ConnectFourGame();
        game.Drop(0);
        game.GetPlayerAt(5, 0).Should().Be(Player.Yellow);
    }

    [Fact]
    public void Drop_AlternatesPlayers()
    {
        var game = new ConnectFourGame();
        game.Drop(0);
        game.PlayerOnTurn.Should().Be(Player.Red);
        game.Drop(1);
        game.PlayerOnTurn.Should().Be(Player.Yellow);
    }

    // ── Gewinnbedingungen ──────────────────────────────────────────────────────

    [Fact]
    public void Drop_HorizontalWin_YellowWins()
    {
        var game = new ConnectFourGame();
        game.Drop(0); game.Drop(4);
        game.Drop(1); game.Drop(4);
        game.Drop(2); game.Drop(4);
        game.Drop(3); // Yellow 4 nebeneinander

        game.IsGameOver.Should().BeTrue();
        game.Winner.Should().Be(Player.Yellow);
        game.PlayerOnTurn.Should().Be(Player.Yellow); // bleibt beim Gewinner
    }

    [Fact]
    public void Drop_VerticalWin_RedWins()
    {
        var game = new ConnectFourGame();
        game.Drop(0); game.Drop(1);
        game.Drop(0); game.Drop(1);
        game.Drop(0); game.Drop(1);
        game.Drop(6); game.Drop(1); // Red 4 übereinander

        game.IsGameOver.Should().BeTrue();
        game.Winner.Should().Be(Player.Red);
    }

    [Fact]
    public void Drop_DiagonalDownRightWin_YellowWins()
    {
        var game = new ConnectFourGame();
        game.Drop(6); game.Drop(1); // Y(5,6) R(5,1)
        game.Drop(6); game.Drop(2); // Y(4,6) R(5,2)
        game.Drop(6); game.Drop(2); // Y(3,6) R(4,2)
        game.Drop(4); game.Drop(3); // Y(5,4) R(5,3)
        game.Drop(4); game.Drop(3); // Y(4,4) R(4,3)
        game.Drop(5); game.Drop(3); // Y(5,5) R(3,3)
        game.Drop(0); game.Drop(5); // Y(5,0) R(4,5)
        game.Drop(1); game.Drop(5); // Y(4,1) R(3,5)
        game.Drop(2); game.Drop(0); // Y(3,2) R(4,0)
        game.Drop(3);               // Y(2,3) → Sieg ↘

        game.IsGameOver.Should().BeTrue();
        game.Winner.Should().Be(Player.Yellow);
    }

    [Fact]
    public void Drop_DiagonalDownLeftWin_RedWins()
    {
        var game = new ConnectFourGame();
        game.Drop(0); game.Drop(6); // Y(5,0)  R(5,6)
        game.Drop(0); game.Drop(1); // Y(4,0)  R(5,1)
        game.Drop(5); game.Drop(5); // Y(5,5)  R(4,5)
        game.Drop(4); game.Drop(1); // Y(5,4)  R(4,1)
        game.Drop(4); game.Drop(4); // Y(4,4)  R(3,4)
        game.Drop(3); game.Drop(2); // Y(5,3)  R(5,2)
        game.Drop(3); game.Drop(2); // Y(4,3)  R(4,2)
        game.Drop(3); game.Drop(3); // Y(3,3)  R(2,3) → Sieg ↙

        game.IsGameOver.Should().BeTrue();
        game.Winner.Should().Be(Player.Red);
    }

    // ── Fehlerbehandlung ───────────────────────────────────────────────────────

    [Fact]
    public void Drop_AfterGameOver_ThrowsInvalidOperationException()
    {
        var game = new ConnectFourGame();
        game.Drop(0); game.Drop(4);
        game.Drop(0); game.Drop(4);
        game.Drop(0); game.Drop(4);
        game.Drop(0); // Yellow gewinnt

        var act = () => game.Drop(1);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Drop_FullColumn_ThrowsInvalidOperationException()
    {
        var game = new ConnectFourGame();
        game.Drop(3); game.Drop(0);
        game.Drop(3); game.Drop(0);
        game.Drop(3); game.Drop(0);
        game.Drop(0); game.Drop(3);
        game.Drop(1); game.Drop(3);
        game.Drop(1); game.Drop(3); // Spalte 3 voll

        var act = () => game.Drop(3);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Drop_NegativeColumn_ThrowsArgumentOutOfRangeException()
    {
        var game = new ConnectFourGame();
        var act = () => game.Drop(-1);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GetPlayerAt_InvalidPosition_ThrowsArgumentOutOfRangeException()
    {
        var game = new ConnectFourGame();
        var act = () => game.GetPlayerAt(-1, 0);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    // ── Reset ──────────────────────────────────────────────────────────────────

    [Fact]
    public void Reset_ClearsBoardAndState()
    {
        var game = new ConnectFourGame();
        game.Drop(0); game.Drop(1); game.Drop(0);
        game.Reset(Player.Red);

        game.PlayerOnTurn.Should().Be(Player.Red);
        game.IsGameOver.Should().BeFalse();
        game.Winner.Should().Be(Player.None);
        game.GetPlayerAt(5, 0).Should().Be(Player.None);
    }

    // ── ToString ───────────────────────────────────────────────────────────────

    [Fact]
    public void ToString_EmptyBoard_ContainsExpectedCharacters()
    {
        var game = new ConnectFourGame(Player.Red);
        string result = game.ToString();

        result.Should().Contain("+---+");
        result.Should().Contain("  1 ");
        result.Should().Contain("Red");
    }

    [Fact]
    public void ToString_AfterWin_ShowsWinner()
    {
        var game = new ConnectFourGame();
        game.Drop(0); game.Drop(4);
        game.Drop(0); game.Drop(4);
        game.Drop(0); game.Drop(4);
        game.Drop(0);

        string result = game.ToString();
        result.Should().Contain("YELLOW");
    }

    [Fact]
    public void ToString_Draw_ShowsDraw()
    {
        var game = new ConnectFourGame();
        // Verifizierte Draw-Sequenz: alle 42 Felder belegt, kein Gewinner
        game.Drop(1); game.Drop(3); game.Drop(3); game.Drop(2); game.Drop(6); game.Drop(5); game.Drop(4);
        game.Drop(2); game.Drop(1); game.Drop(2); game.Drop(4); game.Drop(3); game.Drop(3); game.Drop(6);
        game.Drop(6); game.Drop(4); game.Drop(0); game.Drop(0); game.Drop(2); game.Drop(6); game.Drop(4);
        game.Drop(0); game.Drop(6); game.Drop(5); game.Drop(3); game.Drop(4); game.Drop(3); game.Drop(6);
        game.Drop(2); game.Drop(2); game.Drop(5); game.Drop(0); game.Drop(1); game.Drop(5); game.Drop(5);
        game.Drop(5); game.Drop(0); game.Drop(1); game.Drop(0); game.Drop(1); game.Drop(1); game.Drop(4);

        string result = game.ToString();
        result.Should().Contain("DRAW");
    }
}