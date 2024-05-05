using ClsFusionViewer.Services;

namespace ClsFusionViewer.Commands
{
    public class NavigateCommand : BaseCommand
    {
        private readonly NavigationService _navigationService;

        public NavigateCommand(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}
