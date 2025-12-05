# 📅 PublicHolidayTracker - Resmi Tatil Takip Sistemi

Bu projeyi *Görsel Programlama* dersi ödevi olarak hazırladım. C# diliyle yazılmış, internet üzerinden veri çeken basit bir konsol uygulamasıdır.

## 📝 Proje Hakkında
Uygulama, `Nager.Date` API servisine bağlanarak Türkiye'nin 2023, 2024 ve 2025 yıllarındaki resmi tatil günlerini JSON formatında alıyor ve bize listeliyor.

## 💻 Kullandığım Teknolojiler
Hocamızın istediği teknik şartları yerine getirmeye çalıştım:
* **C# Console App:** Projenin temel yapısı.
* **HttpClient:** İnternetten (API'den) veri çekmek için kullandım.
* **System.Text.Json:** Gelen JSON verisini C# sınıflarına (Class) dönüştürmek için.
* **LINQ:** Listeler içinde isme veya tarihe göre arama yapmak için.

## 🚀 Neler Yapılabiliyor?
Programı çalıştırdığınızda veriler otomatik olarak indiriliyor ve şu menü geliyor:

1.  **Yıla Göre Listeleme:** Sadece seçtiğiniz yılın tatillerini gösterir.
2.  **Tarihe Göre Arama:** "gg-aa" (Örn: 29-10) formatında girilen tarihte tatil var mı kontrol eder.
3.  **İsme Göre Arama:** Örneğin "Ramazan" yazınca ilgili tatilleri bulur.
4.  **Tüm Liste:** 3 yılın bütün tatillerini ekrana basar.

-Kurulum
Projeyi Visual Studio ile açıp *Start* tuşuna basmanız yeterlidir. İnternet bağlantısı gerektirir (API'den veri çektiği için).

---
*Hazırlayan:* Esra Çam
*Öğrenci No:* 20240108003
*Ders Kodu:* BIP2033
