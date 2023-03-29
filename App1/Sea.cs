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
                seaGrid[i, j] = new SeaElement(new Thickness(3),i, j,AppDef.largeurMer/AppDef.nbCol, AppDef.hauteurMer / AppDef.nbRow, 5);
            }
        }
        Debug.WriteLine("constrcteur Sea");
        Debug.WriteLine(seaGrid.Length);
        // ...
    }
    // méthode chargée de retouver l'élément de mer atteint
    // et qui communique la position du tir, en ligne colonne, au champ de bataille qui va étudier
    // les conséquences du tir
    public void FireAt(Ellipse ellipse)
    {
        // code de retour de la méthode ProcessStrike
        AppDef.State code = 0;
        // Element de mer impacté
        SeaElement impactPoint;
        try
        {
            impactPoint = SeaElements.Find(sealElement => sealElement.ellipse == ellipse);
            if (impactPoint != null)
            {
                code = screenGame.battleshipField.ProcessStrike(new Guid(),impactPoint);
            }
      
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception FireAt(): ", ex.Message);
        }
    }
    // redessin de la mer en bleu
    public void Repaint()
    {
        foreach (var item in SeaElements)
        {
            item.ellipse.Fill = AppDef.blueBrush;
        }
    }
}