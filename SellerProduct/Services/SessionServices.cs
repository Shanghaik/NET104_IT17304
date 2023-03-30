using Newtonsoft.Json;
using SellerProduct.Models;

namespace SellerProduct.Services
{
    public static class SessionServices
    {
        // 1. Đọc dữ liệu từ Session => Trả về 1 List
        public static List<Product> GetObjFromSession(ISession session, string key)
        {
            string jsonData = session.GetString(key); // Lấy dữ liệu dạng string từ Session
            if(jsonData == null)
            {
                return new List<Product>(); // Khỏi tạo 1 list mới để chứa sp khi dữ liệu
                // Lấy ra null <=> Session chưa được tạo ra -> Lần đầu làm chuyện ấy
            }else
            { // Nếu dữ liệu có thì ta sẽ chuyển đổi nó về dạng List
                var products = JsonConvert.DeserializeObject<List<Product>>(jsonData);
                return products;
            }
        }
        // 2: Ghi đè dữ liệu vào Session từ 1 list
        public static void SetObjToSession(ISession session, string key, object data)
        {
            var jsonData = JsonConvert.SerializeObject(data); // Chuyển đổi dữ liệu về jsonData
            session.SetString(key, jsonData); // Ghi đè vào Session
        }
        // 3: Kiểm tra xem 1 đối tượng có nằm trong 1 List hay không
        public static bool CheckObjInList(Guid id, List<Product> products)
        {
            return products.Any(p => p.Id == id);
        }
    }
}
