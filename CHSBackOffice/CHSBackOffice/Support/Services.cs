using CHSBackOffice.Views;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CHSBackOffice.Support
{
    class Services
    {
        internal static class Navigation
        {
            internal static void GoBack()
            {
                App.SharedEventAggregator.GetEvent<Events.GoBackDetailPage>().Publish();
            }

            internal static void SetDetailPage(Type pageType)
            {
                var page = (Page)Activator.CreateInstance(pageType);
                App.SharedEventAggregator.GetEvent<Events.SetDetailPage>().Publish(page);
            }

            internal static void NavigateDetailPage(Type pageType)
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() => Support.CommonViewObjects.Instance.IsPageCreating = true);

                var page = (Page)Activator.CreateInstance(pageType);
                App.SharedEventAggregator.GetEvent<Events.NavigateDetailPage>().Publish(page);
            }

            internal static void NavigateToDetailPageAsync(Type pageType)
            {
                
                Task.Run(() => NavigateDetailPage(pageType));
            }
            internal static async Task OpenMainMenuPage(Type detailsPageType)
            {
                await App.NaviService.NavigateAsync("myapp:///" + nameof(MainMenuPage) + "/" + nameof(NavigationPage) + "/" + detailsPageType.Name);
            }

            internal static async void NavigateToPage(Type pageType)
            {
                var page = pageType.GetConstructor(new Type[] { }).Invoke(new object[] { }) as Page;
                await Application.Current.MainPage.Navigation.PushAsync(page);
            }

            internal static void SetMainPage(Type pageType)
            {
                try
                {
                    var page = pageType.GetConstructor(new Type[] { }).Invoke(new object[] { }) as Page;
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() => Application.Current.MainPage = new NavigationPage(page));
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                }
            }
        }

        internal static Assembly CurrentAssembly = typeof(Services).GetTypeInfo().Assembly;
        internal static string AssemblyName => CurrentAssembly.GetName().Name;

        internal static ImageSource GetImageFromResources(string resourceName)
        {
            return ImageSource.FromResource($"{AssemblyName}.Resources.Images.{resourceName}", CurrentAssembly);
        }

        internal static void SafeRunInMainThread(Action action)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                }
            });
        }
    }
}
