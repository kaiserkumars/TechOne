using Windows.UI.Xaml.Navigation;
using AppStudio.Common;
using AppStudio.DataProviders.Rss;
using TechOne;
using TechOne.Sections;
using TechOne.ViewModels;

namespace TechOne.Views
{
    public sealed partial class WiredListPage : PageBase
    {
        public ListViewModel<RssDataConfig, RssSchema> ViewModel { get; set; }

        public WiredListPage()
        {
            this.ViewModel = new ListViewModel<RssDataConfig, RssSchema>(new WiredConfig());
            this.InitializeComponent();
        }

        protected async override void LoadState(object navParameter)
        {
            await this.ViewModel.LoadDataAsync();
        }

    }
}
