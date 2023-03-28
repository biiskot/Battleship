using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            // Créer une grille de 20x20 instances de SeaElement
            for (int row = 0; row < 20; row++)
            {
                for (int col = 0; col < 20; col++)
                {
                    // Remplacer les paramètres du constructeur par les valeurs appropriées pour votre jeu
                    var seaElement = new SeaElement(new Thickness(), row, col, 30, 30, 0);

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

                Console.WriteLine("Champ de bataille créé");
                battleshipField.StartGame(playerList);
                Console.WriteLine("Partie lancée");
            }
            catch(Exception e) { }
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }


        // Fait apparaitre le point d'impact en rouge quand on clique sur une ellipse
        public static void EffectuerUnTir(object sender, PointerRoutedEventArgs e, Sea sea)
        {
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

    }
}
