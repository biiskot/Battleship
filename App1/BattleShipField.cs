using System;
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
    private SeaElement[,] seaGrid = new SeaElement[20, 20];
    // La flotte qui contient les vaisseaux de tous les joueurs.
    public List<Boat> boatList = new List<Boat>();

    public BattleShipField()
    {
        // Initialiser la grille avec des �l�ments de mer vides
        this.size = size;
        SeaElements = new SeaElement[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                SeaElements[i, j] = new SeaElement(new Point(i, j));
            }
        }
    }

    // si un bateau est touch�, il faut le retrouver et marquer un de ses �l�ments 'touch�' et
    // v�rifier s'il n'est pas coul�
    public void MarkHitElement(SeaElement element)
    {
        foreach (var boat in boatList)
        {
            if (boat.Elements.Contains(element))
            {
                element.Status = AppDef.SeaElementStatus.Hit;
                boat.IsSunk = boat.IsBoatSunk();

                if (boat.IsSunk)
                {
                    Console.WriteLine($"Le bateau {boat.Name} est coul� !");
                }

                return;
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
                // On cr�e un nouveau bateau avec une taille al�atoire
                int boatSize = val.Next(2, 6);
                boat = new Boat(boatSize);
                // calcul d'une position al�atoire
                // calcul d'un cap al�atoire
                int posX = val.Next(GridSize - boatSize);
                int posY = val.Next(GridSize - boatSize);
                int heading = val.Next(360);
                boat.Position = new Position(posX, posY);
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
    public void ShowBoat()
    {
        foreach (var elt in ShipElt)
        {
            Point location = new Point(Prow.X + elt.elt.X, Prow.Y + elt.elt.Y);
            MainP.drawEllipseOnCanvas(location, AppDef.shipBrush);
        }
    }

    // Affichage des bateaux d�un joueur
    // Affichage des bateaux d'un joueur
    public void ShowAllBoats()
    {
        foreach (Boat boat in boats)
        {
            boat.ShowBoat();
        }
    }

    // Gestion du tir effectu� par un joueur sur un �l�ment de mer :





    // il faut mettre � jour l'�tat des bateaux
    // Mettre en place la gestion du level et des tirs restants pour les joueurs
    // et d�cider de la fin de la partie
    public AppDef.State ProcessStrike(Guid playerID, SeaElement strikeElt)
    {
        // A compl�ter avec les m�thodes utiles...
        return AppDef.State.NotStarted;
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
            AppDef.State gameState = AppDef.State.NotStarted;
            while (gameState == AppDef.State.NotStarted)
            {
                gameState = player1.PlayTurn(this);
            }

            // V�rification de la fin de partie
            if (gameState == AppDef.State.GameFinished)
            {
                Console.WriteLine("Le joueur 1 a gagn� !");
                gameFinished = true;
                break;
            }

            // Tour du joueur 2
            gameState = AppDef.State.NotStarted;
            while (gameState == AppDef.State.NotStarted)
            {
                gameState = player2.PlayTurn(this);
            }

            // V�rification de la fin de partie
            if (gameState == AppDef.State.GameFinished)
            {
                Console.WriteLine("Le joueur 2 a gagn� !");
                gameFinished = true;
                break;
            }
        }
    }

}