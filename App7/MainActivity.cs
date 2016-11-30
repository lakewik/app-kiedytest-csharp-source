using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using App7;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using Android.Webkit;
//using System.Threading.Tasks;
using System.Text;
using System.Xml;
using Android.Net;
using Android.Util;
using static Android.App.ActivityManager;
using static Android.Resource;

namespace SimpleService
{


    


    [Activity(Label = "Zaloguj się do systemu - KiedyTest", MainLauncher = true)]
    [Service]

  

    public class MainActivity : Activity
    {
        //int count = 1;
        string data, auth_result;
        int auth_result_int;
        bool google_network_ok;

        /// network check functions ///

      //  ConnectivityManager connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);

       // NetworkInfo activeConnection = ConnectivityManager.ActiveNetworkInfo;
       // bool isOnline = (activeConnection != null) && activeConnection.IsConnected;

        ///////////////////////////////


        // Function called from OnDestroy
        protected void saveset()
        {

            //store
            var prefs = Application.Context.GetSharedPreferences("KiedyTest", FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutString("login", "Some value");
            prefEditor.Commit();

        }

        // Function called from OnCreate
        protected void retrieveset()
        {
            //retreive 
            var prefs = Application.Context.GetSharedPreferences("KiedyTest", FileCreationMode.Private);
            var user_login = prefs.GetString("login", null);

            //Show a toast
            RunOnUiThread(() => Toast.MakeText(this, user_login, ToastLength.Long).Show());

        }


        private bool isMyServiceRunning(System.Type cls)
        {
            ActivityManager manager = (ActivityManager)GetSystemService(Context.ActivityService);

            foreach (var service in manager.GetRunningServices(int.MaxValue))
            {
                if (service.Service.ClassName.Equals(Java.Lang.Class.FromType(cls).CanonicalName))
                {
                    return true;
                }
            }
            return false;
        }


        protected override void OnCreate(Bundle bundle)
        {

           
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //// BACKGROUND SERVICE////////////////////////////
            if (ApplicationContext.StartService(new Intent(this, typeof(SimpleServiceBinder))) != null)
            {

            }
            else
            {
                ApplicationContext.StartService(new Intent(this, typeof(SimpleServiceBinder)));
            }
            ///////////////////////////////////////////////////
            /// Check internet access/////////////////////////////////////////////////////////////////////////
            bool google_network_ok;
            string google_urlAddress = "http://google.com";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(google_urlAddress);
                request.Timeout = 3000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    data = readStream.ReadToEnd();

                    response.Close();
                    readStream.Close();
                }
                google_network_ok = true;

            }
            catch (WebException ex)
            {
                google_network_ok = false;
            }

          //  if (google_network_ok)

            /////////////////////////////////////////////////////////////

            /////////////////////////////
            //store data
            var prefs = Application.Context.GetSharedPreferences("KiedyTest", FileCreationMode.Private);

            var first_launch = prefs.GetString("first_launch", null);
            var logged = prefs.GetString("logged", null);
            var prefEditor = prefs.Edit();
            if (first_launch != "no")
            {
             
                prefEditor.PutString("first_launch", "no");
                prefEditor.PutInt("connection_timeout", 3000);
                prefEditor.PutString("heartbeat_enabled", "yes");
                prefEditor.PutString("notify_enabled", "yes");
                




                prefEditor.Commit();

            }

            if (logged == "yes")
            {
                var activity2 = new Intent(this, typeof(Activity2));
               // var mainactivity = new Intent(this, typeof(MainActivity));
                //    activity2.PutExtra("MyData", "Data from Activity1");
             StartActivity(activity2); ///  start menu activity
                //    FinishActivity(mainactivity); ///  close main activity
            //   Finish();

            }


            Button start = FindViewById<Button>(Resource.Id.MyButton);
           // start.Click += (sender, args) => { StartService(new Intent(this, typeof(SimpleService))); };

          //  Uri address = new Uri("http://dziennik.zs1debica.pl/activity/heartbeat");
        //    NameValueCollection nameValueCollection = new NameValueCollection();
          //  nameValueCollection["Name"] = "string-input";

         //   var webClient = new WebClient();
          //  webClient.se
        //    webClient.UploadValuesAsync(address, "POST", nameValueCollection);


          //  saveset();
         //   retrieveset();

