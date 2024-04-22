using InoTec;
using System.Collections.ObjectModel;

namespace ClsFusionViewer.ViewModels
{
    public class BcsBatStatusInfo
    {
        private InoTec.BcsBatStatusInfo _model;
        private ObservableCollection<BatStatus> _bcsBatStatus;

        public BatInfo BcsBatInfo => _model.BcsBatInfo;
        public ObservableCollection<BatStatus> BcsBatStatus
        {
            get => _bcsBatStatus;
            set => _bcsBatStatus = value;
        }
        public BcsBatStatusInfo(InoTec.BcsBatStatusInfo model)
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


            return Equals((BcsBatStatusInfo)obj);
        }
        public bool Equals(BcsBatStatusInfo obj)
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
