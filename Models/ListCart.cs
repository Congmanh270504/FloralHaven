using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FloralHaven.Models
{
    public class ListCart
    {
        public List<CartItem> lst;
        public ListCart()
        {
            lst = new List<CartItem>();
        }
        public ListCart(List<CartItem> lstGH)
        {
            lst = lstGH;
        }
        public int SoMatHang()
        {
            return lst.Count();
        }
        public int ListCount()
        {
            return lst.Sum(t => t.Quantity);
        }
        public decimal Total()
        {
            return lst.Sum(t => t.productTotal);
        }
        public int Add(int id)
        {
            CartItem product = lst.Find(n => n.Id == id);
            if (product == null)
            {
                CartItem tmp = new CartItem(id);
                if (tmp == null)
                {
                    return -1;
                }
                lst.Add(tmp);
            }
            else
            {
                product.Quantity++;
            }
            return 1;
        }
        public int Add(int id, int sl)
        {
            CartItem product = lst.Find(n => n.Id == id);
            if (product == null)
            {
                CartItem tmp = new CartItem(id);
                if (tmp == null)
                {
                    return -1;
                }
                tmp.Quantity = sl;
                lst.Add(tmp);
            }
            else
            {
                product.Quantity += sl;
            }
            return 1;
        }
        public int Update(int id, int sl)
        {
            CartItem product = lst.Find(n => n.Id == id);
            if (product == null)
            {
                CartItem tmp = new CartItem(id);
                if (tmp == null)
                {
                    return -1;
                }
                tmp.Quantity= sl;
                lst.Add(tmp);
            }
            else
            {
                product.Quantity = sl;
            }
            return 1;
        }
    }
}