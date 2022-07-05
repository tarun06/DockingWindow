using System.Windows;
using System.Windows.Controls;

namespace DockingLib
{
    public class FloatingWindow : Window
    {
        public FloatingWindow(Control control)
        {
            Width = 300;
            Height = 300;
            Content = control;
            AllowDrop = true;
        }
    }
}