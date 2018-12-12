using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuickEffect.ViewModels
{
    /// <summary>
    /// A base view model that implements the INotifyPropertyChanged interface.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
