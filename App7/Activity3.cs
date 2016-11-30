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
    [Activity(Label = "KiedyTest - panel konfiguracji")]
    public class Activity3 : Activity
    {
        int timeout_INT;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.settings_layout);

            Button save = FindViewById<Button>(Resource.Id.button2);

            save.Click += delegate
            {
                var prefs = Application.Context.GetSharedPreferences("KiedyTest", FileCreationMode.Private);
                var timeout = FindViewById<EditText>(Resource.Id.editText1).Text;
                int.TryParse(timeout, out timeout_INT);
                CheckBox heartbeat = FindViewById<CheckBox>(Resource.Id.checkBox1);
                CheckBox notify = FindViewById<CheckBox>(Resource.Id.checkBox2);
                var prefEditor = prefs.Edit();
                if (heartbeat.Checked)
                {  prefEditor.PutString("heartbeat_enabled", "yes");
                    } else
                {
                    prefEditor.PutString("heartbeat_enabled", "no");

                }

                if (notify.Checked)
                {

                    prefEditor.PutString("notify_enabled", "yes");
                }
                else
                {
                    prefEditor.PutString("notify_enabled", "no");

                }

                /// Checking timeout
                /// 
                bool check_ok;
                if (timeout_INT < 3000)
                {
                    check_ok = false;
                    var builder2 = new AlertDialog.Builder(this);
                    builder2.SetMessage("Czas oczekiwania na odpowied� (timeout) nie mo�e by� mniejszy ni� 3000! Prosz� wprowadzi� warto�� wi�ksz� lub r�wn� 3000!");
              //      builder2.SetPositiveButton("OK", OkAction);
                    builder2.Show();
                  
                } else
                {
                    check_ok = true;
                    prefEditor.PutInt("connection_timeout", timeout_INT);
                }

                prefEditor.Commit();

                if (check_ok == true)
                {
                    var builder2 = new AlertDialog.Builder(this);
                    builder2.SetMessage("Ustawiania zosta�y zapisane pomy�lnie!");
                  //  builder2.SetPositiveButton("OK", OkAction);
                    builder2.Show();
                }

            };
                // Create your application here
         }

        private void OkAction(object sender, DialogClickEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}