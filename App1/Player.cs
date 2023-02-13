using System;

public class Player
{
    public String Pseudo { get; set; }
    public Guid PlayerID { get; }
    public AppDef.PlayerStatus Status { get; set; }

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
}