            start.Click += delegate {
                /// LOGIN ACTION///
                // Request ///
                string content, content2;
                bool network_ok;

                if (google_network_ok)
                {
                    var et1 = FindViewById<EditText>(Resource.Id.editText3);
                    var user_name = et1.Text;
                    var et2 = FindViewById<EditText>(Resource.Id.editText2);
                    var user_password = et2.Text;
                    string urlAddress = "http://dziennik.zs1debica.pl/kiedytest/android/request_handler/login_app.php?user_name="+user_name+"&user_password="+user_password;




                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                        request.Timeout = 3000;
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Stream receiveStream = response.GetResponseStream();
                            StreamReader readStream = null;

                            if (response.CharacterSet == null)
                            {
                                readStream = new StreamReader(receiveStream);
                            }
                            else
                            {
                                readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                            }

                            data = readStream.ReadToEnd();

                            response.Close();
                            readStream.Close();
                        }
                        network_ok = true;

                    }
                    catch (WebException ex)
                    {
                        network_ok = false;
                    }

                    /// XML Parse ///
                    if (network_ok)
                    {
                        content = data;
                        content = content.Replace("\x00", "");  //  // AWESOME !!!!! // in my case I want to see it, otherwise just replace with ""
                        MemoryStream memStream = new MemoryStream();
                        byte[] data2 = System.Text.Encoding.UTF8.GetBytes(content);
                        memStream.Write(data2, 0, data2.Length);
                        memStream.Position = 0;

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(content);
                        //////////////////////////////////////////////////////////////////

                        foreach (XmlElement x in doc.SelectNodes("result/result_data/auth_result"))
                        {

                            auth_result = x.InnerXml;
                         //   Log.Debug("Tag", auth_result);
                            //Log.Debug(TAG, auth_result);
                            // Console.Write(x.InnerXml);
                            //    noti_type = "pies2";
                        }
                        /////////////////////////////////////////////////////////////////////////////////
                        /// Converting variables//////////////////
                        int.TryParse(auth_result, out auth_result_int);
                        //////////////////////////////////////LOGIN/////////

                        if (auth_result_int == 1)
                        {
                            prefEditor.PutString("logged", "yes");
                            prefEditor.PutString("login", user_name);
                            prefEditor.PutString("password", user_password);
                            prefEditor.Commit();
                           
                            var builder3 = new AlertDialog.Builder(this);
                            builder3.SetMessage("Zostałeś pomyślnie zalogowany");
                         //   builder3.SetPositiveButton("OK", OkAction);
                            builder3.Show();
                            var activity2 = new Intent(this, typeof(Activity2));
                            StartActivity(activity2); ///  start menu activity
                          


                        }
                        else
                        {
                            var builder3 = new AlertDialog.Builder(this);
                            builder3.SetMessage("Login i/lub hasło jest niepoprawne! Proszę spróbować ponownie!");
                            builder3.Show();
                        }
                    } else
                    {
                        var builder3 = new AlertDialog.Builder(this);
                        builder3.SetMessage("Błąd połaczenia z serwerami KiedyTest. Proszę spróbować ponownie później");
                        builder3.Show();
                    }
                } else
                {
                    var builder3 = new AlertDialog.Builder(this);
                    builder3.SetMessage("Błąd łączenia z Internetem! Proszę sprawdzić swoje połączenie sieciowe!");
                    builder3.Show();
                }

                //////////////////////////////////////////////
                //   this.StartService(new Intent(this, typeof(SimpleServiceBinder)));

                // Intent intent2 = new Intent(ApplicationContext, typeof(SimpleService));
                //    PendingIntent pi = PendingIntent.GetBroadcast(ApplicationContext, 0, intent2, 0);
                //  StartService(intent2);
                //   Service.startForeground(int, Notification)
                // StartService(new Intent("App7.SimpleService"));


                ///////////////////////////////////

               

                //   isMyServiceRunning(System.Type cls)
                //isMyServiceRunning(SimpleService);
                ;
              //  context.ApplicationContext.StartService(new Intent(context, typeof(SimpleService.SimpleServiceBinder)));
                //   var activity2 = new Intent(this, typeof(Activity2));
                //    activity2.PutExtra("MyData", "Data from Activity1");
                //    StartActivity(activity2);
            };


            //// starting main app service for notifications ///

        

            //    var notificationIntent = new Intent(this, typeof(SimpleService));
            //  StartService(notificationIntent);
            //   startService(new Intent(this, SimpleService));

            //  Button stop = FindViewById<Button>(Resource.Id.stopService);
            //  stop.Click += (sender, args) => { StopService(new Intent(this, typeof(SimpleService))); };

        }
        private void OkAction(object sender, DialogClickEventArgs e)
        {
            var myButton = sender as Button; //this will give you the OK button on the dialog but you're already in here so you don't really need it - just perform the action you want to do directly unless I'm missing something..
            if (myButton != null)
            {
                var activity24 = new Intent(this, typeof(Activity2));
                StartActivity(activity24); ///  start menu activity
                Finish();
                //do something on ok selected on alert dialog
            }
        }
    }

   
}

