using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App7
{

    //// panel menu g³ównego
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "KiedyTest - menu g³ówne")]
    
    public class Activity2 : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.menu_layout);

            Button dzisiaj = FindViewById<Button>(Resource.Id.button2);
            // start.Click += (sender, args) => { StartService(new Intent(this, typeof(SimpleService))); };

            Button jutro = FindViewById<Button>(Resource.Id.button3);
            Button pojutrze = FindViewById<Button>(Resource.Id.button4);
            Button all = FindViewById<Button>(Resource.Id.button5);
            Button settings = FindViewById<Button>(Resource.Id.button6);

            dzisiaj.Click += delegate {
                var activity2 = new Intent(this, typeof(Activity1));
                activity2.PutExtra("display_type", "dzisiaj");
                StartActivity(activity2);
            };


            jutro.Click += delegate {
                var activity2 = new Intent(this, typeof(Activity1));
                activity2.PutExtra("display_type", "jutro");
                StartActivity(activity2);
            };

            pojutrze.Click += delegate {
                var activity2 = new Intent(this, typeof(Activity1));
                activity2.PutExtra("display_type", "pojutrze");
                StartActivity(activity2);
            };


            all.Click += delegate {
                var activity2 = new Intent(this, typeof(Activity1));
                activity2.PutExtra("display_type", "all");
                StartActivity(activity2);
            };


            settings.Click += delegate {
                var activity3 = new Intent(this, typeof(Activity3));
                StartActivity(activity3);
            };

            // Create your application here
        }
    }
}