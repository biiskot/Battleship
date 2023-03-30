// Il faut ajouter un gestionnaire de parties et l'intégrer à l'interface .. une autre fois..
// Dans cette version, on se contente d'une classe statique qui gère une seule partie.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

public static class GamesManager
{
    // Etat de la partie
    public static AppDef.GameStatus GameStatus { get; set; }
    // Identifiant de d'une partie
    public static Guid GameGuid { get; set; }

    public static Player activePlayer { get; set; }

    public static Sea sea;
    public static List<Player> playerList;

    public static Player winner;
    public static Player loser;


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
    public static bool isGameFinished(List<Player> players)
    {
        bool finished = false;
        Player oponent;
        foreach (Player player in players)
        {
            oponent = getOponentPlayerObject(player.PlayerID,players);
            if (player.isAllMyBoatsSunk() && player.NbStruck > oponent.NbStruck)
            {
                finished = true;
                GamesManager.GameStatus = AppDef.GameStatus.Completed;
                loser = player;
                winner = oponent;
                break;
            }
        }
        return finished;
    }
}
// Cl