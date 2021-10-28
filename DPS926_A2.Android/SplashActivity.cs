using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPS926_A2.Droid
{
    [Activity(Label = "Movie Space", Icon = "@drawable/main_icon", Theme = "@style/MainTheme", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity, Animator.IAnimatorListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_Splash);

            var animationView = FindViewById<LottieAnimationView>(Resource.Id.animation_view);
            animationView.AddAnimatorListener(this);
        }

        public void OnAnimationCancel(Animator animation)
        {
        }

        public void OnAnimationEnd(Animator animation)
        {
        }

        public void OnAnimationRepeat(Animator animation)
        {
        }

        public async void OnAnimationStart(Animator animation)
        {
            await SimulateStartUp();
        }

        async Task SimulateStartUp()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(2650));
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}