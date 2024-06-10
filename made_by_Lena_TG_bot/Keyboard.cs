using made_by_Lena_TG_bot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

public class Keyboard
{
    public InlineKeyboardMarkup GetInlineKeyboardReviewsSelectionRating(ReviewUser reviewUser)
    {
        var keyboard = new List<InlineKeyboardButton[]>();
        for (int i = 5; i >= 1; i--)
        {
            var numberOfRatingScore = reviewUser.GetNumberOfRatingScore(i);
            keyboard.Add([InlineKeyboardButton.WithCallbackData($"{GeneralClass.GetRatingInStars(i)} ({numberOfRatingScore} отзыв{GeneralClass.GetEndOfWord(numberOfRatingScore)})", $"reviewsRating_{i}")]);
        }
        keyboard.Add([InlineKeyboardButton.WithCallbackData("🔙 Назад", "reviews")]);
        return new InlineKeyboardMarkup(keyboard);
    }

    public InlineKeyboardMarkup mainMenuInlineKeyboard = new InlineKeyboardMarkup(
        new List<InlineKeyboardButton[]>()
        {
        new[]
        {
            InlineKeyboardButton.WithCallbackData("Ассортимент", "assortment"),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData("Условия доставки 📦", "deliveryTerms"),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData("Отзывы ⭐️", "reviews"),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData("Рекомендации по уходу 🧺", "careRecommendation"),
        },
        new[]
        {
            InlineKeyboardButton.WithUrl("Обратная связь с мастером 💬", "https://t.me/Tretyakova_Lenka"),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData("Обо мне 😸", "aboutMe"),
        }
        });

    public InlineKeyboardMarkup categorySelectionInlineKeyboard = new InlineKeyboardMarkup(
        new List<InlineKeyboardButton[]>()
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Косметички", "categoryAssortiment_1"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Шопперы", "categoryAssortiment_2"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Резиночки для волос", "categoryAssortiment_3"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Гирлянды", "categoryAssortiment_4"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("🔙 Назад", "backToMenu"),
                },
            });

    public InlineKeyboardMarkup assortmentSelectionInlineKeyboard = new InlineKeyboardMarkup(
        new List<InlineKeyboardButton[]>()
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("⬅️", "moveCardAssortment_-1"),
                    InlineKeyboardButton.WithCallbackData("🔙 Назад", "assortment"),
                    InlineKeyboardButton.WithCallbackData("➡️", "moveCardAssortment_+1"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Добавить в корзину", "addToShoppingCart"),
                    InlineKeyboardButton.WithCallbackData("Удалить из корзины", "deleteFromShoppingCart"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Просмотреть корзину", "lookAtShoopingCart"),
                    InlineKeyboardButton.WithCallbackData("Очистить корзину", "clearShoopingCart"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Заказать 📦", "doOrder"),
                }
            });

    public InlineKeyboardMarkup reviewsSelectionInlineKeyboard = new InlineKeyboardMarkup(
       new List<InlineKeyboardButton[]>()
           {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("⬅️", "moveReviews_-1"),
                    InlineKeyboardButton.WithCallbackData("🔙 Назад", "lookAtReviews"),
                    InlineKeyboardButton.WithCallbackData("➡", "moveReviews_+1"),
                }
           });

    public ReplyKeyboardMarkup reviewReplyKeyboard = new ReplyKeyboardMarkup(
        new List<KeyboardButton>()
        {
            new KeyboardButton("5"),
            new KeyboardButton("4"),
            new KeyboardButton("3"),
            new KeyboardButton("2"),
            new KeyboardButton("1")
        })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };

    public ReplyKeyboardMarkup categoryReplyKeyboard = new ReplyKeyboardMarkup(
    new List<KeyboardButton>()
    {
            new KeyboardButton("Косметички"),
            new KeyboardButton("Шопперы"),
            new KeyboardButton("Резиночки для волос"),
            new KeyboardButton("Гирлянды")
    })
    {
        ResizeKeyboard = true,
        OneTimeKeyboard = true
    };

    public InlineKeyboardMarkup reviewsMenuInlineKeyboard = new InlineKeyboardMarkup(
        new List<InlineKeyboardButton[]>()
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Просмотреть", "lookAtReviews"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Оставить", "addReview"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("🔙 Назад", "backToMenu"),
                },
            });

    public ReplyKeyboardMarkup OrderConfirmationReplyKeyboard = new ReplyKeyboardMarkup(
    new List<KeyboardButton>()
    {
            new KeyboardButton("Подтверждаю ✅"),
            new KeyboardButton("Отмена 🚫")
    })
    {
        ResizeKeyboard = true,
        OneTimeKeyboard = true
    };

}