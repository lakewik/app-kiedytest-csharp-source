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
using Android.Webkit;
using Java.Lang;
using System.Threading;
using Android.Net;
using Android.App.Usage;
using System.Net;
using System.Threading.Tasks;
//using App7.Activity2;
namespace App7
{
    [Activity(Label = "Lista zapowiedzianych testow wiadomoœci")]
    public class Activity1 : Activity
    {
        string user_name;

        public static async Task Sleep(int ms)
        {
            await Task.Delay(ms);
        }

        private AlertDialog dialog_loading;
        private AlertDialog _dialog;
        ///// SPRAWdZANIE DOSTEONOSCI INTERNETU /////

        public bool  CheckInternetConnection()
        {
            string CheckUrl = "http://google.com";

            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);
                iNetRequest.Timeout = 3000;
                WebResponse iNetResponse = iNetRequest.GetResponse();
                iNetResponse.Close();
                return true;
            }
            catch (WebException ex)
            {
                return false;
            }
        }
        ///////////////////////////////////////////
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout1);
            var builder2 = new AlertDialog.Builder(this);
            builder2.SetMessage("Trwa ³adowanie...");
            dialog_loading = builder2.Show();
                if (
                CheckInternetConnection()
             )

            {
                dialog_loading.Dismiss();
                dialog_loading.Cancel();
                dialog_loading.Dispose();
            } else
            {
                var builder = new AlertDialog.Builder(this);
                builder.SetMessage("Nie masz po³aczenia z Internetem!");
                builder.SetPositiveButton("OK", (s, e) => { /* do something on OK click */ });
                dialog_loading.Dismiss();
                dialog_loading.Cancel();
                dialog_loading.Dispose(); 
                builder.Create().Show();
            }

            ///// GET DATA FROM NOTIFICATION CLICK /////
            // Get the message from the intent:
            string display_type = Intent.Extras.GetString("display_type", "");
            string test_id = Intent.Extras.GetString("test_id", "");
            //////////////////////////////////////////
            //retreive user name from preferences ////
            var prefs = Application.Context.GetSharedPreferences("KiedyTest", FileCreationMode.Private);
            var user_login = prefs.GetString("login", null);
            user_name = user_login;
            WebView localWebView = FindViewById<WebView>(Resource.Id.webView1);
            localWebView.Settings.JavaScriptEnabled = true;
            localWebView.SetWebViewClient(new WebViewClient()); // stops request going to Web Browser
            if (display_type == "all")
            {
                localWebView.LoadUrl("http://dziennik.zs1debica.pl/kiedytest/android/show_planned_tests_for_user.php?display_type=all&ref_from=app_uczen&user_name=" + user_name);
            }

            if (display_type == "dzisiaj")
            {
                localWebView.LoadUrl("http://dziennik.zs1debica.pl/kiedytest/android/show_planned_tests_for_user.php?display_type=dzisiaj&ref_from=app_uczen&user_name=" + user_name);
            }
            if (display_type == "jutro")
            {
                localWebView.LoadUrl("http://dziennik.zs1debica.pl/kiedytest/android/show_planned_tests_for_user.php?display_type=jutro&ref_from=app_uczen&user_name=" + user_name);
            }
            if (display_type == "pojutrze")
            {
                localWebView.LoadUrl("http://dziennik.zs1debica.pl/kiedytest/android/show_planned_tests_for_user.php?display_type=pojutrze&ref_from=app_uczen&user_name=" + user_name);
            }
            if (display_type == "onlyone")
            {
                localWebView.LoadUrl("http://dziennik.zs1debica.pl/kiedytest/android/show_planned_test_details.php?apptype=student&user_name=" + user_name+"&test_id="+test_id);
            }

        }

        public void onReceivedError(WebView view, int errorCod, Android.Resource.String description, Android.Resource.String failingUrl)
        {
            var progressDialog = ProgressDialog.Show(this, "Please wait...", "Checking account info...", true);
            new System.Threading.Thread(new ThreadStart(delegate
            {
                //LOAD METHOD TO GET ACCOUNT INFO
                RunOnUiThread(() => Toast.MakeText(this, "Toast within progress dialog.", ToastLength.Long).Show());
                //HIDE PROGRESS DIALOG
                RunOnUiThread(() => progressDialog.Hide());
            })).Start();

        }
    }
}

