using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CHSBackOffice.Support
{
    public class ExceptionProcessor
    {
        public  static void ProcessException(Exception exception)
        {
            try
            {
                if (exception != null)
                {
                    if (exception.GetType() == typeof(TimeoutException))
                    {
                        if ((DateTime.Now - CommonViewObjects.LastTimeoutException).TotalSeconds > Constants.SoapTimeoutSec)
                        {
                            Xamarin.Forms.Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.Toast(CommonViewObjects.TimeoutAlertConfig));
                            CommonViewObjects.LastTimeoutException = DateTime.Now;
                        }
                    }

                    Debug.Print(exception.ToString());
                    Debug.WriteLine(exception);
                    try
                    {
                        Microsoft.AppCenter.Crashes.Crashes.TrackError(exception, new Dictionary<string, string> { { "message", exception.Message ?? "message is null" } });
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                } 
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
