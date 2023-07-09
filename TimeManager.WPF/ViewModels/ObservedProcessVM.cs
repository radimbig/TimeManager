using System;
using System.ComponentModel;
using TimeManager.Core.Models;

namespace TimeManager.WPF.ViewModels
{
    public class ObservedProcessVM : ObservedProcess, INotifyPropertyChanged
    {
        public override DateTime CreatedAt
        {
            get => base.CreatedAt;
            set
            {
                base.CreatedAt = value;
                NotifyOfPropertyChanged(nameof(CreatedAt));
            }
        }
        public override TimeSpan TotalSpent
        {
            get => base.TotalSpent;
            set
            {
                base.TotalSpent = value;
                NotifyOfPropertyChanged(nameof(TotalSpent));
            }
        }

        public override DateTime OpenedAt
        {
            get => base.OpenedAt;
            set
            {
                base.OpenedAt = value;
                NotifyOfPropertyChanged(nameof(OpenedAt));
            }
        }
        public override DateTime ClosedAt
        {
            get => base.ClosedAt;
            set 
            {
                base.ClosedAt = value;
                NotifyOfPropertyChanged(nameof(ClosedAt));
            }
        }

        public ObservedProcessVM(string name)
            : base(name) { }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyOfPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
