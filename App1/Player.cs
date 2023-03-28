// Le player est lui susceptible de participer à plusieurs parties en même temps.
using App1;
using System;
using System.Collections.Generic;

public class Player
{
    public String Pseudo { get; set; }
    public Guid PlayerID { get; }
    public AppDef.PlayerStatus Status { get; set; }
    
    public List<Boat> BoatList;
    public Player(string pseudo, int strikeCredit, AppDef.PlayerStatus status)
    {
        PlayerID = Guid.NewGuid();
        Pseudo = pseudo;
        RemainStrike = strikeCredit;
        Status = status;
    }
    // nombre de tirs restants pour le joueur
    public int RemainStrike { get; set; }
    // nombre de de tirs ayant touché un bateau
    private int nbStruck = 0;
    public int NbStruck { get => nbStruck; set => nbStruck = value; }

    public void ShowGrid()
    {

    }
    public AppDef.GameStatus PlayTurn()
    {
          return AppDef.GameStatus.NotStarted;
    }

}