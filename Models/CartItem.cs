using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FloralHaven.Models;
using FloralHaven.Controllers;
namespace FloralHaven.Models
{
    public class CartItem
    {
        private FloralHavenDataContext _db = FloralHavenDBContextConfig.GetFloralHavenDataContext();
        public int Id { get; set; }
        public string Name { get; set; }
        public string MainImg { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Handle { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal productTotal
        {
            get { return Quantity * Price; }
        }
        public CartItem()
        {

        }
        public CartItem(int id)
        {
            PRODUCT product = _db.PRODUCTs.Single(n => n.id == id);
            if (product != null)
            {
                Id = id;
                Name = product.title;
                MainImg = _db.IMAGEs.FirstOrDefault(image => image.productid == product.id).path ?? "";
                Price = product.price;
                Quantity = 1;
                Handle = product.handle;
                SalePrice = product.saleprice;
            }
        }

    }
}