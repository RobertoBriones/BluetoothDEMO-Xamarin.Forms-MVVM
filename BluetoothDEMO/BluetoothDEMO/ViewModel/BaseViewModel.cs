using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace BluetoothDEMO.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected INavigation Navigation { get; set; }

        private string _AppName;
        protected string AppName { get { return _AppName; } set { _AppName = value; OnPropertyChanged("AppName"); } }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool isBusy = false;
        public bool IsBusy
        {
            get { return this.isBusy = false; }
            set
            {
                this.isBusy = value;
                this.OnPropertyChanged();

            }
        }
    }
}
