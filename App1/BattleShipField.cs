using System;
using System.Timers;
using Windows.Foundation;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;

// La classe BattleShipField est le moteur du jeu de bataille
// elle ne doit pas interf�rer avec le rendu graphique.
// elle permet d'instancier une partie
public class BattleShipField
{
    // identifiant de la partie
    private Guid gameID;
    // D�claration d'un timer pour faire repasser les points d'impact au bleu
    private System.Timers.Timer aTimer;
    // Liste des �l�ments de mer qui ont �t� touch�s qui doivent repasser au 'bleu' au bout d'un certain temps
    // Utilisation d'une collection de donn�es Safe-Thread afin d'�viter les conflits d'acc�s concurrents
    // il faut d�truire manuellement cette collection quand elle n'est plus utilis�e (m�thode Dispose dans finalizer)
    public BlockingCollection<SeaElement> impactedSeaElements = new BlockingCollection<SeaElement>();
    // g�n�rateur de nombres al�atoires
    private Random val = new Random();
    // niveau de jeu
    // Level : joue sur le nombre de tirs autoris�s par exemple
    public int Level { get; set; }
    // Liste des joueurs (on se limite � 1 ou deux joueurs pour l'instant)
    private List<Player> playersList = new List<Player>();
    // La grille de jeu
    private SeaElement[,] seaGrid;
    // La flotte qui contient les vaisseaux de tous les joueurs.
    public List<Boat> boatList = new List<Boat>();

    public int size;

    public Sea sea = new Sea();

    // si un bateau est touch�, il faut le retrouver et marquer un de ses �l�ments 'touch�' et
    // v�rifier s'il n'est pas coul�
    public void MarkHitElement(SeaElement seaElt)
    {
        foreach (var boat in boatList)
        {
            foreach(var shipElt in boat.ShipElt)
            {
                //Si les coord du ship element et �gal � la coordonn�e du sea element
                if (shipElt.coord == seaElt.coord)
                {
                    element.Status = AppDef.SeaElementStatus.Hit;
                    boat.boolSunk = boat.isBoatSunk();

                    if (boat.isBoatSunk())
                    {
                        Console.WriteLine($"Le bateau est coul� !");

                    }

                    return;
                }
            }
      
        }
    }

    // A compl�ter avec toutes les m�thodes utiles �
    // Cr�ation de l'ensemble des bateaux de chaque joueur
    public void CreateBoatsForPlayer(Player player, List<Boat> existingBoats)
    {
        for (int i = 0; i < NumBoatsPerPlayer; i++)
        {
            bool positionFound = false;
            Boat boat;
            while (!positionFound)
            {

                // calcul d'une position al�atoire
                // calcul d'un cap al�atoire
                int posX = val.Next(AppDef.nbCol - boatSize);
                int posY = val.Next(AppDef.nbRow - boatSize);
                int heading = val.Next(360);

                // On cr�e un nouveau bateau avec une taille al�atoire
                int boatSize = val.Next(2, 5);
                boat = new Boat(boatSize,player.Pseudo,new Position(posX,posY);
               
               
                boat.Heading = heading;
                // test la collision de deux bateaux, utile pour le placement
                bool collision = false;
                foreach (Boat existingBoat in existingBoats)
                {
                    if (boat.CollidesWith(existingBoat))
                    {
                        collision = true;
                        break;
                    }
                }
                if (!collision)
                {
                    positionFound = true;
                }
            }
            // On ajoute le bateau � la liste des bateaux du joueur
            player.BoatList.Add(boat);
            // On ajoute le bateau � la liste globale de tous les bateaux
            boatList.Add(boat);
        }
    }

    // recherche un emplacement disponible sur la mer et fixe la position d'un bateau

    // Affichage d�un bateau
    // Affichage d'un bateau
    public void ShowBoat(Boat boat)
    {
        foreach (var elt in boat.ShipElt)
        {
            Point location = new Point(Prow.X + elt.elt.X, Prow.Y + elt.elt.Y);
            MainPage.drawEllipseOnCanvas(location, AppDef.shipBrush);
        }
    }

    // Affichage des bateaux d�un joueur
    // Affichage des bateaux d'un joueur
    public void ShowAllBoats()
    {
        foreach (Boat boat in boatList)
        {
            ShowBoat(boat);
        }
    }

    // Gestion du tir effectu� par un joueur sur un �l�ment de mer :





    // il faut mettre � jour l'�tat des bateaux
    // Mettre en place la gestion du level et des tirs restants pour les joueurs
    // et d�cider de la fin de la partie

    // Fait apparaitre le point d'impact en rouge quand on clique sur une ellipse
 public static void EffectuerUnTir(object sender, PointerRoutedEventArgs e)
    {
        // si une partie est en cours d'ex�cution
        if (GamesManager.GameStatus == AppDef.GameStatus.Running)
        {
            AppDef.PlayerStatus playerStatus = bSF.GetPlayerStatus(myPlayerID);
            // si le joueur courant n'a pas encore perdu la partie
            if (playerStatus != AppDef.PlayerStatus.Loser)
            {
                // si on a cliqu� sur une ellipse, on la peint en rouge et on appelle la m�thode
                // qui va rechercher l'�l�ment de mer concern� (sea.FireAt() )
                if (sender is Windows.UI.Xaml.Shapes.Ellipse)
                {
                    (sender as Ellipse).Fill = AppDef.redBrush;
                    // D�clenchement du tir
                    sea.FireAt(sender as Ellipse);
                }
            }

        }


    }


    public void StartGame()
    {
        // Cr�ation des joueurs
        Player player1 = new Player("Joueur 1");
        Player player2 = new Player("Joueur 2");

        // Ajout des joueurs � la liste des joueurs de la partie
        playersList.Add(player1);
        playersList.Add(player2);

        // Cr�ation des bateaux pour chaque joueur
        CreateBoatsForPlayer(player1, new List<Boat>());
        CreateBoatsForPlayer(player2, player1.BoatList);

        // Initialisation du timer pour la gestion des �l�ments de mer touch�s
        aTimer = new System.Timers.Timer(5000);
        aTimer.Elapsed += OnTimedEvent;
        aTimer.AutoReset = true;
        aTimer.Enabled = true;

        // Affichage de la grille pour chaque joueur
        player1.ShowGrid();
        player2.ShowGrid();

        // Boucle de jeu jusqu'� la fin de la partie
        bool gameFinished = false;
        while (!gameFinished)
        {
            // Tour du joueur 1
            AppDef.GameStatus gameState = AppDef.GameStatus.NotStarted;
            while (gameState == AppDef.GameStatus.NotStarted)
            {
                gameState = player1.PlayTurn(this);
            }

            // V�rification de la fin de partie
            if (gameState == AppDef.GameStatus.Completed)
            {
                Console.WriteLine("Le joueur 1 a gagn� !");
                gameFinished = true;
                break;
            }

            // Tour du joueur 2
            gameState = AppDef.GameStatus.NotStarted;
            while (gameState == AppDef.GameStatus.NotStarted)
            {
                gameState = player2.PlayTurn(this);
            }

            // V�rification de la fin de partie
            if (gameState == AppDef.GameStatus.Completed)
            {
                Console.WriteLine("Le joueur 2 a gagn� !");
                gameFinished = true;
                break;
            }
        }
    }

}