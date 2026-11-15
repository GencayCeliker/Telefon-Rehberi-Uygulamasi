/* =====================================================================
   Telefon Rehberi Uygulaması - Veritabanı Kurulum Script'i
   ---------------------------------------------------------------------
   SQL Server Management Studio (SSMS) 22 ile:
     1. Sunucunuza bağlanın (ör. .\SQLEXPRESS veya localhost)
     2. File > Open > File... ile bu dosyayı açın
     3. Execute (F5) ile çalıştırın
   Script; veritabanını, TBLKISILER tablosunu ve örnek kayıtları oluşturur.
   Veritabanı zaten varsa yeniden oluşturmaz, sadece eksikleri tamamlar.
   ===================================================================== */

IF DB_ID(N'DbTelRehberB') IS NULL
BEGIN
    CREATE DATABASE [DbTelRehberB] COLLATE Turkish_CI_AS;
END
GO

USE [DbTelRehberB];
GO

IF OBJECT_ID(N'dbo.TBLKISILER', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[TBLKISILER]
    (
        [KISIID]    INT IDENTITY(1,1) NOT NULL,
        [AD]        NVARCHAR(30)  NULL,
        [SOYAD]     NVARCHAR(30)  NULL,
        [TEL]       CHAR(15)      NULL,
        [DOGUMGUNU] SMALLDATETIME NULL,
        [ADRES]     NVARCHAR(50)  NULL,
        [CINSIYET]  CHAR(1)       NULL,
        CONSTRAINT [PK_TBLKISILER] PRIMARY KEY CLUSTERED ([KISIID] ASC)
    );
END
GO

-- Örnek kayıtlar (tablo boşsa eklenir)
IF NOT EXISTS (SELECT 1 FROM [dbo].[TBLKISILER])
BEGIN
    INSERT INTO [dbo].[TBLKISILER] ([AD], [SOYAD], [TEL], [DOGUMGUNU], [ADRES], [CINSIYET]) VALUES
        (N'Ali',   N'GÜNEŞ',  '05001234567', '2005-06-20', N'Zile',     'E'),
        (N'Deniz', N'YILDIZ', '05002345678', '2008-12-12', N'Turhal',   'K'),
        (N'Ahmet', N'Taşkın', '05003456789', '1999-03-15', N'İstanbul', 'E'),
        (N'Selin', N'Kaçar',  '05004567890', '2001-09-18', N'Ankara',   'K');
END
GO

SELECT * FROM [dbo].[TBLKISILER];
GO
