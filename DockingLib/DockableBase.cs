using System.Windows.Controls;

namespace DockingLib
{
    public abstract class DockableBase : UserControl, IDockable
    {
        public DockableBase(string title)
        {
            Title = title;
        }

        public string Title { get; }
    }
}