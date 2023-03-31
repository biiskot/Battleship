using System;
using System.Diagnostics;
using Windows.Foundation;


public struct ShipElement
{
    // Status d'un élément de bateau = flotte, Touché
    public AppDef.State status;
    // Coordonnée de cet élément
    public Point coord;
    // besoin d'autre chose?

    public void changeShipElementStatus(AppDef.State state)
    {
        status = state;
    }

}
public class Boat
{
    private Guid owner { get; set; }
    public Guid Owner { get=>owner; set => owner = value; }

    // le vaisseau flotte encore, est touché ou est coulé
    public bool boolSunk;

    // coordonnées de la proue, avant du bateau
    public Point bow { get;}

    //Cap, pour savoir comment orienter les autres elements:
    public AppDef.Cap cap { get; }

    // taille du bateau en nombre d'éléments
    public int size;
    public int Size { get => size; }

    // Tableau d'élements de bateau qui constituent le corps du bateau
    public ShipElement[] ShipElt;

    public AppDef.State status;

    // constructeur
    public Boat( int size, Guid owner, Point bow, AppDef.Cap cap)
    {
        this.size = size;
        this.owner = owner;
        this.bow = bow;
        this.status = AppDef.State.Afloat;
        this.cap = cap;

        ShipElt = new ShipElement[size];
        for (int i = 0; i < size; i++)
        {
            switch(cap)
            {
                case AppDef.Cap.N:
                    ShipElt[i] = new ShipElement { status = AppDef.State.Afloat, coord = new Point(bow.X + i, bow.Y ) };
                    break;

                case AppDef.Cap.E:
                    ShipElt[i] = new ShipElement { status = AppDef.State.Afloat, coord = new Point(bow.X, bow.Y - i) };
                    break;
                case AppDef.Cap.S:
                    ShipElt[i] = new ShipElement { status = AppDef.State.Afloat, coord = new Point(bow.X - i, bow.Y) };
                    break;
                case AppDef.Cap.W:
                    ShipElt[i] = new ShipElement { status = AppDef.State.Afloat, coord = new Point(bow.X, bow.Y+i) };
                    break;
            }
            
        }
    }
    // Vérifie si le bateau est coulé
    // méthode qui vérifie si le bateau est coulé
    public bool isBoatSunk()
    {
        bool sank = true;
        foreach (ShipElement elt in ShipElt)
        {
            if (elt.coord != null && elt.status == AppDef.State.Struck)
            {
                status= AppDef.State.Struck;
               
            }
            else
            {
                sank = false;
            }
        }
        if(sank) {
            Debug.WriteLine("Bateau coulé ");
            this.status = AppDef.State.Sank;
        }
        
        return sank; // tous les éléments du bateau sont touchés
    }

    public bool CollidesWith(Boat boat)
    {
        foreach (ShipElement elt in ShipElt)
        {
            foreach (ShipElement elt2 in boat.ShipElt)
            {
                if (elt.coord == elt2.coord)
                {
                    return true;
                }
            }
        }
        return false;
    }
}