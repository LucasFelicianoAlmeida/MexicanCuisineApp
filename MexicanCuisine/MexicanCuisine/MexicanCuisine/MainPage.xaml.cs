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

            var bc = BindingContext as BaseViewModel;
            //bc.Buttons.Where(x => x.Text == "")


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

            
            await AnimationPosition();
            await GetData();
            carousell.HeightRequest = 80;
        }

        private async Task AnimationPosition()
        {
            await Task.Delay(1500);
            var lastPos = carousell.Bounds.Location.Y;
            var y = carousell.Bounds.Y;
            var trans = carousell.Y;
            var animation = new Animation();
       
            var titlePosition = new Animation(p => title.TranslationX = p, title.TranslationX, 0);
            var framePostion = new Animation(p => frame1.TranslationX = p, frame1.TranslationX, 0);
            var rect = new Rectangle(0, 150, carousell.Width,500);

            animation.Add(0, 1, titlePosition);
            animation.Add(.5,1, framePostion);
            //animation.Add(.99, 1, position);
            animation.Commit(this, "startPositionAnimation", 16, 2000, Easing.CubicInOut);

            await Task.Delay(2000);



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

      

        private async void LayoutMariaTo(object sender, EventArgs e)
        {

            //TODO : How to use layoutTO
            mariaMenu.Opacity = .5;

            //Set to invisible
            await fade.FadeTo(0);
            fade.IsVisible = false;

            mariaMenu.IsVisible = true;
            var centered = new Rect(Width / 2 + mariaMenu.WidthRequest / 2, Height / 2 + mariaMenu.HeightRequest / 2, mariaMenu.Width, mariaMenu.Height);

            mariaMenu.Layout(centered);
            await mariaMenu.FadeTo(1,1000);
            var rect2 = new Rect(maria2.X + 10, stackOrder.Y + stackOrderNow.Y + maria2.Y + maria2.Margin.Top, 50, 50);
            await mariaMenu.LayoutTo(rect2, 1000,easing: Easing.SinInOut);

            maria2.Opacity = 1;
            mariaMenu.IsVisible = false;
            

      
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            fade.IsVisible = true;
            _ =fade.FadeTo(1);
            vm.Pedidos.Clear();
            txtPopup.Text = "Your order was sucessfully made. Wait 30 minutes till it's done.";
            maria2.Opacity = 0;
        }
    }
}
