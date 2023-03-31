using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void launchGame(object sender, RoutedEventArgs e)
        {
            string j1pseudo = pseudoj1.Text;
            string j2pseudo= pseudoj2.Text;

            if(j1pseudo != j2pseudo && j1pseudo!=null && j2pseudo!=null) {

                // Create an object to hold the two pseudonyms
                var parameters = new { Pseudo1 = j1pseudo, Pseudo2 = j2pseudo };
                // Serialize the object into a string
                string param = JsonConvert.SerializeObject(parameters);
                // Pass the serialized object as a parameter when navigating to the PlayScreen page
                this.Frame.Navigate(typeof(PlayScreen), param);
            }
            else
            {
                warningTxt.Text = "Veuillez entrez des noms pour chacun des joueurs";
            }
           
        }
     
    }
}
