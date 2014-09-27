using System.ComponentModel;
using int32.Utils.Generics.ViewModel.Contracts;

namespace int32.Utils.Generics.ViewModel
{
    public abstract class ViewModel : IViewModel
    {
        private bool _isLoaded;

        public bool IsLoaded
        {
            get { return _isLoaded; }
            set { SetProperty(ref _isLoaded, value, "IsLoaded"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public abstract void Load();

        protected bool SetProperty<T>(ref T storage, T value, string propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged(string propertyName = null)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
