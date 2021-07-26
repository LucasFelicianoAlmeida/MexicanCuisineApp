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

        }

        private void StartAnimation()
        {
            var children = flexLayout.Children.OfType<TypeMealCardView>().ToList();
            var index = 0;
            var animation = new Animation();
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
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
                return true;
            });
            //foreach (var item in flexLayout.Children.OfType<TypeMealCardView>())
            //{

            //    var rotation = new Animation(r => item.RotationY = r, item.RotationY, 360);
            //    animation.Add(0, 1, rotation);
            //}
        }

        //Get foods

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await GetData();
            await AnimationPosition();
            StartAnimation();
            lbl.Text = $"Translaion: {scrollList.TranslationY}, Y: {scrollList.Y}";

        }

        private async Task AnimationPosition()
        {
            var lastPos = scrollList.Bounds.Location.Y;
            var y = scrollList.Bounds.Y;
            var trans = scrollList.Y;
            var animation = new Animation();
            //var position = new Animation((d) =>
            //{
            //  scrollList.TranslationY = d;
            //}, Height, 100);

            var titlePosition = new Animation(p => title.TranslationX = p, title.TranslationX, 0);
            var framePostion = new Animation(p => frame1.TranslationX = p, frame1.TranslationX, 0);
            var rect = new Rectangle(0, 150, scrollList.Width,500);

            animation.Add(0, 1, titlePosition);
            animation.Add(.5,1, framePostion);
            //animation.Add(.99, 1, position);
            animation.Commit(this, "startPositionAnimation", 16, 2000, Easing.CubicInOut);
            await scrollList.LayoutTo(rect, 250);


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
                        vm.Foods.Add(item);
                    }
                    BindableLayout.SetItemsSource(flexLayout, vm.Foods);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
        }

        private void FlexLayout_BindingContextChanged(object sender, EventArgs e)
        {
            var bind = flexLayout.BindingContext.ToString();
            if (flexLayout.BindingContext.Equals(vm.Foods))
            {

            }
        }

        private void slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            //vm.SliderValue = slider.Value;
        }
    }
}
