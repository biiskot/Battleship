// Le player est lui susceptible de participer à plusieurs parties en même temps.
using App1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Notifications;

public class Player
{
    public String Pseudo { get; set; }
    public Guid PlayerID { get; }
    public AppDef.PlayerStatus Status { get; set; }
    
    public List<Boat> BoatList;

    // nombre de tirs restants pour le joueur
    public int RemainStrike { get; set; }
    // nombre de de tirs ayant touché un bateau
    private int nbStruck = 0;
    public int NbStruck { get => nbStruck; set => nbStruck = value; }

    public int nbELements; 
    public Player(string pseudo, int strikeCredit, AppDef.PlayerStatus status)
    {
        PlayerID = Guid.NewGuid();
        Pseudo = pseudo;
        RemainStrike = strikeCredit;
        Status = status;
        BoatList = new List<Boat>();
        RemainStrike = 20;
    }
    
    public bool isAllMyBoatsSunk()
    {
        bool allSunk = true;
        
        foreach (Boat boat in BoatList)
        {
            if (!boat.isBoatSunk())
            {
                allSunk = false;
            }
            else
            {
                Debug.WriteLine("Bateau de " + boat.Owner + " est coulé");
            }
        }
        return allSunk;
    }

    public void ShowGrid()
    {

    }

}