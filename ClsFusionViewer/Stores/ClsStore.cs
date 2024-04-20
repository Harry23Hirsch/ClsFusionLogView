using InoTec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClsFusionViewer.Stores
{
    public class ClsStore
    {
        private IEnumerable<IEnumerable<ClsLogFileLine>> _clsLogFiles;
        private IEnumerable<BcsBatStatusInfo> _bcsLogFiles;
        private IEnumerable<ClsFaultInfo> _clsStatusLogFiles;

        public event Action ClsLogFiles_Changed;
        public event Action BcsLogFiles_Changed;
        public event Action ClsStatusLogFiles_Changed;

        public IEnumerable<IEnumerable<ClsLogFileLine>> ClsLogFiles
        {
            get => _clsLogFiles;
            set
            {
                _clsLogFiles = value;
                OnClsLogFilesChanged();
            }
        }
        public IEnumerable<BcsBatStatusInfo> BcsLogFiles
        {
            get => _bcsLogFiles;
            set
            {
                _bcsLogFiles = value;
                OnBcsLogFilesChanged();
            }
        }
        public IEnumerable<ClsFaultInfo> ClsStatusLogFiles
        {
            get => _clsStatusLogFiles;
            set
            {
                _clsStatusLogFiles = value;
                OnClsStatusLogFilesChanged();
            }
        }

        private void OnClsStatusLogFilesChanged()
        {
            ClsStatusLogFiles_Changed?.Invoke();
        }

        private void OnBcsLogFilesChanged()
        {
            BcsLogFiles_Changed?.Invoke();
        }

        private void OnClsLogFilesChanged()
        {
            ClsLogFiles_Changed?.Invoke();
        }
    }
}
