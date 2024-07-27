using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using System.Threading.Tasks;

namespace LocationTracking
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            StartFloatingButtonServiceAsync();
        }

        private async Task StartFloatingButtonServiceAsync()
        {
            bool hasPermission = await LocationPermissionHelper.RequestLocationPermissionAsync();

            if (hasPermission)
            {
                var intent = new Intent(this, typeof(LocationForegroundService));
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    StartForegroundService(intent);
                }
                else
                {
                    StartService(intent);
                }
            }
            else
            {
                // Handle the case where permission is not granted
                Android.Widget.Toast.MakeText(this, "Location permission is required to start tracking.", Android.Widget.ToastLength.Long).Show();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            StopService(new Intent(this, typeof(LocationForegroundService)));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}