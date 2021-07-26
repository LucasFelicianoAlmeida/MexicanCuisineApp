using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace MexicanCuisine.ViewModel
{
    public class BaseViewModel : BindableObject
    {

        private ObservableCollection<Food> foods = new ObservableCollection<Food>();

        public ObservableCollection<Food> Foods
        {
            get { return foods; }
            set
            {
                OnPropertyChanged();
                foods = value;
            }
        }

        private double sliderValue;

        public double SliderValue
        {
            get  {return sliderValue; }
            set 
            {
                OnPropertyChanged();
                sliderValue = value; 
            }
        }


    }
}
