using System;
using Windows.Foundation;


public struct ShipElement
{
    // Status d'un élément de bateau = flotte, Touché
    public AppDef.State status;
    // Coordonnée de cet élément
    public Point coord;
    // besoin d'autre chose?

    public void changeElementStatus(AppDef.State state)
    {
        status = state;
    }

}
public class Boat
{
    private Guid owner { get; set; }

    // le vaisseau flotte encore, est touché ou est coulé
    public bool boolSunk;

    // coordonnées de la proue, avant du bateau
    public Point bow { get;}

    // taille du bateau en nombre d'éléments
    public int size;
    public int Size { get => size; }

    // allure du bateau.
    // On peut prévoir une implémentation où les bateaux se déplacent !


    // Tableau d'élements de bateau qui constituent le corps du bateau
    public ShipElement[] ShipElt;

    public AppDef.State status;

    // constructeur
    public Boat( int size, Guid owner, Point bow)
    {
        this.size = size;
        this.owner = owner;
        this.bow = bow;
        this.status = AppDef.State.Afloat;

        ShipElt = new ShipElement[size];
        for (int i = 0; i < size; i++)
        {
            ShipElt[i] = new ShipElement { status = AppDef.State.Afloat, coord = new Point(bow.X, bow.Y + i) };
        }
    }
    // Vérifie si le bateau est coulé
    // méthode qui vérifie si le bateau est coulé
    public bool isBoatSunk()
    {
        foreach (ShipElement elt in ShipElt)
        {
            if (elt.coord != null && elt.status != AppDef.State.Struck)
            {
                return false; // il reste au moins un élément du bateau à flot
            }
        }
        this.status = AppDef.State.Sank;
        return true; // tous les éléments du bateau sont touchés
    }
}