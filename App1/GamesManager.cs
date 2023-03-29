// Il faut ajouter un gestionnaire de parties et l'intégrer à l'interface .. une autre fois..
// Dans cette version, on se contente d'une classe statique qui gère une seule partie.
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class GamesManager
{
    // Etat de la partie
    public static AppDef.GameStatus GameStatus { get; set; }
    // Identifiant de d'une partie
    public static Guid GameGuid { get; set; }

    public static Player activePlayer { get; set; }

    public static Sea sea;
    public static List<Player> playerList;


    public static Player getOponentPlayerObject(Guid playerGuid,List<Player> players)
    {
        foreach (Player player in players)
        {
            if (player.PlayerID != playerGuid)
            {
                return player;
            }
        }
        return null;
    }
}
// Cl