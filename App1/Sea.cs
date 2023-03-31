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
        SeaElement tmpElt;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                tmpElt = new SeaElement(new Thickness(10),i, j,AppDef.largeurMer/AppDef.nbCol, AppDef.hauteurMer / AppDef.nbRow, 10);
                seaGrid[i, j] = tmpElt;
                SeaElements.Add(tmpElt);
            }
        }
        Debug.WriteLine("constructor Sea");
        // ...
    }
    // méthode chargée de retouver l'élément de mer atteint
    // et qui communique la position du tir, en ligne colonne, au champ de bataille qui va étudier
    // les conséquences du tir
    public void FireAt(Ellipse ellipse)
    {
        Debug.WriteLine("FireAt()");
        
        // code de retour de la méthode ProcessStrike
        AppDef.State code = 0;

        // Element de mer impacté
        SeaElement impactPoint;
      
        impactPoint = SeaElements.Find(sealElement => sealElement.ellipse == ellipse);

        foreach (SeaElement elt in SeaElements)
        {
            if (ellipse.Name == elt.ellipse.Name)
            {
                impactPoint = elt;
            }
        }

        if (impactPoint != null)
        {
            code = screenGame.battleshipField.ProcessStrike(GamesManager.activePlayer.PlayerID,impactPoint);
            Debug.WriteLine("impactPoint not null : ["+impactPoint.row.ToString()+","+impactPoint.col.ToString());
        }
        else {
            Debug.WriteLine("impactPoint null");
        }
      

        //On décrémente le n de tirs restants 
        GamesManager.activePlayer.RemainStrike--;

        //Et on incrémente le score si le tir touche un shipelem
        if (code == AppDef.State.Afloat)
        {
            Debug.WriteLine("Le tir n'a pas touché de shipelem");
        }
        else if (code == AppDef.State.Struck)
        {
            Debug.WriteLine("Le tir a touché un shipelem");
            GamesManager.activePlayer.NbStruck += 1;
        }
        else
        {
            Debug.WriteLine("code n'a pas de valeur retournée");

        }

        //On vérifie si le joueur a gagné
        if (!GamesManager.isGameFinished(GamesManager.playerList))
        {
            //Si personne n'a win, tour suivant, on change d'activePlayer

            GamesManager.activePlayer = GamesManager.getOponentPlayerObject(GamesManager.activePlayer.PlayerID, GamesManager.playerList);
            Debug.WriteLine("Fin du tour, c'est au tour de " + GamesManager.activePlayer.Pseudo);
        }
        else
        {
            
        }
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