CREATE DATABASE [Ecommerce]
GO
USE [Ecommerce]

CREATE TABLE [dbo].[AspNetRoles] (
    [Id] [nvarchar](128) NOT NULL,
    [Name] [nvarchar](256) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
)

CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [nvarchar](128) NOT NULL,
    [ClaimType] [nvarchar](max) NULL,
    [ClaimValue] [nvarchar](max) NULL,
    CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC)
)

CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] [nvarchar](128) NOT NULL,
    [ProviderKey] [nvarchar](128) NOT NULL,
    [UserId] [nvarchar](128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC)
)

CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] [nvarchar](128) NOT NULL,
    [RoleId] [nvarchar](128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC)
)

CREATE TABLE [dbo].[AspNetUsers] (
    [Id] [nvarchar](128) NOT NULL,
    [Email] [nvarchar](256) NULL,
    [EmailConfirmed] [bit] NOT NULL,
    [PasswordHash] [nvarchar](max) NULL,
    [SecurityStamp] [nvarchar](max) NULL,
    [PhoneNumber] [nvarchar](max) NULL,
    [PhoneNumberConfirmed] [bit] NOT NULL,
    [TwoFactorEnabled] [bit] NOT NULL,
    [LockoutEndDateUtc] [datetime] NULL,
    [LockoutEnabled] [bit] NOT NULL,
    [AccessFailedCount] [int] NOT NULL,
    [UserName] [nvarchar](256) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
)

CREATE TABLE [dbo].[customers] (
    [id_customer] [uniqueidentifier] NOT NULL DEFAULT (newid()),
    [name] [varchar](50) NOT NULL,
    [last_name] [varchar](50) NOT NULL,
    [email] [varchar](50) NOT NULL,
    [address] [varchar](100) NOT NULL,
    [city] [varchar](50) NOT NULL,
    PRIMARY KEY CLUSTERED ([id_customer] ASC)
)

CREATE TABLE [dbo].[products] (
    [id_product] [uniqueidentifier] NOT NULL DEFAULT (newid()),
    [description] [varchar](100) NOT NULL,
    [price] [decimal](10, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([id_product] ASC)
)

CREATE TABLE [dbo].[sales] (
    [id_sale] [uniqueidentifier] NOT NULL DEFAULT (newid()),
    [id_customer] [uniqueidentifier] NOT NULL,
    [id_product] [uniqueidentifier] NOT NULL,
    [quantity] [int] NOT NULL,
    [date] [datetime] NOT NULL,
    PRIMARY KEY CLUSTERED ([id_sale] ASC)
)

ALTER TABLE [dbo].[AspNetRoles] ADD CONSTRAINT [RoleNameIndex] UNIQUE NONCLUSTERED ([Name] ASC)
ALTER TABLE [dbo].[AspNetUserClaims] WITH CHECK ADD CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
ALTER TABLE [dbo].[AspNetUserLogins] WITH CHECK ADD CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
ALTER TABLE [dbo].[AspNetUserRoles] WITH CHECK ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
ALTER TABLE [dbo].[AspNetUserRoles] WITH CHECK ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
ALTER TABLE [dbo].[sales] WITH CHECK ADD CONSTRAINT [fk_customers] FOREIGN KEY([id_customer]) REFERENCES [dbo].[customers] ([id_customer])
ALTER TABLE [dbo].[sales] CHECK CONSTRAINT [fk_customers]
ALTER TABLE [dbo].[sales] WITH CHECK ADD CONSTRAINT [fk_products] FOREIGN KEY([id_product]) REFERENCES [dbo].[products] ([id_product])
ALTER TABLE [dbo].[sales] CHECK CONSTRAINT [fk_products]
