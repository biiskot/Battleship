using App1;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml;
using Windows.Foundation;

public class SeaElement
{
    // représentation graphique du SeaElement de type Windows.UI.Xaml.Shapes.Ellipse
    // permet de déterminer la position des évenements 'Tir'
    public Ellipse ellipse = new Ellipse();
    public Point coord;
    public SeaElement(Thickness thic, int row, int col, int width, int height, int regenCycles)
    {
        RegenCycles = regenCycles;
        coord.X = col;
        coord.Y = row;
        ellipse.PointerPressed += PlayScreen.EffectuerUnTir;
        // ..
    }
}

