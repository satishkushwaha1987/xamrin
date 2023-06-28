using CHSBackOffice.CustomControls;
using CHSBackOffice.Support;
using CHSBackOffice.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CHSBackOffice.Views
{
    public partial class TransactionsCarouselPage : CarouselPage
    {
        int transactionsCount => TransactionsCache.Instance.TransationsCount;

        int currentTransactionNumber = 0;

        public TransactionsCarouselPage()
        {
            try
            {
                InitializeComponent();
                currentTransactionNumber = Support.CommonViewObjects.Instance.SelectedTransactionNumber;
                InitializePages();
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        void InitializePages()
        {
            try
            {
                if (transactionsCount > 2)
                {

                    var page1 = new TransactionDetailsFrame();
                    page1.Appearing += Page_Appearing;
                    var page2 = new TransactionDetailsFrame();
                    page2.Appearing += Page_Appearing;
                    var page3 = new TransactionDetailsFrame();
                    page3.Appearing += Page_Appearing;

                    Children.Add(page1);
                    Children.Add(page2);
                    Children.Add(page3);

                    SetBindingContexts();

                    if (currentTransactionNumber == 0)
                        CurrentPage = Children[0];
                    else if (currentTransactionNumber == transactionsCount - 1)
                        CurrentPage = Children[2];
                    else
                        CurrentPage = Children[1];
                }
                else
                {
                    for (int i = 0; i < transactionsCount; i++)
                    {
                        var page = new TransactionDetailsFrame();
                        Children.Add(page);
                    }
                    SetBindingContexts();
                    CurrentPage = Children[currentTransactionNumber];
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        async void SetBindingContexts()
        {
            if (transactionsCount > 2)
            {
                int page1Number = currentTransactionNumber == 0 ?
                0 :
                (currentTransactionNumber == transactionsCount - 1 ? transactionsCount - 3 : currentTransactionNumber - 1);
                int page2Number = currentTransactionNumber == 0 ?
                    1 :
                    (currentTransactionNumber == transactionsCount - 1 ? transactionsCount - 2 : currentTransactionNumber);
                int page3Number = currentTransactionNumber == 0 ?
                    2 :
                    (currentTransactionNumber == transactionsCount - 1 ? transactionsCount - 1 : currentTransactionNumber + 1);

                try
                {
                    Children[0].BindingContext = await GetModel(page1Number);
                    Children[1].BindingContext = await GetModel(page2Number);
                    Children[2].BindingContext = await GetModel(page3Number);
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                }
            }
            else
            {
                for (int i = 0; i < transactionsCount; i++)
                {
                    Children[i].BindingContext = await GetModel(i);
                }
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.SharedEventAggregator.GetEvent<Events.ShowTransaction>().Subscribe(ShowTransaction);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.SharedEventAggregator.GetEvent<Events.ShowTransaction>().Unsubscribe(ShowTransaction);
        }

        void ShowTransaction(string tag)
        {
            CommonViewObjects.Instance.IsLoadingVisible = true;
            try
            {
                switch (tag)
                {
                    case "First":
                        currentTransactionNumber = 0;
                        SetBindingContexts();
                        CurrentPage = Children[0];
                        break;
                    case "Last":
                        currentTransactionNumber = transactionsCount - 1;
                        SetBindingContexts();
                        CurrentPage = Children[Children.Count - 1];
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
            CommonViewObjects.Instance.IsLoadingVisible = false;
        }

        private async void Page_Appearing(object sender, System.EventArgs e)
        {
            try
            {
                var number = Children.IndexOf((ContentPage)sender);

                if (number == 0 && currentTransactionNumber > 1) //swipe to left page and has more than one transaction at the left
                {
                    currentTransactionNumber--;

                    var pageToMove = Children[2];
                    Children.Remove(pageToMove);
                    pageToMove.BindingContext = await GetModel(currentTransactionNumber - 1);
                    Children.Insert(0, pageToMove);
                }

                if (number == 1 && currentTransactionNumber == 0) //swipe from 0 to 1
                    currentTransactionNumber = 1;

                if (number == 1 && currentTransactionNumber == transactionsCount - 1) //swipe from last to previous
                    currentTransactionNumber--;

                    if (number == 2 && currentTransactionNumber < transactionsCount - 2) //swipe to right page and has more than one transaction at the right
                {
                    currentTransactionNumber++;

                    var pageToMove = Children[0];
                    Children.Remove(pageToMove);
                    pageToMove.BindingContext = await GetModel(currentTransactionNumber + 1);
                    Children.Add(pageToMove);
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        async Task<TransactionsDetailsFrameViewModel> GetModel(int transactionNumber)
        {
            return new TransactionsDetailsFrameViewModel(await TransactionsCache.Instance.GetTransaction(transactionNumber), transactionNumber, transactionsCount);
        }
    }
}
