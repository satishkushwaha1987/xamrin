using CHSBackOffice.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RadioButtonGroup : ContentView, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public new event PropertyChangedEventHandler PropertyChanged;

        internal void RaisePropertyChanged(string nameOfProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameOfProperty));
        }

        void SetProperty<T>(ref T backingStore, T value, [CallerMemberName]string propertyName = "")
        {
            backingStore = value;

            RaisePropertyChanged(propertyName);
        }
        #endregion

        #region Bindable Property
        
        #region Specific Date

        public DateTime SpecificDateDateTime
        {
            get => (DateTime)GetValue(SpecificDateDateTimeProperty);
            set => base.SetValue(SpecificDateDateTimeProperty, value);
        }

        public static readonly BindableProperty SpecificDateDateTimeProperty = BindableProperty.Create(
                                                            propertyName: "SpecificDateDateTime",
                                                            returnType: typeof(DateTime),
                                                            declaringType: typeof(RadioButtonGroup),
                                                            defaultValue: DateTime.UtcNow,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: SpecificDateDateTimeChanged);

        static void SpecificDateDateTimeChanged(object sender, object oldValue, object newValue)
        {
            var radioGroup = sender as RadioButtonGroup;
            if (radioGroup != null && newValue != null)
            {
                radioGroup.specificDate.Text = string.Format("{0:dd/MM/yy}", newValue);
                AddDataPicker.SetDate(radioGroup.specificDate, (DateTime)newValue);
            }
        }
        #endregion

        #region Date Range

        #region From
        public DateTime DateRangeFromDateTime
        {
            get => (DateTime)GetValue(DateRangeFromDateTimeProperty);
            set => base.SetValue(DateRangeFromDateTimeProperty, value);
        }

        public static readonly BindableProperty DateRangeFromDateTimeProperty = BindableProperty.Create(
                                                            propertyName: "DateRangeFromDateTime",
                                                            returnType: typeof(DateTime),
                                                            declaringType: typeof(RadioButtonGroup),
                                                            defaultValue: DateTime.UtcNow,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: DateRangeFromDateTimeChanged);

        static void DateRangeFromDateTimeChanged(object sender, object oldValue, object newValue)
        {
            var radioGroup = sender as RadioButtonGroup;
            if (radioGroup != null && newValue != null)
            {
                radioGroup.fromDay.Text = string.Format("{0:dd/MM/yy}", newValue);
                AddDataPicker.SetDate(radioGroup.fromDay, (DateTime)newValue);
            }
        }

        public DateTime DateRangeFromTimeSpan
        {
            get => (DateTime)GetValue(DateRangeFromTimeSpanProperty);
            set => base.SetValue(DateRangeFromTimeSpanProperty, value);
        }

        public static readonly BindableProperty DateRangeFromTimeSpanProperty = BindableProperty.Create(
                                                            propertyName: "DateRangeFromTimeSpan",
                                                            returnType: typeof(DateTime),
                                                            declaringType: typeof(RadioButtonGroup),
                                                            defaultValue: DateTime.UtcNow.Date.AddHours(0).AddMinutes(0).AddSeconds(0),
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: DateRangeFromTimeSpanChanged);

        static void DateRangeFromTimeSpanChanged(object sender, object oldValue, object newValue)
        {
            var radioGroup = sender as RadioButtonGroup;
            if (radioGroup != null && newValue != null)
            {
                radioGroup.fromTime.Text = string.Format("{0:HH:mm:ss tt}", newValue);
                var date = (DateTime)newValue;
                var time = date - date.Date;
                AddTimePicker.SetTime(radioGroup.fromTime, time);
            }
        }

        #endregion

        #region To
        public DateTime DateRangeToDateTime
        {
            get => (DateTime)GetValue(DateRangeToDateTimeProperty);
            set => base.SetValue(DateRangeToDateTimeProperty, value);
        }

        public static readonly BindableProperty DateRangeToDateTimeProperty = BindableProperty.Create(
                                                            propertyName: "DateRangeToDateTime",
                                                            returnType: typeof(DateTime),
                                                            declaringType: typeof(RadioButtonGroup),
                                                            defaultValue: DateTime.UtcNow.Date.AddHours(0).AddMinutes(0).AddSeconds(0),
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: DateRangeToDateTimeChanged);

        static void DateRangeToDateTimeChanged(object sender, object oldValue, object newValue)
        {
            var radioGroup = sender as RadioButtonGroup;
            if (radioGroup != null && newValue != null)
            {
                radioGroup.toDay.Text = string.Format("{0:dd/MM/yy}", newValue);
                AddDataPicker.SetDate(radioGroup.toDay, (DateTime)newValue);
            }
        }

        public DateTime DateRangeToTimeSpan
        {
            get => (DateTime)GetValue(DateRangeToTimeSpanProperty);
            set => base.SetValue(DateRangeToTimeSpanProperty, value);
        }

        public static readonly BindableProperty DateRangeToTimeSpanProperty = BindableProperty.Create(
                                                            propertyName: "DateRangeToTimeSpan",
                                                            returnType: typeof(DateTime),
                                                            declaringType: typeof(RadioButtonGroup),
                                                            defaultValue: DateTime.UtcNow.Date.AddHours(0).AddMinutes(0).AddSeconds(0),
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: DateRangeToTimeSpanChanged);

        static void DateRangeToTimeSpanChanged(object sender, object oldValue, object newValue)
        {
            var radioGroup = sender as RadioButtonGroup;
            if (radioGroup != null && newValue != null)
            {
                radioGroup.toTime.Text = string.Format("{0:HH:mm:ss tt}", newValue);
                var date = (DateTime)newValue;
                var time = date - date.Date;
                AddTimePicker.SetTime(radioGroup.toTime, time);
            }
        }


        #endregion

        #region Start & End Date
        public DateTime StartDateTime
        {
            get => (DateTime)GetValue(StartDateTimeProperty);
            set => base.SetValue(StartDateTimeProperty, value);
        }

        public static readonly BindableProperty StartDateTimeProperty = BindableProperty.Create(
                                                            propertyName: "StartDateTimeProperty",
                                                            returnType: typeof(DateTime),
                                                            declaringType: typeof(RadioButtonGroup),
                                                            defaultValue: new DateTime(1900, 1, 1),
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: StartDateTimeChanged);

        static void StartDateTimeChanged(object sender, object oldValue, object newValue)
        {

        }

        public DateTime EndDateTime
        {
            get => (DateTime)GetValue(EndDateTimeProperty);
            set => base.SetValue(EndDateTimeProperty, value);
        }

        public static readonly BindableProperty EndDateTimeProperty = BindableProperty.Create(
                                                            propertyName: "EndDateTimeProperty",
                                                            returnType: typeof(DateTime),
                                                            declaringType: typeof(RadioButtonGroup),
                                                            defaultValue: DateTime.UtcNow.AddDays(1),
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: EndDateTimeChanged);

        static void EndDateTimeChanged(object sender, object oldValue, object newValue)
        {
            
        }

        #region Selected Period

        public DateTime SelectedStartDateTime
        {
            get => (DateTime)GetValue(SelectedStartDateTimeProperty);
            set => base.SetValue(SelectedStartDateTimeProperty, value);
        }

        public static readonly BindableProperty SelectedStartDateTimeProperty = BindableProperty.Create(
                                                            propertyName: "SelectedStartDateTime",
                                                            returnType: typeof(DateTime),
                                                            declaringType: typeof(RadioButtonGroup),
                                                            defaultValue: new DateTime(1900, 1, 1),
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: SelectedStartDateTimeChanged);

        static void SelectedStartDateTimeChanged(object sender, object oldValue, object newValue)
        {

        }

        public DateTime SelectedEndDateTime
        {
            get => (DateTime)GetValue(SelectedEndDateTimeProperty);
            set => base.SetValue(SelectedEndDateTimeProperty, value);
        }

        public static readonly BindableProperty SelectedEndDateTimeProperty = BindableProperty.Create(
                                                            propertyName: "SelectedEndDateTime",
                                                            returnType: typeof(DateTime),
                                                            declaringType: typeof(RadioButtonGroup),
                                                            defaultValue: DateTime.UtcNow.AddDays(1),
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: SelectedEndDateTimeChanged);

        static void SelectedEndDateTimeChanged(object sender, object oldValue, object newValue)
        {
            (sender as RadioButtonGroup).RaisePropertyChanged(nameof(SelectedEndDateTime));

        }

        public IEnumerable<CustomRadioButton> ItemsSource
        {
            get { return (IEnumerable<CustomRadioButton>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static BindableProperty ItemsSourceProperty = BindableProperty.Create(
                                                    propertyName: "ItemsSource",
                                                    returnType: typeof(IEnumerable<CustomRadioButton>),
                                                    declaringType: typeof(RadioButtonGroup),
                                                    defaultValue: default(IEnumerable));
        public int SelectedCheckIndex
        {
            get { return (int)GetValue(SelectedCheckIndexProperty); }
            set { SetValue(SelectedCheckIndexProperty, value); }
        }

        public static BindableProperty SelectedCheckIndexProperty = BindableProperty.Create(
                                                    propertyName: "SelectedCheckIndex",
                                                    returnType: typeof(int),
                                                    declaringType: typeof(RadioButtonGroup),
                                                    defaultValue: -1,
                                                    propertyChanged: SelectedCheckIndexChanged);

        static void SelectedCheckIndexChanged(object sender, object oldValue, object newValue)
        {
            int index = (int)newValue;
            if (index != -1 && oldValue != newValue)
            {
                var source = (sender as RadioButtonGroup).ItemsSource.ToList();
                var button = source[index] as CustomRadioButton;
                button.Checked = true;
            }
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region Command
        public ICommand SelectedSpecificDateCommand => new Command<DateTime>(SelectedSpecificDateExecute);
        public ICommand SelectedDateRangeFromDateCommand => new Command<DateTime>(SelectedDateRangeFromDateExecute);
        public ICommand SelectedDateRangeFromTimeCommand => new Command<TimeSpan>(SelectedDateRangeFromTimeExecute);
        public ICommand SelectedDateRangeToDateCommand => new Command<DateTime>(SelectedDateRangeToDateExecute);
        public ICommand SelectedDateRangeToTimeCommand => new Command<TimeSpan>(SelectedDateRangeToTimeExecute);
        public ICommand SpecificDateTapCommand => new Command(SpecificDateTapExecute);
        public ICommand SpecificDateRangeTapCommand => new Command(SpecificDateRangeTapExecute);
        #endregion

        #region CTOR
        public RadioButtonGroup ()
		{
            try
            {
                InitializeComponent();

                specificDayButton.CheckedChanged += OnSpecificDateCheckedChanged;
                dateRangeButton.CheckedChanged += OnDateRangeCheckedChanged;
                allDatesButton.CheckedChanged += OnAllDatesCheckedChanged;
                transaction_icon.Text = Constants.CHSIcons.Calendar;
                ItemsSource = new List<CustomRadioButton> { specificDayButton, dateRangeButton, allDatesButton };

            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }
        #endregion

        #region Methods
        protected override void OnParentSet()
        {
            base.OnParentSet();
        }

        protected void OnSpecificDateCheckedChanged(object sender, EventArgs<bool> arg)
        {
            if (arg.Value)
            {
                dateRangeButton.Checked = false;
                allDatesButton.Checked = false;
                SelectedStartDateTime = SpecificDateDateTime;
                SelectedEndDateTime = SpecificDateDateTime;
                SelectedCheckIndex = 0;
            }
        }

        protected void OnDateRangeCheckedChanged(object sender, EventArgs<bool> arg)
        {
            if (arg.Value)
            {
                specificDayButton.Checked = false;
                allDatesButton.Checked = false;

                var fromDate = DateRangeFromDateTime.Date;
                var fromTime = DateRangeFromTimeSpan.TimeOfDay;
                SelectedStartDateTime = fromDate.AddHours(fromTime.Hours).AddMinutes(fromTime.Minutes).AddSeconds(fromTime.Seconds);
                
                var toDate = DateRangeToDateTime.Date;
                var toTime = DateRangeToTimeSpan.TimeOfDay;

                SelectedEndDateTime = toDate.AddHours(toTime.Hours).AddMinutes(toTime.Minutes).AddSeconds(toTime.Seconds);
                SelectedCheckIndex = 1;
            }
        }

        protected void OnAllDatesCheckedChanged(object sender, EventArgs<bool> arg)
        {
            if (arg.Value)
            {
                dateRangeButton.Checked = false;
                specificDayButton.Checked = false;
                SelectedStartDateTime = StartDateTime;
                SelectedEndDateTime = EndDateTime;
                SelectedCheckIndex = 2;
            }
        }

        protected void SpecificDateTapExecute()
        {
            specificDayButton.Checked = true;
        }

        protected void SpecificDateRangeTapExecute()
        {
            dateRangeButton.Checked = true;
        }

        protected void SelectedSpecificDateExecute(DateTime selectedDate)
        {
            SelectedStartDateTime = selectedDate;
            SelectedEndDateTime = selectedDate;
        }

        protected void SelectedDateRangeFromDateExecute(DateTime selectedDate)
        {
            var currentTime = SelectedStartDateTime.TimeOfDay;
            SelectedStartDateTime = selectedDate.Add(currentTime);
        }

        protected void SelectedDateRangeFromTimeExecute(TimeSpan selectedTime)
        {
            var currentDate = SelectedStartDateTime.Date;
            SelectedStartDateTime = currentDate.AddHours(selectedTime.Hours).AddMinutes(selectedTime.Minutes).AddSeconds(selectedTime.Seconds);
        }

        protected void SelectedDateRangeToDateExecute(DateTime selectedDate)
        {
            var currentTime = SelectedEndDateTime.TimeOfDay;
            SelectedEndDateTime = selectedDate.Add(currentTime);
        }

        protected void SelectedDateRangeToTimeExecute(TimeSpan selectedTime)
        {
            var currentDate = SelectedEndDateTime.Date;
            SelectedEndDateTime = currentDate.AddHours(selectedTime.Hours).AddMinutes(selectedTime.Minutes).AddSeconds(selectedTime.Seconds);
        }
        #endregion
    }
}