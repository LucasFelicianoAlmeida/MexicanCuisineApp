using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MexicanCuisine.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TypeMealCardView : ContentView
    {
        public TypeMealCardView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
       
        }
    }
}