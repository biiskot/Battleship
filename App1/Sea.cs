using App1;
using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml;
using System;

// Gestion de l'affichage de la mer pour un joueur
public class Sea
{
    public List<SeaElement> SeaElements = new List<SeaElement>();
    public int Row { get; set; }
    public int Col { get; set; }
    private MainPage MainP;
    public Sea(int row, int col, MainPage main)
    {
        Row = row;
        Col = col;
        MainP = main;
        int ellipseWidth = AppDef.largeurMer / row;
        int ellipseHeight = AppDef.hauteurMer / col;
        int leftMargin = 0, rightMargin = 0, topMargin = 0, bottomMargin = 0;
        Thickness thic = new Thickness(leftMargin, topMargin, rightMargin, bottomMargin);
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

            }
            // ...
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