using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using SoftSkillsAML.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SoftSkillsAML.ViewModels
{
    internal class QuestionDetailsPageViewModel : ViewModelBase
    {
        public Question Question { get; }
        public string QuestionText => Question.Text;
        public ObservableCollection<Answer> Answers { get; }

        Answer? _selectedAnswer;
        public Answer? SelectedAnswer
        {
            get => _selectedAnswer;
            set => this.RaiseAndSetIfChanged(ref _selectedAnswer, value);
        }

        public QuestionDetailsPageViewModel(int questionId)
        {
            Question = MainWindowViewModel.db.Questions.First(x => x.Id == questionId);
            Answers = new ObservableCollection<Answer>(MainWindowViewModel.db.Answers.Where(x => x.Question == questionId).ToList());
        }

        public async void SubmitAnswer()
        {
            if (SelectedAnswer == null)
            {
                var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите вариант ответа", ButtonEnum.Ok, Icon.Info);
                await message.ShowAsync();
                return;
            }

            var userQuestion = MainWindowViewModel.db.UserQuestions.First(x => x.User == CurrentUserId && x.Question == Question.Id);
            userQuestion.IsAnswered = true;
            userQuestion.Answer = SelectedAnswer.Id;

            var skillBonuses = MainWindowViewModel.db.AnswerSoftSkills.Where(x => x.Answer == SelectedAnswer.Id).ToList();
            foreach (var bonus in skillBonuses)
            {
                var userSkill = MainWindowViewModel.db.UserSoftSkills.FirstOrDefault(x => x.User == CurrentUserId && x.SoftSkill == bonus.SoftSkill);
                if (userSkill != null)
                {
                    userSkill.Points += bonus.Points;
                }
            }


            await TryAwardItMasterAchievementAsync();

            MainWindowViewModel.db.SaveChanges();
            MainWindowViewModel.Instance.Page = new QuestionsPageView(Question.Department);
        }

        private async Task TryAwardItMasterAchievementAsync()
        {
            var department = MainWindowViewModel.db.Departments.FirstOrDefault(x => x.Id == Question.Department);
            if (department == null || department.Name != "IT-мастерская") return;

            var totalQuestions = MainWindowViewModel.db.Questions.Count(x => x.Department == Question.Department);
            if (totalQuestions == 0) return;

            var answeredQuestions = MainWindowViewModel.db.UserQuestions.Count(x => x.User == CurrentUserId && x.IsAnswered && x.QuestionNavigation.Department == Question.Department);
            if (answeredQuestions < totalQuestions) return;

            var achievement = MainWindowViewModel.db.Achievements.FirstOrDefault(x => x.Name == "Айтишник");
            if (achievement == null) return;

            var exists = MainWindowViewModel.db.UserAchievements.Any(x => x.User == CurrentUserId && x.Achievement == achievement.Id);
            if (exists) return;

            MainWindowViewModel.db.UserAchievements.Add(new UserAchievement { User = CurrentUserId, Achievement = achievement.Id });

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && desktop.MainWindow != null)
            {
                var dialog = new AchievementAwardWindow(achievement);
                await dialog.ShowDialog(desktop.MainWindow);
            }
        }

        public void BackToQuestions()
        {
            MainWindowViewModel.Instance.Page = new QuestionsPageView(Question.Department);
        }
    }
}
