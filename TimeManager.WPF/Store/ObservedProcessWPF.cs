using System;
using System.ComponentModel;
using TimeManager.Core.Interfaces;
using TimeManager.Core.Models;

namespace TimeManager.WPF.Store
{
    public class ObservedProcessWPF : INotifyPropertyChanged, IObservedProcess
    {
        private ObservedProcess _realObservedProcess;
        public DateTime ClosedAt
        {
            get => _realObservedProcess.ClosedAt;
            set
            {
                _realObservedProcess.ClosedAt = value;
                NotifyOfPropertyChanged(nameof(ClosedAt));
            }
        }
        public DateTime CreatedAt
        {
            get => _realObservedProcess.CreatedAt;
            set
            {
                _realObservedProcess.CreatedAt = value;
                NotifyOfPropertyChanged(nameof(CreatedAt));
            }
        }
        public int Id
        {
            get => _realObservedProcess.Id;
            set { _realObservedProcess.Id = value; }
        }
        public string Name
        {
            get => _realObservedProcess.Name;
            set { _realObservedProcess.Name = value; }
        }
        public DateTime OpenedAt
        {
            get => _realObservedProcess.OpenedAt;
            set
            {
                _realObservedProcess.OpenedAt = value;
                NotifyOfPropertyChanged(nameof(OpenedAt));
            }
        }
        public TimeSpan TotalSpent
        {
            get => _realObservedProcess.TotalSpent;
            set
            {
                _realObservedProcess.TotalSpent = value;
                NotifyOfPropertyChanged(nameof(OpenedAt));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyOfPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservedProcessWPF(string name)
        {
            _realObservedProcess = new ObservedProcess(name);
        }
    }
}
