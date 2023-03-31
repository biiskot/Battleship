using Newtonsoft.Json;
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
using static AppDef;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class PlayScreen : Page
    {
        public BattleShipField battleshipField;
        public string j1pseudo;
        public string j2pseudo;


        public PlayScreen()
        {
            this.InitializeComponent();
            Background = new SolidColorBrush(Windows.UI.Colors.AntiqueWhite);
            GamesManager.sea = new Sea(AppDef.nbRow, AppDef.nbCol, this);

            // Créer une grille de 20x20 instances de SeaElement
            for (int row = 0; row < AppDef.nbRow; row++)
            {
                for (int col = 0; col < AppDef.nbCol; col++)
                {
                    // Remplacer les paramètres du constructeur par les valeurs appropriées pour votre jeu
                    

                    // Ajouter l'ellipse de l'instance de SeaElement à la grille
                    seaGridXML.Children.Add(GamesManager.sea.seaGrid[row, col].ellipse);
                    
                    // Définir la position de l'ellipse dans la grille
                    Grid.SetRow(GamesManager.sea.seaGrid[row, col].ellipse, GamesManager.sea.seaGrid[row, col].row);
                    Grid.SetColumn(GamesManager.sea.seaGrid[row, col].ellipse, GamesManager.sea.seaGrid[row, col].col);
                }
            }     
            pinkEllipse.Fill = AppDef.pinkBrush;
            blueEllipse.Fill = AppDef.blueBrush;
            greenEllipse.Fill = AppDef.greenBrush;
            orangeEllipse.Fill = AppDef.orangeBrush;

            try
            { 
                showBoatsBtn.Click += (sender, e1) =>
                {
                    battleshipField.ShowAllBoats();
                    foreach (SeaElement elt in GamesManager.sea.SeaElements)
                    {
                        foreach (SeaElement el in GamesManager.elementsJ1)
                        {
                            if (elt.coord == el.coord)
                            {
                                elt.ellipse.Fill = AppDef.greenBrush;
                                try
                                {
                                    seaGridXML.Children.Add(elt.ellipse);
                                    Grid.SetRow(elt.ellipse, elt.row);
                                    Grid.SetColumn(elt.ellipse, elt.col);
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.ToString());
                                }

                            }
                            foreach (SeaElement el2 in GamesManager.elementsJ2)
                            {
                                if (elt.coord == el2.coord)
                                {
                                    elt.ellipse.Fill = AppDef.orangeBrush;
                                    try
                                    {
                                        seaGridXML.Children.Add(elt.ellipse);
                                        Grid.SetRow(elt.ellipse, elt.row);
                                        Grid.SetColumn(elt.ellipse, elt.col);
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine(ex.ToString());
                                    }

                                }
                            }
                        }
                    }
                };

                seaGridXML.Tapped += (sender, e2) =>
                {
                    j1Score.Text = GamesManager.playerList[0].Pseudo + " : " + GamesManager.playerList[0].NbStruck.ToString();
                    j2Score.Text = GamesManager.playerList[1].Pseudo + " : " + GamesManager.playerList[1].NbStruck.ToString();

                    j1Tirs.Text = GamesManager.playerList[0].Pseudo + " : " + GamesManager.playerList[0].RemainStrike.ToString();
                    j2Tirs.Text = GamesManager.playerList[1].Pseudo + " : " + GamesManager.playerList[1].RemainStrike.ToString();

                    logTxt.Text = GamesManager.log;
                    tourTxt.Text = "C'est au tour de "+GamesManager.activePlayer.Pseudo + " de tirer";
                };
                repaintBtn.Click += (sender, e) =>
                {
                    GamesManager.sea.Repaint();
                };
            }

            catch(Exception exp) {
                Debug.WriteLine(exp.ToString());
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            var parameters = JsonConvert.DeserializeObject<dynamic>(e.Parameter as string);

            j1pseudo = parameters.Pseudo1;
            j2pseudo = parameters.Pseudo2;
            string tmp = parameters.Nbboats ;
            GamesManager.nbBoats = Int16.Parse(tmp);

            Player player1 = new Player(j1pseudo, 10, AppDef.PlayerStatus.NotSet);
            Player player2 = new Player(j2pseudo, 10, AppDef.PlayerStatus.NotSet);

            GamesManager.playerList = new List<Player>();
            Debug.WriteLine(player1.Pseudo);
            GamesManager.playerList.Add(player1);
            GamesManager.playerList.Add(player2);

            battleshipField = new BattleShipField();
            GamesManager.sea = new Sea(AppDef.nbRow, AppDef.nbCol, this);
            battleshipField.CreateBoatsForPlayer(GamesManager.playerList[0],GamesManager.nbBoats);
            battleshipField.CreateBoatsForPlayer(GamesManager.playerList[1], GamesManager.nbBoats);


            

            Debug.WriteLine("Champ de bataille créé");

            Debug.WriteLine("Partie lancée");
            GamesManager.GameStatus = AppDef.GameStatus.Running;

            Debug.WriteLine("C'est au joueur 1 de commencer");
            GamesManager.activePlayer = GamesManager.playerList[0];
            tourTxt.Text = "C'est au tour de " + GamesManager.activePlayer.Pseudo + " de tirer";


            j1Score.Text = GamesManager.playerList[0].Pseudo + " : " + GamesManager.playerList[0].NbStruck.ToString();
            j2Score.Text = GamesManager.playerList[1].Pseudo + " : " + GamesManager.playerList[1].NbStruck.ToString();

            j1Tirs.Text = GamesManager.playerList[0].Pseudo + " : " + GamesManager.playerList[0].RemainStrike.ToString();
            j2Tirs.Text = GamesManager.playerList[1].Pseudo + " : " + GamesManager.playerList[1].RemainStrike.ToString();

         

            base.OnNavigatedTo(e);

        }
        // Fait apparaitre le point d'impact en rouge quand on clique sur une ellipse
        public static void EffectuerUnTir(object sender, PointerRoutedEventArgs e, Sea sea)
        {
            Debug.WriteLine("EffectuerUnTir()");
            Debug.WriteLine("active player : "+GamesManager.activePlayer.Pseudo);
            // si une partie est en cours d'exécution
            if (GamesManager.GameStatus == AppDef.GameStatus.Running && GamesManager.GameStatus!=AppDef.GameStatus.Completed)
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
            else if (GamesManager.GameStatus == AppDef.GameStatus.Completed)
            {
                // Créer une nouvelle fenêtre
                var newWindow = new MainPage();

                // Définir le contenu de la nouvelle fenêtre
                newWindow.Content = new MainPage();

                // Changer la fenêtre actuelle
                Window.Current.Content = newWindow;
            }
        }
    }
}
