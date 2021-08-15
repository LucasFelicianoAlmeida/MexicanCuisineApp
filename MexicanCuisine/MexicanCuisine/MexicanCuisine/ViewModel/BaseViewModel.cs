using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MexicanCuisine.ViewModel
{
    public class BaseViewModel : BindableObject
    {
        public ObservableCollection<Food> AllFoods = new ObservableCollection<Food>();


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

        public BaseViewModel()
        {
            Buttons.Add(new Button { Text = "Meal", Opacity = 1 });
            Buttons.Add(new Button { Text = "Drinks", Opacity = .4 });
            Buttons.Add(new Button { Text = "Snacks", Opacity = .4 });
            Buttons.Add(new Button { Text = "Fruit", Opacity = .4 });
        }

        private ObservableCollection<Food> pedidos = new ObservableCollection<Food>();

        public ObservableCollection<Food> Pedidos
        {
            get { return pedidos; }
            set
            {
                foods = value;
                OnPropertyChanged();
            }
        }


        private double sliderValue;

        public double SliderValue
        {
            get { return sliderValue; }
            set
            {
                OnPropertyChanged();
                sliderValue = value;
            }
        }

        public ICommand DragStartingCommand => new Command<Food>((param) =>
        {
            dragFood = param;
        });

        public ICommand DropOverCommand => new Command(() =>
        {
            if (Foods.Contains(dragFood))
            {
                Pedidos.Add(dragFood);
                Foods.Remove(dragFood);
                AllFoods.Remove(dragFood);

            }
        });
        int indexLB;
        public ICommand EnterCommand => new Command<object>((o) =>
        {
            if (o is Button obj)
            {

                obj.FadeTo(1);
                obj.TranslateTo(0, -10);

                Buttons[indexLB].FadeTo(.4);
                Buttons[indexLB].TranslateTo(0, 10);

                if (obj.Text == "Drinks")
                {
                    Foods.Clear();
                    var foods = AllFoods.Where(x => x.IdTypeMeal == 2);
                    foreach (var item in foods)
                    {
                        Foods.Add(item);
                    }
                }
                else if (obj.Text == "Meal")
                {
                    Foods.Clear();
                    var foods = AllFoods.Where(x => x.IdTypeMeal == 1);
                    foreach (var item in foods)
                    {
                        Foods.Add(item);
                    }
                }
                else if (obj.Text == "Snacks")
                {
                    var foods = AllFoods.Where(x => x.IdTypeMeal == 3);
                    Foods.Clear();
                    foreach (var item in foods)
                    {
                    Foods.Add(item);

                    }
                }
                else
                {
                    var foods = AllFoods.Where(x => x.IdTypeMeal == 4);
                    foreach (var item in foods)
                    {
                        Foods.Clear();
                        Foods.Add(item);
                    }
                }
                //LastB
                indexLB = Buttons.IndexOf(obj);

            }
        });

        public ObservableCollection<Button> Buttons { get; set; } = new ObservableCollection<Button>();






        private Food dragFood;


    }
}
