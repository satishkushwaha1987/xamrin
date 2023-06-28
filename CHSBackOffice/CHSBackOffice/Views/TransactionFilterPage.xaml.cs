using CHSBackOffice.CustomControls;
using CHSBackOffice.Events;
using CHSBackOffice.Support;
using System;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TransactionFilterPage : ExtendedNaviPage
	{
        #region .CTOR
        public TransactionFilterPage ()
		{
            try
            {
                InitializeComponent();
                OnOrientartionChanged += TransactionFilterPage_OnOrientartionChanged;
                scroll.Scrolled += Scroll_Scrolled;
                var popupLayout = PopupLayout.AddToPage(this);
                machineIdPopover.ParentContext = popupLayout;
                transactionTypePopover.ParentContext = popupLayout;
                transactionStatusPopover.ParentContext = popupLayout;


                if (PopoverData.Instance.EndDateTimeSelected.Date != DateTime.MinValue)
                {
                    DatesRadioGroup.DateRangeFromDateTime = PopoverData.Instance.StartDateTimeSelected.Date;
                    DatesRadioGroup.DateRangeFromTimeSpan = PopoverData.Instance.StartDateTimeSelected;

                    DatesRadioGroup.SpecificDateDateTime = PopoverData.Instance.StartDateTimeSelected.Date;
                }

                if (PopoverData.Instance.EndDateTimeSelected.Date != DateTime.MinValue)
                {
                    DatesRadioGroup.DateRangeToDateTime = PopoverData.Instance.EndDateTimeSelected.Date;
                    DatesRadioGroup.DateRangeToTimeSpan = PopoverData.Instance.EndDateTimeSelected;
                }
                else
                    DatesRadioGroup.DateRangeToTimeSpan = new DateTime(1, 1, 1, 23, 59, 59);




            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
		}

        #endregion

        #region "Event Handling"

        private void Scroll_Scrolled(object sender, Xamarin.Forms.ScrolledEventArgs e)
        {
            App.SharedEventAggregator.GetEvent<ScrollYChanged>().Publish(e.ScrollY);
            
        }

        private void TransactionFilterPage_OnOrientartionChanged(object sender, PageOrientationEventsArgs e)
        {
            App.SharedEventAggregator.GetEvent<OrientationChanged>().Publish();
        }

        #endregion
    }
}
 