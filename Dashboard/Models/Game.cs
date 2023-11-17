using System.Text;

namespace Dashboard.Models;

/*
 * Handler providing enum for the supported games.
 */
public static class GameHandler
{
    /*
     * Supported games enum.
     */
    public enum Game
    {
        LeagueOfLegends,
        LegendsOfRuneterra,
        TeamfightTactics
    }

    /*
     * Provide readable name for enum.
     */
    public static string GameName(Game game)
    {
        var enumName = game.ToString();

        if (string.IsNullOrWhiteSpace(enumName))
            return "";
        var newText = new StringBuilder(enumName.Length * 2);
        newText.Append(enumName[0]);
        for (var i = 1; i < enumName.Length; i++)
        {
            if (char.IsUpper(enumName[i]) && enumName[i - 1] != ' ')
                newText.Append(' ');
            newText.Append(enumName[i]);
        }

        return newText.ToString();
    }
}