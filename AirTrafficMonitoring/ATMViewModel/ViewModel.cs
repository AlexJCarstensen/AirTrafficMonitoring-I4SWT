using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ATMModel;
using ATMModel.Data;
using ATMModel.Events;
using ATMViewModel.Annotations;
using TransponderReceiver;

namespace ATMViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<IATMTransponderData> TransponderDataItem { get; }

        public ObservableCollection<NotificationEventArgs> NotificationCollection { get; } 
        

        public ViewModel()
        {
            var atmModelObj = new ATMDataDecoder(TransponderReceiverFactory.CreateTransponderDataReceiver());
            //var atmModelObj = new ATMDataDecoder(new FakeTransponderSource());
            this.TransponderDataItem = new ObservableCollection<IATMTransponderData>();
            NotificationCollection = new ObservableCollection<NotificationEventArgs>();

            atmModelObj._event += (sender, items) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    TransponderDataItem.Clear();
                    foreach (var item in items)
                    {
                        TransponderDataItem.Add(item);
                    }
                });
            };

            ATMNotification.NotificationEvent += (sender, args) =>
            {
                args.StopMeEvent += (o, eventArgs) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        NotificationCollection.Remove(
                        NotificationCollection.First(t => t.Tag == ((NotificationEventArgs)o)?.Tag));
                    });
                };
                Application.Current.Dispatcher.Invoke(() =>
                {
                    NotificationCollection.Add(args);
                });
            };

            //ATMEventHandler.Notification += (sender, args) =>
            //{
            //    if (args.EventType != EventTypes.Separation)
            //    {
            //        args.StopMe += (o, s) =>
            //        {
            //            Application.Current.Dispatcher.Invoke(() =>
            //            {
            //                NotificationCollection.Remove(NotificationCollection.First(t => t.Tag == s));
            //            });
            //        };
            //    }
            //    else
            //    {
            //        if (NotificationCollection.Any(t => t.Tag == args.Tag))
            //            NotificationCollection.Remove(NotificationCollection.First(t => t.Tag == args.Tag));
            //    }

            //    Application.Current.Dispatcher.Invoke(() =>
            //    {
            //        NotificationCollection.Add(args);
            //    });
            //};
        }
        

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
