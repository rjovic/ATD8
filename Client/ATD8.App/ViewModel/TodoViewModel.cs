using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace ATD8.App.ViewModel
{
    public class TodoViewModel : ViewModelBase
    {
        private int _todoId;
        public int TodoId
        {
            get { return _todoId; }
            set { _todoId = value; RaisePropertyChanged(() => TodoId); }
        }

        private string _task;
        public string Task
        {
            get { return _task; }
            set { _task = value; RaisePropertyChanged(() => Task); }
        }

        private bool _isDone;
        public bool IsDone
        {
            get { return _isDone; }
            set { _isDone = value; RaisePropertyChanged(() => IsDone); }
        }
    }
}
