using MexicanCuisine.Service;
using MexicanCuisine.ViewModel;
using MexicanCuisine.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MexicanCuisine
{
    public partial class MainPage : ContentPage
    {
        BaseViewModel vm = new BaseViewModel();
        public MainPage()
        {
            InitializeComponent();
             BindingContext = vm;


            Xamarin.Essentials.Battery.BatteryInfoChanged += (e, o) =>
            {
                var bat = Battery.ChargeLevel;
                if (bat < .5)
                {
                    Batery= 0;
                }
                else if (bat >= .5 & bat < 1)
                {
                    Batery = 1;
                }
                else if (bat == 1)
                {
                    Batery =  2;
                }
            };

        }
        private int batery;

        public int Batery
        {
            get 
            {
                var  bat = Battery.ChargeLevel;
                if (bat < .5)
                {
                    return 0;
                }
                else if (bat >=.5 & bat <1)
                {
                    return 1;
                }
                else if (bat == 1)
                {
                    return 2;
                }
                return batery;
            }
            set 
            {
                batery = value;
                OnPropertyChanged();
            }
        }


        private void StartAnimation()
        {
            var children = carousell.ItemsSource.OfType<TypeMealCardView>().ToList();
            var swipe = new SwipeGestureRecognizer();
            foreach (var item in children)
            {
                item.HeightRequest = 100;
                item.GestureRecognizers.Add(swipe);
            }
            var index = 0;
            var animation = new Animation();
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if(children.Count != 0)
                {

                var item = children[index];

                var rotation = new Animation(r => item.RotationY = r, 0, 360);
                animation.Add(0, 1, rotation);
                index++;
                animation.Commit(this, "rotationCards", 16, 800, Easing.SinInOut);
                if (index >= children.Count)
                {
                    index = 0;
                }
                }

                return true;
            });

        }

        //Get foods

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await GetData();
            await AnimationPosition();
            carousell.HeightRequest = 80;
            //StartAnimation();
            

        }

        private async Task AnimationPosition()
        {
            var lastPos = carousell.Bounds.Location.Y;
            var y = carousell.Bounds.Y;
            var trans = carousell.Y;
            var animation = new Animation();
            //var position = new Animation((d) =>
            //{
            //  scrollList.TranslationY = d;
            //}, Height, 100);

            var titlePosition = new Animation(p => title.TranslationX = p, title.TranslationX, 0);
            var framePostion = new Animation(p => frame1.TranslationX = p, frame1.TranslationX, 0);
            var rect = new Rectangle(0, 150, carousell.Width,500);

            animation.Add(0, 1, titlePosition);
            animation.Add(.5,1, framePostion);
            //animation.Add(.99, 1, position);
            animation.Commit(this, "startPositionAnimation", 16, 2000, Easing.CubicInOut);
            //await carousell.LayoutTo(rect, 250);


        }

        private async Task GetData()
        {
            try
            {
                var foods = await ApiService<Food>.Get("food");

                if (foods != null)
                {
                    foreach (var item in foods)
                    {
                        vm.AllFoods.Add(item);
                        if(item.IdTypeMeal == 1)
                        vm.Foods.Add(item);
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
        }

        private void FlexLayout_BindingContextChanged(object sender, EventArgs e)
        {
       
        }

        private void slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            //vm.SliderValue = slider.Value;
        }
    }
}
