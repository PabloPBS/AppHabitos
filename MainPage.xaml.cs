using Microsoft.Maui.Graphics.Text;
using System.Collections.ObjectModel;
using System.Formats.Tar;
using System.Text.Json;
namespace AppHabitos
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Habit> HabitList { get; set; } = new ObservableCollection<Habit>();

        public MainPage()
        {
            InitializeComponent();
            LoadHabits();
            habitsListView.ItemsSource = HabitList;
        }

        private void OnAddHabitClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(habitEntry.Text))
            {
                HabitList.Add(new Habit { Name = habitEntry.Text, Frequency = frequencyEntry.Text, TimesDone = 0 });
                habitEntry.Text = string.Empty;
                frequencyEntry.Text = string.Empty;


                SaveHabits();
            }
        }

        private void OnRegisterHabitClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var habit = button.BindingContext as Habit;

            if (habit != null)
            {
                habit.TimesDone += 1;
                if (habit.TimesDone == int.Parse(habit.Frequency) || habit.TimesDone > int.Parse(habit.Frequency))
                {
                    DisplayAlert("Meta alcançada!", "A tarefa bateu a quantidade de vezes desejadas." , "Ok");
                    habit.Name += " (Concluída)";

                } else
                {
                    DisplayAlert("Registrado com sucesso.", "A tarefa foi realizada " + habit.TimesDone + " vezes.", "Ok");
                }
            }
        }

        private void OnDeleteHabitClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var habit = button.BindingContext as Habit;

            if (habit != null)
            {
                HabitList.Remove(habit);
            }
        }
        private void SaveHabits()
        {
            var habitsJson = JsonSerializer.Serialize(HabitList);
            Preferences.Set("habits", habitsJson);
        }

        private void LoadHabits()
        {
            var habitsJson = Preferences.Get("habits", string.Empty);
            if (!string.IsNullOrEmpty(habitsJson))
            {
                HabitList = JsonSerializer.Deserialize<ObservableCollection<Habit>>(habitsJson);
                habitsListView.ItemsSource = HabitList;
            }
        }
    }
}
