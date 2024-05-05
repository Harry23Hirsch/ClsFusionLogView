using InoTec;
using System;

namespace ClsFusionViewer.ViewModels
{
    public class ClsLogFileLine : IComparable<ClsLogFileLine>
    {
        private ClsLogFileLineType _model;

        public int Year => int.Parse(_model.Year);
        public int Month => int.Parse(_model.Month);
        public int Day => int.Parse(_model.Day);
        public string Time => _model.Time;
        public string Text => _model.Text;

        public ClsLogFileLine(ClsLogFileLineType model)
        {
            _model = model;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != this.GetType())
                return false;


            return Equals((ClsLogFileLine)obj);
        }

        public bool Equals(ClsLogFileLine other)
        {
            if (this.Time == other.Time &&
                this.Text == other.Text)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int CompareTo(ClsLogFileLine other)
        {
            int result = this.Year.CompareTo(other.Year);
            if (result == 0)
            {
                result = this.Month.CompareTo(other.Month);
                if (result == 0)
                {
                    result = this.Day.CompareTo(other.Day);
                    {
                        if (result == 0)
                        {
                            result = this.Time.CompareTo(other.Time);
                        }
                    }
                }
            }

            return result;
        }
    }
}
