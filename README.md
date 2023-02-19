# MailSender ASP.NET .NETFramework MVC

## Opis aplikacji

- aplikacja do wysyłania e-maili
- profile użytkowników - możliwość dodania kilku adresów e-mail
- tworzenie i wysyłanie e-maili wraz z załącznikami
- wysłane e-maile zapisywane są w bazie danych
- możliwość wydruku e-maili
- obsługa załączników

## Tech 

- UI - Bootstrap 5 - [Bootswatch JOURNAL Theme](https://bootswatch.com/journal/)
- zastosowanie kontrolki [CKEDITOR 5](https://ckeditor.com/ckeditor-5/) do wprowadzania treści wiadomości w formacie html
- zastosowanie biblioteki [HTML.Sanitizer](https://github.com/mganss/HtmlSanitizer) w celu ochrony przed atakami XSS
- zastosowanie kontrolki [DataTables](https://datatables.net/) w celu wyświetlania listy wiadomości
- emaile przechowywane w bazie danych MSSQL
