using InoTec;
using System.Collections.ObjectModel;

namespace ClsFusionViewer.ViewModels
{
    public class BcsBatStatusInfo
    {
        private InoTec.BcsBatStatusInfoType _model;
        private ObservableCollection<BatStatusType> _bcsBatStatus;

        public BatInfo BcsBatInfo => _model.BcsBatInfo;
        public ObservableCollection<BatStatusType> BcsBatStatus
        {
            get => _bcsBatStatus;
            set => _bcsBatStatus = value;
        }
        public BcsBatStatusInfo(InoTec.BcsBatStatusInfoType model)
        {
            _model = model;
            _bcsBatStatus = new ObservableCollection<BatStatusType>(_model.BcsBatStatus);
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
