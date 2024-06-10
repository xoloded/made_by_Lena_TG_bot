using made_by_Lena_TG_bot.DataBase;
using made_by_Lena_TG_bot.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
public class ReviewUser
{
    public bool IsInReview { get; set; }

    private ProductCategory _category;
    private string _text;
    private int _scoreUser;
    private string _destinationFilePath;

    private int _currentPage;
    private int _allCountNumberReviews;
    private int _score;

    private const int AvailableLengthReview = 150;
    private const int _offSet = 2;
    public bool CheckingAvailabilityOfReviews(int score)
    {
        using var context = new DatabaseContext();
        var reviewsByRating = context.Reviews
            .Where(q => q.Rating == score)
            .OrderByDescending(q => q.DataTime)
            .ToList();

        _allCountNumberReviews = reviewsByRating.Count;
        _currentPage = 0;
        _score = score;

        return (_allCountNumberReviews == 0) ? false : true;
    }
    public bool ValidateAndChangePageReview(int step)
    {
        if (_currentPage + step >= 0 && _currentPage + step < Math.Ceiling((double)_allCountNumberReviews / _offSet))
        {  
            _currentPage += step;
            return true;
        }
        else
        {
            return false;
        }
    }
    public string FillAndReturnReviews()
    {
        var reviewPage = new StringBuilder();

        using var context = new DatabaseContext();
        var reviewsByRating = context.Reviews
            .Where(q => q.Rating == _score)
            .OrderByDescending(q => q.DataTime)
            .Skip(_currentPage * _offSet)
            .Take(_offSet)
            .ToList();

        foreach (var review in reviewsByRating)
        { 
            reviewPage.Append($"{review.UserName}\n" +
                              $"🕘 {review.DataTime}\n" +
                              $"{GeneralClass.GetRatingInStars(review.Rating)}\n" +
                              $"Отзыв: {review.Text}\n" +
                              $"=======\n");
        }

        reviewPage.Append($"Страница: {_currentPage + 1} из {Math.Ceiling((double)_allCountNumberReviews / _offSet)}");
        return reviewPage.ToString();
    }
    public void ChangeReviewStatus()
    {
        if (IsInReview)
        {
            IsInReview = false;
        }
        else
        {
            IsInReview = true;
        }     
    }
    public double GetAverageRatingByBrand()
    {
        using var context = new DatabaseContext();
        if (context.Reviews.Count() == 0)
        {
            return 0;
        }
        else
        {
            var averageRatingByBrand = context.Reviews
                .Average(q => q.Rating);
            return Math.Round(averageRatingByBrand, 2);
        }
    }
    public int GetNumberOfRatingByBrand()
    {
        using var context = new DatabaseContext();
        return context.Reviews
            .Count();
    }
    public int GetNumberOfRatingScore(int score)
    {
        using var context = new DatabaseContext();
        return context.Reviews
            .Where(q => q.Rating == score)
            .Count();
    }
    public void ResetReviewState()
    {
        IsInReview = false;
        _category = 0;
        _text = null;
        _scoreUser = 0;
        _destinationFilePath = null;
    }
    public string SendingThanks(int rating)
    {
        if (rating > 3)
        {
            return "Благодарю за Вашу поддержку 🥰";
        }
        else
        {
            return "Очень жаль, что товар не оправдал ваших ожиданий 😔\nБлагодаря Вашему отзыву, мои изделия станут лучше";
        }           
    }
    public bool CheckFiveRating(int rating)
    {
        return rating >= 1 && rating <= 5;
    }
    public (bool check, string ending) CheckLengthReview(string review)
    {
        if (review.Length <= AvailableLengthReview)
            return (true, "");
        else
        {
            int maxLengthReview = 150;
            int count = review.Length - maxLengthReview;
            return (false, review.Length - maxLengthReview + " символ" + GeneralClass.GetEndOfWord(count));
        }
    }
    public async Task DownloadPhotoForReviewAsync(Message message, TelegramBotClient botclient)
    {
        var fileId = message.Photo.Last().FileId;
        var fileInfo = await botclient.GetFileAsync(fileId);
        var filePath = fileInfo.FilePath;
        string time = DateTime.Now.ToString().Replace(':','.').Replace(' ', '_');

        string photoPathReview = $"{time}_{message.Chat.FirstName}_{Guid.NewGuid().ToString().Remove(7)}";
        _destinationFilePath = $"D:\\Програмирование\\C#\\made_by_Lena_TG_bot\\made_by_Lena_TG_bot\\made_by_Lena_TG_bot\\DataBase\\reviews\\{photoPathReview}.jpg";

        await using Stream fileStream = System.IO.File.Create(_destinationFilePath);
        await botclient.DownloadFileAsync(
            filePath: filePath,
            destination: fileStream);
        fileStream.Close();
    }
    public async Task AddNewReview(Message message)
    {
        using var context = new DatabaseContext();
        var assortimentCard = await context.Products
            .Where(x => x.Id == 16)
            .FirstAsync();
        var photo = new Photo
        {
            Path = _destinationFilePath
        };
        var category = new Category
        {
            ProductCategory = _category
        };
        var review = new Review
        {
            UserName = message.From.FirstName + " " + message.From.LastName,
            DataTime = message.Date,
            Text = this._text,
            Rating = _scoreUser,
            Photo = photo,
            Category = category,
            Product = assortimentCard
        };
        context.Add(review);
        await context.SaveChangesAsync();
    }
    public bool ValidateAndAssignCategory(string message)
    {
        if (message != null)
        {
            switch (message.ToLower())
            {
                case "косметички":
                    {
                        _category = ProductCategory.Косметичка;
                        return true;
                    }
                case "шопперы":
                    {
                        _category = ProductCategory.Шоппер;
                        return true;
                    }
                case "резиночки для волос":
                    {
                        _category = ProductCategory.РезиночкаДляВолос;
                        return true;
                    }
                case "гирлянды":
                    {
                        _category = ProductCategory.Гирлянда;
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }
        else
        {
            return false;
        }  
    }
    public async Task LeaveReview(ReviewUser _reviewUser, Keyboard _control, TelegramBotClient _botClient, Message message, ITelegramBotClient client)
    {
        if (_reviewUser._category == 0)
        {
            if (_reviewUser.ValidateAndAssignCategory(message.Text))
            {
                await client.SendTextMessageAsync(chatId: message.Chat.Id, "Напишите отзыв о покупке");
            }
            else
            {
                await client.SendTextMessageAsync(chatId: message.Chat.Id, "Ошибка: указана неверная категория ⛔️");
            }
            return;
        }
        else if (_reviewUser._text == null && message.Text != null)
        {
            if (_reviewUser.CheckLengthReview(message.Text).check)
            {
                _reviewUser._text = message.Text;
                await client.SendTextMessageAsync(chatId: message.Chat.Id, "Оцените покупку", replyMarkup: _control.reviewReplyKeyboard);
            }
            else
            {
                var currentLenghtReview = _reviewUser.CheckLengthReview(message.Text).ending;
                await client.SendTextMessageAsync(chatId: message.Chat.Id, $"Доступная длина отзыва {AvailableLengthReview}.\nУменьшите длину отзыва на {currentLenghtReview}");
            }
            return;
        }
        else if (_reviewUser._scoreUser == 0)
        {
            int rating;
            if (message.Text != null && int.TryParse(message.Text, out rating))
            {
                if (int.Parse(message.Text) % 1 == 0 && _reviewUser.CheckFiveRating(rating))
                {
                    _reviewUser._scoreUser = int.Parse(message.Text);
                    await client.SendTextMessageAsync(chatId: message.Chat.Id, "Прикрепите фото для подтверждения покупки");
                }
                else
                {
                    await client.SendTextMessageAsync(chatId: message.Chat.Id, "Оцените покупку от 1 до 5 баллов");
                }
            }
            else
            {
                await client.SendTextMessageAsync(chatId: message.Chat.Id, "Оцените покупку от 1 до 5 баллов");
            }
            return;
        }
        else if (message.Document != null)
        {
            await client.SendTextMessageAsync(chatId: message.Chat.Id, "Ошибка: прикрепите фото не документом, а фотографией");
            return;
        }
        else if (message.Photo != null)
        {
            await _reviewUser.DownloadPhotoForReviewAsync(message, _botClient);
            await _reviewUser.AddNewReview(message);
            await client.SendTextMessageAsync(chatId: message.Chat.Id, _reviewUser.SendingThanks(_reviewUser._scoreUser));
            _reviewUser.ResetReviewState();
            await client.SendTextMessageAsync(chatId: message.Chat.Id, "Меню ☰", replyMarkup: _control.mainMenuInlineKeyboard);
            return;
        }
        else
        {
            await client.SendTextMessageAsync(chatId: message.Chat.Id, "Не понимаю, что Вы хотите 😐");
            return;
        }
    }
}