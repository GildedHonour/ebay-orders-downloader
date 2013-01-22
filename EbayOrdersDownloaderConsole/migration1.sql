USE [Ebay]
GO

/****** Object:  StoredProcedure [dbo].[PrListings_GetMPN]    Script Date: 01/22/2013 16:16:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PrListings_GetMPN](@ListingID varchar(50))
AS
SELECT [MPN] FROM [EBAY].[dbo].[Listings] 
WHERE  [ListingID] = @ListingID
GO


USE [OrderManager]
GO

/****** Object:  StoredProcedure [dbo].[PrDetailRecord_Add]    Script Date: 01/22/2013 16:16:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PrDetailRecord_Add]
( @InvoiceID varchar(50)
 ,@PurchaseID varchar(50)
 ,@VolumeID varchar(50)
 ,@VolumeName varchar(255)
 ,@SourceCode varchar(50)
 ,@ProductSKUCode varchar(100)
 ,@ProductDescription varchar(255)
 ,@Quantity numeric(18, 0)
 ,@UnitPrice float
 ,@ExtendedPrice float
 ,@CouponCodes varchar(50)
 ,@I_StatusCode numeric(18, 0)
 ,@I_ShipDate datetime
 ,@I_Tracking varchar(35)
 ,@I_ShippingMethod numeric(18, 0)
 ,@I_SyncWithShop int
 ,@I_OriginalCost float
 ,@I_OriginalQTY numeric(18, 0)
 ,@Commision numeric(18, 2)) 
AS
INSERT INTO [OrderManager].[dbo].[DetailRecord]
           ([InvoiceID]
           ,[PurchaseID]
           ,[VolumeID]
           ,[VolumeName]
           ,[SourceCode]
           ,[ProductSKUCode]
           ,[ProductDescription]
           ,[Quantity]
           ,[UnitPrice]
           ,[ExtendedPrice]
           ,[CouponCodes]
           ,[I_StatusCode]
           ,[I_ShipDate]
           ,[I_Tracking]
           ,[I_ShippingMethod]
           ,[I_SyncWithShop]
           ,[I_OriginalCost]
           ,[I_OriginalQTY]
           ,[Commision])
     VALUES
           (@InvoiceID
           ,@PurchaseID
           ,@VolumeID
           ,@VolumeName
           ,@SourceCode
           ,@ProductSKUCode
           ,@ProductDescription
           ,@Quantity
           ,@UnitPrice
           ,@ExtendedPrice
           ,@CouponCodes
           ,@I_StatusCode
           ,@I_ShipDate
           ,@I_Tracking
           ,@I_ShippingMethod
           ,@I_SyncWithShop
           ,@I_OriginalCost
           ,@I_OriginalQTY
           ,@Commision)

GO



USE [OrderManager]
GO

/****** Object:  StoredProcedure [dbo].[PrHeaderRecords_Add]    Script Date: 01/22/2013 16:17:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PrHeaderRecords_Add](
			@OrderID varchar(50)
           ,@InvoiceID varchar(50)
           ,@OrderDate varchar(50)
           ,@Email varchar(150)
           ,@ShopperID varchar(50)
           ,@BilltoFirstName varchar(50)
           ,@BilltoLastName varchar(50)
           ,@BilltoCompanyName varchar(50)
           ,@BilltoStreetAddress varchar(250)
           ,@BilltoOptionalAddress varchar(250)
           ,@BilltoCity varchar(50)
           ,@BilltoState varchar(50)
           ,@BilltoZip varchar(50)
           ,@BilltoCountry varchar(50)
           ,@BilltoRegion varchar(50)
           ,@BilltoPhone varchar(50)
           ,@DeliveryMethod varchar(50)
           ,@ProductSubtotal varchar(53)
           ,@ShippingAndHandling varchar(53)
           ,@TaxMultiplier varchar(50)
           ,@Tax varchar(53)
           ,@Discount varchar(53)
           ,@OverallTotal varchar(53)
           ,@CreditCardType varchar(50)
           ,@CreditCardNumber varchar(50)
           ,@CreditCardExpiration varchar(50)
           ,@CreditCardSecurityNumberCCV varchar(50)
           ,@NameOnCreditCard varchar(50)
           ,@ShiptoFirstName varchar(50)
           ,@ShiptoLastName varchar(50)
           ,@ShiptoCompanyName varchar(50)
           ,@ShiptoStreetAddress varchar(250)
           ,@ShiptoOptionalAddress varchar(250)
           ,@ShiptoCity varchar(50)
           ,@ShiptoState varchar(50)
           ,@ShiptoZip varchar(50)
           ,@ShiptoCountry varchar(50)
           ,@ShiptoRegion varchar(50)
           ,@ShiptoPhone varchar(50)
           ,@SHOPCOMCatalogID varchar(50)
           ,@CatalogName varchar(50)
           ,@MultiplePaymentsQuantity varchar(50)
           ,@CanSellNamePrivacyFlag char(1)
           ,@CanSendOffersPrivacyFlag char(1)
           ,@ShopperComments varchar(MAX)
           ,@Source varchar(20)
           ,@PayPalTxnID varchar(50)
           ,@GiftMessage varchar(MAX)
           ,@Commision varchar(50))

as
INSERT INTO [OrderManager].[dbo].[HeaderRecords]
           ([OrderID]
           ,[InvoiceID]
           ,[OrderDate]
           ,[Email]
           ,[ShopperID]
           ,[BilltoFirstName]
           ,[BilltoLastName]
           ,[BilltoCompanyName]
           ,[BilltoStreetAddress]
           ,[BilltoOptionalAddress]
           ,[BilltoCity]
           ,[BilltoState]
           ,[BilltoZip]
           ,[BilltoCountry]
           ,[BilltoRegion]
           ,[BilltoPhone]
           ,[DeliveryMethod]
           ,[ProductSubtotal]
           ,[ShippingAndHandling]
           ,[TaxMultiplier]
           ,[Tax]
           ,[Discount]
           ,[OverallTotal]
           ,[CreditCardType]
           ,[CreditCardNumber]
           ,[CreditCardExpiration]
           ,[CreditCardSecurityNumberCCV]
           ,[NameOnCreditCard]
           ,[ShiptoFirstName]
           ,[ShiptoLastName]
           ,[ShiptoCompanyName]
           ,[ShiptoStreetAddress]
           ,[ShiptoOptionalAddress]
           ,[ShiptoCity]
           ,[ShiptoState]
           ,[ShiptoZip]
           ,[ShiptoCountry]
           ,[ShiptoRegion]
           ,[ShiptoPhone]
           ,[SHOPCOMCatalogID]
           ,[CatalogName]
           ,[MultiplePaymentsQuantity]
           ,[CanSellNamePrivacyFlag]
           ,[CanSendOffersPrivacyFlag]
           ,[ShopperComments]
           ,[Source]
           ,[PayPalTxnID]
           ,[GiftMessage]
           ,[Commision])
     VALUES
           (@OrderID
           ,@InvoiceID
           ,@OrderDate
           ,@Email
           ,@ShopperID
           ,@BilltoFirstName
           ,@BilltoLastName
           ,@BilltoCompanyName
           ,@BilltoStreetAddress
           ,@BilltoOptionalAddress
           ,@BilltoCity
           ,@BilltoState
           ,@BilltoZip
           ,@BilltoCountry
           ,@BilltoRegion
           ,@BilltoPhone
           ,@DeliveryMethod
           ,@ProductSubtotal
           ,@ShippingAndHandling
           ,@TaxMultiplier
           ,@Tax
           ,@Discount
           ,@OverallTotal
           ,@CreditCardType
           ,@CreditCardNumber
           ,@CreditCardExpiration
           ,@CreditCardSecurityNumberCCV
           ,@NameOnCreditCard
           ,@ShiptoFirstName
           ,@ShiptoLastName
           ,@ShiptoCompanyName
           ,@ShiptoStreetAddress
           ,@ShiptoOptionalAddress
           ,@ShiptoCity
           ,@ShiptoState
           ,@ShiptoZip
           ,@ShiptoCountry
           ,@ShiptoRegion
           ,@ShiptoPhone
           ,@SHOPCOMCatalogID
           ,@CatalogName
           ,@MultiplePaymentsQuantity
           ,@CanSellNamePrivacyFlag
           ,@CanSendOffersPrivacyFlag
           ,@ShopperComments
           ,@Source
           ,@PayPalTxnID
           ,@GiftMessage
           ,@Commision)

GO


USE [OrderManager]
GO

/****** Object:  StoredProcedure [dbo].[PrHeaderRecords_Exists]    Script Date: 01/22/2013 16:17:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PrHeaderRecords_Exists](@OrderID varchar(50))
as
if exists (select * FROM [OrderManager].[dbo].[HeaderRecords] where OrderID = @OrderID)
	select 1
else 
	select 0



GO


USE [OrderManager]
GO

/****** Object:  StoredProcedure [dbo].[PrOrderMessage_Add]    Script Date: 01/22/2013 16:17:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PrOrderMessage_Add](
   @InvoiceID varchar(50)
  ,@To varchar(50)
  ,@From varchar(50)
  ,@Message varchar(50)
  ,@DeliveryDate varchar(50)
)
AS
INSERT INTO [OrderManager].[dbo].[OrderMessage]
           ([InvoiceID]
           ,[To]
           ,[From]
           ,[Message]
           ,[DeliveryDate])
     VALUES
           (   @InvoiceID
			  ,@To
			  ,@From
			  ,@Message
			  ,@DeliveryDate)

GO


USE [OrderManager]
GO

/****** Object:  StoredProcedure [dbo].[PrOrderStatus_Add]    Script Date: 01/22/2013 16:17:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PrOrderStatus_Add](
			@OrderID varchar(50)
           ,@Status int
           ,@Notes ntext
           ,@Tracking varchar(35)
           ,@ShipMethod int
           ,@ShipDate varchar(50)
           ,@Synced int
           ,@ProcessedBy int
           ,@PONum varchar(25)
           ,@Vendor int
           ,@InvoiceNum varchar(25)
)
AS
 INSERT INTO [OrderManager].[dbo].[OrderStatus]
           ([OrderID]
           ,[Status]
           ,[Notes]
           ,[Tracking]
           ,[ShipMethod]
           ,[ShipDate]
           ,[Synced]
           ,[ProcessedBy]
           ,[PONum]
           ,[Vendor]
           ,[InvoiceNum])
     VALUES
           (@OrderID
           ,@Status
           ,@Notes
           ,@Tracking
           ,@ShipMethod
           ,@ShipDate
           ,@Synced
           ,@ProcessedBy
           ,@PONum
           ,@Vendor
           ,@InvoiceNum)

GO


