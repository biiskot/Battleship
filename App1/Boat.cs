using System;
using Windows.Foundation;


public struct ShipElement
{
    // Status d'un élément de bateau = flotte, Touché
    public AppDef.State status;
    // Coordonnée de cet élément
    public Point coord;
    // besoin d'autre chose?
}
public class Boat
{
    private string owner { get; set; }

    // le vaisseau flotte encore, est touché ou est coulé
    public bool boolSunk;

    // coordonnées de la proue, avant du bateau
    private Point bow { get;}

    // taille du bateau en nombre d'éléments
    private int size;
    public int Size { get => size; }

    // allure du bateau.
    // On peut prévoir une implémentation où les bateaux se déplacent !


    // Tableau d'élements de bateau qui constituent le corps du bateau
    public ShipElement[] ShipElt;

    public AppDef.State status;

    // constructeur
    public Boat( int size, string owner, Point bow)
    {
        this.size = size;
        this.owner = owner;
        this.bow = bow;
        this.status = AppDef.State.Afloat;

        ShipElt = new ShipElement[size];
        for (int i = 0; i < size; i++)
        {
            ShipElt[i] = new ShipElement { status = AppDef.State.Afloat, elt = new Point(bow.X, bow.Y + i) };
        }
    }

    // Vérifie si le bateau est touché par un tir sur l'élément donné
    public bool IsHit(Point elt)
    {
        foreach (ShipElement element in ShipElt)
        {
            if (element.coord == elt)
            {
                //element.status = AppDef.State.Struck;
                return true;
            }
        }
        return false;
    }

    // Vérifie si le bateau est coulé
    // méthode qui vérifie si le bateau est coulé
    public bool isBoatSunk()
    {
        foreach (ShipElement elt in ShipElt)
        {
            if (elt.elt != null && elt.status != AppDef.State.Struck)
            {
                return false; // il reste au moins un élément du bateau à flot
            }
        }
        this.status = AppDef.State.Sank;
        return true; // tous les éléments du bateau sont touchés
    }
}