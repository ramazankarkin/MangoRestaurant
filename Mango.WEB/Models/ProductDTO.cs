namespace Mango.WEB.Models.DTO
{
    public class ProductDTO // bu DTO(data transfer object) class database'ye veri ekleme çıkarma işlemi yapmak için kullanılmıyor.
                            // DTO'lar içlerinde business kod bulundurmazlar görevleri sadece verileri taşımak ve geçiçi olarak saklamaktır.
                            //Aşağıdaki propertylerin adları Product entity içindeki propertyle ile aynı olmalı, yoksa mapper içerisinde profiller oluştururken özel kurallar yazmamız gerekir.
                            // İsimler aynı olursa AutoMapper otomatik olarak hangi property nin Dto daki hangi propertye eşit olduğunu anlayacak ve atamaları yapacak.
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageURL { get; set; }
    }
}
