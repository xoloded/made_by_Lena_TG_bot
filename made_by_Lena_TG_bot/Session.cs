using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

public class Session
{
    Dictionary<long, SessionUser> _session = new Dictionary<long, SessionUser>();
    public SessionUser GetSession(long id)
    {
        if (!_session.ContainsKey(id))
        {
            _session.Add(id, new SessionUser());
        }
        return _session[id];
    }
}
public class SessionUser
{
    public Keyboard _control;
    public ReviewUser _reviewUser;
    public Assortment _assortment;
    public ShopingCart _shopingCart;
    public SessionUser()
    {
        _control = new Keyboard();
        _reviewUser = new ReviewUser();
        _assortment = new Assortment();
        _shopingCart = new ShopingCart();
    }
}