using int32.Utils.Generics.ViewModel;

namespace Tests.Samples
{
    public class SampleModelViewModel : ViewModel
    {
        private int x = 3;

        public int Test
        {
            get { return x; }
            set { SetProperty(ref x, value, "Test"); }
        }
        public override void Load()
        {
            IsLoaded = true;
        }
    }
}
