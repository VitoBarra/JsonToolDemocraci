using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MVVM.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged// notificare il cambiamento dei dati
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        protected bool IsDesignMode
        {
            get { return DesignerProperties.GetIsInDesignMode(new DependencyObject()); }
        }
    }
}