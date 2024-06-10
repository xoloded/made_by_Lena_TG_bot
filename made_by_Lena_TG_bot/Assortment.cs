using made_by_Lena_TG_bot.DataBase;
using made_by_Lena_TG_bot.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using Telegram.Bot;
using Telegram.Bot.Types;

public class Assortment
{
    private double _averageRatingCategory;
    private int _currentCard;
    private int _allAssortimentCards;
    private int _numberOfPhotosSend;
    private int _numberOfPasses;
    public ProductCategory _currentProductCategory;
    public Product _assortimentCard;

    public void GetCurrentProductCategory(ProductCategory productCategory)
    {
        _currentProductCategory = productCategory;
    }
    public async Task GetAverageRating(ProductCategory productCategory)
    {
        using var context = new DatabaseContext();

        var averageRating = await context.Reviews
            .Include(q => q.Category)
            .Where(q => q.Category.ProductCategory == productCategory)
            .ToListAsync();

        if (averageRating.Count == 0)
        {
            _averageRatingCategory = 0;
        }
        else
        {
            var average = averageRating.Average(q => q.Rating);
            _averageRatingCategory = Math.Round(average, 2);
        }
    }
    public void GetAllAssortimentCards(ProductCategory productCategory)
    {
        using var context = new DatabaseContext();
        _allAssortimentCards = context.Products
            .Include(q => q.Category)
            .Where(q => q.Category.ProductCategory == productCategory)
            .Count(q => q.Available == true);
    }
    public bool CheckingAvailabilityAllAssortimentCards()
    {
        return _allAssortimentCards == 0;
    }
    public async Task GetAssortimentCard(ProductCategory productCategory)
    {
        using var context = new DatabaseContext();
        _assortimentCard = await context.Products
            .Include(q => q.Category)
            .Include(q => q.Photos)
            .Where(q => q.Category.ProductCategory == productCategory)
            .Where(q => q.Available == true)
            .OrderBy(q => q.Id)
            .Skip(_currentCard)
            .FirstOrDefaultAsync();
    }
    public List<InputMediaPhoto> SendPhotosProduct()
    {
        var media = new List<InputMediaPhoto>();
        var photosProduct = _assortimentCard.Photos.ToList();
        var i = 0;
        foreach (var e in photosProduct)
        {
            var photo = System.IO.File.OpenRead(e.Path);
            media.Add(new InputMediaPhoto(InputFile.FromStream(photo, $"{i}")));
            i++;
        }
        _numberOfPhotosSend = i;
        return media;
    }
    public string GetDescriptionCard(ShopingCart shopingCart)
    {
        return $"Категория: {_assortimentCard.Category.ProductCategory} (⭐️ {_averageRatingCategory}/5)\n" +
               $"Товар: {_currentCard + 1} из {_allAssortimentCards}\n" +
               $"Название: {_assortimentCard.Name} (ID: {_assortimentCard.Id})\n" +
               $"Описание: {_assortimentCard.Description}\n" +
               $"Цена: {_assortimentCard.Price}₽\n" +
               $"В корзине 🛒: {shopingCart.GetProductInProductCard(_assortimentCard.Id)} (Всего: {shopingCart.GetCountAssortimnetInCart()})";
    }
    public bool ValidateAndChangeCurrentCard(int step)
    {
        if (_currentCard + step >= 0 && _currentCard + step < _allAssortimentCards)
        {
            _currentCard += step;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ClearAssortimentCard()
    {
        _assortimentCard = null;
        _currentCard = 0;
        _averageRatingCategory = 0;
        _allAssortimentCards = 0;
        _numberOfPhotosSend = 0;
        _currentProductCategory = 0;
        _numberOfPasses = 0;
    }
    public async Task DeleteAndSendNextCard(long chatId, SessionUser sessionUser, TelegramBotClient _botClient, CallbackQuery callbackQuery)
    {
        await _botClient.DeleteMessageAsync(chatId: chatId, callbackQuery.Message.MessageId);
        for (int i = 1; i <= _numberOfPhotosSend; i++)
        {
            await _botClient.DeleteMessageAsync(chatId: chatId, callbackQuery.Message.MessageId - _numberOfPasses - i);
        }
        await sessionUser._assortment.GetAssortimentCard(sessionUser._assortment._currentProductCategory);
        var mediaPhotos = sessionUser._assortment.SendPhotosProduct();
        await _botClient.SendMediaGroupAsync(chatId: chatId, media: mediaPhotos);
        await _botClient.SendTextMessageAsync(chatId: chatId, sessionUser._assortment.GetDescriptionCard(sessionUser._shopingCart), replyMarkup: sessionUser._control.assortmentSelectionInlineKeyboard);
        _numberOfPasses = 0;
    }
    public async Task СhangeShopingCartAndSendCurrentCard(long chatId, SessionUser sessionUser, TelegramBotClient _botClient, CallbackQuery callbackQuery)
    {
        _numberOfPasses++;
        await _botClient.DeleteMessageAsync(chatId: chatId, callbackQuery.Message.MessageId);
        await _botClient.SendTextMessageAsync(chatId: chatId, sessionUser._assortment.GetDescriptionCard(sessionUser._shopingCart), replyMarkup: sessionUser._control.assortmentSelectionInlineKeyboard);
    }
    public async Task DeleteCurrentCard(long chatId, SessionUser sessionUser, TelegramBotClient _botClient, CallbackQuery callbackQuery)
    {
        await _botClient.DeleteMessageAsync(chatId: chatId, callbackQuery.Message.MessageId);
        for (int i = 1; i <= _numberOfPhotosSend; i++)
        {
            await _botClient.DeleteMessageAsync(chatId: chatId, callbackQuery.Message.MessageId - _numberOfPasses - i);
        }
    }
}