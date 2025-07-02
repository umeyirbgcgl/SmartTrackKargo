-- Veritabanını oluştur
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SmartTrackKargo')
BEGIN
    CREATE DATABASE SmartTrackKargo;
END
GO

USE SmartTrackKargo;
GO

-- DeliveryPoints tablosunu oluştur
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeliveryPoints]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DeliveryPoints](
        [Id] [int] IDENTITY(1,1) PRIMARY KEY,
        [Latitude] [float] NOT NULL,
        [Longitude] [float] NOT NULL,
        [Info] [nvarchar](500) NOT NULL,
        [IsDelivered] [bit] NOT NULL DEFAULT(0)
    )
END
GO

-- WeatherPrediction tablosunu oluştur
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WeatherPrediction]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[WeatherPrediction](
        [Id] [int] IDENTITY(1,1) PRIMARY KEY,
        [Sicaklik] [float] NOT NULL,
        [Nem] [float] NOT NULL,
        [HavaDurumu] [nvarchar](50) NOT NULL,
        [Tarih] [datetime] NOT NULL DEFAULT(GETDATE())
    )
END
GO

-- Örnek veri ekle
IF NOT EXISTS (SELECT TOP 1 1 FROM [dbo].[DeliveryPoints])
BEGIN
    INSERT INTO [dbo].[DeliveryPoints] (Latitude, Longitude, Info, IsDelivered) VALUES
    (41.0082, 28.9784, 'Sultanahmet Meydanı', 0),
    (41.0096, 28.9652, 'Kapalıçarşı', 0),
    (41.0054, 28.9768, 'Ayasofya', 0),
    (41.0136, 28.9550, 'Beyazıt Meydanı', 0);
END
GO 