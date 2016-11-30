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
using SimpleService;
namespace BrReceiver
{

    [BroadcastReceiver]
   // [IntentFilter(new[] { Intent.ActionBootCompleted }, Categories = new[] { Intent.CategoryDefault })]
    [IntentFilter(new[] { Intent.ActionBootCompleted }, Priority = (int)IntentFilterPriority.LowPriority)]

    public class StartReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {

        //      context.StartActivity(typeof(MainActivity));
            //  Intent serviceStart = new Intent(context, typeof(Si));
            //   serviceStart.AddFlags(ActivityFlags.NewTask);
            //     context.StartActivity(serviceStart);

          //  if (intent.Action == Intent.ActionBootCompleted)
           // {
                context.ApplicationContext.StartService(new Intent(context, typeof(SimpleService.SimpleServiceBinder)));
           // }

            //  Intent intent2 = new Intent(context, SimpleService);
            //      var activity2 = new Intent(context, typeof(Activity2));
            //    activity2.PutExtra("MyData", "Data from Activity1");
            // context.ApplicationContext.StartActivity(activity2);
            //  StartService(intent2);
        }
    }
}