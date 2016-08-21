using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.Common;
using AppStudio.Common.Actions;
using AppStudio.Common.Commands;
using AppStudio.Common.Navigation;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Rss;
using AppStudio.DataProviders.LocalStorage;
using TechOne.Sections;


namespace TechOne.ViewModels
{
    public class MainViewModel : ObservableBase
    {
        public MainViewModel(int visibleItems)
        {
            PageTitle = "TechOne";
            HackerEarth = new ListViewModel<RssDataConfig, RssSchema>(new HackerEarthConfig(), visibleItems);
            Quora = new ListViewModel<RssDataConfig, RssSchema>(new QuoraConfig(), visibleItems);
            GeeksForGeeks = new ListViewModel<RssDataConfig, RssSchema>(new GeeksForGeeksConfig(), visibleItems);
            Wired = new ListViewModel<RssDataConfig, RssSchema>(new WiredConfig(), visibleItems);
            TechCrunch = new ListViewModel<RssDataConfig, RssSchema>(new TechCrunchConfig(), visibleItems);
            CodeChef = new ListViewModel<RssDataConfig, RssSchema>(new CodeChefConfig(), visibleItems);
            HackerRank = new ListViewModel<RssDataConfig, RssSchema>(new HackerRankConfig(), visibleItems);
            Actions = new List<ActionInfo>();

            if (GetViewModels().Any(vm => !vm.HasLocalData))
            {
                Actions.Add(new ActionInfo
                {
                    Command = new RelayCommand(Refresh),
                    Style = ActionKnownStyles.Refresh,
                    Name = "RefreshButton",
                    ActionType = ActionType.Primary
                });
            }
        }

        public string PageTitle { get; set; }
        public ListViewModel<RssDataConfig, RssSchema> HackerEarth { get; private set; }
        public ListViewModel<RssDataConfig, RssSchema> Quora { get; private set; }
        public ListViewModel<RssDataConfig, RssSchema> GeeksForGeeks { get; private set; }
        public ListViewModel<RssDataConfig, RssSchema> Wired { get; private set; }
        public ListViewModel<RssDataConfig, RssSchema> TechCrunch { get; private set; }
        public ListViewModel<RssDataConfig, RssSchema> CodeChef { get; private set; }
        public ListViewModel<RssDataConfig, RssSchema> HackerRank { get; private set; }

        public RelayCommand<INavigable> SectionHeaderClickCommand
        {
            get
            {
                return new RelayCommand<INavigable>(item =>
                    {
                        NavigationService.NavigateTo(item);
                    });
            }
        }

        public DateTime? LastUpdated
        {
            get
            {
                return GetViewModels().Select(vm => vm.LastUpdated)
                            .OrderByDescending(d => d).FirstOrDefault();
            }
        }

        public List<ActionInfo> Actions { get; private set; }

        public bool HasActions
        {
            get
            {
                return Actions != null && Actions.Count > 0;
            }
        }

        public async Task LoadDataAsync()
        {
            var loadDataTasks = GetViewModels().Select(vm => vm.LoadDataAsync());

            await Task.WhenAll(loadDataTasks);

            OnPropertyChanged("LastUpdated");
        }

        private async void Refresh()
        {
            var refreshDataTasks = GetViewModels()
                                        .Where(vm => !vm.HasLocalData)
                                        .Select(vm => vm.LoadDataAsync(true));

            await Task.WhenAll(refreshDataTasks);

            OnPropertyChanged("LastUpdated");
        }

        private IEnumerable<DataViewModelBase> GetViewModels()
        {
            yield return HackerEarth;
            yield return Quora;
            yield return GeeksForGeeks;
            yield return Wired;
            yield return TechCrunch;
            yield return CodeChef;
            yield return HackerRank;
        }
    }
}
