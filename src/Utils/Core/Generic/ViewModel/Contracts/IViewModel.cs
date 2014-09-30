using System.ComponentModel;

namespace int32.Utils.Core.Generic.ViewModel.Contracts
{
    public interface IViewModel : INotifyPropertyChanged
    {
        bool IsLoaded { get; set; }
        void Load();
    }
}
