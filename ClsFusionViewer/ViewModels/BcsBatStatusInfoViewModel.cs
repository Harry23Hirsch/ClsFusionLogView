using InoTec;
using System.Collections.ObjectModel;

namespace ClsFusionViewer.ViewModels
{
    public class BcsBatStatusInfoViewModel
    {
        private BcsBatStatusInfo _model;
        private ObservableCollection<BatStatus> _bcsBatStatus;

        public BatInfo BcsBatInfo => _model.BcsBatInfo;
        public ObservableCollection<BatStatus> BcsBatStatus
        {
            get => _bcsBatStatus;
            set => _bcsBatStatus = value;
        }
        public BcsBatStatusInfoViewModel(BcsBatStatusInfo model)
        {
            _model = model;
            _bcsBatStatus = new ObservableCollection<BatStatus>(_model.BcsBatStatus);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;


            return Equals((BcsBatStatusInfoViewModel)obj);
        }
        public bool Equals(BcsBatStatusInfoViewModel obj)
        {
            if (BcsBatInfo == null ||
                obj.BcsBatInfo == null)
                return false;

            if (BcsBatInfo.N == obj.BcsBatInfo.N)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
