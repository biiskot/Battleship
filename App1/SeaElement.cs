using App1;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml;

public class SeaElement
{
    // représentation graphique du SeaElement de type Windows.UI.Xaml.Shapes.Ellipse
    // permet de déterminer la position des évenements 'Tir'
    public Ellipse ellipse = new Ellipse();
    // ..
    public SeaElement(Thickness thic, int row, int col, int width, int height, int regenCycles)
    {
        RegenCycles = regenCycles;
        pos.X = col;
        pos.Y = row;
        ellipse.PointerPressed += MainPage.EffectuerUnTir;
        // ..
    }
}

