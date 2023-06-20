using ArukWpfApp.MVVM.Model;
using ArukWpfApp.MVVM.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArukWpfApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserRepository userRepository;
        private UserViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();

            const string connectionUri = "mongodb+srv://ArukAdmin:2ahL8Ogd97EwPh35@arukcluster.v7i9lzc.mongodb.net/?retryWrites=true&w=majority";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);

            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            var client = new MongoClient(settings);

            userRepository = new UserRepository(client.GetDatabase("ManagerDB"));

            viewModel = new UserViewModel(userRepository);


            _ = viewModel.LoadUsersAsync(reloadImage);

            DataContext = viewModel;

        }









        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point newPoint = e.GetPosition(this);
                if (newPoint.Y < 40)
                    DragMove();
            }
        }


        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OpenPopup_Click(object sender, RoutedEventArgs e)
        {
            addUserWindow.Visibility = Visibility.Visible;
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 0.5;
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.15));
            MyBorderBack.BeginAnimation(OpacityProperty, animation);

            TransformGroup transformGroup = (TransformGroup)MyBorder.RenderTransform;
            ScaleTransform scaleTransform = (ScaleTransform)transformGroup.Children[0];
            TranslateTransform translateTransform = (TranslateTransform)transformGroup.Children[1];

            DoubleAnimation scaleXAnimation = new DoubleAnimation();
            scaleXAnimation.From = 0.1;
            scaleXAnimation.To = 1;
            scaleXAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.15));

            DoubleAnimation scaleYAnimation = new DoubleAnimation();
            scaleYAnimation.From = 0.1;
            scaleYAnimation.To = 1;
            scaleYAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.15));

            DoubleAnimation translateXAnimation = new DoubleAnimation();
            translateXAnimation.From = translateTransform.X;
            translateXAnimation.To = 0;
            translateXAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.15));

            DoubleAnimation translateYAnimation = new DoubleAnimation();
            translateYAnimation.From = translateTransform.Y;
            translateYAnimation.To = 0;
            translateYAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.15));

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleXAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleYAnimation);
            translateTransform.BeginAnimation(TranslateTransform.XProperty, translateXAnimation);
            translateTransform.BeginAnimation(TranslateTransform.YProperty, translateYAnimation);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string fieldName = textBox.Tag.ToString();
            if (fieldName.Equals("f_name") && textBox.Text == "Enter name...")
            {
                textBox.Text = string.Empty;
                BrushConverter converter = new BrushConverter();
                textBox.BorderBrush = (Brush)converter.ConvertFromString("#FFDCE4FD");
                textBox.Foreground = Brushes.Black;
            }
            if (fieldName.Equals("l_name") && textBox.Text == "Enter surname...")
            {
                textBox.Text = string.Empty;
                BrushConverter converter = new BrushConverter();
                textBox.BorderBrush = (Brush)converter.ConvertFromString("#FFDCE4FD");
                textBox.Foreground = Brushes.Black;
            }
            if (fieldName.Equals("age") && textBox.Text == "Enter age...")
            {
                textBox.Text = string.Empty;
                BrushConverter converter = new BrushConverter();
                textBox.BorderBrush = (Brush)converter.ConvertFromString("#FFDCE4FD");
                textBox.Foreground = Brushes.Black;
            }
            if (fieldName.Equals("email") && textBox.Text == "Enter email...")
            {
                textBox.Text = string.Empty;
                BrushConverter converter = new BrushConverter();
                textBox.BorderBrush = (Brush)converter.ConvertFromString("#FFDCE4FD");
                textBox.Foreground = Brushes.Black;
            }
            if (fieldName.Equals("info") && textBox.Text == "Enter additional info...")
            {
                textBox.Text = string.Empty;
                BrushConverter converter = new BrushConverter();
                textBox.BorderBrush = (Brush)converter.ConvertFromString("#FFDCE4FD");
                textBox.Foreground = Brushes.Black;
            }

        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string fieldName = textBox.Tag.ToString();
            if (fieldName.Equals("f_name") && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Enter name...";
                textBox.Foreground = Brushes.Gray; // Змініть колір тексту на сірий
            }
            if (fieldName.Equals("l_name") && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Enter surname...";
                textBox.Foreground = Brushes.Gray; // Змініть колір тексту на сірий
            }
            if (fieldName.Equals("age") && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Enter age...";
                textBox.Foreground = Brushes.Gray; // Змініть колір тексту на сірий
            }
            if (fieldName.Equals("email") && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Enter email...";
                textBox.Foreground = Brushes.Gray; // Змініть колір тексту на сірий
            }
            if (fieldName.Equals("info") && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Enter additional info...";
                textBox.Foreground = Brushes.Gray; // Змініть колір тексту на сірий
            }
        }

        private async void AddUser_Click(object sender, RoutedEventArgs e)
        {
            string fName = "";
            if (!FirstName.Text.Equals("Enter name..."))
                fName = FirstName.Text;

            string lName = "";
            if (!LastName.Text.Equals("Enter surname..."))
                lName = LastName.Text;

            uint age = 0;
            if (!Age.Text.Equals("Enter age..."))
            {
                if (uint.TryParse(Age.Text, out uint f_age))
                {
                    age = f_age;
                }
                else
                {
                    if (!MBActive)
                        ShowErrorMessage("Wrong age");
                    return;
                }

            }

            string email = "";
            if (!Email.Text.Equals("Enter email..."))
                email = Email.Text;

            string addInfo = "";
            if (!AdditionalInfo.Text.Equals("Enter additional info..."))
                addInfo = AdditionalInfo.Text;

            if (fName == "")
            {
                if (!MBActive)
                    ShowErrorMessage();
                FirstName.BorderBrush = Brushes.OrangeRed;
            }
            if (lName == "")
            {
                if (!MBActive)
                    ShowErrorMessage();
                LastName.BorderBrush = Brushes.OrangeRed;
            }
            if (email == "")
            {
                if (!MBActive)
                    ShowErrorMessage();
                Email.BorderBrush = Brushes.OrangeRed;
            }
            if (addInfo == "")
            {
                if (!MBActive)
                    ShowErrorMessage();
                AdditionalInfo.BorderBrush = Brushes.OrangeRed;
            }

            if (MBActive)
                return;

            //

            var newUser = new User
            {
                Id = ObjectId.GenerateNewId(),
                FirstName = fName,
                LastName = lName,
                Age = age,
                ActiveStatus = true,
                Email = email,
                Info = addInfo,
                CreatedAt = DateTime.Now
            };

            FirstName.Text = "Enter name...";
            FirstName.Foreground = Brushes.Gray;
            LastName.Text = "Enter surname...";
            LastName.Foreground = Brushes.Gray;
            Age.Text = "Enter age...";
            Age.Foreground = Brushes.Gray;
            Email.Text = "Enter email...";
            Email.Foreground = Brushes.Gray;
            AdditionalInfo.Text = "Enter additional info...";
            AdditionalInfo.Foreground = Brushes.Gray;

            userRepository.AddUser(newUser);

            PopupFadeAnim();

            await Task.Delay(150);
            addUserWindow.Visibility = Visibility.Hidden;

            MessageBoxAnim("New user added");

            viewModel.UpdateUsers();
        }

        private async void MessageBoxAnim(string message)
        {
            Message.Visibility = Visibility.Visible;
            MessageText.Text = message;
            TransformGroup transformGroup = (TransformGroup)Message.RenderTransform;
            TranslateTransform translateTransform = (TranslateTransform)transformGroup.Children[0];

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 1;
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.15));
            Message.BeginAnimation(OpacityProperty, animation);

            DoubleAnimation translateYAnimation = new DoubleAnimation();
            translateYAnimation.From = 0;
            translateYAnimation.To = 20;
            translateYAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.15));

            translateTransform.BeginAnimation(TranslateTransform.YProperty, translateYAnimation);

            await Task.Delay(2000);

            animation = new DoubleAnimation();
            animation.From = 1;
            animation.To = 0;
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.15));
            Message.BeginAnimation(OpacityProperty, animation);

            translateYAnimation = new DoubleAnimation();
            translateYAnimation.From = 20;
            translateYAnimation.To = 0;
            translateYAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.15));

            translateTransform.BeginAnimation(TranslateTransform.YProperty, translateYAnimation);
            await Task.Delay(150);
            Message.Visibility = Visibility.Hidden;
        }



        bool MBActive = false;
        private async void ShowErrorMessage(string mes)
        {
            MBActive = true;
            ErrorMessageBox.Visibility = Visibility.Visible;
            ErrorMesText.Text = mes;
            TransformGroup transformGroup = (TransformGroup)ErrorMessageBox.RenderTransform;
            TranslateTransform translateTransform = (TranslateTransform)transformGroup.Children[0];



            DoubleAnimation translateYAnimation = new DoubleAnimation();
            translateYAnimation.From = 24;
            translateYAnimation.To = 0;
            translateYAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.15));

            translateTransform.BeginAnimation(TranslateTransform.YProperty, translateYAnimation);

            await Task.Delay(3000);

            translateYAnimation = new DoubleAnimation();
            translateYAnimation.From = 0;
            translateYAnimation.To = 24;
            translateYAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.15));

            translateTransform.BeginAnimation(TranslateTransform.YProperty, translateYAnimation);
            await Task.Delay(150);
            MBActive = false;
            ErrorMessageBox.Visibility = Visibility.Hidden;
        }
        private async void ShowErrorMessage()
        {
            MBActive = true;
            ErrorMessageBox.Visibility = Visibility.Visible;
            ErrorMesText.Text = "Fill in all input fields";
            TransformGroup transformGroup = (TransformGroup)ErrorMessageBox.RenderTransform;
            TranslateTransform translateTransform = (TranslateTransform)transformGroup.Children[0];



            DoubleAnimation translateYAnimation = new DoubleAnimation();
            translateYAnimation.From = 24;
            translateYAnimation.To = 0;
            translateYAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.15));

            translateTransform.BeginAnimation(TranslateTransform.YProperty, translateYAnimation);

            await Task.Delay(3000);

            translateYAnimation = new DoubleAnimation();
            translateYAnimation.From = 0;
            translateYAnimation.To = 24;
            translateYAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.15));

            translateTransform.BeginAnimation(TranslateTransform.YProperty, translateYAnimation);
            await Task.Delay(150);
            MBActive = false;
            ErrorMessageBox.Visibility = Visibility.Hidden;
        }

        private async void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessageBox.Visibility = Visibility.Hidden;
            PopupFadeAnim();
            await Task.Delay(150);
            addUserWindow.Visibility = Visibility.Hidden;
        }

        User currentSelectedUser;
        User selectedUserToDelete;
        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            selectedUserToDelete = (sender as Button)?.DataContext as User;

            Button deleteButton = (Button)sender;

            Point buttonPosition = deleteButton.TranslatePoint(new Point(0, 0), this);

            ShowConfirmPopup(buttonPosition);


        }
        private void PopupFadeAnim()
        {
            ErrorMessageBox.Visibility = Visibility.Hidden;
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0.5;
            animation.To = 0;
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.15));
            MyBorderBack.BeginAnimation(OpacityProperty, animation);

            TransformGroup transformGroup = (TransformGroup)MyBorder.RenderTransform;
            ScaleTransform scaleTransform = (ScaleTransform)transformGroup.Children[0];
            TranslateTransform translateTransform = (TranslateTransform)transformGroup.Children[1];

            DoubleAnimation scaleXAnimation = new DoubleAnimation();
            scaleXAnimation.From = 1;
            scaleXAnimation.To = 0.1;
            scaleXAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.15));

            DoubleAnimation scaleYAnimation = new DoubleAnimation();
            scaleYAnimation.From = 1;
            scaleYAnimation.To = 0.1;
            scaleYAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.15));

            DoubleAnimation translateXAnimation = new DoubleAnimation();
            translateXAnimation.From = 0;
            translateXAnimation.To = MyBorder.ActualWidth / 2;
            translateXAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.15));

            DoubleAnimation translateYAnimation = new DoubleAnimation();
            translateYAnimation.From = 0;
            translateYAnimation.To = MyBorder.ActualHeight / 2;
            translateYAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.15));

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleXAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleYAnimation);
            translateTransform.BeginAnimation(TranslateTransform.XProperty, translateXAnimation);
            translateTransform.BeginAnimation(TranslateTransform.YProperty, translateYAnimation);
        }

        private void ShowConfirmPopup(Point point)
        {
            ConfirmDelete.Visibility = Visibility.Visible;

            double left = point.X - ConfirmDeletePopup.ActualWidth - 24;
            double top = point.Y - (ConfirmDeletePopup.ActualHeight / 2) - 25;

            // Встановити властивість Margin об'єкта Border
            ConfirmDeletePopup.Margin = new Thickness(left, top, 0, 0);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmDelete.Visibility = Visibility.Hidden;

            if (currentSelectedUser != null && selectedUserToDelete.Id.Equals(currentSelectedUser.Id))
                currentSelectedUser = null;

            userRepository.DeleteUser(selectedUserToDelete.Id);

            MessageBoxAnim("User deleted");

            viewModel.UpdateUsers();

            UpdateUserPanelInfo();
        }
        private void ConfirmWinButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmDelete.Visibility = Visibility.Hidden;
        }
        private void CancelConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmDelete.Visibility = Visibility.Hidden;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (listBox.SelectedItem != null)
            {
                ListBoxItem selectedItem = (ListBoxItem)listBox.ItemContainerGenerator.ContainerFromItem(listBox.SelectedItem);
                if (selectedItem != null)
                {
                    currentSelectedUser = (User)selectedItem.DataContext;
                    UpdateUserPanelInfo();
                }
            }
        }

        private void UpdateUserPanelInfo()
        {
            if (currentSelectedUser == null)
            {
                UserInfoName.Text = "";
                UserInfoSurname.Text = "";
                UserInfoCreateDate.Text = "";
                UserInfoAge.Text = "";
                UserInfoEmail.Text = "";
                UserInfoAddInf.Text = "";
                return;
            }
            UserInfoName.Text = currentSelectedUser.FirstName;
            UserInfoSurname.Text = currentSelectedUser.LastName;
            UserInfoCreateDate.Text = currentSelectedUser.CreatedAt.ToString("dd MMMM yyyy");
            UserInfoAge.Text = currentSelectedUser.Age.ToString();
            UserInfoEmail.Text = currentSelectedUser.Email;
            UserInfoAddInf.Text = currentSelectedUser.Info;
            UserToggleButton.IsChecked = currentSelectedUser.ActiveStatus;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Task.Delay(1);
            if (viewModel.Users == null) return;

            SortImgF1.Visibility = Visibility.Hidden;
            SortImgF2.Visibility = Visibility.Hidden;
            SortImgA1.Visibility = Visibility.Hidden;
            SortImgA2.Visibility = Visibility.Hidden;
            SortImgE1.Visibility = Visibility.Hidden;
            SortImgE2.Visibility = Visibility.Hidden;

            if (viewModel.CurrentSortingProperty.Equals("FirstName"))
            {
                if (viewModel.IsSortingAscending)
                    SortImgF1.Visibility = Visibility.Visible;
                else
                    SortImgF2.Visibility = Visibility.Visible;
            }
            if (viewModel.CurrentSortingProperty.Equals("ActiveStatus"))
            {
                if (viewModel.IsSortingAscending)
                    SortImgA1.Visibility = Visibility.Visible;
                else
                    SortImgA2.Visibility = Visibility.Visible;
            }
            if (viewModel.CurrentSortingProperty.Equals("Email"))
            {
                if (viewModel.IsSortingAscending)
                    SortImgE1.Visibility = Visibility.Visible;
                else
                    SortImgE2.Visibility = Visibility.Visible;
            }
        }

        private bool checkIsEdited()
        {
            uint temp_age = 0;
            uint.TryParse(UserInfoEditAge.Text, out temp_age);
            if (currentSelectedUser.FirstName != UserInfoEditName.Text
                || currentSelectedUser.LastName != UserInfoEditSurName.Text
                || currentSelectedUser.Age != temp_age
                || currentSelectedUser.Email != UserInfoEditEmail.Text
                || currentSelectedUser.Info != UserInfoEditAddInfo.Text
                || currentSelectedUser.ActiveStatus != UserToggleButton.IsChecked)
                return true;
            return false;
        }

        private void EditWinButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmEdit.Visibility = Visibility.Hidden;
            UserToggleButton.IsEnabled = false;
            UserToggleButtonBorder.BorderBrush = new SolidColorBrush(Colors.Transparent);
            if (!checkIsEdited()) return;
            currentSelectedUser.FirstName = UserInfoEditName.Text;
            currentSelectedUser.LastName = UserInfoEditSurName.Text;
            uint temp_age = 0;
            if (uint.TryParse(UserInfoEditAge.Text, out temp_age))
            {
                currentSelectedUser.Age = temp_age;
            }
            currentSelectedUser.Email = UserInfoEditEmail.Text;
            currentSelectedUser.Info = UserInfoEditAddInfo.Text;
            currentSelectedUser.ActiveStatus = UserToggleButton.IsChecked ?? false;
            userRepository.UpdateUser(currentSelectedUser);
            viewModel.UpdateUserPage();
            UpdateUserPanelInfo();
            MessageBoxAnim("User edited");
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentSelectedUser == null) return;
            ConfirmEdit.Visibility = Visibility.Visible;
            UserInfoEditName.Text = currentSelectedUser.FirstName;
            UserInfoEditSurName.Text = currentSelectedUser.LastName;
            UserInfoEditAge.Text = currentSelectedUser.Age.ToString();
            UserInfoEditEmail.Text = currentSelectedUser.Email;
            UserInfoEditAddInfo.Text = currentSelectedUser.Info;
            UserInfoEditAddInfo.Text = currentSelectedUser.Info;
            UserToggleButton.IsEnabled = true;
            string colorCode = "#FFABADB3"; 
            Color color = (Color)ColorConverter.ConvertFromString(colorCode);
            SolidColorBrush brush = new SolidColorBrush(color);
            UserToggleButtonBorder.BorderBrush = brush;
            UserToggleButton.IsChecked = currentSelectedUser.ActiveStatus ? true : false;
            
        }
    }
}
