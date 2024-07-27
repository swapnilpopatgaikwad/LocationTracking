using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Widget;
using System;

namespace LocationTracking
{
    [Service]
    public class LocationForegroundService : Service
    {
        private LocationManager _locationManager;
        private LocationListener _locationListener;

        public override void OnCreate()
        {
            base.OnCreate();
            _locationManager = (LocationManager)GetSystemService(LocationService);

            _locationListener = new LocationListener();
            _locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 3000, 0, _locationListener);
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var channelId = "location_channel_id";
            var channelName = "location Service";

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var notificationChannel = new NotificationChannel(channelId, channelName, NotificationImportance.Low);
                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(notificationChannel);
            }

            var notification = new Notification.Builder(this, channelId)
                .SetContentTitle("Location Service")
                .SetContentText("Location Service is active.")
                .SetSmallIcon(Resource.Mipmap.ic_launcher)
                .SetOngoing(true)
                .Build();

            StartForeground(1, notification);

            return StartCommandResult.Sticky;
        }

        private class LocationListener : Java.Lang.Object, ILocationListener
        {
            public void OnLocationChanged(Android.Locations.Location location)
            {
                Toast.MakeText(Android.App.Application.Context, $"Location: {location.Latitude}, {location.Longitude}", ToastLength.Long).Show();
                // Handle location update
                Console.WriteLine($"Location: {location.Latitude}, {location.Longitude}");
            }

            public void OnStatusChanged(string provider, Availability status, Bundle extras) { }
            public void OnProviderEnabled(string provider) { }
            public void OnProviderDisabled(string provider) { }
        }
    }
}