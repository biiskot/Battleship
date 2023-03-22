using System;
using Windows.Foundation


public struct ShipElement
{
    // Status d'un �l�ment de bateau = flotte, Touch�
    public AppDef.State status;
    // Coordonn�e de cet �l�ment
    public Point elt;
    // besoin d'autre chose?
}
public class Boat
{
    // Type de bateau
    private string name;
    public string Name { get => name; }
    // propri�taire du bateau
    private Guid owner;
    public Guid Owner { get => owner; }

    // le vaisseau flotte encore, est touch� ou est coul�
    public bool boolSunk;

    // coordonn�es de la proue, avant du bateau
    private Point bow;
    public Point Bow { get => bow; }

    // taille du bateau en nombre d'�l�ments
    private int size;
    public int Size { get => size; }

    // allure du bateau.
    // On peut pr�voir une impl�mentation o� les bateaux se d�placent !
    private int pace;
    public int Pace { get => pace; }

    // Tableau d'�lements de bateau qui constituent le corps du bateau
    public ShipElement[] ShipElt;

    public AppDef.State status;

    // constructeur
    public Boat(string name, int size, int pace, Guid owner, Point bow)
    {
        this.name = name;
        this.size = size;
        this.pace = pace;
        this.owner = owner;
        this.bow = bow;
        this.status = AppDef.State.Afloat;

        ShipElt = new ShipElement[size];
        for (int i = 0; i < size; i++)
        {
            ShipElt[i] = new ShipElement { status = AppDef.State.Afloat, elt = new Point(bow.X, bow.Y + i) };
        }
    }

    // V�rifie si le bateau est touch� par un tir sur l'�l�ment donn�
    public bool IsHit(Point elt)
    {
        foreach (ShipElement element in ShipElt)
        {
            if (element.elt == elt)
            {
                element.status = AppDef.State.Struck;
                return true;
            }
        }
        return false;
    }

    // V�rifie si le bateau est coul�
    // m�thode qui v�rifie si le bateau est coul�
    public bool isBoatSunk()
    {
        foreach (ShipElement elt in ShipElt)
        {
            if (elt.elt != null && elt.status != AppDef.State.Struck)
            {
                return false; // il reste au moins un �l�ment du bateau � flot
            }
        }
        this.status = AppDef.State.Sank;
        return true; // tous les �l�ments du bateau sont touch�s
    }
}