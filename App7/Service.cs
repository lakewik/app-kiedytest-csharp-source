using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Android.Widget;
using Android;
using App7;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;
using System.Text;

namespace SimpleService
{
    [Service]
    [IntentFilter(new String[] { "App7.SimpleService" })]
    public class SimpleServiceBinder : Service
    {
        //    WebRequest request = HttpWebRequest.Create("http://dziennik.zs1debica.pl/kiedytest/android/request_handler/get_planned_test_notification.php?user_name=olaf&user_password=4334");
        //   WebResponse response;
        string response2;
        string response3;
        string tests_grammar;
        string data;
     //   data = null;
            bool network_ok;
        //  static readonly string TAG = "X:" + typeof(SimpleService).Name;
        static readonly int TimerWait = 4000;
        Timer _timer;
        private AlertDialog dialog_loading;
  //      private MyServiceBinder binder;

        public static StartCommandResult START_STICKY { get; private set; }

        private async void starthttp()
        {

        //    response = await Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null);
         //   using (var response2 = new StreamReader(response.GetResponseStream()))
           //     response3 = await response2.ReadToEndAsync();

        }

        public override void OnStart(Intent intent, int startId)
        {


    
        

        }


        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var prefs = GetSharedPreferences("KiedyTest", FileCreationMode.Private);
            int req_timeout = prefs.GetInt("connection_timeout", 0);
            string heartbeat_url = "http://dziennik.zs1debica.pl/kiedytest/android/request_handler/heartbeat.php?app_type=uczen&app_version=1.0";

            ////// HEARTBEAT EXCUTER ///////
            _timer = new Timer(o => {

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(heartbeat_url);
                    request.Timeout = req_timeout;
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

            },
          null,
          0,
          60000); /// Koniec timera
            ///////////////////////////////



            //   OnStart(intent, startId);
            //  Intent intent2 = new Intent(this, intent);
            //  intent.SetFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_SINGLE_TOP);
            //  PendingIntent pendingIntent = PendingIntent.getActivity(this, 0, intent, 0);

            //   Log.Debug(TAG, "OnStartCommand called at {2}, flags={0}, startid={1}", flags, startId, DateTime.UtcNow);
            //  _timer = new Timer(o => { Log.Debug(TAG, "Hello from SimpleService. {0}", DateTime.UtcNow); },
            //     null,
            //   0,
            //   TimerWait);


            ////Reading from XML for notification////


            ////////////////////
            ///Get data from servers///
            ///

         
         
            

            string content, content2;

            string user_name = prefs.GetString("login", null);
            string user_password = prefs.GetString("password", null);



            string urlAddress = "http://dziennik.zs1debica.pl/kiedytest/android/request_handler/get_planned_test_notification.php?user_name="+user_name+"&user_password="+user_password;

