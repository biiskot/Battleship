using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class PlayScreen : Page
    {
        public BattleShipField battleshipField;
        public static Sea sea;
        public PlayScreen()
        {
            this.InitializeComponent();
            Background = new SolidColorBrush(Windows.UI.Colors.AntiqueWhite);
            sea = new Sea(AppDef.nbRow, AppDef.nbCol, this);
            SeaElement seaElement;
            // Créer une grille de 20x20 instances de SeaElement
            for (int row = 0; row < 20; row++)
            {
                for (int col = 0; col < 20; col++)
                {
                    // Remplacer les paramètres du constructeur par les valeurs appropriées pour votre jeu
                    seaElement = sea.seaGrid[row, col];

                    // Ajouter l'ellipse de l'instance de SeaElement à la grille
                    seaGrid.Children.Add(seaElement.ellipse);

                    // Définir la position de l'ellipse dans la grille
                    Grid.SetRow(seaElement.ellipse, row);
                    Grid.SetColumn(seaElement.ellipse, col);
                }
            }
            

            //On instancie un champ de bataille à la création du Play Screen
            try
            {
                // Création des joueurs
                Player player1 = new Player("joueur1", 10, AppDef.PlayerStatus.NotSet);
                Player player2 = new Player("joueur2", 10, AppDef.PlayerStatus.NotSet);
                List<Player> playerList = new List<Player>();
                playerList.Add(player1);
                playerList.Add(player2);

                // Création du champ de bataille
                battleshipField = new BattleShipField();
                sea = new Sea(AppDef.nbRow, AppDef.nbCol, this);

                Debug.WriteLine("Champ de bataille créé");
                //this.StartGame(playerList,battleshipField);
                Debug.WriteLine("Partie lancée");
            }
            catch(Exception e) {
                Debug.WriteLine(e.ToString());
            }
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }


        // Fait apparaitre le point d'impact en rouge quand on clique sur une ellipse
        public static void EffectuerUnTir(object sender, PointerRoutedEventArgs e, Sea sea)
        {
            Debug.WriteLine("EffectuerUnTir()");
            // si une partie est en cours d'exécution
            if (GamesManager.GameStatus == AppDef.GameStatus.Running)
            {

                /*
                AppDef.PlayerStatus playerStatus = BattleShipField.GetPlayerStatus(BattleShipField.activePlayer);
                // si le joueur courant n'a pas encore perdu la partie
                if (playerStatus != AppDef.PlayerStatus.Loser)
                {
                    // si on a cliqué sur une ellipse, on la peint en rouge et on appelle la méthode
                    // qui va rechercher l'élément de mer concerné (sea.FireAt() )
                    if (sender is Windows.UI.Xaml.Shapes.Ellipse)
                    {
                        (sender as Ellipse).Fill = AppDef.redBrush;
                        sea.FireAt(sender as Ellipse);
                    }
                }
                */
                if (sender is Windows.UI.Xaml.Shapes.Ellipse)
                {
                    (sender as Ellipse).Fill = AppDef.redBrush;
                    sea.FireAt(sender as Ellipse);
                }

            }


        }

        public static void printShipElement(Ellipse ellipse, RoutedEventArgs e)
        {

           ellipse.Fill = AppDef.pinkBrush;
        }
        public void StartGame(List<Player> players, BattleShipField bsf)
        {
            Debug.WriteLine("battleShipField.startGame()");
            Player player1 = players[0];
            Player player2 = players[1];
            //on change le stats des joueurs:

            Player activePlayer = null;


            player1.Status = AppDef.PlayerStatus.NotSet;
            player2.Status = AppDef.PlayerStatus.NotSet;


            // Création des bateaux pour chaque joueur
            bsf.CreateBoatsForPlayer(player1, new List<Boat>());
            bsf.CreateBoatsForPlayer(player2, player1.BoatList);


            /*
                    // Initialisation du timer pour la gestion des éléments de mer touchés
                    aTimer = new System.Timers.Timer(5000);
                    aTimer.Elapsed += async (sender, e) => {
                        //
                    };
                    aTimer.AutoReset = true;
                    aTimer.Enabled = true;

            */
            // Affichage de la grille pour chaque joueur
            player1.ShowGrid();
            player2.ShowGrid();

            // Boucle de jeu jusqu'à la fin de la partie
            bool gameFinished = false;
            while (!gameFinished)
            {
                // Tour du joueur 1
                AppDef.GameStatus gameState = AppDef.GameStatus.NotStarted;
                while (gameState == AppDef.GameStatus.NotStarted)
                {
                    player1.PlayTurn();
                    activePlayer = player1;
                }

                // Vérification de la fin de partie
                if (gameState == AppDef.GameStatus.Completed)
                {
                    Console.WriteLine("Le joueur 1 a gagné !");
                    player1.Status = AppDef.PlayerStatus.Winner;
                    player2.Status = AppDef.PlayerStatus.Loser;
                    gameFinished = true;
                    break;
                }

                // Tour du joueur 2
                gameState = AppDef.GameStatus.NotStarted;
                while (gameState == AppDef.GameStatus.NotStarted)
                {
                    gameState = player2.PlayTurn();
                    activePlayer = player2;
                }

                // Vérification de la fin de partie
                if (gameState == AppDef.GameStatus.Completed)
                {
                    Console.WriteLine("Le joueur 2 a gagné !");
                    player2.Status = AppDef.PlayerStatus.Winner;
                    player1.Status = AppDef.PlayerStatus.Loser;
                    gameFinished = true;
                    break;
                }
            }
        }
    }
}
