using made_by_Lena_TG_bot.DataBase;
using made_by_Lena_TG_bot.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

public class ShopingCart
{
    public bool IsInReview { get; set; }
    private string _deliveryCity;
    private Dictionary<long, int> _assortimentInCart = new Dictionary<long, int>();
    public void ChangeShopingCartStatus()
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
    public void ResetShopingCartState()
    {
        IsInReview = false;
        _deliveryCity = null;
        ClearProductCart();
    }
    public string GetProductInProductCard(long productId)
    {
        if (_assortimentInCart.ContainsKey(productId))
        {
            return _assortimentInCart[productId].ToString() + " товар" + GeneralClass.GetEndOfWord(_assortimentInCart[productId]);  
        }
        else
        {
            return "0 товаров";
        }
    }
    public string GetCountAssortimnetInCart()
    {
        int totalProduct = 0;
        foreach (var e in _assortimentInCart)
        {
            totalProduct += e.Value;
        }
        return totalProduct.ToString();
    }
    public void AddProductIdInCart(Product assortimentCard)
    {
        if (_assortimentInCart.ContainsKey(assortimentCard.Id))
        {
            _assortimentInCart[assortimentCard.Id]++;
        }
        else
        {
            _assortimentInCart.Add(assortimentCard.Id, 1);
        } 
    }
    public string GetProductAtProductCart()
    {
        var order = new StringBuilder();
        if (_assortimentInCart.Count == 0)
        {
            return "Корзина пуста 😔";
        }
        else
        {
            using var context = new DatabaseContext();
            var count = 1;
            double finalPrice = 0;
            foreach (var e in _assortimentInCart)
            {
                var product = context.Products
                    .Where(q => q.Id == e.Key)
                    .ToList();
                order.Append($"{count}. {product[0].Name} (ID: {product[0].Id})\n" +
                          $"{product[0].Price} х {e.Value}\n" +
                          $"Стоимость: {product[0].Price * e.Value}₽\n");
                finalPrice += product[0].Price * e.Value;
                count++;
            }
            return $"{order.ToString()}" +
                   $"=======\n" +
                   $"Итого: {finalPrice}₽";
        }  
    }
    public void ClearProductCart()
    {
        _assortimentInCart.Clear();
    }
    public async Task<bool> ChangeAndDeleteProductInCart(Product assortimentCard)
    {
        if (_assortimentInCart.ContainsKey(assortimentCard.Id))
        {
            if (_assortimentInCart[assortimentCard.Id] > 1)
            {
                _assortimentInCart[assortimentCard.Id]--;
            }
            else
            {
                _assortimentInCart.Remove(assortimentCard.Id);
            }
            return false;
        }
        else
        {
            return true;
        }
        
    }
    private string OrderConfirmation()
    {
        return $"{ GetProductAtProductCart()} + стоимость доставки 🚚\n=======\nГород и адрес получателя: { _deliveryCity}";

    }
    public bool ProofEmptyProductCard()
    {
        return (_assortimentInCart.Count == 0) ? true : false;
    }

    public async Task MakeOrder(ShopingCart _shopingCart, Keyboard _control, Message message, ITelegramBotClient client)
    {
        if (message.Text == null)
        {
            await client.SendTextMessageAsync(chatId: message.Chat.Id, "Не понимаю, что Вы хотите 😐");
            return;
        }
        else if (_shopingCart._deliveryCity == null)
        {
            _shopingCart._deliveryCity = message.Text;
            await client.SendTextMessageAsync(chatId: message.Chat.Id, _shopingCart.OrderConfirmation(), replyMarkup: _control.OrderConfirmationReplyKeyboard);
            return;
        }
        else if (message.Text == "Подтверждаю ✅" || message.Text.ToLower() == "подтверждаю")
        {
            await client.SendTextMessageAsync(chatId: message.Chat.Id, "Спасибо за заказ 🥰\nВ ближайшее время с Вами свяжется мастер");
            _shopingCart.ResetShopingCartState();
            await client.SendTextMessageAsync(chatId: message.Chat.Id, "Меню ☰", replyMarkup: _control.mainMenuInlineKeyboard);
            //что дальше с заказом
            return;
        }
        else if (message.Text == "Отмена 🚫" || message.Text.ToLower() == "отмена")
        {
            await client.SendTextMessageAsync(chatId: message.Chat.Id, "Очень жаль 😔\n");
            _shopingCart.ResetShopingCartState();
            await client.SendTextMessageAsync(chatId: message.Chat.Id, "Меню ☰", replyMarkup: _control.mainMenuInlineKeyboard);
        }
        else
        {
            await client.SendTextMessageAsync(chatId: message.Chat.Id, "Не понимаю, что Вы хотите 😐");
            return;
        }
    }
}

