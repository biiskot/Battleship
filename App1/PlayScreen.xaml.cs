using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
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

        public PlayScreen()
        {
            this.InitializeComponent();
            Background = new SolidColorBrush(Windows.UI.Colors.AntiqueWhite);
            GamesManager.sea = new Sea(AppDef.nbRow, AppDef.nbCol, this);
            SeaElement seaElement;
            // Créer une grille de 20x20 instances de SeaElement
            for (int row = 0; row < AppDef.nbRow; row++)
            {
                for (int col = 0; col < AppDef.nbCol; col++)
                {
                    // Remplacer les paramètres du constructeur par les valeurs appropriées pour votre jeu
                    seaElement = GamesManager.sea.seaGrid[row, col];

                    // Ajouter l'ellipse de l'instance de SeaElement à la grille
                    seaGridXML.Children.Add(seaElement.ellipse);
                    
                    // Définir la position de l'ellipse dans la grille
                    Grid.SetRow(seaElement.ellipse, seaElement.row);
                    Grid.SetColumn(seaElement.ellipse, seaElement.col);
                }
            }     

            //On instancie un champ de bataille à la création du Play Screen
            try
            {
                // Création des joueurs
                Player player1 = new Player("joueur1", 10, AppDef.PlayerStatus.NotSet);
                Player player2 = new Player("joueur2", 10, AppDef.PlayerStatus.NotSet);
                GamesManager.playerList = new List<Player>
                {
                    player1,
                    player2
                };

                // Création du champ de bataille
                battleshipField = new BattleShipField();
                GamesManager.sea = new Sea(AppDef.nbRow, AppDef.nbCol, this);

                battleshipField.CreateBoatsForPlayer(player1);
                battleshipField.CreateBoatsForPlayer(player2);

                Debug.WriteLine("Champ de bataille créé");
                
                Debug.WriteLine("Partie lancée");
                GamesManager.GameStatus = AppDef.GameStatus.Running;

                Debug.WriteLine("C'est au joueur 1 de commencer");
                GamesManager.activePlayer = GamesManager.playerList[0];
            }

            catch(Exception e) {
                Debug.WriteLine(e.ToString());
            }

            j1Tirs.Text = GamesManager.playerList[0].NbStruck.ToString();
            j2Tirs.Text = GamesManager.playerList[0].NbStruck.ToString();
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        // Fait apparaitre le point d'impact en rouge quand on clique sur une ellipse
        public static void EffectuerUnTir(object sender, PointerRoutedEventArgs e, Sea sea)
        {
            Debug.WriteLine("EffectuerUnTir()");
            Debug.WriteLine("active player : "+GamesManager.activePlayer.Pseudo);
            // si une partie est en cours d'exécution
            if (GamesManager.GameStatus == AppDef.GameStatus.Running)
            {
                Debug.WriteLine("Le partie est bien lancée.");
                
                AppDef.PlayerStatus playerStatus = GamesManager.activePlayer.Status;
                // si le joueur courant n'a pas encore perdu la partie
                if (playerStatus != AppDef.PlayerStatus.Loser)
                {
                    // si on a cliqué sur une ellipse, on la peint en rouge et on appelle la méthode
                    // qui va rechercher l'élément de mer concerné (sea.FireAt() )
                    if (sender is Windows.UI.Xaml.Shapes.Ellipse)
                    {
                        (sender as Ellipse).Fill = AppDef.redBrush;
                        Debug.WriteLine("Le joueur " + GamesManager.activePlayer.Pseudo + " envoie un tir");
                        sea.FireAt(sender as Ellipse);

                       
                    }
                }
            }


        }

        public static void displayShipElement(Ellipse ellipse, RoutedEventArgs e)
        {

           ellipse.Fill = AppDef.pinkBrush;
        }


        /*
        public void StartGame(List<Player> players, BattleShipField bsf)
        {
            
            Debug.WriteLine("battleShipField.startGame()");

            GamesManager.GameStatus = AppDef.GameStatus.Running;

            //GamesManager.GameGuid = new Random()

            Player player1 = players[0];
            Player player2 = players[1];
            //on change le stats des joueurs:


            player1.Status = AppDef.PlayerStatus.NotSet;
            player2.Status = AppDef.PlayerStatus.NotSet;


            // Création des bateaux pour chaque joueur
            bsf.CreateBoatsForPlayer(player1, new List<Boat>());
            bsf.CreateBoatsForPlayer(player2, player1.BoatList);

            
                    // Initialisation du timer pour la gestion des éléments de mer touchés
                    aTimer = new System.Timers.Timer(5000);
                    aTimer.Elapsed += async (sender, e) => {
                        //
                    };
                    aTimer.AutoReset = true;
                    aTimer.Enabled = true;

            

            // Affichage de la grille pour chaque joueur
            player1.ShowGrid();
            player2.ShowGrid();


            // Boucle de jeu jusqu'à la fin de la partie
            while (GamesManager.GameStatus == AppDef.GameStatus.Running)
            {
                if(GamesManager.activePlayer == player1)
                {
                    player1.PlayTurn();
                    GamesManager.activePlayer = player2;
                }
                else if(GamesManager.activePlayer == player2)
                {
                    player2.PlayTurn();
                    GamesManager.activePlayer = player1;
                }

                // Vérification de la fin de partie
                if (GamesManager.GameStatus == AppDef.GameStatus.Completed)
                {
                    if (GamesManager.activePlayer == player1)
                    {
                        Console.WriteLine("Le joueur 2 a gagné !");
                        {
                            player2.Status = AppDef.PlayerStatus.Winner;
                            player1.Status = AppDef.PlayerStatus.Loser;
                        }
                    }
                    else if (GamesManager.activePlayer == player2)
                    {
                        Console.WriteLine("Le joueur 1 a gagné !");
                        player1.Status = AppDef.PlayerStatus.Winner;
                        player2.Status = AppDef.PlayerStatus.Loser;
                        //gameFinished = true;
                    }
                }
            }
                
        }
        */
    }
}
