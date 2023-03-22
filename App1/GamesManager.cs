// Il faut ajouter un gestionnaire de parties et l'intégrer à l'interface .. une autre fois..
// Dans cette version, on se contente d'une classe statique qui gère une seule partie.
using System;

public static class GamesManager
{
    // Etat de la partie
    public static AppDef.GameStatus GameStatus { get; set; }
    // Identifiant de d'une partie
    public static Guid GameGuid { get; set; }
}
// Cl