using Plugin.BLE;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System.Windows.Input;

namespace BluetoothDEMO.ViewModel
{
    public class MainViewModel: BaseViewModel
    {


        #region Atributos

        private IAdapter adapter;

        private IBluetoothLE bluetoothBLE;

        private IDevice device;

        private string _textobutton = "Buscar";

        private IDevice _dispositivoseleccionado;

        #endregion


        #region Propiedades

        public ObservableCollection<IDevice> ListaViewModel { get; set; }

        public IDevice DispositivoSeleccionado
        {
            get { return _dispositivoseleccionado; }
            set
            {
                _dispositivoseleccionado = value;
                OnPropertyChanged();
            }
        }

        public string TextoButton
        {
            get { return this._textobutton; }
            set
            {
                this._textobutton = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Comandos

        public ICommand SeleccionarDispositivoCommand => new Command(() => { Dispositivoseleccionado(); });

        public ICommand BuscarDispositivoCommand => new Command(() => BuscarDispositivo());

        #endregion


        #region Constructor

        public MainViewModel()
        {
            bluetoothBLE = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            ListaViewModel = new ObservableCollection<IDevice>();
        }

        #endregion


        #region Metodos

        private async void BuscarDispositivo()
        {

            TextoButton = "Buscando...";
            if (bluetoothBLE.State == BluetoothState.Off)
            {
                await App.Current.MainPage.DisplayAlert("Atencion", "Bluetooth deshabilitado.", "OK");
                TextoButton = "Buscar";
            }
            else
            {

                ListaViewModel.Clear();

                adapter.ScanTimeout = 10000;
                adapter.ScanMode = ScanMode.Balanced;



                adapter.DeviceDiscovered += (obj, a) =>
                {
                    if (!ListaViewModel.Contains(a.Device))
                        ListaViewModel.Add(a.Device);


                };


                await adapter.StartScanningForDevicesAsync();
                TextoButton = "Buscar";
            }
        }

        private async void Dispositivoseleccionado()
        {
            device = DispositivoSeleccionado as IDevice;

            var result = await App.Current.MainPage.DisplayAlert("Aviso", "Deseas conectarte a este dispositivo?", "Conectar", "Cancelar");

            if (!result)
                return;

            //Stop Scanner
            await adapter.StopScanningForDevicesAsync();

            try
            {
                await adapter.ConnectToDeviceAsync(device);
                await App.Current.MainPage.DisplayAlert("Conectado", "Status:" + device.State, "OK");
            }
            catch (DeviceConnectionException ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        #endregion


    }
}
