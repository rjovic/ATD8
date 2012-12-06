using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ATD8.DataService;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RtMapper;
using Windows.UI.Xaml;

namespace ATD8.App.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly TodoService _todoService;

        private TodoViewModel _selectedTodo;
        public TodoViewModel SelectedTodo
        {
            get { return _selectedTodo; }
            set { _selectedTodo = value; RaisePropertyChanged(() => SelectedTodo); }
        }

        private ObservableCollection<TodoViewModel> _todoes;
        public ObservableCollection<TodoViewModel> Todoes
        {
            get { return _todoes; }
            set { _todoes = value; RaisePropertyChanged(() => Todoes); }
        }

        private Visibility _showCrudGrid;
        public Visibility ShowCrudGrid
        {
            get { return _showCrudGrid; }
            set { _showCrudGrid = value; RaisePropertyChanged(() => ShowCrudGrid); }
        }

        private bool _isWorking;
        public bool IsWorking
        {
            get { return _isWorking; }
            set { _isWorking = value; RaisePropertyChanged(() => IsWorking); }
        }

        public ICommand DeleteTodoCommand
        {
            get
            {
                return new RelayCommand<TodoViewModel>(async (todo) =>
                {
                    await _todoService.Delete(todo.TodoId);
                    var todoToRemove = _todoes.FirstOrDefault(x => x.TodoId == todo.TodoId);

                    _todoes.Remove(todoToRemove);
                });
            }
        }

        public ICommand EditTodoCommand
        {
            get
            {
                return new RelayCommand<TodoViewModel>((todo) =>
                {
                    SelectedTodo = todo;
                    ShowCrudGrid = Visibility.Visible;
                });
            }
        }

        public ICommand CrudTodoCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    if (string.IsNullOrEmpty(_selectedTodo.Task)) return;

                    IsWorking = true;

                    if (_selectedTodo.TodoId == 0)
                    {
                        var newTodo = await _todoService.Create(RtMap.Map<TodoViewModel, Todo>(_selectedTodo));
                        _todoes.Add(RtMap.Map<Todo, TodoViewModel>(newTodo));
                    }
                    else
                    {
                        _todoService.Update(RtMap.Map<TodoViewModel, Todo>(_selectedTodo));
                    }

                    ShowCrudGrid = Visibility.Collapsed;
                    IsWorking = false;
                });
            }
        }

        public ICommand ShowCrudGridCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ShowCrudGrid = Visibility.Visible;
                    SelectedTodo = new TodoViewModel();
                });
            }
        }

        public MainViewModel()
        {
            _todoService = new TodoService();

            _todoes = new ObservableCollection<TodoViewModel>();
            _showCrudGrid = Visibility.Collapsed;

            RtMap.ConfigureMapping<Todo, TodoViewModel>();
            RtMap.ConfigureMapping<TodoViewModel, Todo>();

            GetData();
        }

        private async void GetData()
        {
            IsWorking = true;

            var result = await _todoService.GetAll();
            foreach (var todo in result)
            {
                _todoes.Add(RtMap.Map<Todo, TodoViewModel>(todo));
            }

            IsWorking = false;
        }
    
    }
}