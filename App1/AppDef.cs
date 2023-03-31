using App1;
using System;
using Windows.UI.Xaml.Media;

public static class AppDef
{
    // temps au bout duquel l'impact d'un tir est effacé
    public static int eraseEllipseDelay = 1000;
    public static int regenCyclesInitialCredit = 5;
    // définit l'orientation du bateau et donc la position de la proue
    public enum Cap
    {
        W, // Ouest
        N, // Nord
        E, // Est
        S // Sud
    }
    // pas utilisé, définit le sens d'avance
    public enum Machinery
    {
        Ahead, // avant
        Astern, // arrière
        Stop // stop
    }
    // pas utilisé, définit le régime des moteurs
    public enum Power
    {
        Full,
        Half,
        Slow
    }
    // Etat d'un bateau, d'un tir
    public enum State
    {
        // Flotte, A l'eau
        Afloat = 1,
        // Touché
        Struck = 2,
        // Coulé
        Sank = 3,
        // refusé : plus de munition par exemple
        Denied = 4
    }
    // Etat d'une partie
    public enum GameStatus
    {
        NotStarted,
        // En pause
        Idle,
        // Partie en cours
        Running,
        // Terminé avant la fin
        Terminated,
        // Terminé normalement
        Completed
        // Prévoir de mettre en place une sauvegarde et la possibilité de reprendre une partie
    }
    public enum PlayerStatus
    {
        // Gagnant
        Winner,
        // Perdant
        Loser,
        // pas encore fixé
        NotSet
    }

    // Dimensions de la Mer en pixel
    public static int largeurMer = 600;
    public static int hauteurMer = 600;
    public static double largeurMenu = 300;
    // largeur de la mer en nombre de colonnes
    public static int nbCol = 20;
    // hauteur de la mer en nombre de lignes
    public static int nbRow = 20;
    // Création de pinceaux pour le remplissage des ellipses correspondant à des 
    // élements de mer, des élements de bateau ou des tirs
    // Tirs
    public static SolidColorBrush redBrush = new SolidColorBrush(Windows.UI.Colors.IndianRed);
    // Mer
    public static SolidColorBrush blueBrush = new SolidColorBrush(Windows.UI.Colors.SteelBlue);
    // Visualisation des bateaux pour deboggage
    public static SolidColorBrush greenBrush = new SolidColorBrush(Windows.UI.Colors.Green);
    // Visualisation des bateaux pour deboggage
    public static SolidColorBrush pinkBrush = new SolidColorBrush(Windows.UI.Colors.DeepPink);
    public static SolidColorBrush orangeBrush = new SolidColorBrush(Windows.UI.Colors.DarkOrange);
    // Permet de garder une référence sur l'instance de MainPage
    // pour afficher provisoirement l'emplacement des bateaux sur la mer en mode debug
    public static MainPage debugP;
    // Une partie est instanciée et prête à être jouée
    public static bool readyPlayerOne = false;
    // Etat de la partie en cours
    static public AppDef.GameStatus currentGameStatus = AppDef.GameStatus.Completed;
}