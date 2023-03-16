using SellerProduct.IServices;
using SellerProduct.Models;

namespace SellerProduct.Services
{
    public class ProductServices : IProductServices
    {
        ShopDbContext context;
        public ProductServices()
        {
            context = new ShopDbContext();
        }
        public bool CreateProduct(Product p)
        {
            try
            {
                context.Products.Add(p);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteProduct(Guid id)
        {
            try
            {// Find(id) chỉ dùng được khi id là khóa chính
                var product = context.Products.Find(id);
                context.Products.Remove(product);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Product> GetAllProducts()
        {
            return context.Products.ToList();
        }

        public Product GetProductById(Guid id)
        {
            return context.Products.FirstOrDefault(p => p.Id == id);
            // return context.Products.SingleOrDefault(p => p.Id == id);
        }

        public List<Product> GetProductByName(string name)
        {
            return context.Products.Where(p => p.Name.Contains(name)).ToList();
        }

        public bool UpdateProduct(Product p)
        {
            try
            {// Find(id) chỉ dùng được khi id là khóa chính
                var product = context.Products.Find(p.Id);
                product.Name = p.Name;
                product.Description = p.Description; 
                // Có thể sửa thêm thuộc tính
                context.Products.Update(product);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
