using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LocationTracking
{
    public static class LocationPermissionHelper
    {
        public static async Task<bool> RequestLocationPermissionAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            return status == PermissionStatus.Granted;
        }
    }
}