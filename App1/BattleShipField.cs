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
using App1;

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
    private int NumBoatsPerPlayer = 4;


    // si un bateau est touch�, il faut le retrouver et marquer un de ses �l�ments 'touch�' et
    // v�rifier s'il n'est pas coul�
  
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
                // On cr�e un nouveau bateau avec une taille al�atoire
                int boatSize = val.Next(2, 5);
                // calcul d'une position al�atoire
                // calcul d'un cap al�atoire
                int posX = val.Next(AppDef.nbCol - boatSize);
                int posY = val.Next(AppDef.nbRow - boatSize);
                int heading = val.Next(360);

                
                boat = new Boat(boatSize,player.PlayerID,new Point(posX,posY));
               
               
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
    public AppDef.State ProcessStrike(Guid playerID, SeaElement strikeElt)
    {

        // il faut mettre � jour l'�tat des bateaux
        // Mettre en place la gestion du level et des tirs restants pour les joueurs
        // et d�cider de la fin de la partie


        foreach (Boat boat in boatList)
        {
            foreach(ShipElement element in boat.ShipElt)
            {
                if (element.coord == strikeElt.coord)        //Si shipElement � l'emplacement du seaElement touch� :
                {

                    element.changeElementStatus(AppDef.State.Struck);   //Si element touch�, on change son statut
                    return AppDef.State.Struck;
                }
            }
        }
        return AppDef.State.Afloat;
    }


    public AppDef.PlayerStatus GetPlayerStatus(Guid PID)
    {
        foreach(Player player in playersList)
        {
            if(player.PlayerID == PID)
            {
                return player.Status;
            }
        }
        return AppDef.PlayerStatus.NotSet;
    }
        public void StartGame()
    {
        // Cr�ation des joueurs
        Player player1 = new Player("joueur1",10,AppDef.PlayerStatus.NotSet);
        Player player2 = new Player("joueur2", 10, AppDef.PlayerStatus.NotSet);

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