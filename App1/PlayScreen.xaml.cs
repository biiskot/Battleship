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
        public PlayScreen()
        {
            this.InitializeComponent();
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

 
// Fait apparaitre le point d'impact en rouge quand on clique sur une ellipse
 public static void EffectuerUnTir(object sender, PointerRoutedEventArgs e)
        {
            // si une partie est en cours d'exécution
            if (GamesManager.GameStatus == AppDef.GameStatus.Running)
            {
                AppDef.PlayerStatus playerStatus = bSF.GetPlayerStatus(myPlayerID);
                // si le joueur courant n'a pas encore perdu la partie
                if (playerStatus != AppDef.PlayerStatus.Loser)
                {
                    // si on a cliqué sur une ellipse, on la peint en rouge et on appelle la méthode
                    // qui va rechercher l'élément de mer concerné (sea.FireAt() )
                    if (sender is Windows.UI.Xaml.Shapes.Ellipse)
                    {
                        (sender as Ellipse).Fill = AppDef.redBrush;
                        // Déclenchement du tir
                        sea.FireAt(sender as Ellipse);
                    }
                }

            }


        }

    }
}
