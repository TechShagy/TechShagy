using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace InventoryAPIService.Controllers
{
    public class InventoryController : ApiController
    {
        //private InventoryDBEntities db = new InventoryDBEntities();
        [System.Web.Http.HttpPost]
        public async Task<string> AddOrUpdateItemInInventory([FromBody] ProductInventory data) //async Task<string>
        {
            using (var db = new InventoryDBEntities())
            {
                try
                {
                    ProductInventory products = db.ProductInventories.Where(x => x.ProductId == data.ProductId).FirstOrDefault();
                    if (products != null) //update existing
                    {
                        if (!String.IsNullOrEmpty(data.ProductName)) products.ProductName = data.ProductName;
                        if (data.ProductPrice != 0) products.ProductPrice = data.ProductPrice;
                        if (!String.IsNullOrEmpty(data.ProductDesc)) products.ProductDesc = data.ProductDesc;
                        if (!String.IsNullOrEmpty(data.ProductManufacturer)) products.ProductManufacturer = data.ProductManufacturer;
                        products.ProductModifiedDt = DateTime.Now;
                        await db.SaveChangesAsync();
                        return "Inventory Updated Successfully!";
                    }
                    else
                    {
                        var Newproducts = new ProductInventory();
                        Newproducts.ProductName = data.ProductName;
                        Newproducts.ProductPrice = data.ProductPrice;
                        Newproducts.ProductDesc = data.ProductDesc;
                        Newproducts.ProductManufacturer = data.ProductManufacturer;
                        Newproducts.ProductModifiedDt = DateTime.Now;
                        db.ProductInventories.Add(Newproducts);
                        await db.SaveChangesAsync();
                        return "Inventory Added Successfully!";
                    }
                }
                catch (Exception ex)
                {
                    return ex.InnerException.Message;

                }
            }

        }

        [System.Web.Http.HttpGet]
        public async Task<string> DeleteItemInInventory(int ProductId)
        {
            using (var db = new InventoryDBEntities())
            {
                try
                {
                    ProductInventory products = db.ProductInventories.Where(x => x.ProductId == ProductId).FirstOrDefault();
                    if (products != null)
                    {
                        //ProductInventory ProducttoDelete = db.ProductInventories.Where(x => x.ProductId == ProductId).Single<ProductInventory>();
                        db.ProductInventories.Remove(products);
                        await db.SaveChangesAsync();
                        return "Inventory item has successfully Deleted!";
                    }
                    else 
                        return "Inventory item does not exists or has been Deleted by someone!";
                }
                catch (Exception ex)
                {
                    return ex.InnerException.Message;

                }
            }
        }

        [System.Web.Http.HttpGet]
        public async Task<List<ProductInventory>> GetAllItemInInventory()
        {
            using (var db = new InventoryDBEntities())
            {
                try
                {
                    List<ProductInventory> allProducts = await Task.Run(() => db.ProductInventories.ToList());
                    return allProducts;
                }
                catch (Exception ex)
                {
                    throw;

                }
            }
        }

        
    }
}
