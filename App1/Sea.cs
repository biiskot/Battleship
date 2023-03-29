using App1;
using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml;
using System;
using System.Diagnostics;

// Gestion de l'affichage de la mer pour un joueur
public class Sea
{
    public List<SeaElement> SeaElements = new List<SeaElement>();
    public int Row { get; set; }
    public int Col { get; set; }
    private PlayScreen screenGame;
    public SeaElement[,] seaGrid;
    public Sea(int row, int col, PlayScreen playscreen)
    {
        Row = row;
        Col = col;
        screenGame = playscreen;
        int ellipseWidth = AppDef.largeurMer / row;
        int ellipseHeight = AppDef.hauteurMer / col;
        int leftMargin = 0, rightMargin = 0, topMargin = 0, bottomMargin = 0;
        Thickness thic = new Thickness(leftMargin, topMargin, rightMargin, bottomMargin);

        seaGrid = new SeaElement[row, col];

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                seaGrid[i, j] = new SeaElement(new Thickness(10),i, j,AppDef.largeurMer/AppDef.nbCol, AppDef.hauteurMer / AppDef.nbRow, 10);
            }
        }
        Debug.WriteLine("constructor Sea");
        Debug.WriteLine(seaGrid[0,0].ToString());
        Debug.WriteLine(seaGrid[19, 19].ToString());
        // ...
    }
    // m�thode charg�e de retouver l'�l�ment de mer atteint
    // et qui communique la position du tir, en ligne colonne, au champ de bataille qui va �tudier
    // les cons�quences du tir
    public void FireAt(Ellipse ellipse)
    {
        Debug.WriteLine("FireAt()");
        // code de retour de la m�thode ProcessStrike
        AppDef.State code = 0;
        // Element de mer impact�
        SeaElement impactPoint;
        try
        {
            impactPoint = SeaElements.Find(sealElement => sealElement.ellipse == ellipse);
            if (impactPoint != null)
            {
                code = screenGame.battleshipField.ProcessStrike(GamesManager.activePlayer.PlayerID,impactPoint);
            }
            else {
                Debug.WriteLine("impactPoint null");
            }
           
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Exception FireAt(): ", ex.Message);
        }

        //On d�cr�mente le n de tirs restants 
        GamesManager.activePlayer.RemainStrike--;

        //Et on incr�mente le score si le tir touche un shipelem
        if (code == AppDef.State.Afloat)
        {
            Debug.WriteLine("Le tir n'a pas touch� de shipelem");
        }
        else if (code == AppDef.State.Struck)
        {
            Debug.WriteLine("Le tir a touch� un shipelem");
            GamesManager.activePlayer.NbStruck += 1;
        }
        else
        {
            Debug.WriteLine("code n'a pas de valeur retourn�e");

        }

        //On v�rifie si le joueur a gagn�

        //Si non, tour siuvant, on change d'activePlayer

        GamesManager.activePlayer = GamesManager.getOponentPlayerObject(GamesManager.activePlayer.PlayerID,GamesManager.playerList);
        Debug.WriteLine("Le joueur actif est : " + GamesManager.activePlayer.PlayerID);
    }
    // redessin de la mer en bleu
    public void Repaint()
    {
        Debug.WriteLine("Repaint()");
        foreach (var item in SeaElements)
        {
            item.ellipse.Fill = AppDef.blueBrush;
        }
    }
}