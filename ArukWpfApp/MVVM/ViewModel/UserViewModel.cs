using ArukWpfApp.MVVM.Commands;
using ArukWpfApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArukWpfApp.MVVM.ViewModel
{
    class UserViewModel: INotifyPropertyChanged
    {
        UserRepository userRepository;
        private ObservableCollection<User> _users;

        private int currentPage = 1;
        private int itemsPerPage = 9;

        public ICommand NextPageCommand => new RelayCommand(GoToNextPage);
        public ICommand PreviousPageCommand => new RelayCommand(GoToPreviousPage);

        private int _totalPages;
        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        public int TotalPages
        {
            get { return _totalPages; }
            set
            {
                _totalPages = value;
                OnPropertyChanged(nameof(TotalPages));
            }
        }

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        public void UpdateUserPage()
        {
            OnPropertyChanged(nameof(UsersPerPage));
        }

        public void UpdateUsers()
        {
            Users = new ObservableCollection<User>(userRepository.GetAllUsers());
            TotalPages = (int)Math.Ceiling((double)Users.Count / itemsPerPage);
            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;
            OnPropertyChanged(nameof(UsersPerPage));
  
        }

        public async Task LoadUsersAsync(Image reloadImage)
        {
            reloadImage.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {
                List<User> users = userRepository.GetAllUsers();
                // Оновлення колекції Users у головному потоці
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Users = new ObservableCollection<User>(users);
                    TotalPages = (int)Math.Ceiling((double)Users.Count / itemsPerPage);
                });
                
            });
            OnPropertyChanged(nameof(UsersPerPage));
            reloadImage.Visibility = Visibility.Hidden;
        }

        public UserViewModel(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<User> _usersPerPage;

        public ObservableCollection<User> UsersPerPage
        {
            get
            {
                if (!SearchText.Equals("")) { return _usersPerPage; }
                int startIndex = (currentPage - 1) * itemsPerPage;
                return new ObservableCollection<User>(Users.Skip(startIndex).Take(itemsPerPage));
            }
            set
            {
                _usersPerPage = value;
                OnPropertyChanged(nameof(UsersPerPage));
            }
        }

        public void GoToNextPage(object parameter)
        {
            int totalPages = (int)Math.Ceiling((double)Users.Count / itemsPerPage);
            if (currentPage < totalPages)
            {
                currentPage++;
                CurrentPage = currentPage; // Оновлення властивості CurrentPage
                OnPropertyChanged(nameof(UsersPerPage));
            }
        }

        public void GoToPreviousPage(object parameter)
        {
            if (currentPage > 1)
            {
                currentPage--;
                CurrentPage = currentPage; 
                OnPropertyChanged(nameof(UsersPerPage));
            }
        }
        private string currentSortingProperty = "FirstName";
        public string CurrentSortingProperty
        {
            get { return currentSortingProperty; } 
        }
        private bool isSortingAscending = true;

        public bool IsSortingAscending
        {
            get { return isSortingAscending; }
        }

        public ICommand SortCommand => new RelayCommand(Sort);

        public void Sort(object propertyName)
        {
            if (Users == null) return;
            string property = propertyName as string;

            if (currentSortingProperty == property)
            {
                isSortingAscending = !isSortingAscending;
            }
            else
            {
                currentSortingProperty = property;
                isSortingAscending = true;
            }

            ApplySorting();
        }

        IOrderedEnumerable<User> sortedUsers;
        private void ApplySorting()
        {
            switch (currentSortingProperty)
            {
                case "FirstName":
                    sortedUsers = isSortingAscending ? Users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName) : Users.OrderByDescending(u => u.FirstName).ThenByDescending(u => u.LastName);
                    break;
                case "Email":
                    sortedUsers = isSortingAscending ? Users.OrderBy(u => u.Email) : Users.OrderByDescending(u => u.Email);
                    break;
                case "ActiveStatus":
                    sortedUsers = isSortingAscending ? Users.OrderBy(u => u.ActiveStatus) : Users.OrderByDescending(u => u.ActiveStatus);
                    break;
                default:
                    // Сортування за замовчуванням за ім'ям
                    sortedUsers = Users.OrderBy(u => u.CreatedAt); 
                    break;
            }

            Users = new ObservableCollection<User>(sortedUsers);
            ApplySearch();
            OnPropertyChanged(nameof(UsersPerPage));
            OnPropertyChanged(nameof(CurrentSortingProperty));
            OnPropertyChanged(nameof(IsSortingAscending));
        }

 


        private string _searchText = "";
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                ApplySearch();
            }
        }
       
        private void ApplySearch()
        {
            IEnumerable<User> filteredUsers;
            int startIndex = (currentPage - 1) * itemsPerPage;
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // Если текст для поиска пустой или содержит только пробелы, отобразите все пользователей
   
                UsersPerPage = new ObservableCollection<User>(Users.Skip(startIndex).Take(itemsPerPage));
             
            }
            else
            {
                // Фильтрация пользователей по тексту для поиска (например, по имени или email)
                if (sortedUsers == null)
                {
                    sortedUsers = Users.OrderBy(u => u.CreatedAt);
                }

                filteredUsers = sortedUsers.Where(u => u.FirstName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0
                                         || u.LastName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0
                                         || u.Email.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0);
         
                UsersPerPage = new ObservableCollection<User>(filteredUsers.Skip(startIndex).Take(itemsPerPage));
                
            }

           
            OnPropertyChanged(nameof(UsersPerPage));
        }
    }

}

