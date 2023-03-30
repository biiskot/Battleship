using App1;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System.Diagnostics;

public class SeaElement
{
    // représentation graphique du SeaElement de type Windows.UI.Xaml.Shapes.Ellipse
    // permet de déterminer la position des évenements 'Tir'
    public Ellipse ellipse { get; set; }
    public Point coord;
    public int RegenCycles;
    public int row { get; set; }
    public int col { get; set; }
    public SeaElement(Thickness thic, int row, int col, int width, int height, int regenCycles)
    {
        RegenCycles = regenCycles;
        this.col = col;
        this.row = row;
        coord.X = col;
        coord.Y = row;

        //ellipse.Name = ellipse.Name;
        ellipse = new Ellipse();
        ellipse.Fill = AppDef.blueBrush;
        ellipse.Name = "ellipse_"+row.ToString()+"_"+col.ToString();
        ellipse.Height = height;
        ellipse.Width = width;

        ellipse.PointerPressed += Ellipse_PointerPressed;

    }
    private void Ellipse_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        Debug.WriteLine("Ellipse_PointerPressed() sur seaElement [" + this.col.ToString() + "," + this.row.ToString()+"]");
        PlayScreen.EffectuerUnTir(sender, e, GamesManager.sea);
    }
}

