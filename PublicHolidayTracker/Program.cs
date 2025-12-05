using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json; // JSON işlemleri için gerekli
using System.Threading.Tasks;

namespace PublicHolidayTracker
{
    // Hocanın istediği sınıf yapısı
    public class Holiday
    {
        public string date { get; set; }
        public string localName { get; set; }
        public string name { get; set; }
        public string countryCode { get; set; }

        // 'fixed' C#'ta özel bir kelime olduğu için başına @ koyuyoruz
        public bool @fixed { get; set; }
        public bool global { get; set; }
    }

    class Program
    {
        // API'den çektiğimiz tüm tatilleri bu listede tutacağız
        static List<Holiday> allHolidays = new List<Holiday>();

        // Web istekleri yapmak için HttpClient nesnesi
        static HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Veriler API'den çekiliyor, lütfen bekleyiniz...");

            // 1. ADIM: 2023, 2024 ve 2025 verilerini çekip hafızaya atıyoruz
            await FetchDataAsync(2023);
            await FetchDataAsync(2024);
            await FetchDataAsync(2025);

            Console.Clear(); // Ekranı temizle

            // 2. ADIM: Menü Döngüsü
            while (true)
            {
                Console.WriteLine("\n===== PublicHolidayTracker =====");
                Console.WriteLine("1. Tatil listesini göster (yıl seçmeli)");
                Console.WriteLine("2. Tarihe göre tatil ara (gg-aa formatı)");
                Console.WriteLine("3. İsme göre tatil ara");
                Console.WriteLine("4. Tüm tatilleri 3 yıl boyunca göster (2023–2025)");
                Console.WriteLine("5. Çıkış");
                Console.Write("Seçiminiz: ");

                string secim = Console.ReadLine();

                switch (secim)
                {
                    case "1":
                        ListByYear();
                        break;
                    case "2":
                        SearchByDate();
                        break;
                    case "3":
                        SearchByName();
                        break;
                    case "4":
                        ShowAllHolidays();
                        break;
                    case "5":
                        Console.WriteLine("Çıkış yapılıyor...");
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim! Tekrar deneyin.");
                        break;
                }
            }
        }

        // API'den veri çeken metod (Async)
        static async Task FetchDataAsync(int year)
        {
            string url = $"https://date.nager.at/api/v3/PublicHolidays/{year}/TR";
            try
            {
                // API'ye istek atıp cevabı string (JSON) olarak alıyoruz
                string jsonResponse = await client.GetStringAsync(url);

                // Gelen JSON verisini C# listesine çeviriyoruz (Deserialize)
                List<Holiday> holidaysOfYear = JsonSerializer.Deserialize<List<Holiday>>(jsonResponse);

                // Ana listemize ekliyoruz
                if (holidaysOfYear != null)
                {
                    allHolidays.AddRange(holidaysOfYear);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{year} yılı verisi çekilirken hata oluştu: {ex.Message}");
            }
        }

        // 1. Seçenek: Yıla göre listeleme
        static void ListByYear()
        {
            Console.Write("Görmek istediğiniz yılı girin (2023, 2024, 2025): ");
            string inputYear = Console.ReadLine();

            // Girilen yılı içeren tarihleri filtrele (API tarihi YYYY-MM-DD formatında verir)
            var filteredList = allHolidays.Where(h => h.date.StartsWith(inputYear)).ToList();

            if (filteredList.Count > 0)
            {
                PrintList(filteredList);
            }
            else
            {
                Console.WriteLine("Bu yıla ait veri bulunamadı.");
            }
        }

        // 2. Seçenek: Tarihe göre arama (gg-aa)
        static void SearchByDate()
        {
            Console.Write("Tarih girin (Örn: 23-04): ");
            string inputDate = Console.ReadLine();

            // Kullanıcı 23-04 girer ama API 2024-04-23 verir.
            // Bu yüzden "dd-MM" formatındaki girdiyi ters çevirip "MM-dd" olarak aratacağız
            // Çünkü API formatının sonu -MM-dd şeklinde biter.

            // Basit bir yöntem: Girdinin formatını kontrol edelim
            string[] parts = inputDate.Split('-');
            if (parts.Length == 2)
            {
                string searchPattern = $"-{parts[1]}-{parts[0]}"; // -Ay-Gün (API formatına uygun hale getirme)

                var foundHolidays = allHolidays.Where(h => h.date.EndsWith(searchPattern)).ToList();

                if (foundHolidays.Count > 0)
                {
                    PrintList(foundHolidays);
                }
                else
                {
                    Console.WriteLine("Bu tarihte bir tatil bulunamadı.");
                }
            }
            else
            {
                Console.WriteLine("Hatalı format! Lütfen gg-aa şeklinde girin.");
            }
        }

        // 3. Seçenek: İsme göre arama
        static void SearchByName()
        {
            Console.Write("Tatil adı girin (Örn: Cumhuriyet): ");
            string searchName = Console.ReadLine().ToLower();

            var foundHolidays = allHolidays
                .Where(h => h.localName.ToLower().Contains(searchName) || h.name.ToLower().Contains(searchName))
                .ToList();

            if (foundHolidays.Count > 0)
            {
                PrintList(foundHolidays);
            }
            else
            {
                Console.WriteLine("Bu isimde bir tatil bulunamadı.");
            }
        }

        // 4. Seçenek: Tüm listeyi göster
        static void ShowAllHolidays()
        {
            PrintList(allHolidays);
        }

        // Ekrana yazdırma yardımcı metodu (Kod tekrarını önlemek için)
        static void PrintList(List<Holiday> list)
        {
            Console.WriteLine($"\n--- Toplam {list.Count} kayıt bulundu ---");
            Console.WriteLine("{0,-12} {1,-35} {2}", "TARİH", "YEREL AD", "ULUSLARARASI AD");
            Console.WriteLine(new string('-', 70));

            foreach (var h in list)
            {
                Console.WriteLine($"{h.date,-12} {h.localName,-35} {h.name}");
            }
            Console.WriteLine(new string('-', 70));
        }
    }
}