            _timer = new Timer(o => {

                //// Timer odliczajacy czas do nastêpnego spawdzenia
                string notify_enabled = prefs.GetString("notify_enabled", null);
             

                if (notify_enabled == "yes")
                {

                    try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                    request.Timeout = req_timeout;
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



                //  request.Timeout = 3000;
                // starthttp();








                content2 = data;
                //Log.Debug(content2, content2);
                //  _timer = new Timer(o => { Log.Debug(content2, content2); },
                // null,
                //   0,
                // TimerWait);


                //   var builder2 = new AlertDialog.Builder(this);
                //     builder2.SetMessage(content2);

                //  dialog_loading = builder2.Create().Show();
                //   builder2.Create();
                ///      dialog_loading = builder2.Show();

               

                

                    //////////////////////////
                    if (network_ok)
                {
                  


                        content = data;
                        //   content = "<notification><notification_data><notification_type>2</notification_type><pending_notifications_count>2</pending_notifications_count><notification_body>piesek24 | kotek56 | gramatyka | ierwiastki | fonetyka | potêgowanie</notification_body><vibration>1</vibration><sound>0</sound></notification_data></notification>";
                        //  content = "<data><zwierze><kot>5565</kot></zwierze></data>";

                        //   XmlDocument xml = new XmlDocument();
                      try
                        {
                            // xml.LoadXml(details.Xml);
                            content = content.Replace("\x00", "");  //  // AWESOME !!!!! // in my case I want to see it, otherwise just replace with ""
                        MemoryStream memStream = new MemoryStream();
                        byte[] data2 = System.Text.Encoding.UTF8.GetBytes(content);
                        memStream.Write(data2, 0, data2.Length);
                        memStream.Position = 0;

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(content);
                        //  XmlString.Replace("\x00", "[0x00]");
                        //  string name;
                        //  string pref;
                        // Variable initialization ////
                        string noti_type;
                        int noti_type_INT;
                        noti_type_INT = 0;
                        string noti_test_id;
                        noti_test_id = null;
                        int noti_test_id_INT;
                        noti_test_id_INT = 0;
                        string noti_pending;
                        noti_pending = null;
                        int noti_pending_INT;
                        noti_pending_INT = 0;
                        noti_type = "kot";
                        int c = 0;

                        string noti_body;
                        noti_body = null;


                        string vibration, sound;
                        vibration = null;
                        sound = null;
                        int vibration_INT, sound_INT;
                        vibration_INT = 0;
                        sound_INT = 0;

                        foreach (XmlElement x in doc.SelectNodes("notification/notification_data/notification_type"))
                        {
                            c++;
                            noti_type = x.InnerXml;
                            Console.Write(x.InnerXml);
                            //    noti_type = "pies2";
                        }

                        ////// Checking if any notifications available ////////
                        foreach (XmlElement x in doc.SelectNodes("notification/notification_data/pending_notifications_count"))
                        {
                            c++;
                            noti_pending = x.InnerXml;
                            Console.Write(x.InnerXml);
                            //    noti_type = "pies2";
                        }
                        ////// Checking test id ////////
                        foreach (XmlElement x in doc.SelectNodes("notification/notification_data/test_id"))
                        {
                            c++;
                            noti_test_id = x.InnerXml;
                            Console.Write(x.InnerXml);
                            //    noti_type = "pies2";
                        }
                        ////// Checking notification text ////////
                        foreach (XmlElement x in doc.SelectNodes("notification/notification_data/notification_body"))
                        {
                            c++;
                            noti_body = x.InnerXml;
                            Console.Write(x.InnerXml);
                            //    noti_type = "pies2";
                        }
                        ////// Checking if user enabled sound ////////
                        foreach (XmlElement x in doc.SelectNodes("notification/notification_data/sound"))
                        {
                            c++;
                            sound = x.InnerXml;
                            Console.Write(x.InnerXml);
                            //    noti_type = "pies2";
                        }
                        ////// Checking if user enabled vibration ////////
                        foreach (XmlElement x in doc.SelectNodes("notification/notification_data/vibration"))
                        {
                            c++;
                            vibration = x.InnerXml;
                            Console.Write(x.InnerXml);
                            //    noti_type = "pies2";
                        }

                        /// Converting variables//////////////////
                        int.TryParse(noti_pending, out noti_pending_INT);
                        int.TryParse(noti_type, out noti_type_INT);
                        //int.TryParse(noti_test_id, out noti_test_id_INT);
                        int.TryParse(sound, out sound_INT);
                        int.TryParse(vibration, out vibration_INT);
                            ///////////////////////////////////////


                            if (noti_pending_INT > 0)
                            {


                                /// DATA FOR OTHER ACTIVITY///
                                int test_id;
                                int randomcount;
                                //   float count2;
                                //    count2 = 08.06f;
                                test_id = noti_test_id_INT;
                                Bundle valuesForActivity = new Bundle();
                                valuesForActivity.PutString("test_id", noti_test_id);
                                if (noti_type_INT == 1)
                                {
                                    valuesForActivity.PutString("display_type", "onlyone");
                                }

                                if (noti_type_INT == 2)
                                {
                                    if (noti_pending_INT > 1)
                                    {
                                        valuesForActivity.PutString("display_type", "all");
                                    }
                                    else
                                    {
                                        valuesForActivity.PutString("display_type", "onlyone");
                                    }
                                }

                                // When the user clicks the notification, SecondActivity will start up.
                                Intent resultIntent = new Intent(this, typeof(Activity1));

                                // Pass some values to SecondActivity:
                                resultIntent.PutExtras(valuesForActivity);

                                // Construct a back stack for cross-task navigation:
                                TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);
                                stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(Activity1)));
                                stackBuilder.AddNextIntent(resultIntent);

                                // Create the PendingIntent with the back stack:            
                                PendingIntent resultPendingIntent =
                                    stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);
                                /////////////////////////////////////////////////////////////////////////////////

                                if (noti_type_INT == 1)
                                {


                                    // Build the notification:
                                    Notification.Builder builder = new Notification.Builder(this)
                                        .SetAutoCancel(true)                    // Dismiss from the notif. area when clicked
                                        .SetContentIntent(resultPendingIntent)  // Start 2nd activity when the intent is clicked.
                                        .SetContentTitle("Nowy zapowiedziany test!")      // Set its title
                                                                                          //  .SetNumber(count)                          // Display the count in the Content Info
                                        .SetSmallIcon(App7.Resource.Drawable.Icon)  // Display this icon
                                        .SetContentText(String.Format(
                                            noti_body)); // The message to display.

                                    // Finally, publish the notification:
                                    NotificationManager notificationManager =
                                        (NotificationManager)GetSystemService(Context.NotificationService);
                                    Random rand1 = new Random();
                                    randomcount = rand1.Next();

                                    if (sound_INT == 1 && vibration_INT == 1)
                                    {
                                        builder.SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate);
                                    }
                                    else if (sound_INT == 1)
                                    {
                                        builder.SetDefaults(NotificationDefaults.Sound);
                                    }
                                    else if (vibration_INT == 1)
                                    {
                                        builder.SetDefaults(NotificationDefaults.Vibrate);
                                    }

                                    //  builder.SetDefaults( NotificationDefaults.Vibrate);
                                    notificationManager.Notify(randomcount, builder.Build());



                                }


                                if (noti_type_INT == 2)
                                {

                                    string new_test_parsed_title;

                                    if (noti_pending_INT > 1)
                                    {
                                        new_test_parsed_title = "Nowe zapowiedziane testy!";
                                    }
                                    else
                                    {
                                        new_test_parsed_title = "Nowy zapowiedziany test!";
                                    }
                                    // Build the notification:
                                    Notification.Builder builder = new Notification.Builder(this)
                                        .SetAutoCancel(true)                    // Dismiss from the notif. area when clicked
                                        .SetContentIntent(resultPendingIntent)  // Start 2nd activity when the intent is clicked.
                                        .SetContentTitle(new_test_parsed_title)      // Set its title
                                                                                     //  .SetNumber(count)                          // Display the count in the Content Info
                                        .SetSmallIcon(App7.Resource.Drawable.Icon)  // Display this icon
                                        .SetContentText(String.Format(
                                           noti_body)); // The message to display.

                                    // Instantiate the Big Text style:
                                    Notification.BigTextStyle textStyle = new Notification.BigTextStyle();

                                    // Fill it with text:
                                    string longTextMessage = noti_body;
                                    // longTextMessage += " / Just like me. ";
                                    //...
                                    textStyle.BigText(longTextMessage);

                                    // Set the summary text:

                                    if (noti_pending_INT == 1)
                                    {
                                        tests_grammar = " nowy zapowiedziany test";
                                    }

                                    if (noti_pending_INT > 1 && noti_pending_INT < 5)
                                    {
                                        tests_grammar = " nowe zapowiedziane testy";
                                    }

                                    if (noti_pending_INT > 5)
                                    {
                                        tests_grammar = " nowych zapowiedzianych testów";
                                    }


                                    textStyle.SetSummaryText(noti_pending + tests_grammar);

                                    // Plug this style into the builder:
                                    builder.SetStyle(textStyle);


                                    // Finally, publish the notification:
                                    NotificationManager notificationManager =
                                        (NotificationManager)GetSystemService(Context.NotificationService);
                                    Random rand1 = new Random();
                                    randomcount = rand1.Next();

                                    if (sound_INT == 1 && vibration_INT == 1)
                                    {
                                        builder.SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate);
                                    }
                                    else if (sound_INT == 1)
                                    {
                                        builder.SetDefaults(NotificationDefaults.Sound);
                                    }
                                    else if (vibration_INT == 1)
                                    {
                                        builder.SetDefaults(NotificationDefaults.Vibrate);
                                    }

                                    //  builder.SetDefaults( NotificationDefaults.Vibrate);
                                    notificationManager.Notify(randomcount, builder.Build());



                                }

                            }



                        }

                        catch (Exception ex)
                        {
                            // just suppress any error logging exceptions
                        }

                    } /// IF network ok END


                }
            },
          null,
          0,
          TimerWait); /// Koniec timera


            ///////////////////////////////////////



            ////////////////////////////////////////


            // _timer = new Timer(o => { Log.Debug(noti_type, noti_type); },
            //        null,
            //        0,
            //     TimerWait);

         //   return StartCommandResult.Sticky;
            return START_STICKY;
        }

        public override IBinder OnBind(Intent intent)
        {
   //         binder = new SimpleService(this);
           return null;
        }

        public override void OnDestroy()
        {

         
        base.OnDestroy();

         _timer.Dispose();
         _timer = null;

         //   Log.Debug(TAG, "SimpleService destroyed at {0}.", DateTime.UtcNow);
        }

       
    }
}