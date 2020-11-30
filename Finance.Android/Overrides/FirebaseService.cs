using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Finance.View;
using Firebase.Messaging;
using System;
using System.Linq;
using WindowsAzure.Messaging;

namespace Finance.Droid.Overrides
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseService : FirebaseMessagingService
    {
        public FirebaseService()
        {

        }

        public override void OnNewToken(string token)
        {
           
            SendRegistrationToAzure(token);
        }

        private void SendRegistrationToAzure(string token)
        {
            try
            {
                NotificationHub hub = new NotificationHub("FiananceNotificationHub", "Endpoint=sb://fianance.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=3lB0yqYQ1qB/tSo7hiW6OWZqDWtgUt8rzlY/l43rEY0=", this);
                Registration registration = hub.Register(token, new string[] { "default" });

                string pnsHandle = registration.PNSHandle;
                hub.RegisterTemplate(pnsHandle, "defaultTemplate", "{\"data\": {\"message\": \"Notification Hub test notification\"}}", new string[] { "default" });
            }
            catch (Exception)
            {

            }
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            string messageBody = string.Empty;

            if (message.GetNotification() != null)
            {
                messageBody = message.GetNotification().Body;
            }
            else
            {
                messageBody = message.Data.Values.First();
            }

            SendLocalNotification(messageBody);
            // (App.Current.MainPage as MainPage) 
            // todo add tolist view perharps?
        }

        private void SendLocalNotification(string messageBody)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.PutExtra("message", messageBody);

            var requestCode = new Random().Next();
            var pendingIntent = PendingIntent.GetActivity(this, requestCode, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this)
                .SetContentTitle("Finance Message")
                .SetSmallIcon(Resource.Drawable.ic_launcher)
                .SetContentText(messageBody)
                .SetAutoCancel(true)
                .SetShowWhen(false)
                .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(this);

            if(Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel channel = new NotificationChannel(
                    "XamarinNotifyChannel",
                    "Fianance App",
                    NotificationImportance.High);
                notificationManager.CreateNotificationChannel(channel);

                notificationBuilder.SetChannelId("XamarinNotifyChannel");
            }
            
            notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
}

