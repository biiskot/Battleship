using System;

public struct ShipElement
{
    // Status d'un élément de bateau = flotte, Touché
    // Coordonnée de cet élément
    public Point elt;
    // besoin d'autre chose?
}
public class Boat
{
    // Type de bateau
    private string name;
    public string Name { get => name; }
    // propriétaire du bateau
    private Guid owner;
    public Guid Owner { get => owner; }

    // le vaisseau flotte encore, est touché ou est coulé
    private bool isSunk;
    public bool IsSunk { get => isSunk; }

    // coordonnées de la proue, avant du bateau
    private Point bow;
    public Point Bow { get => bow; }

    // taille du bateau en nombre d'éléments
    private int size;
    public int Size { get => size; }

    // allure du bateau.
    // On peut prévoir une implémentation où les bateaux se déplacent !
    private int pace;
    public int Pace { get => pace; }

    // Tableau d'élements de bateau qui constituent le corps du bateau
    public ShipElement[] ShipElt;

    // constructeur
    public Boat(string name, int size, int pace, Guid owner, Point bow)
    {
        this.name = name;
        this.size = size;
        this.pace = pace;
        this.owner = owner;
        this.bow = bow;

        ShipElt = new ShipElement[size];
        for (int i = 0; i < size; i++)
        {
            ShipElt[i] = new ShipElement { status = ShipElement.Status.Flotte, elt = new Point(bow.X, bow.Y + i) };
        }
    }

    // Vérifie si le bateau est touché par un tir sur l'élément donné
    public bool IsHit(Point elt)
    {
        foreach (ShipElement element in ShipElt)
        {
            if (element.elt == elt)
            {
                element.status = ShipElement.Status.Touche;
                return true;
            }
        }
        return false;
    }

    // Vérifie si le bateau est coulé
    // méthode qui vérifie si le bateau est coulé
    public bool IsSunk()
    {
        foreach (ShipElement elt in ShipElt)
        {
            if (elt.elt != null && !elt.elt.IsEmpty)
            {
                return false; // il reste au moins un élément du bateau à flot
            }
        }
        return true; // tous les éléments du bateau sont touchés
    }
}