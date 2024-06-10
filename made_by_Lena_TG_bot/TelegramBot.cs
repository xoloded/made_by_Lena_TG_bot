using made_by_Lena_TG_bot.Entities;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using made_by_Lena_TG_bot.DataBase;

public class TelegramBot
{
    private readonly TelegramBotClient _botClient = new TelegramBotClient("");
    private readonly Session _session;

    //private static async Task CreateProducts()
    //{
    //    using var context = new DatabaseContext();
    //    var category = new Category
    //    {
    //        ProductCategory = ProductCategory.Гирлянда
    //    };
    //    var photosProduct = new List<Photo>
    //    {
    //        new Photo
    //        {
    //           Path = "D:\\Програмирование\\C#\\made_by_Lena_TG_bot\\made_by_Lena_TG_bot\\made_by_Lena_TG_bot\\DataBase\\assortment\\garland\\0_0.jpg"
    //        },
    //        new Photo
    //        {
    //           Path = "D:\\Програмирование\\C#\\made_by_Lena_TG_bot\\made_by_Lena_TG_bot\\made_by_Lena_TG_bot\\DataBase\\assortment\\garland\\0_1.jpg"
    //        },
    //        new Photo
    //        {
    //           Path = "D:\\Програмирование\\C#\\made_by_Lena_TG_bot\\made_by_Lena_TG_bot\\made_by_Lena_TG_bot\\DataBase\\assortment\\garland\\0_2.jpg"
    //        }
    //    };
    //    var prod = new Product
    //    {
    //        Name = "Гирлянда из хлопка",
    //        Description = "Уютная домашняя гирлянда. Длина - 2,2 метра, 13 флажков.",
    //        Price = 1200,
    //        Available = true,
    //        Category = category,
    //        Photos = photosProduct
    //    };
    //    context.Add(prod);
    //    await context.SaveChangesAsync();
    //}
    public TelegramBot()
    {
        _session = new Session();
    }
    public void Run()
    {
        //CreateProducts();
        _botClient.StartReceiving(Update, Error);
        Console.WriteLine($"made_by_Lena_TG_bot is WORK. [Date of activation {DateTime.Now}]");
        Console.ReadLine();
        return;
    }
    private static Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        Console.WriteLine("Error");
        throw new NotImplementedException();
    }
    async Task Update(ITelegramBotClient client, Update update, CancellationToken token)
    {
        //return;
        switch (update.Type)
        {
            case UpdateType.Message:
                {
                    var message = update.Message;
                    var sessionUser = _session.GetSession(message.From.Id);
                   
                    if (message.Text == "/start")
                    {
                        Console.WriteLine($"{update.Message.From.FirstName} {update.Message.From.LastName} [ID: {update.Message.From.Id}] uses a bot. [Date of use {DateTime.Now}]");
                        sessionUser._assortment.ClearAssortimentCard();
                        sessionUser._shopingCart.ResetShopingCartState();
                        sessionUser._reviewUser.ResetReviewState();
                        await client.SendTextMessageAsync(chatId: message.Chat.Id, "Меню ☰", replyMarkup: sessionUser._control.mainMenuInlineKeyboard);          
                        return;
                    }
                    else if (message.Text == "/cancel")
                    {
                        sessionUser._shopingCart.ResetShopingCartState();
                        sessionUser._reviewUser.ResetReviewState();
                        await client.SendTextMessageAsync(chatId: message.Chat.Id, "Меню ☰", replyMarkup: sessionUser._control.mainMenuInlineKeyboard);
                        return;
                    }
                    else if (sessionUser._reviewUser.IsInReview)
                    {
                        await sessionUser._reviewUser.LeaveReview(sessionUser._reviewUser, sessionUser._control, _botClient, message, client);
                        return;
                    }
                    else if (sessionUser._shopingCart.IsInReview)
                    {
                        await sessionUser._shopingCart.MakeOrder(sessionUser._shopingCart, sessionUser._control, message, client);
                        return;
                    }
                    else
                    {
                        await client.SendTextMessageAsync(chatId: message.Chat.Id, "Не понимаю, что Вы хотите 😐");
                        return;
                    }
                }
            case UpdateType.CallbackQuery:
                {                 
                    var callbackQuery = update.CallbackQuery;
                    var sessionUser = _session.GetSession(callbackQuery.From.Id);
                    var chat = callbackQuery.Message.Chat;

                    var request = callbackQuery.Data;
                    int indexOfLower = request.IndexOf('_');
                    int data = -1;
                    if (indexOfLower != -1)
                    {
                        data = int.Parse(request.Remove(0, indexOfLower + 1));
                        request = request.Remove(indexOfLower, request.Length - indexOfLower);
                    }

                    switch (request)
                    {
                        case "assortment":
                            {
                                await sessionUser._assortment.DeleteCurrentCard(chat.Id, sessionUser, _botClient, callbackQuery);
                                sessionUser._assortment.ClearAssortimentCard();
                                await client.SendTextMessageAsync(chatId: chat.Id, "Выберете категорию", replyMarkup: sessionUser._control.categorySelectionInlineKeyboard);
                                return;
                            };
                        case "deliveryTerms":
                            {
                                await client.SendTextMessageAsync(chatId: chat.Id, GeneralClass.DeliveryTerms());
                                return;
                            };
                        case "reviews":
                            {
                                await _botClient.EditMessageTextAsync(chatId: chat.Id, callbackQuery.Message.MessageId, "Отзывы");
                                await _botClient.EditMessageReplyMarkupAsync(chatId: chat.Id, callbackQuery.Message.MessageId, replyMarkup: sessionUser._control.reviewsMenuInlineKeyboard);
                                return;
                            };
                        case "careRecommendation":
                            {
                                await client.SendTextMessageAsync(chatId: chat.Id, GeneralClass.CareRecommendation());
                                return;
                            };
                        case "aboutMe":
                            {
                                await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, GeneralClass.AboutMe(), showAlert: true);
                                return;
                            };
                        case "lookAtReviews":
                            {
                                var numberRatingByBrand = sessionUser._reviewUser.GetNumberOfRatingByBrand();
                                await _botClient.EditMessageTextAsync(chatId: chat.Id, 
                                                                   messageId: callbackQuery.Message.MessageId, 
                                                                        text: $"Средняя оценка ⭐️ {sessionUser._reviewUser.GetAverageRatingByBrand()}/5 (Всего {numberRatingByBrand} отзыв{GeneralClass.GetEndOfWord(numberRatingByBrand)})");
                                await _botClient.EditMessageReplyMarkupAsync(chatId: chat.Id, 
                                                                          messageId: callbackQuery.Message.MessageId, 
                                                                        replyMarkup: sessionUser._control.GetInlineKeyboardReviewsSelectionRating(sessionUser._reviewUser));
                                return;
                            };
                        case "addReview":
                            {
                                sessionUser._reviewUser.ChangeReviewStatus();
                                await _botClient.SendTextMessageAsync(chatId: chat.Id, "Какая категория товаров", replyMarkup: sessionUser._control.categoryReplyKeyboard);
                                return;
                            };
                        case "backToMenu":
                            {
                                await _botClient.EditMessageTextAsync(chatId: chat.Id, callbackQuery.Message.MessageId, "Меню ☰");
                                await _botClient.EditMessageReplyMarkupAsync(chatId: chat.Id, callbackQuery.Message.MessageId, replyMarkup: sessionUser._control.mainMenuInlineKeyboard);
                                return;
                            };
                        case "reviewsRating":
                            {
                                if (sessionUser._reviewUser.CheckingAvailabilityOfReviews(data))
                                {
                                    await _botClient.EditMessageTextAsync(chatId: chat.Id, callbackQuery.Message.MessageId, sessionUser._reviewUser.FillAndReturnReviews());
                                    await _botClient.EditMessageReplyMarkupAsync(chatId: chat.Id, callbackQuery.Message.MessageId, replyMarkup: sessionUser._control.reviewsSelectionInlineKeyboard);
                                }
                                else
                                {
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Отзывов пока нет... 😔");
                                }
                                return;
                            }
                        case "moveReviews":
                            {
                                if (sessionUser._reviewUser.ValidateAndChangePageReview(data))
                                {
                                    await _botClient.EditMessageTextAsync(chatId: chat.Id, callbackQuery.Message.MessageId, sessionUser._reviewUser.FillAndReturnReviews());
                                    await _botClient.EditMessageReplyMarkupAsync(chatId: chat.Id, callbackQuery.Message.MessageId, replyMarkup: sessionUser._control.reviewsSelectionInlineKeyboard);
                                }
                                else
                                {
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Ошибка: выход за границу ⛔️");
                                }
                                return;
                            }
                        case "categoryAssortiment":
                            {
                                sessionUser._assortment.GetAllAssortimentCards((ProductCategory)data);
                                if (sessionUser._assortment.CheckingAvailabilityAllAssortimentCards())
                                {
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Нет доступных товаров\nМастер шьет ✂️🪡");
                                }
                                else
                                {
                                    sessionUser._assortment.GetCurrentProductCategory((ProductCategory)data);
                                    await sessionUser._assortment.GetAverageRating((ProductCategory)data);
                                    await sessionUser._assortment.GetAssortimentCard((ProductCategory)data);
                                    await sessionUser._assortment.DeleteAndSendNextCard(chat.Id, sessionUser, _botClient, callbackQuery);
                                }
                                return;
                            }
                        case "moveCardAssortment":
                            {
                                if (sessionUser._assortment.ValidateAndChangeCurrentCard(data))
                                {
                                    await sessionUser._assortment.DeleteAndSendNextCard(chat.Id, sessionUser, _botClient, callbackQuery);
                                }
                                else
                                {
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Ошибка: выход за границу ⛔️");
                                }
                                return;
                            }
                        case "addToShoppingCart":
                            {
                                sessionUser._shopingCart.AddProductIdInCart(sessionUser._assortment._assortimentCard);
                                await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Товар добавлен в корзину 🛒");
                                await sessionUser._assortment.СhangeShopingCartAndSendCurrentCard(chat.Id, sessionUser, _botClient, callbackQuery);
                                return;
                            }
                        case "deleteFromShoppingCart":
                            {
                                if (await sessionUser._shopingCart.ChangeAndDeleteProductInCart(sessionUser._assortment._assortimentCard))
                                {
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Ошибка: товар не в корзине 🛒");
                                }
                                else
                                {
                                    await sessionUser._assortment.СhangeShopingCartAndSendCurrentCard(chat.Id, sessionUser, _botClient, callbackQuery);
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Товар удален из корзины 🛒");
                                }
                                return;
                            }
                        case "lookAtShoopingCart":
                            {
                                await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, sessionUser._shopingCart.GetProductAtProductCart(), showAlert: true);
                                //возможна ошибка из за длины текста корзины
                                return;
                            }
                        case "clearShoopingCart":
                            {
                                sessionUser._shopingCart.ClearProductCart();
                                await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Корзина очищена 🛒");
                                await sessionUser._assortment.СhangeShopingCartAndSendCurrentCard(chat.Id, sessionUser, _botClient, callbackQuery);
                                return;
                            }
                        case "doOrder":
                            {
                                if (sessionUser._shopingCart.ProofEmptyProductCard())
                                {
                                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Корзина пуста 🛒", showAlert: true);
                                }
                                else
                                {
                                    sessionUser._shopingCart.ChangeShopingCartStatus();
                                    await _botClient.SendTextMessageAsync(chatId: chat.Id, "Напишите город и адрес доставки 🚚📦");
                                }
                                return;
                            }
                        default:
                            {
                                await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Error", showAlert: true);
                                return;
                            }
                    }
                }
            default:
                {
                    await client.SendTextMessageAsync(chatId: update.Message.Chat.Id, "Не понимаю, что Вы хотите 😐");
                    return;
                }
        }

    }
}
