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
using Windows.UI.Core;
using System.Diagnostics;
using System.Xml.Linq;

// La classe BattleShipField est le moteur du jeu de bataille
// elle ne doit pas interférer avec le rendu graphique.
// elle permet d'instancier une partie
public class BattleShipField
{

    // générateur de nombres aléatoires
    private Random val = new Random();
    // niveau de jeu
    // Level : joue sur le nombre de tirs autorisés par exemple
    public int Level { get; set; }
 
    // La flotte qui contient les vaisseaux de tous les joueurs.
    public List<Boat> boatList = new List<Boat>();

    public int size;
   


    // si un bateau est touché, il faut le retrouver et marquer un de ses éléments 'touché' et
    // vérifier s'il n'est pas coulé

    // A compléter avec toutes les méthodes utiles …
    // Création de l'ensemble des bateaux de chaque joueur
    public void CreateBoatsForPlayer(Player player,int nb)
    {
        for (int i = 0; i <nb; i++)
        {
            bool positionFound = false;
            while (!positionFound)
            {
                Boat boat = null;
                // On crée un nouveau bateau avec une taille aléatoire
                int boatSize = val.Next(2, 5);
                // calcul d'une position aléatoire
                // calcul d'un cap aléatoire
                int posX = val.Next(AppDef.nbCol - boatSize*2);
                int posY = val.Next(AppDef.nbRow - boatSize*2);
                AppDef.Cap[] caps = new AppDef.Cap[] { AppDef.Cap.N, AppDef.Cap.E, AppDef.Cap.S, AppDef.Cap.W };
                AppDef.Cap heading = caps[val.Next(caps.Length)];

                boat = new Boat(boatSize,player.PlayerID,new Point(posX,posY),heading);            
              
                // test la collision de deux bateaux, utile pour le placement
                bool collision = false;
                foreach (Boat existingBoat in boatList)
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
                try
                {
                    // On ajoute le bateau à la liste des bateaux du joueur
                    player.nbELements += boat.Size;
                    player.BoatList.Add(boat);
                    // On ajoute le bateau à la liste globale de tous les bateaux
                    boatList.Add(boat);
                }
                catch (Exception e){
                    Debug.WriteLine(e.ToString());
                }
            }
        }
    }

    // recherche un emplacement disponible sur la mer et fixe la position d'un bateau

    // Affichage d’un bateau
    // Affichage d'un bateau
    public List<SeaElement> ShowBoat(Boat boat)
    {
        List<SeaElement> elements = new List<SeaElement>();

        foreach (var elt in boat.ShipElt)
        {
            foreach(SeaElement seaElt in GamesManager.sea.SeaElements)
            {
                if (seaElt.coord == elt.coord)  //Si un élément d'un bateau est sur un seaElement
                {
                    if (seaElt.ellipse is Windows.UI.Xaml.Shapes.Ellipse)
                    { 
                        elements.Add(seaElt);
                    }

                }
            }   
        }
        return elements;
    }

    // Affichage des bateaux d’un joueur
    // Affichage des bateaux d'un joueur
    public void ShowAllBoats()
    {
        Debug.WriteLine("ShowAllBoats()");
        foreach (Boat boat in boatList)
        {
            if (GamesManager.getPlayerObject(boat.Owner) == GamesManager.playerList[0]) {
                GamesManager.elementsJ1.AddRange(ShowBoat(boat));
            }
            else if(GamesManager.getPlayerObject(boat.Owner) == GamesManager.playerList[1])
            {
                GamesManager.elementsJ2.AddRange(ShowBoat(boat));
            }
            
            else {
                Debug.WriteLine("Pas de proprietaire trouvé");
            }
            
        }
    }

    // Gestion du tir effectué par un joueur sur un élément de mer :
    public AppDef.State ProcessStrike(Guid playerID, SeaElement strikeElt)
    {
        Debug.WriteLine("BSF.ProcessStike()");
        // il faut mettre à jour l'état des bateaux
        // Mettre en place la gestion du level et des tirs restants pour les joueurs
        // et décider de la fin de la partie
     
        GamesManager.impactedSeaElements.Add(strikeElt);
        foreach (Boat boat in boatList)
        {
            foreach(ShipElement element in boat.ShipElt)
            {
                if (element.coord == strikeElt.coord)        //Si shipElement à l'emplacement du seaElement touché :
                {
                    if(GamesManager.getPlayerObject(boat.Owner)!=GamesManager.activePlayer)
                    {
                        GamesManager.log = "Le tir a touché le bateau de " + GamesManager.getPlayerObject(boat.Owner).Pseudo;
                        element.changeShipElementStatus(AppDef.State.Struck);   //Si element touché, on change son statut
                        strikeElt.ellipse.Fill = AppDef.pinkBrush;            //On change la couleur du seaElement
                        boat.status = AppDef.State.Struck;
                        boat.isBoatSunk();
                        return AppDef.State.Struck;
                    }
                    else
                    {
                        Debug.WriteLine( "Vous ne pouvez pas tirer sur votre propre bateau");
                    }
                }
                else
                {
                    GamesManager.log = "Le tir n'a pas touché de bateau adverse";
                }
            }
        }
        
        return AppDef.State.Afloat;
    }


    public static AppDef.PlayerStatus GetPlayerStatus(Guid PID,List<Player> players)
    {
        foreach(Player player in players)
        {
            if(player.PlayerID == PID)
            {
                return player.Status;
            }
        }
        return AppDef.PlayerStatus.NotSet;
    }
    

}