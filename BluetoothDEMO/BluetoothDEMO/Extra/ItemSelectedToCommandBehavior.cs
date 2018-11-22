using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace BluetoothDEMO
{
    public class ItemSelectedToCommandBehavior : Behavior<ListView>
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(
                propertyName: "Command",
                returnType: typeof(ICommand),
                declaringType: typeof(ItemSelectedToCommandBehavior));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.ItemSelected += ListView_ItemSelected;
            bindable.BindingContextChanged += ListView_BindingContextChanged;

            bindable.SelectedItem = null;
        }



        protected override void OnDetachingFrom(ListView bindable)
        {
            bindable.ItemSelected -= ListView_ItemSelected;
            bindable.BindingContextChanged -= ListView_BindingContextChanged;
            base.OnDetachingFrom(bindable);
        }

        private void ListView_BindingContextChanged(object sender, EventArgs eventArgs)
        {
            var listView = sender as ListView;
            BindingContext = listView?.BindingContext;


        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            if (e.SelectedItem == null) return;
            Command.Execute(null);
            ((ListView)sender).SelectedItem = null;




        }




    }
}