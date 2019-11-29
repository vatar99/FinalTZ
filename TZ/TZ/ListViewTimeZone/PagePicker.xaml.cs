using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TZ.ListViewTimeZone
{
    public partial class PagePicker : ContentPage
    {
        public PagePicker()
        {
            InitializeComponent();
            BindingContext = new ClassPicker();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageClock());
        }
    }
}