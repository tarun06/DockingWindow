using DockingLib;
using System.Windows.Controls;

namespace DockingWindow
{
    /// <summary>
    /// Interaction logic for PropertyUc.xaml
    /// </summary>
    public partial class PropertyUc : DockableBase
    {
        public PropertyUc(string title) : base(title)
        {
            InitializeComponent();
        }
    }
}